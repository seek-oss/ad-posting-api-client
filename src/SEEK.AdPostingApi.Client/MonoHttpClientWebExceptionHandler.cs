using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Exceptions;

namespace SEEK.AdPostingApi.Client
{
    /// <summary>
    /// Sometimes Mono does not get the response content in time and throws a WebException instead of
    /// returning the HttpResponseMessage for 400 responses in particular.
    /// 
    /// This delegating handler is a workaround to throw a ResourceActionException instead which allows
    /// access to the response headers and content e.g. to deserialize the ValidationDataDictionary
    /// in the BadRequestHandler.
    /// 
    /// For more info on the Mono HttpClient.SendAsync behaviour, see Martin Booth who can direct you
    /// to the Mono code which is the root cause of this behaviour.
    /// </summary>
    public class MonoHttpClientWebExceptionHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (WebException ex)
            {
                await ThrowResourceActionExceptionIfExceptionIsProtocolError(request, ex);
                throw;
            }
        }

        private static async Task ThrowResourceActionExceptionIfExceptionIsProtocolError(HttpRequestMessage request, WebException ex)
        {
            if ((ex.Status != WebExceptionStatus.ProtocolError) || (!(ex.Response is HttpWebResponse))) return;

            var webResponse = (HttpWebResponse)ex.Response;
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
                    // Try add the headers since e.g. the "Content-Type" header throws a "System.InvalidOperationException : Content-Type" exception.
                    responseMessage.Headers.TryAddWithoutValidation(headerName, webResponse.Headers[headerName]);
                }

                throw new ResourceActionException(request.Method, responseMessage.StatusCode, responseMessage.Headers, responseContent, ex);
            }
        }
    }
}
