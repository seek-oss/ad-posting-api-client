using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SEEK.AdPostingApi.Client.Hal
{
    public class HalResource
    {
        private HttpClient httpClient;
        private Uri baseUri;
        private IOAuth2TokenClient tokenClient;

        [JsonProperty("_links")]
        protected Dictionary<string, Link> Links { get; } = new Dictionary<string, Link>();

        protected bool ShouldSerializeLinks()
        {
            return false;
        }

        protected static JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Converters = {new StringEnumConverter()}
        };


        internal async Task Initialise(HttpClient client, Uri uri, IOAuth2TokenClient tokenClient)
        {
            this.httpClient = client;
            this.baseUri = uri;
            this.tokenClient = tokenClient;

            var token = await tokenClient.GetOAuth2TokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, uri)
            {
                Headers =
                {
                    Accept = {new MediaTypeWithQualityHeaderValue(this.GetType().GetMediaType("application/hal+json")) },
                    Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken)
                }
            };

            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            JsonConvert.PopulateObject(content, this);
        }

        protected async Task<Uri> PostResourceAsync<T>(string relation, object parameters, T resource)
        {
            var uri = new Uri(this.baseUri, this.Links[relation].Resolve(parameters));
            var content = JsonConvert.SerializeObject(resource, SerializerSettings);
            var request = await CreateRequest<T>(uri, HttpMethod.Post, content);

            var response = await this.httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }

        protected Task<Uri> PostResourceAsync<T>(string relation, T resource)
        {
            return this.PostResourceAsync(relation, null, resource);
        }

        protected async Task PutResourceAsync<T>(string relation, object parameters, T resource)
        {
            var uri = new Uri(this.baseUri, this.Links[relation].Resolve(parameters));
            var content = JsonConvert.SerializeObject(resource, SerializerSettings);
            var request = await CreateRequest<T>(uri, HttpMethod.Put, content);

            var response = await this.httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        protected Task PutResourceAsync<T>(string relation, T resource)
        {
            return this.PutResourceAsync(relation, null, resource);
        }

        protected async Task<T> GetResourceAsync<T>(string relation, object parameters) where T : HalResource, new()
        {
            var uri = new Uri(this.baseUri, this.Links[relation].Resolve(parameters));
            var resource = new T();
            await resource.Initialise(this.httpClient, uri, this.tokenClient);
            return resource;
        }

        protected Task<T> GetResourceAsync<T>(string relation) where T : HalResource, new()
        {
            return this.GetResourceAsync<T>(relation, null);
        }

        private async Task<HttpRequestMessage> CreateRequest<T>(Uri uri, HttpMethod method, string content)
        {
            var token = await this.tokenClient.GetOAuth2TokenAsync();

            return new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(content, Encoding.UTF8, typeof (T).GetMediaType("application/json")),
                Headers =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken)
                }
            };
        }
    }
}
