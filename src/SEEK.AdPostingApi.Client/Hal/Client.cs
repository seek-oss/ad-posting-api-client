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
                httpRequest.Headers.Accept.Add(this.CreateResourceHeader<TResponseResource>());
                httpRequest.Headers.Accept.Add(this.CreateErrorResponseHeader());

                using (HttpResponseMessage httpResponse = await this._httpClient.SendAsync(httpRequest))
                {
                    await this.HandleBadResponse(httpRequest, httpResponse);

                    return await this.CreateResponseResource<TResponseResource>(uri, httpResponse);
                }
            }
        }

        public async Task<HttpResponseHeaders> HeadResourceAsync<TResponseResource>(Uri uri)
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Head, uri))
            {
                httpRequest.Headers.Accept.Add(this.CreateResourceHeader<TResponseResource>());
                httpRequest.Headers.Accept.Add(this.CreateErrorResponseHeader());

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
                httpRequest.Headers.Accept.Add(this.CreateResourceHeader<TResponseResource>());
                httpRequest.Headers.Accept.Add(this.CreateErrorResponseHeader());

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
                httpRequest.Headers.Accept.Add(this.CreateResourceHeader<TResponseResource>());
                httpRequest.Headers.Accept.Add(this.CreateErrorResponseHeader());

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
                httpRequest.Headers.Accept.Add(this.CreateResourceHeader<TResponseResource>());
                httpRequest.Headers.Accept.Add(this.CreateErrorResponseHeader());

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

        private MediaTypeWithQualityHeaderValue CreateResourceHeader<TResponseResource>()
        {
            MediaTypeWithQualityHeaderValue resourceHeader = MediaTypeWithQualityHeaderValue.Parse(typeof(TResponseResource).GetMediaType());

            resourceHeader.CharSet = "utf-8";

            return resourceHeader;
        }

        private MediaTypeWithQualityHeaderValue CreateErrorResponseHeader()
        {
            MediaTypeWithQualityHeaderValue resourceHeader = MediaTypeWithQualityHeaderValue.Parse(typeof(AdvertisementErrorResponse).GetMediaType());

            resourceHeader.CharSet = "utf-8";

            return resourceHeader;
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
                    AdvertisementErrorResponse advertisementErrorResponse;

                    if (string.IsNullOrEmpty(content))
                    {
                        throw new UnauthorizedException(requestId, $"[{httpRequest.Method}] {httpRequest.RequestUri.AbsoluteUri} is not authorized.");
                    }

                    if (this.TryDeserializeError(content, out advertisementErrorResponse))
                    {
                        throw new UnauthorizedException(requestId, advertisementErrorResponse);
                    }

                    break;

                case (int)HttpStatusCode.NotFound:
                    throw new AdvertisementNotFoundException(requestId);

                case (int)HttpStatusCode.Conflict:
                    AdvertisementErrorResponse conflictMessage;
                    string respContent = await httpResponse.Content.ReadAsStringAsync();

                    if (this.TryDeserializeError(respContent, out conflictMessage))
                    {
                        throw new CreationIdAlreadyExistsException(requestId, httpResponse.Headers.Location, conflictMessage);
                    }

                    break;

                case 422:
                    AdvertisementErrorResponse validationMessage;
                    string responseContent = await httpResponse.Content.ReadAsStringAsync();

                    if (this.TryDeserializeError(responseContent, out validationMessage))
                    {
                        throw new ValidationException(requestId, httpRequest.Method, validationMessage);
                    }

                    break;

                case 429:
                    string retryAfterHeaderValue = httpResponse.GetHeaderValue("Retry-After");

                    int retryAfterSeconds;
                    if (int.TryParse(retryAfterHeaderValue, out retryAfterSeconds))
                    {
                        throw new TooManyRequestsException(requestId, retryAfterSeconds);
                    }

                    throw new TooManyRequestsException(requestId);
            }

            if (!httpResponse.IsSuccessStatusCode)
            {
                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                string responseContentType = httpResponse.GetHeaderValue("Content-Type");
                throw new RequestException(requestId, (int)httpResponse.StatusCode, httpResponse.ReasonPhrase, responseContent, responseContentType);
            }
        }

        private bool TryDeserializeError(string responseContent, out AdvertisementErrorResponse error)
        {
            try
            {
                error = JsonConvert.DeserializeObject<AdvertisementErrorResponse>(responseContent);
            }
            catch
            {
                error = null;
            }

            return error != null;
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