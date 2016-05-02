using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using Xunit;

namespace SEEK.AdPostingApi.Client.Tests
{
    [Collection(AdPostingApiCollection.Name)]
    public class ExpireAdTests : IDisposable
    {
        private const string AdvertisementLink = "/advertisement";
        private const string AdvertisementErrorContentType = "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8";
        private const string PatchContentType = "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8";

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
            var advertisementId = new Guid("8e2fde50-bc5f-4a12-9cfb-812e50500184");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("An expire request for advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", PatchContentType}
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
                            { "Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8" }
                        },
                        Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                            .WithAgentId(null)
                            .WithState(AdvertisementState.Expired.ToString())
                            .WithAdditionalProperties(AdditionalPropertyType.ResidentsOnly.ToString(), AdditionalPropertyType.Graduate.ToString())
                            .WithResponseLink("self", link)
                            .WithResponseLink("view", viewRenderedAdvertisementLink)
                            .Build()
                    });

            AdvertisementResource result;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                result = await client.ExpireAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link));
            }

            var expectedResult = new AdvertisementResource
            {
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = link } },
                    { "view", new Link { Href = viewRenderedAdvertisementLink } }
                }
            };

            new AdvertisementModelBuilder(AllFieldsInitializer, expectedResult).WithAgentId(null).Build();

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
                .UponReceiving("An expire request for advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", PatchContentType}
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
                        Status = 422,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", AdvertisementErrorContentType }
                        },
                        Body = new
                        {
                            message = "Validation Failure",
                            errors = new[] {
                                new { code = "InvalidState", message = "Advertisement has already expired." }
                            }
                        }
                    });

            ValidationException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<ValidationException>(
                    async () => await client.ExpireAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            var expectedException = new ValidationException(
                new HttpMethod("PATCH"),
                new ValidationMessage
                {
                    Message = "Validation Failure",
                    Errors = new[] { new ValidationData { Code = "InvalidState", Message = "Advertisement has already expired." } }
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
                .UponReceiving("An expire request for a non-existent advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", PatchContentType}
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
        public async Task ExpireAdvertisementWithDisabledThirdPartyUploader()
        {
            var advertisementId = new Guid("8e2fde50-bc5f-4a12-9cfb-812e50500184");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_Disabled).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a request to expire a job with a disabled third party uploader")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", PatchContentType}
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
        public async Task ExpireAdvertisementWhereAdvertiserNotRelatedToThirdPartyUploader()
        {
            var advertisementId = new Guid("8e2fde50-bc5f-4a12-9cfb-812e50500184");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.OtherThirdPartyUploader).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a request to expire a job for an advertiser not related to the third party uploader")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", PatchContentType}
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