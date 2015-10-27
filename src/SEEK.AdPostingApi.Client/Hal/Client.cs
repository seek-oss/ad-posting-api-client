using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SEEK.AdPostingApi.Client.Models;

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
                    await HandleBadResponse(request, response);

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
                    await HandleBadResponse(request, response);

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
                    await HandleBadResponse(request, response);

                    responseResource.PopulateResource(JObject.Parse(await response.Content.ReadAsStringAsync()));
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
                    await HandleBadResponse(request, response);

                    responseResource.PopulateResource(JObject.Parse(await response.Content.ReadAsStringAsync()));
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
                    await HandleBadResponse(request, response);

                    responseResource.PopulateResource(JObject.Parse(await response.Content.ReadAsStringAsync()));
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

        private async Task HandleBadResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;

            string responseContent = await response.Content.ReadAsStringAsync();
            switch ((int)response.StatusCode)
            {
                case (int)HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException($"[{request.Method}] {request.RequestUri.AbsoluteUri} is not authorized.");
                case (int)HttpStatusCode.NotFound:
                    throw new AdvertisementNotFoundException();
                case (int)HttpStatusCode.Conflict:
                    throw new AdvertisementAlreadyExistsException(response.Headers.Location);
                case 422:
                    ValidationMessage validationMessage;
                    if (TryDeserialize(responseContent, out validationMessage))
                    {
                        throw new ValidationException(request.Method, validationMessage);
                    }
                    break;
            }

            response.EnsureSuccessStatusCode();
        }

        private bool TryDeserialize(string responseContent, out ValidationMessage validationMessage)
        {
            try
            {
                validationMessage = JsonConvert.DeserializeObject<ValidationMessage>(responseContent);
            }
            catch
            {
                validationMessage = null;
            }

            return validationMessage != null;
        }
    }
}