using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using Xunit;

namespace SEEK.AdPostingApi.Client.Tests
{
    [Collection(AdPostingApiCollection.Name)]
    public class ExpireAdTests : IDisposable
    {
        private const string AdvertisementLink = "/advertisement";
        private const string AdvertisementContentType = "application/vnd.seek.advertisement+json; version=1; charset=utf-8";
        private const string AdvertisementErrorContentType = "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8";
        private const string AdvertisementPatchContentType = "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8";

        private IBuilderInitializer AllFieldsInitializer => new AllFieldsInitializer();

        public ExpireAdTests(AdPostingApiPactService adPostingApiPactService)
        {
            this.Fixture = new AdPostingApiFixture(adPostingApiPactService);
        }

        public void Dispose()
        {
            this.Fixture.Dispose();
        }

        [Fact]
        public async Task ExpireAdvertisement()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            string link = $"{AdvertisementLink}/{advertisementId}";
            string viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";
            DateTime expiryDate = new DateTime(2015, 10, 7, 21, 19, 00, DateTimeKind.Utc);

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a PATCH advertisement request to expire an advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", AdvertisementPatchContentType}
                        },
                        Body = new[]
                        {
                            new
                            {
                                op = "replace",
                                path = "state",
                                value = AdvertisementState.Expired.ToString()
                            }
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 202,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", AdvertisementContentType }
                        },
                        Body = new AdvertisementResponseContentBuilder(AllFieldsInitializer)
                            .WithState(AdvertisementState.Expired.ToString())
                            .WithLink("self", link)
                            .WithLink("view", viewRenderedAdvertisementLink)
                            .WithExpiryDate(expiryDate)
                            .WithAgentId(null)
                            .WithAdditionalProperties(AdditionalPropertyType.ResidentsOnly.ToString(), AdditionalPropertyType.Graduate.ToString())
                            .Build()
                    });

            AdvertisementResource result;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                result = await client.ExpireAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link));
            }

            AdvertisementResource expectedResult = new AdvertisementResourceBuilder(AllFieldsInitializer)
                .WithLinks(advertisementId)
                .WithState(AdvertisementState.Expired)
                .WithExpiryDate(expiryDate)
                .WithAgentId(null)
                .Build();

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ExpireAlreadyExpiredAdvertisement()
        {
            var advertisementId = new Guid("c294088d-ff50-4374-bc38-7fa805790e3e");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is an expired advertisement")
                .UponReceiving("a PATCH advertisement request to expire an advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", AdvertisementPatchContentType}
                        },
                        Body = new[]
                        {
                            new
                            {
                                op = "replace",
                                path = "state",
                                value = AdvertisementState.Expired.ToString()
                            }
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", AdvertisementErrorContentType }
                        },
                        Body = new
                        {
                            message = "Forbidden",
                            errors = new[] {
                                new { code = "AlreadyExpired", message = "Advertisement has already expired." }
                            }
                        }
                    });

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.ExpireAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            var expectedException = new UnauthorizedException(
                new ForbiddenMessage
                {
                    Message = "Forbidden",
                    Errors = new[] { new ForbiddenMessageData { Code = "AlreadyExpired" } }
                });

            actualException.ShouldBeEquivalentToException(expectedException);
        }

        [Fact]
        public async Task ExpireNonExistentAdvertisment()
        {
            var advertisementId = new Guid("9b650105-7434-473f-8293-4e23b7e0e064");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .UponReceiving("a PATCH advertisement request to expire a non-existent advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", AdvertisementPatchContentType}
                        },
                        Body = new[]
                        {
                            new
                            {
                                op = "replace",
                                path = "state",
                                value = AdvertisementState.Expired.ToString()
                            }
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 404
                    });

            AdvertisementNotFoundException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<AdvertisementNotFoundException>(
                    async () => await client.ExpireAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            actualException.ShouldBeEquivalentToException(new AdvertisementNotFoundException());
        }

        [Fact]
        public async Task ExpireAdvertisementUsingDisabledRequestorAccount()
        {
            var advertisementId = new Guid("8e2fde50-bc5f-4a12-9cfb-812e50500184");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_Disabled).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a PATCH advertisement request to expire a job using a disabled requestor account")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", AdvertisementPatchContentType}
                        },
                        Body = new[]
                        {
                            new
                            {
                                op = "replace",
                                path = "state",
                                value = AdvertisementState.Expired.ToString()
                            }
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", AdvertisementErrorContentType }
                        },
                        Body = new
                        {
                            message = "Forbidden",
                            errors = new[]
                            {
                                new { code = "AccountError" }
                            }
                        }
                    });

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.ExpireAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    new ForbiddenMessage
                    {
                        Message = "Forbidden",
                        Errors = new[] { new ForbiddenMessageData { Code = "AccountError" } }
                    }));
        }

        [Fact]
        public async Task ExpireAdvertisementWhereAdvertiserNotRelatedToRequestor()
        {
            var advertisementId = new Guid("8e2fde50-bc5f-4a12-9cfb-812e50500184");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.OtherThirdPartyUploader).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a PATCH advertisement request to expire a job for an advertiser not related to the requestor's account")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", AdvertisementPatchContentType}
                        },
                        Body = new[]
                        {
                            new
                            {
                                op = "replace",
                                path = "state",
                                value = AdvertisementState.Expired.ToString()
                            }
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", AdvertisementErrorContentType }
                        },
                        Body = new
                        {
                            message = "Forbidden",
                            errors = new[]
                            {
                                new { code = "RelationshipError" }
                            }
                        }
                    });

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.ExpireAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    new ForbiddenMessage
                    {
                        Message = "Forbidden",
                        Errors = new[] { new ForbiddenMessageData { Code = "RelationshipError" } }
                    }));
        }

        private AdPostingApiFixture Fixture { get; }
    }
}