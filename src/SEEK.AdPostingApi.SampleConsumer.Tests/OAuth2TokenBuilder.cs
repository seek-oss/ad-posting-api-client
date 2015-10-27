using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    public class OAuth2TokenBuilder
    {
        private string _accessToken;

        public OAuth2TokenBuilder()
        {
            this.WithAccessToken("b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e");
        }

        public OAuth2TokenBuilder WithAccessToken(string accessToken)
        {
            _accessToken = accessToken;
            return this;
        }

        public OAuth2Token Build()
        {
            return new OAuth2Token
            {
                AccessToken = _accessToken,
                ExpiresIn = 3600,
                Scope = "seek",
                TokenType = "Bearer"
            };
        }
    }
}