using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    [TestFixture]
    public class GetAdStatusTests
    {
        private const string AdvertisementLink = "/advertisement";

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
        public async Task GetExistingAdvertisementStatus()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There is a pending standout advertisement with maximum data and id '{advertisementId}'")
                .UponReceiving("HEAD request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Head,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", "application/vnd.seek.advertisement+json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8" },
                        { "Processing-Status", "Pending" }
                    }
                });

            ProcessingStatus status;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                status = await client.GetAdvertisementStatusAsync(new Uri(PactProvider.MockServiceUri, link));
            }

            Assert.AreEqual(ProcessingStatus.Pending, status);
        }

        [Test]
        public void GetNonExistentAdvertisementStatus()
        {
            const string advertisementId = "9b650105-7434-473f-8293-4e23b7e0e064";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_InvalidForAdvertiserId).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .UponReceiving("HEAD request for a non-existent advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Head,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", "application/vnd.seek.advertisement+json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 404
                });

            AdvertisementNotFoundException actualException;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<AdvertisementNotFoundException>(
                    async () => await client.GetAdvertisementStatusAsync(new Uri(PactProvider.MockServiceUri, link)));
            }

            actualException.ShouldBeEquivalentToException(new AdvertisementNotFoundException());
        }

        [Test]
        public void GetExistingAdvertisementNotPermitted()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_InvalidForAdvertiserId).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There is a pending standout advertisement with maximum data and id '{advertisementId}'")
                .UponReceiving("Unauthorised HEAD request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Head,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", "application/vnd.seek.advertisement+json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 403
                });

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<UnauthorizedException>(
                    async () => await client.GetAdvertisementStatusAsync(new Uri(PactProvider.MockServiceUri, link)));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException($"[HEAD] {PactProvider.MockServiceUri}advertisement/{advertisementId} is not authorized."));
        }

        private AdPostingApiClient GetClient(OAuth2Token token)
        {
            var oAuthClient = Mock.Of<IOAuth2TokenClient>(c => c.GetOAuth2TokenAsync() == Task.FromResult(token));

            return new AdPostingApiClient(PactProvider.MockServiceUri, oAuthClient);
        }
    }
}