using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;
using Xunit;

namespace SEEK.AdPostingApi.Client.Tests
{
    [Collection(AdPostingApiCollection.Name)]
    public class GetApiLinksTests : IDisposable
    {
        public GetApiLinksTests(AdPostingApiPactService adPostingApiPactService)
        {
            this.Fixture = new AdPostingApiFixture(adPostingApiPactService);
        }

        public void Dispose()
        {
            this.Fixture.Dispose();
        }

        [Fact]
        public async Task GetApiLinksWithInvalidAccessTokenTriggersTokenRenewal()
        {
            this.Fixture.AdPostingApiService
                .UponReceiving("a request to retrieve API links with an invalid access token")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/",
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + AccessTokens.InvalidAccessToken},
                        {"Accept", "application/hal+json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Headers = new Dictionary<string, string>
                    {
                        {"WWW-Authenticate", "Bearer error=\"Invalid request\""}
                    }
                });

            this.Fixture.RegisterIndexPageInteractions(new OAuth2TokenBuilder().Build());

            using (var fakeOAuth2Client = new FakeOAuth2Client())
            {
                using (var client = new AdPostingApiClient(this.Fixture.AdPostingApiServiceBaseUri, fakeOAuth2Client))
                {
                    await client.InitialiseIndexResource(this.Fixture.AdPostingApiServiceBaseUri);
                }
            }
        }

        [Fact]
        public async Task GetApiLinksNotPermitted()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_InvalidService).Build();

            this.Fixture.AdPostingApiService
                .UponReceiving("unauthorised request to retrieve API links")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/",
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + AccessTokens.ValidAccessToken_InvalidService}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = (int)HttpStatusCode.Unauthorized
                });

            UnauthorizedException actualException;

            using (var client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(async () => await client.InitialiseIndexResource(this.Fixture.AdPostingApiServiceBaseUri));
            }

            actualException.ShouldBeEquivalentToException(new UnauthorizedException($"[GET] {this.Fixture.AdPostingApiServiceBaseUri} is not authorized."));
        }

        private AdPostingApiFixture Fixture { get; }
    }

    public class FakeOAuth2Client : IOAuth2TokenClient
    {
        private bool _neverCalled = true;

        public void Dispose()
        {
        }

        public Task<OAuth2Token> GetOAuth2TokenAsync()
        {
            if (_neverCalled)
            {
                _neverCalled = false;

                return Task.FromResult(new OAuth2TokenBuilder().WithAccessToken(AccessTokens.InvalidAccessToken).Build());
            }

            return Task.FromResult(new OAuth2TokenBuilder().Build());
        }
    }
}