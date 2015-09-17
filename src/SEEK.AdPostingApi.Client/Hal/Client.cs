using System;
using System.IO;
using System.Linq;
using System.Net;
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
                try
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
                catch (WebException ex)
                {
                    await ThrowResourceActionExceptionIfExceptionIsProtocolError(ex);
                    throw;
                }
            }
        }

        private static async Task ThrowResourceActionExceptionIfExceptionIsProtocolError(WebException ex)
        {
            // Ugly workaround for mono in docker where the repo is on a unix file system throws a WebException :'(
            if ((ex.Status != WebExceptionStatus.ProtocolError) || (!(ex.Response is HttpWebResponse))) return;

            var webResponse = (HttpWebResponse) ex.Response;
            string responseContent = null;
            using (var responseStream = webResponse.GetResponseStream())
            {
                var encoding = string.IsNullOrWhiteSpace(webResponse.ContentEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(webResponse.ContentEncoding);
                using (var streamReader = new StreamReader(responseStream, encoding))
                {
                    responseContent = await streamReader.ReadToEndAsync();
                }
            }

            using (var responseMessage = new HttpResponseMessage(webResponse.StatusCode))
            {
                foreach (var headerName in webResponse.Headers.Keys.Cast<string>()
                    .Where(headerName => !string.IsNullOrWhiteSpace(webResponse.Headers[headerName])))
                {
                    // Try add the header since e.g. the "Content-Type" header throws a "System.InvalidOperationException : Content-Type" exception.
                    responseMessage.Headers.TryAddWithoutValidation(headerName, webResponse.Headers[headerName]);
                }

                throw new ResourceActionException(HttpMethod.Post, responseMessage.StatusCode, responseMessage.Headers, responseContent);
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
