using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client
{
    public class AdPostingApiMessageHandler : DelegatingHandler
    {
        static AdPostingApiMessageHandler()
        {
            SetProductVersion(typeof(AdPostingApiMessageHandler).GetTypeInfo().Assembly.GetName().Version.ToString());
        }

        internal static void SetProductVersion(string productVersion)
        {
            UserAgentProduct = new ProductInfoHeaderValue(typeof(AdPostingApiMessageHandler).GetTypeInfo().Assembly.GetName().Name, productVersion);
        }

        private static ProductInfoHeaderValue UserAgentProduct { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.UserAgent?.Add(UserAgentProduct);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}