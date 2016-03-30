using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    public class OAuth2TokenBuilder
    {
        private string _accessToken;

        public OAuth2TokenBuilder()
        {
            this.WithAccessToken(AccessTokens.ValidAccessToken);
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