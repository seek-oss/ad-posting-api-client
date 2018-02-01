using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Tests.Framework
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
            this._accessToken = accessToken;
            return this;
        }

        public OAuth2Token Build()
        {
            return new OAuth2Token
            {
                AccessToken = this._accessToken,
                ExpiresIn = 3600,
                Scope = "seek",
                TokenType = "Bearer"
            };
        }
    }
}