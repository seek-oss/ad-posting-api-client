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
        private SerializerSettings _serializerSettings;

        protected Uri BaseUri { get; set; }

        protected internal void Initialise(HttpClient client, Uri uri)
        {
            this._httpClient = client;
            this.BaseUri = uri;
            this._serializerSettings = new SerializerSettings { Converters = { new HalResourceConverter(this._httpClient, this.BaseUri) } };
        }

        protected async Task<TResponseResource> GetResourceAsync<TResponseResource>(Uri uri) where TResponseResource : HalResource, new()
        {
            using (HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(typeof(TResponseResource).GetMediaType("application/hal+json")));

                using (HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest))
                {
                    await HandleBadResponse(httpRequest, httpResponse);

                    return await this.CreateResponseResource<TResponseResource>(uri, httpResponse);
                }
            }
        }

        protected async Task<TResponseResource> HeadResourceAsync<TResponseResource, TRequest>(Uri uri)
        {
            using (HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Head, uri) { Headers = { Accept = { new MediaTypeWithQualityHeaderValue(typeof(TRequest).GetMediaType("application/hal+json")) } } })
            {
                using (HttpResponseMessage httpResponse = await this._httpClient.SendAsync(httpRequest))
                {
                    await HandleBadResponse(httpRequest, httpResponse);

                    return typeof(TResponseResource).GetCustomAttribute<FromHeaderAttribute>().GetValue<TResponseResource>(httpResponse.Headers);
                }
            }
        }

        protected async Task<TResponseResource> PatchResourceAsync<TResponseResource, TRequest>(Uri uri, TRequest request) where TResponseResource : HalResource, new()
        {
            string content = JsonConvert.SerializeObject(request, _serializerSettings);

            using (HttpRequestMessage httpRequest = this.CreateHttpRequest<TRequest>(uri, new HttpMethod("PATCH"), content))
            {
                using (HttpResponseMessage httpResponse = await this._httpClient.SendAsync(httpRequest))
                {
                    await HandleBadResponse(httpRequest, httpResponse);

                    return await this.CreateResponseResource<TResponseResource>(uri, httpResponse);
                }
            }
        }

        protected async Task<TResponseResource> PutResourceAsync<TResponseResource, TRequest>(Uri uri, TRequest request) where TResponseResource : HalResource, new()
        {
            string content = JsonConvert.SerializeObject(request, _serializerSettings);

            using (var httpRequest = this.CreateHttpRequest<TResponseResource>(uri, HttpMethod.Put, content))
            {
                using (var httpResponse = await this._httpClient.SendAsync(httpRequest))
                {
                    await HandleBadResponse(httpRequest, httpResponse);

                    return await this.CreateResponseResource<TResponseResource>(uri, httpResponse);
                }
            }
        }

        protected async Task<TResponseResource> PostResourceAsync<TResponseResource, TRequest>(Uri uri, TRequest requestResource) where TResponseResource : HalResource, new()
        {
            string content = JsonConvert.SerializeObject(requestResource, _serializerSettings);

            using (HttpRequestMessage httpRequest = this.CreateHttpRequest<TRequest>(uri, HttpMethod.Post, content))
            {
                using (HttpResponseMessage httpResponse = await this._httpClient.SendAsync(httpRequest))
                {
                    await HandleBadResponse(httpRequest, httpResponse);

                    return await this.CreateResponseResource<TResponseResource>(uri, httpResponse);
                }
            }
        }

        private HttpRequestMessage CreateHttpRequest<TResource>(Uri uri, HttpMethod method, string content)
        {
            return new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(content, Encoding.UTF8, typeof(TResource).GetMediaType("application/json"))
            };
        }

        private async Task<TResponseResource> CreateResponseResource<TResponseResource>(Uri uri, HttpResponseMessage response) where TResponseResource : HalResource, new()
        {
            var resource = new TResponseResource();

            resource.Initialise(this._httpClient, uri);
            resource.PopulateResource(JObject.Parse(await response.Content.ReadAsStringAsync()));
            resource.ResponseHeaders = response.Headers;

            return resource;
        }

        private async Task HandleBadResponse(HttpRequestMessage httpRequest, HttpResponseMessage httpResponse)
        {
            switch ((int)httpResponse.StatusCode)
            {
                case (int)HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException($"[{httpRequest.Method}] {httpRequest.RequestUri.AbsoluteUri} is not authorized.");
                case (int)HttpStatusCode.NotFound:
                    throw new AdvertisementNotFoundException();
                case (int)HttpStatusCode.Conflict:
                    throw new AdvertisementAlreadyExistsException(httpResponse.Headers.Location);
                case 422:
                    ValidationMessage validationMessage;
                    string responseContent = await httpResponse.Content.ReadAsStringAsync();

                    if (TryDeserialize(responseContent, out validationMessage))
                    {
                        throw new ValidationException(httpRequest.Method, validationMessage);
                    }
                    break;

                default:
                    httpResponse.EnsureSuccessStatusCode();
                    break;
            }
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