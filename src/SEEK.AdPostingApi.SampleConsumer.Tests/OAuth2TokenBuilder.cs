using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    public class OAuth2TokenBuilder
    {
        public OAuth2Token Build()
        {
            return new OAuth2Token
            {
                AccessToken = "b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
                ExpiresIn = 3600,
                Scope = "seek",
                TokenType = "Bearer"
            };
        }
    }
}