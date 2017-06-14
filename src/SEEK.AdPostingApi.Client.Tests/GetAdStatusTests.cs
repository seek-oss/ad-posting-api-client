using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Tests.Framework;
using Xunit;

namespace SEEK.AdPostingApi.Client.Tests
{
    [Collection(AdPostingApiCollection.Name)]
    public class GetAdStatusTests : IDisposable
    {
        private const string AdvertisementLink = "/advertisement";
        private const string RequestId = "PactRequestId";

        public GetAdStatusTests(AdPostingApiPactService adPostingApiPactService)
        {
            this.Fixture = new AdPostingApiFixture(adPostingApiPactService);
        }

        public void Dispose()
        {
            this.Fixture.Dispose();
        }

        [Fact]
        public async Task GetExistingAdvertisementStatusUsingHalSelfLink()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.SetupPactForGettingExistingAdvertisementStatus(link, oAuth2Token);

            ProcessingStatus status;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                status = await client.GetAdvertisementStatusAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link));
            }

            Assert.Equal(ProcessingStatus.Completed, status);
        }

        [Fact]
        public async Task GetExistingAdvertisementStatusUsingHalTemplateWithAdvertisementId()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.SetupPactForGettingExistingAdvertisementStatus(link, oAuth2Token);

            ProcessingStatus status;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                status = await client.GetAdvertisementStatusAsync(new Guid(advertisementId));
            }

            Assert.Equal(ProcessingStatus.Completed, status);
        }

        private void SetupPactForGettingExistingAdvertisementStatus(string link, OAuth2Token oAuth2Token)
        {
            this.Fixture.MockProviderService
                .Given("There is a standout advertisement with maximum data")
                .UponReceiving("a HEAD advertisement request")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Head,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}"},
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", ResponseContentTypes.AdvertisementVersion1},
                        {"Processing-Status", "Completed"}
                    }
                });
        }

        [Fact]
        public async Task GetNonExistentAdvertisementStatus()
        {
            const string advertisementId = "9b650105-7434-473f-8293-4e23b7e0e064";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.MockProviderService
                .UponReceiving("a HEAD advertisement request for a non-existent advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Head,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 404,
                    Headers = new Dictionary<string, string> { { "X-Request-Id", RequestId } }
                });

            AdvertisementNotFoundException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<AdvertisementNotFoundException>(
                    async () => await client.GetAdvertisementStatusAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            actualException.ShouldBeEquivalentToException(new AdvertisementNotFoundException(RequestId));
        }

        [Fact]
        public async Task GetAdvertisementStatusUsingDisabledRequestorAccount()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_Disabled).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.MockProviderService
                .Given("There is a standout advertisement with maximum data")
                .UponReceiving("a HEAD advertisement request for an advertisement using a disabled requestor account")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Head,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 403,
                    Headers = new Dictionary<string, string> { { "X-Request-Id", RequestId } }
                });

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.GetAdvertisementStatusAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    RequestId,
                    403,
                    $"[HEAD] {this.Fixture.AdPostingApiServiceBaseUri}advertisement/{advertisementId} is not authorized."));
        }

        [Fact]
        public async Task GetAdvertisementStatusWhereAdvertiserNotRelatedToRequestor()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.OtherThirdPartyUploader).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.MockProviderService
                .Given("There is a standout advertisement with maximum data")
                .UponReceiving("a HEAD advertisement request for an advertisement of an advertiser not related to the requestor's account")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Head,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string> { { "X-Request-Id", RequestId } }
                    });

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.GetAdvertisementStatusAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    RequestId,
                    403,
                    $"[HEAD] {this.Fixture.AdPostingApiServiceBaseUri}advertisement/{advertisementId} is not authorized."));
        }

        private AdPostingApiFixture Fixture { get; }
    }
}