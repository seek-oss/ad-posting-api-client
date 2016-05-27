using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
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

        public void Dispose()
        {
            this._httpClient.Dispose();
        }

        public async Task<TResponseResource> GetResourceAsync<TResponseResource>(Uri uri) where TResponseResource : IResource, new()
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                var acceptHeader = MediaTypeWithQualityHeaderValue.Parse(typeof(TResponseResource).GetMediaType());

                acceptHeader.CharSet = "utf-8";
                httpRequest.Headers.Accept.Add(acceptHeader);

                using (HttpResponseMessage httpResponse = await this._httpClient.SendAsync(httpRequest))
                {
                    await this.HandleBadResponse(httpRequest, httpResponse);

                    return await this.CreateResponseResource<TResponseResource>(uri, httpResponse);
                }
            }
        }

        public async Task<HttpResponseHeaders> HeadResourceAsync<TRequest>(Uri uri)
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Head, uri))
            {
                var acceptHeader = MediaTypeWithQualityHeaderValue.Parse(typeof(TRequest).GetMediaType());

                acceptHeader.CharSet = "utf-8";
                httpRequest.Headers.Accept.Add(acceptHeader);

                using (HttpResponseMessage httpResponse = await this._httpClient.SendAsync(httpRequest))
                {
                    await this.HandleBadResponse(httpRequest, httpResponse);

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
                    await this.HandleBadResponse(httpRequest, httpResponse);

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
                    await this.HandleBadResponse(httpRequest, httpResponse);

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
                    await this.HandleBadResponse(httpRequest, httpResponse);

                    return await this.CreateResponseResource<TResponseResource>(uri, httpResponse);
                }
            }
        }

        private HttpRequestMessage CreateHttpRequest<TResource>(Uri uri, HttpMethod method, string content)
        {
            var encoding = Encoding.UTF8;
            var stringContent = new StringContent(content, encoding);

            stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse(typeof(TResource).GetMediaType());
            stringContent.Headers.ContentType.CharSet = encoding.WebName;

            return new HttpRequestMessage(method, uri) { Content = stringContent };
        }

        private async Task<TResponseResource> CreateResponseResource<TResponseResource>(Uri uri, HttpResponseMessage response) where TResponseResource : IResource, new()
        {
            return JsonConvert.DeserializeObject<TResponseResource>(await response.Content.ReadAsStringAsync(), new ResourceConverter(this, uri, response.Headers));
        }

        private async Task HandleBadResponse(HttpRequestMessage httpRequest, HttpResponseMessage httpResponse)
        {
            IEnumerable<string> requestIds;
            string requestId = httpResponse.Headers.TryGetValues("X-Request-Id", out requestIds) ? requestIds.First() : null;

            switch ((int)httpResponse.StatusCode)
            {
                case (int)HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException(requestId, $"[{httpRequest.Method}] {httpRequest.RequestUri.AbsoluteUri} is not authorized.");

                case (int)HttpStatusCode.Forbidden:
                    string content = await httpResponse.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(content))
                    {
                        throw new UnauthorizedException(requestId, $"[{httpRequest.Method}] {httpRequest.RequestUri.AbsoluteUri} is not authorized.");
                    }

                    ForbiddenMessage forbiddenMessage;

                    if (this.TryDeserializeForbiddenMessage(content, out forbiddenMessage))
                    {
                        throw new UnauthorizedException(requestId, forbiddenMessage);
                    }
                    break;

                case (int)HttpStatusCode.NotFound:
                    throw new AdvertisementNotFoundException(requestId);

                case (int)HttpStatusCode.Conflict:
                    throw new AdvertisementAlreadyExistsException(requestId, httpResponse.Headers.Location);

                case 422:
                    ValidationMessage validationMessage;
                    string responseContent = await httpResponse.Content.ReadAsStringAsync();

                    if (this.TryDeserialize(responseContent, out validationMessage))
                    {
                        throw new ValidationException(requestId, httpRequest.Method, validationMessage);
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

        private class SerializerSettings : JsonSerializerSettings
        {
            public SerializerSettings()
            {
                this.ContractResolver = new CamelCasePropertyNamesContractResolver();
                this.NullValueHandling = NullValueHandling.Ignore;
                this.Converters.Add(new StringEnumConverter());
            }
        }
    }
}