using System;
using System.Net;
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
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenClient.AccessToken);

            var res = await base.SendAsync(request, cancellationToken);

            if (res.StatusCode == HttpStatusCode.Unauthorized)
            {
                res.Dispose();

                var token = await _tokenClient.GetOAuth2TokenAsync();

                _tokenClient.AccessToken = token.AccessToken;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenClient.AccessToken);

                res = await base.SendAsync(request, cancellationToken);
            }

            return res;
        }
    }
}
