using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SEEK.AdPostingApi.Client.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client
{
    internal class SeekOAuth2TokenClient : ISeekOAuth2TokenClient
    {
        private readonly Uri _tokenUri;
        private readonly HttpClient _httpClient;

        public SeekOAuth2TokenClient()
        {
            _tokenUri = new Uri("https://api.seek.com.au/auth/oauth2/token");
            _httpClient = new HttpClient();
        }

        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public async Task<OAuth2Token> GetOAuth2TokenAsync(string id, string secret)
        {
            using (var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _tokenUri))
            {
                var content = string.Format("client_id={0}&client_secret={1}&grant_type=client_credentials", id, secret);

                tokenRequest.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

                using (HttpResponseMessage result = (await _httpClient.SendAsync(tokenRequest)).EnsureSuccessStatusCode())
                {
                    string responseContent = await result.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<OAuth2Token>(responseContent, _jsonSettings);

                    return token;
                }
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}