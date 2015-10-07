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
        private HttpClient _httpClient;
        protected Uri BaseUri { get; set; }
        private SerializerSettings _serializerSettings;

        protected internal void Initialise(HttpClient client, Uri uri)
        {
            this._httpClient = client;
            this.BaseUri = uri;
            this._serializerSettings = new SerializerSettings { Converters = { new HalResourceConverter(this._httpClient, this.BaseUri) } };
        }

        protected async Task<TResource> GetResourceAsync<TResource>(Uri uri) where TResource : HalResource, new()
        {
            var resource = new TResource();

            resource.Initialise(this._httpClient, uri);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(typeof(TResource).GetMediaType("application/hal+json")));

                using (HttpResponseMessage response = await _httpClient.SendAsync(request))
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
            using (var request = new HttpRequestMessage(HttpMethod.Head, uri) { Headers = { Accept = { new MediaTypeWithQualityHeaderValue(typeof(TResource).GetMediaType("application/hal+json")) } } })
            {
                using (var response = await this._httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();

                    return typeof(T).GetCustomAttribute<FromHeaderAttribute>().GetValue<T>(response.Headers);
                }
            }
        }

        protected async Task<TResource> PatchResourceAsync<TResource, T>(Uri uri, T resource) where TResource : HalResource, new()
        {
            var responseResource = new TResource();
            var content = JsonConvert.SerializeObject(resource, _serializerSettings);

            using (var request = this.CreateRequest<T>(uri, new HttpMethod("PATCH"), content))
            {
                using (var response = await this._httpClient.SendAsync(request))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ResourceActionException(new HttpMethod("PATCH"), response.StatusCode, response.Headers, responseContent);
                    }

                    responseResource.PopulateResource(JObject.Parse(responseContent));
                    responseResource.ResponseHeaders = response.Headers;
                }
            }

            return responseResource;
        }

        protected async Task<T> PutResourceAsync<T, TResource>(Uri uri, TResource resource) where T : HalResource, new()
        {
            var responseResource = new T();
            var content = JsonConvert.SerializeObject(resource, _serializerSettings);

            using (var request = this.CreateRequest<T>(uri, HttpMethod.Put, content))
            {
                using (var response = await this._httpClient.SendAsync(request))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ResourceActionException(HttpMethod.Put, response.StatusCode, response.Headers, responseContent);
                    }
                    responseResource.PopulateResource(JObject.Parse(responseContent));
                    responseResource.ResponseHeaders = response.Headers;
                }
                return responseResource;
            }
        }

        protected async Task<TResource> PostResourceAsync<TResource, T>(Uri uri, T resource) where TResource : HalResource, new()
        {
            var responseResource = new TResource();
            var content = JsonConvert.SerializeObject(resource, _serializerSettings);

            using (HttpRequestMessage request = this.CreateRequest<T>(uri, HttpMethod.Post, content))
            {
                using (HttpResponseMessage response = await this._httpClient.SendAsync(request))
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ResourceActionException(HttpMethod.Post, response.StatusCode, response.Headers, responseContent);
                    }

                    responseResource.PopulateResource(JObject.Parse(responseContent));
                    responseResource.ResponseHeaders = response.Headers;
                }

                return responseResource;
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