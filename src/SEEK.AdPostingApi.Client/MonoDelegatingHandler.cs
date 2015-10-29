using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client
{
    /// <summary>
    /// Sometimes Mono does not get the response content in time and throws a WebException instead of
    /// returning the HttpResponseMessage for 400 responses in particular.
    ///
    /// This delegating handler is a workaround to throw a ResourceActionException instead which allows
    /// access to the response headers and content e.g. to deserialize the ValidationDataDictionary
    /// in the BadRequestHandler.
    /// </summary>
    public class MonoDelegatingHandler : DelegatingHandler
    {
        public MonoDelegatingHandler() : base(new HttpClientHandler())
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response is HttpWebResponse)
                {
                    return CreateResponseMessage((HttpWebResponse)ex.Response, request);
                }
                throw;
            }
        }

        private HttpResponseMessage CreateResponseMessage(HttpWebResponse webResponse, HttpRequestMessage request)
        {
            var httpResponseMessage = new HttpResponseMessage(webResponse.StatusCode)
            {
                ReasonPhrase = webResponse.StatusDescription,
                Version = webResponse.ProtocolVersion,
                RequestMessage = request,
                Content = new StreamContent(webResponse.GetResponseStream())
            };

            request.RequestUri = webResponse.ResponseUri;

            WebHeaderCollection webResponseHeaders = webResponse.Headers;
            HttpContentHeaders contentHeaders = httpResponseMessage.Content.Headers;
            HttpResponseHeaders headers = httpResponseMessage.Headers;

            if (webResponse.ContentLength >= 0L)
            {
                contentHeaders.ContentLength = webResponse.ContentLength;
            }

            for (int index = 0; index < webResponseHeaders.Count; ++index)
            {
                string key = webResponseHeaders.GetKey(index);

                if (string.Compare(key, "Content-Length", StringComparison.OrdinalIgnoreCase) != 0)
                {
                    string[] values = webResponseHeaders.GetValues(index);

                    if (!headers.TryAddWithoutValidation(key, values))
                    {
                        contentHeaders.TryAddWithoutValidation(key, values);
                    }
                }
            }

            return httpResponseMessage;
        }
    }
}