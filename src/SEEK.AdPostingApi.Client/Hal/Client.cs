using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SEEK.AdPostingApi.Client.Exceptions;

namespace SEEK.AdPostingApi.Client.Hal
{
    public class Client
    {
        private HttpClient httpClient;
        protected Uri BaseUri { get; set; }
        private SerializerSettings serializerSettings;

        internal protected void Initialise(HttpClient client, Uri uri)
        {
            this.httpClient = client;
            this.BaseUri = uri;
            this.serializerSettings = new SerializerSettings { Converters = { new HalResourceConverter(this.httpClient, this.BaseUri) } };
        }

        protected async Task<TResource> GetResourceAsync<TResource>(Uri uri) where TResource : HalResource, new()
        {
            var resource = new TResource();

            resource.Initialise(this.httpClient, uri);

            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(typeof(TResource).GetMediaType("application/hal+json")));

                using (var response = await httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();

                    resource.PopulateResource(JObject.Parse(await response.Content.ReadAsStringAsync()));
                    resource.ResponseHeaders = response.Headers;
                }
            }

            return resource;
        }

        protected async Task<T> HeadResourceAsync<T, TResource>(Uri uri)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Head, uri)
            {
                Headers =
                {
                    Accept = {new MediaTypeWithQualityHeaderValue(typeof(TResource).GetMediaType("application/hal+json")) }
                }
            })
            using (var response = await this.httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                var fromHeaderAttribute = typeof(T).GetCustomAttribute<FromHeaderAttribute>();
                return fromHeaderAttribute.GetValue<T>(response.Headers);
            }
        }
        protected async Task PutResourceAsync<TResource>(Uri uri, TResource resource)
        {
            var content = JsonConvert.SerializeObject(resource, serializerSettings);

            using (var request = CreateRequest<TResource>(uri, HttpMethod.Put, content))
            using (var response = await this.httpClient.SendAsync(request))
                response.EnsureSuccessStatusCode();
        }

        protected async Task<Uri> PostResourceAsync<TResource>(Uri uri, TResource resource)
        {
            var content = JsonConvert.SerializeObject(resource, serializerSettings);

            using (var request = CreateRequest<TResource>(uri, HttpMethod.Post, content))
            {
                using (var response = await this.httpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode) return response.Headers.Location;

                    string responseContent = null;
                    if (response.Content != null)
                    {
                        responseContent = await response.Content.ReadAsStringAsync();
                    }
                    throw new ResourceActionException(HttpMethod.Post, response.StatusCode, response.Headers, responseContent);
                }
            }
        }

        private HttpRequestMessage CreateRequest<TResource>(Uri uri, HttpMethod method, string content)
        {
            return new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(content, Encoding.UTF8, typeof(TResource).GetMediaType("application/json"))
            };
        }
    }
}
