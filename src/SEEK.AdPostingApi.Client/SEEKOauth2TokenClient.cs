using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SEEK.AdPostingApi.Client
{
    internal class SEEKOauth2TokenClient : ISEEKOauth2TokenClient
    {
        private readonly Uri _tokenUri;
        private readonly HttpClient _httpClient;

        public SEEKOauth2TokenClient()
        {
            _tokenUri = new Uri("https://api.seek.com.au/auth/oauth2/token");
            _httpClient = new HttpClient();
        }

        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public async Task<Oauth2Token> GetOauth2Token(string id, string secret)
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _tokenUri);
            var content = String.Format("client_id={0}&client_secret={1}&grant_type=client_credentials", id, secret);
            tokenRequest.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

            try
            {
                var result = (await _httpClient.SendAsync(tokenRequest)).EnsureSuccessStatusCode();
                var responseContent = await result.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<Oauth2Token>(responseContent,
                    _jsonSettings);
                return token;
            }
            finally
            {
                Dispose(tokenRequest);
            }
        }

        public void Dispose()
        {
            Dispose(_httpClient);
        }

        private void Dispose(params IDisposable[] disposables)
        {
            if (disposables == null)
            {
                return;
            }

            foreach (var disposable in disposables.Where(d => d != null))
            {
                disposable.Dispose();
            }
        }
    }
}
