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
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.Client.Hal
{
    public class HalResource<T> : HalResource where T : new()
    {
        public T Properties { get; set; }

        protected override void PopulateResource(string content)
        {
            this.Properties = new T();
            JsonConvert.PopulateObject(content, this.Properties);
        }
    }

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

        internal static JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings
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
            this.PopulateResource(content);
        }

        protected virtual void PopulateResource(string content)
        {
            JsonConvert.PopulateObject(content, this);
        }

        protected async Task<Uri> PostResourceAsync<TResource>(string relation, object parameters, TResource resource)
        {
            var uri = new Uri(this.baseUri, this.Links[relation].Resolve(parameters));
            var content = JsonConvert.SerializeObject(resource, SerializerSettings);
            var request = await CreateRequest<TResource>(uri, HttpMethod.Post, content, this.tokenClient);

            var response = await this.httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }

        protected Task<Uri> PostResourceAsync<TResource>(string relation, TResource resource)
        {
            return this.PostResourceAsync(relation, null, resource);
        }

        protected async Task PutResourceAsync<TResource>(string relation, object parameters, TResource resource)
        {
            var uri = new Uri(this.baseUri, this.Links[relation].Resolve(parameters));
            var content = JsonConvert.SerializeObject(resource, SerializerSettings);
            var request = await CreateRequest<TResource>(uri, HttpMethod.Put, content, this.tokenClient);
            var response = await this.httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        protected Task PutResourceAsync<TResource>(string relation, TResource resource)
        {
            return this.PutResourceAsync(relation, null, resource);
        }

        protected async Task<TResource> GetResourceAsync<TResource>(string relation, object parameters) where TResource : HalResource, new()
        {
            var uri = new Uri(this.baseUri, this.Links[relation].Resolve(parameters));
            var resource = new TResource();
            await resource.Initialise(this.httpClient, uri, this.tokenClient);
            return resource;
        }

        protected Task<TResource> GetResourceAsync<TResource>(string relation) where TResource : HalResource, new()
        {
            return this.GetResourceAsync<TResource>(relation, null);
        }

        internal static async Task<HttpRequestMessage> CreateRequest<TResource>(Uri uri, HttpMethod method, string content, IOAuth2TokenClient tokenClient)
        {
            var token = await tokenClient.GetOAuth2TokenAsync();
            return new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(content, Encoding.UTF8, typeof (TResource).GetMediaType("application/json")),
                Headers =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken)
                }
            };
        }
    }
}
