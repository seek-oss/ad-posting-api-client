using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace SEEK.AdPostingApi.Client.Hal
{
    public class HalResource<TProperties> : HalResource where TProperties : new()
    {
        public TProperties Properties { get; private set; }

        internal override void PopulateResource(JToken content)
        {
            base.PopulateResource(content);

            this.Properties = new TProperties();
            this.Properties = content.ToObject<TProperties>();
        }
    }

    public class HalResource
    {
        private HttpClient httpClient;
        private Uri baseUri;
        private IOAuth2TokenClient tokenClient;

        protected Dictionary<string, Link> Links { get; private set; }

        internal JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Converters = {new StringEnumConverter()},
        };

        internal void InitialiseEmbedded(HttpClient client, Uri uri, IOAuth2TokenClient tokenClient)
        {
            this.httpClient = client;
            this.baseUri = uri;
            this.tokenClient = tokenClient;
            this.SerializerSettings.Converters.Add(new HalResourceConverter(this.httpClient, this.baseUri, this.tokenClient));
        }

        internal async Task Initialise(HttpClient client, Uri uri, IOAuth2TokenClient tokenClient)
        {
            this.InitialiseEmbedded(client, uri, tokenClient);
            var token = await tokenClient.GetOAuth2TokenAsync();

            using (var request = new HttpRequestMessage(HttpMethod.Get, uri)
            {
                Headers =
                {
                    Accept = {new MediaTypeWithQualityHeaderValue(this.GetType().GetMediaType("application/hal+json")) },
                    Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken)
                }
            })
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                this.PopulateResource(JObject.Parse(await response.Content.ReadAsStringAsync()));
            }
        }

        internal virtual void PopulateResource(JToken content)
        {
            if (content["_links"] != null)
                this.Links = content["_links"].ToObject<Dictionary<string, Link>>();

            if (content["_embedded"] != null)
            {
                var property = this.GetType().GetProperty("Embedded", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance);

                if (property != null)
                {
                    var embeddedResources = content["_embedded"].ToObject(property.PropertyType, JsonSerializer.Create(this.SerializerSettings));

                    property.SetValue(this, embeddedResources);
                }
            }
        }

        protected async Task<Uri> PostResourceAsync<TResource>(string relation, object parameters, TResource resource)
        {
            var uri = new Uri(this.baseUri, this.Links[relation].Resolve(parameters));
            var content = JsonConvert.SerializeObject(resource, SerializerSettings);
            using (var request = await CreateRequest<TResource>(uri, HttpMethod.Post, content, this.tokenClient))

            using (var response = await this.httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
        }

        protected Task<Uri> PostResourceAsync<TResource>(string relation, TResource resource)
        {
            return this.PostResourceAsync(relation, null, resource);
        }

        protected async Task PutResourceAsync<TResource>(string relation, object parameters, TResource resource)
        {
            var uri = new Uri(this.baseUri, this.Links[relation].Resolve(parameters));
            var content = JsonConvert.SerializeObject(resource, SerializerSettings);
            using (var request = await CreateRequest<TResource>(uri, HttpMethod.Put, content, this.tokenClient))
            using (var response = await this.httpClient.SendAsync(request))
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
