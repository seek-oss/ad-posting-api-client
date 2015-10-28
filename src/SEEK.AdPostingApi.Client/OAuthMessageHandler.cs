using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    internal class OAuthMessageHandler : DelegatingHandler
    {
        private readonly IOAuth2TokenClient _tokenClient;
        private OAuth2Token _oAuth2Token;

        public OAuthMessageHandler(IOAuth2TokenClient tokenClient) : base(new MonoDelegatingHandler())
        {
            _tokenClient = tokenClient;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var currentOAuth2AccessToken = _oAuth2Token ?? await GetOAuth2TokenAsync(_oAuth2Token);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", currentOAuth2AccessToken.AccessToken);
            var res = await base.SendAsync(request, cancellationToken);

            if ((res.StatusCode == HttpStatusCode.Unauthorized) && (res.Headers.WwwAuthenticate.Any(v => v.Scheme.ToLowerInvariant() == "bearer")))
            {
                res.Dispose();

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", (await GetOAuth2TokenAsync(currentOAuth2AccessToken)).AccessToken);
                res = await base.SendAsync(request, cancellationToken);
            }

            return res;
        }

        private async Task<OAuth2Token> GetOAuth2TokenAsync(OAuth2Token currentOAuth2Token)
        {
            if (currentOAuth2Token != _oAuth2Token) return _oAuth2Token;

            return _oAuth2Token = await _tokenClient.GetOAuth2TokenAsync();
        }
    }
}
