using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client.Models;
using Xunit;

namespace SEEK.AdPostingApi.Client.Tests
{
    [Collection(AdPostingApiCollection.Name)]
    public class GetAdStatusTests : IDisposable
    {
        private const string AdvertisementLink = "/advertisement";

        public GetAdStatusTests(AdPostingApiPactService adPostingApiPactService)
        {
            this.Fixture = new AdPostingApiFixture(adPostingApiPactService);
        }

        public void Dispose()
        {
            this.Fixture.Dispose();
        }

        [Fact]
        public async Task GetExistingAdvertisementStatus()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("HEAD advertisement request")
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

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                status = await client.GetAdvertisementStatusAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link));
            }

            Assert.Equal(ProcessingStatus.Pending, status);
        }

        [Fact]
        public async Task GetNonExistentAdvertisementStatus()
        {
            const string advertisementId = "9b650105-7434-473f-8293-4e23b7e0e064";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_Disabled).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .UponReceiving("HEAD advertisement request for a non-existent advertisement")
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

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<AdvertisementNotFoundException>(
                    async () => await client.GetAdvertisementStatusAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            actualException.ShouldBeEquivalentToException(new AdvertisementNotFoundException());
        }

        [Fact]
        public async Task GetAdvertisementStatusUsingDisabledRequestorAccount()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_Disabled).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("HEAD advertisement request for an advertisement using a disabled requestor account")
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

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.GetAdvertisementStatusAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException($"[HEAD] {this.Fixture.AdPostingApiServiceBaseUri}advertisement/{advertisementId} is not authorized."));
        }

        [Fact]
        public async Task GetAdvertisementStatusWhereAdvertiserNotRelatedToRequestor()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.OtherThirdPartyUploader).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("HEAD advertisement request for an advertisement of an advertiser not related to the requestor's account")
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
                .WillRespondWith(new ProviderServiceResponse { Status = 403 });

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.GetAdvertisementStatusAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException($"[HEAD] {this.Fixture.AdPostingApiServiceBaseUri}advertisement/{advertisementId} is not authorized."));
        }

        private AdPostingApiFixture Fixture { get; }
    }
}