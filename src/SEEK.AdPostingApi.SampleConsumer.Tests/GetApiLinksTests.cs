using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
        public void GetApiLinksWithInvalidAccessToken()
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

            PactProvider.MockService
                .UponReceiving(
                    "a request to retrieve API links with a valid access token that is not authorised for use with the Ad Posting API")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/",
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + AccessTokens.ValidAccessToken_InvalidForApi},
                        {"Accept", "application/hal+json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse { Status = (int)HttpStatusCode.Unauthorized });

            UnauthorizedException actualException, expectedException;

            using (var oauthClient = new FakeOAuth2Client())
            {
                var client = new AdPostingApiClient(PactProvider.MockServiceUri, oauthClient);

                expectedException = new UnauthorizedException($"[GET] {PactProvider.MockServiceUri} is not authorized.");
                actualException = Assert.Throws<UnauthorizedException>(async () => await client.GetAllAdvertisementsAsync());
            }

            actualException.ShouldBeEquivalentToException(expectedException);
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

                return Task.FromResult(new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_InvalidForApi).Build());
            }
        }
    }
}