using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SEEK.AdPostingApi.Client.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client
{
    internal class OAuth2TokenClient : IOAuth2TokenClient
    {
        private readonly string _id;
        private readonly string _secret;
        private readonly Uri _tokenUri;
        private readonly HttpClient _httpClient;

        public OAuth2TokenClient(string id, string secret)
        {
            _id = id;
            _secret = secret;
            _tokenUri = new Uri("https://api.seek.com.au/auth/oauth2/token");
            _httpClient = new HttpClient();
            AccessToken = GetOAuth2TokenAsync().Result.AccessToken;
        }

        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public async Task<OAuth2Token> GetOAuth2TokenAsync()
        {
            using (var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _tokenUri))
            {
                var content = $"client_id={_id}&client_secret={_secret}&grant_type=client_credentials";

                tokenRequest.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

                using (HttpResponseMessage response = await _httpClient.SendAsync(tokenRequest))
                {
                    HandleBadResponse(response);

                    string responseContent = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<OAuth2Token>(responseContent, _jsonSettings);

                    return token;
                }
            }
        }

        private void HandleBadResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException("Could not get OAuth2 access token.");
            }

            response.EnsureSuccessStatusCode();
        }

        public string AccessToken { get; set; }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}