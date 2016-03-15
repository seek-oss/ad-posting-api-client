using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    [TestFixture]
    public class GetApiLinksTests
    {
        [SetUp]
        public void TestInitialize()
        {
            PactProvider.ClearInteractions();
        }

        [TearDown]
        public void TestCleanup()
        {
            PactProvider.VerifyInteractions();
        }

        [Test]
        public async Task GetApiLinksWithInvalidAccessTokenTriggersTokenRenewal()
        {
            PactProvider.MockService
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

            PactProvider.RegisterIndexPageInteractions(new OAuth2TokenBuilder().Build());

            using (var fakeOAuth2Client = new FakeOAuth2Client())
            {
                using (var client = new AdPostingApiClient(PactProvider.MockServiceUri, fakeOAuth2Client))
                {
                    await client.InitialiseIndexResource(PactProvider.MockServiceUri);
                }
            }
        }

        [Test]
        public void GetApiLinksNotPermitted()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_InvalidService).Build();

            PactProvider.MockService
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

            using (var client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<UnauthorizedException>(async () => await client.InitialiseIndexResource(PactProvider.MockServiceUri));
            }

            actualException.ShouldBeEquivalentToException(new UnauthorizedException($"[GET] {PactProvider.MockServiceUri} is not authorized."));
        }

        private AdPostingApiClient GetClient(OAuth2Token token)
        {
            var oAuthClient = Mock.Of<IOAuth2TokenClient>(c => c.GetOAuth2TokenAsync() == Task.FromResult(token));

            return new AdPostingApiClient(PactProvider.MockServiceUri, oAuthClient);
        }
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