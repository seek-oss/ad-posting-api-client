using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Hal
{
    public class Client : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly SerializerSettings _serializerSettings;

        public Client(HttpClient client)
        {
            this._httpClient = client;
            this._serializerSettings = new SerializerSettings();
        }

        public async Task<TResponseResource> GetResourceAsync<TResponseResource>(Uri uri) where TResponseResource : IResource, new()
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(typeof(TResponseResource).GetMediaType("application/hal+json")));

                using (HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest))
                {
                    await HandleBadResponse(httpRequest, httpResponse);

                    return await this.CreateResponseResource<TResponseResource>(uri, httpResponse);
                }
            }
        }

        public async Task<HttpResponseHeaders> HeadResourceAsync<TRequest>(Uri uri)
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Head, uri) { Headers = { Accept = { new MediaTypeWithQualityHeaderValue(typeof(TRequest).GetMediaType("application/hal+json")) } } })
            {
                using (HttpResponseMessage httpResponse = await this._httpClient.SendAsync(httpRequest))
                {
                    await HandleBadResponse(httpRequest, httpResponse);

                    return httpResponse.Headers;
                }
            }
        }

        public async Task<TResponseResource> PatchResourceAsync<TResponseResource, TRequest>(Uri uri, TRequest request) where TResponseResource : IResource, new()
        {
            string content = JsonConvert.SerializeObject(request, this._serializerSettings);

            using (HttpRequestMessage httpRequest = this.CreateHttpRequest<TRequest>(uri, new HttpMethod("PATCH"), content))
            {
                using (HttpResponseMessage httpResponse = await this._httpClient.SendAsync(httpRequest))
                {
                    await HandleBadResponse(httpRequest, httpResponse);

                    return await this.CreateResponseResource<TResponseResource>(uri, httpResponse);
                }
            }
        }

        public async Task<TResponseResource> PutResourceAsync<TResponseResource, TRequest>(Uri uri, TRequest request) where TResponseResource : IResource, new()
        {
            string content = JsonConvert.SerializeObject(request, this._serializerSettings);

            using (var httpRequest = this.CreateHttpRequest<TResponseResource>(uri, HttpMethod.Put, content))
            {
                using (var httpResponse = await this._httpClient.SendAsync(httpRequest))
                {
                    await HandleBadResponse(httpRequest, httpResponse);

                    return await this.CreateResponseResource<TResponseResource>(uri, httpResponse);
                }
            }
        }

        public async Task<TResponseResource> PostResourceAsync<TResponseResource, TRequest>(Uri uri, TRequest requestResource) where TResponseResource : IResource, new()
        {
            string content = JsonConvert.SerializeObject(requestResource, this._serializerSettings);

            using (HttpRequestMessage httpRequest = this.CreateHttpRequest<TRequest>(uri, HttpMethod.Post, content))
            {
                using (HttpResponseMessage httpResponse = await this._httpClient.SendAsync(httpRequest))
                {
                    await HandleBadResponse(httpRequest, httpResponse);

                    return await this.CreateResponseResource<TResponseResource>(uri, httpResponse);
                }
            }
        }

        public void Dispose()
        {
            this._httpClient.Dispose();
        }

        private HttpRequestMessage CreateHttpRequest<TResource>(Uri uri, HttpMethod method, string content)
        {
            return new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(content, Encoding.UTF8, typeof(TResource).GetMediaType("application/json"))
            };
        }

        private async Task<TResponseResource> CreateResponseResource<TResponseResource>(Uri uri, HttpResponseMessage response) where TResponseResource : IResource, new()
        {
            return JsonConvert.DeserializeObject<TResponseResource>(await response.Content.ReadAsStringAsync(), new ResourceConverter(this, uri, response.Headers));
        }

        private async Task HandleBadResponse(HttpRequestMessage httpRequest, HttpResponseMessage httpResponse)
        {
            switch ((int)httpResponse.StatusCode)
            {
                case (int)HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException($"[{httpRequest.Method}] {httpRequest.RequestUri.AbsoluteUri} is not authorized.");

                case (int)HttpStatusCode.Forbidden:
                    string content = await httpResponse.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(content))
                    {
                        throw new UnauthorizedException($"[{httpRequest.Method}] {httpRequest.RequestUri.AbsoluteUri} is not authorized.");
                    }

                    //JToken token = JToken.Parse(content);

                    //throw new UnauthorizedException(token["message"].ToString());

                    ForbiddenMessage forbiddenMessage;

                    if (TryDeserializeForbiddenMessage(content, out forbiddenMessage))
                    {
                        throw new UnauthorizedException(forbiddenMessage);
                    }
                    break;

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

        private bool TryDeserializeForbiddenMessage(string responseContent, out ForbiddenMessage forbiddenMessage)
        {
            try
            {
                forbiddenMessage = JsonConvert.DeserializeObject<ForbiddenMessage>(responseContent);
            }
            catch
            {
                forbiddenMessage = null;
            }

            return forbiddenMessage != null;
        }

    }
}