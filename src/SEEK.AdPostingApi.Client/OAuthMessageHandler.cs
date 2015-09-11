using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client
{
    internal class OAuthMessageHandler : DelegatingHandler
    {
        private readonly IOAuth2TokenClient _tokenClient;

        public OAuthMessageHandler(IOAuth2TokenClient tokenClient)
        {
            _tokenClient = tokenClient;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenClient.GetOAuth2TokenAsync();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
