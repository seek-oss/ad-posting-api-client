using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using Xunit;

namespace SEEK.AdPostingApi.Client.Tests
{
    [Collection(AdPostingApiCollection.Name)]
    public class UpdateAdTests : IDisposable
    {
        private const string AdvertisementLink = "/advertisement";
        private const string AdvertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

        private IBuilderInitializer MinimumFieldsInitializer => new MinimumFieldsInitializer();

        private IBuilderInitializer AllFieldsInitializer => new AllFieldsInitializer();

        public UpdateAdTests(AdPostingApiPactService adPostingApiPactService)
        {
            this.Fixture = new AdPostingApiFixture(adPostingApiPactService);
        }

        public void Dispose()
        {
            this.Fixture.Dispose();
        }

        [Fact]
        public async Task UpdateExistingAdvertisement()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{AdvertisementId}";
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{AdvertisementId}/view";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a PUT advertisement request")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Put,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                    },
                    Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                        .WithAgentId(null)
                        .WithAgentJobReference(null)
                        .WithJobTitle("Exciting Senior Developer role in a great CBD location. Great $$$ - updated")
                        .WithVideoUrl("https://www.youtube.com/v/dVDk7PXNXB8")
                        .WithApplicationFormUrl("http://FakeATS.com.au")
                        .WithEndApplicationUrl("http://endform.com/updated")
                        .WithStandoutBullets("new Uzi", "new Remington Model", "new AK-47")
                        .Build()
                })
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 202,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8"}
                        },
                        Body = new AdvertisementResponseContentBuilder(AllFieldsInitializer)
                            .WithState(AdvertisementState.Open.ToString())
                            .WithLink("self", link)
                            .WithLink("view", viewRenderedAdvertisementLink)
                            .WithAgentId(null)
                            .WithAgentJobReference(null)
                            .WithJobTitle("Exciting Senior Developer role in a great CBD location. Great $$$ - updated")
                            .WithVideoUrl("https://www.youtube.com/v/dVDk7PXNXB8")
                            .WithApplicationFormUrl("http://FakeATS.com.au")
                            .WithEndApplicationUrl("http://endform.com/updated")
                            .WithStandoutBullets("new Uzi", "new Remington Model", "new AK-47")
                            .Build()
                    });

            Advertisement requestModel = this.SetupModelForExistingAdvertisement(new AdvertisementModelBuilder(AllFieldsInitializer)).Build();
            AdvertisementResource result;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                result = await client.UpdateAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link), requestModel);
            }

            AdvertisementResource expectedResult = this.SetupModelForExistingAdvertisement(
                new AdvertisementResourceBuilder(AllFieldsInitializer).WithLinks(AdvertisementId)).Build();

            result.ShouldBeEquivalentTo(expectedResult);
        }

        private AdvertisementModelBuilder<TAdvertisement> SetupModelForExistingAdvertisement<TAdvertisement>(
            AdvertisementModelBuilder<TAdvertisement> builder) where TAdvertisement : Advertisement, new()
        {
            return builder
                .WithAgentId(null)
                .WithAgentJobReference(null)
                .WithJobTitle("Exciting Senior Developer role in a great CBD location. Great $$$ - updated")
                .WithVideoUrl("https://www.youtube.com/v/dVDk7PXNXB8")
                .WithApplicationFormUrl("http://FakeATS.com.au")
                .WithEndApplicationUrl("http://endform.com/updated")
                .WithStandoutBullets("new Uzi", "new Remington Model", "new AK-47");
        }

        [Fact]
        public async Task UpdateNonExistentAdvertisement()
        {
            const string advertisementId = "9b650105-7434-473f-8293-4e23b7e0e064";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .UponReceiving("a PUT advertisement request for a non-existent advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Put,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                    },
                    Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                        .WithAdvertisementDetails("This advertisement should not exist.")
                        .Build()
                })
                .WillRespondWith(new ProviderServiceResponse { Status = 404 });

            AdvertisementNotFoundException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<AdvertisementNotFoundException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link),
                        new AdvertisementModelBuilder(MinimumFieldsInitializer)
                            .WithAdvertisementDetails("This advertisement should not exist.")
                            .Build()));
            }

            actualException.ShouldBeEquivalentToException(new AdvertisementNotFoundException());
        }

        [Fact]
        public async Task UpdateWithBadAdvertisementData()
        {
            const string advertisementId = "7e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .UponReceiving("a PUT advertisement request for advertisement with bad data")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Put,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                    },
                    Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                        .WithSalaryMinimum(-1.0)
                        .WithVideoUrl("htp://www.youtube.com/v/abc")
                        .WithAdvertisementType(AdvertisementType.StandOut.ToString())
                        .WithVideoPosition(VideoPosition.Below.ToString())
                        .WithStandoutBullets("new Uzi", "new Remington Model".PadRight(85, '!'), "new AK-47")
                        .WithApplicationEmail("someone(at)some.domain")
                        .WithApplicationFormUrl("htp://somecompany.domain/apply")
                        .WithTemplateItems(
                            new KeyValuePair<object, object>("Template Line 1", "Template Value 1"),
                            new KeyValuePair<object, object>("", "value2"))
                        .Build()
                })
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 422,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8" }
                        },
                        Body = new
                        {
                            message = "Validation Failure",
                            errors = new[]
                            {
                                new { field = "applicationEmail", code = "InvalidEmailAddress" },
                                new { field = "applicationFormUrl", code = "InvalidUrl" },
                                new { field = "salary.minimum", code = "ValueOutOfRange" },
                                new { field = "standout.bullets[1]", code = "MaxLengthExceeded" },
                                new { field = "template.items[1].name", code = "Required" },
                                new { field = "video.url", code = "RegexPatternNotMatched" }
                            }
                        }
                    });

            ValidationException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<ValidationException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link),
                        new AdvertisementModelBuilder(MinimumFieldsInitializer)
                            .WithAdvertisementType(AdvertisementType.StandOut)
                            .WithSalaryMinimum(-1)
                            .WithVideoUrl("htp://www.youtube.com/v/abc")
                            .WithVideoPosition(VideoPosition.Below)
                            .WithStandoutBullets("new Uzi", "new Remington Model".PadRight(85, '!'), "new AK-47")
                            .WithApplicationEmail("someone(at)some.domain")
                            .WithApplicationFormUrl("htp://somecompany.domain/apply")
                            .WithTemplateItems(
                                new TemplateItem { Name = "Template Line 1", Value = "Template Value 1" },
                                new TemplateItem { Name = "", Value = "value2" })
                            .Build()));
            }

            var expectedException = new ValidationException(
                HttpMethod.Put,
                new ValidationMessage
                {
                    Message = "Validation Failure",
                    Errors = new[]
                    {
                        new ValidationData { Field = "applicationEmail", Code = "InvalidEmailAddress" },
                        new ValidationData { Field = "applicationFormUrl", Code = "InvalidUrl" },
                        new ValidationData { Field = "salary.minimum", Code = "ValueOutOfRange" },
                        new ValidationData { Field = "standout.bullets[1]", Code = "MaxLengthExceeded" },
                        new ValidationData { Field = "template.items[1].name", Code = "Required" },
                        new ValidationData { Field = "video.url", Code = "RegexPatternNotMatched" }
                    }
                });

            actualException.ShouldBeEquivalentToException(expectedException);
        }

        [Fact]
        public async Task UpdateWithInvalidSalaryData()
        {
            const string advertisementId = "7e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .UponReceiving("a PUT advertisement request for advertisement with invalid salary data")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Put,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                    },
                    Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                        .WithSalaryMinimum(2.0)
                        .WithSalaryMaximum(1.0)
                        .Build()
                })
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 422,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8" }
                        },
                        Body = new
                        {
                            message = "Validation Failure",
                            errors = new[]
                            {
                                new { field = "salary.maximum", code = "InvalidValue" }
                            }
                        }
                    });

            ValidationException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<ValidationException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link),
                        new AdvertisementModelBuilder(MinimumFieldsInitializer)
                            .WithSalaryMinimum(2)
                            .WithSalaryMaximum(1)
                            .Build()));
            }

            var expectedException = new ValidationException(
                HttpMethod.Put,
                new ValidationMessage
                {
                    Message = "Validation Failure",
                    Errors = new[]
                    {
                        new ValidationData { Field = "salary.maximum", Code = "InvalidValue" }
                    }
                });

            actualException.ShouldBeEquivalentToException(expectedException);
        }

        [Fact]
        public async Task UpdateAdWithADifferentAdvertiserToTheOneOwningTheJob()
        {
            var oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{AdvertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a PUT advertisement request to update a job ad with a different advertiser from the one owning the job")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Put,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                        },
                        Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                            .WithAdvertiserId("99887766")
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8" }
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

            var requestModel = new AdvertisementModelBuilder(AllFieldsInitializer)
                .WithAdvertiserId("99887766")
                .Build();

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link), requestModel));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    new ForbiddenMessage
                    {
                        Message = "Forbidden",
                        Errors = new[] { new ForbiddenMessageData { Code = "InvalidValue" } }
                    }
                    ));
        }

        [Fact]
        public async Task UpdateAdUsingDisabledRequestorAccount()
        {
            var oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_Disabled).Build();
            var link = $"{AdvertisementLink}/{AdvertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a PUT advertisement request to update a job using a disabled requestor account")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Put,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                        },
                        Body = new AdvertisementContentBuilder(AllFieldsInitializer).Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8" }
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

            var requestModel = new AdvertisementModelBuilder(AllFieldsInitializer).Build();

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link), requestModel));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    new ForbiddenMessage
                    {
                        Message = "Forbidden",
                        Errors = new[] { new ForbiddenMessageData { Code = "AccountError" } }
                    }
                    ));
        }

        [Fact]
        public async Task UpdateAdWhereAdvertiserNotRelatedToRequestor()
        {
            var oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.OtherThirdPartyUploader).Build();
            var link = $"{AdvertisementLink}/{AdvertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a PUT advertisement request to update a job for an advertiser not related to the requestor's account")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Put,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                        },
                        Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8" }
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

            var requestModel = new AdvertisementModelBuilder(AllFieldsInitializer).Build();

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link), requestModel));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    new ForbiddenMessage
                    {
                        Message = "Forbidden",
                        Errors = new[] { new ForbiddenMessageData { Code = "RelationshipError" } }
                    }
                    ));
        }

        [Fact]
        public async Task UpdateStandoutJobToClassic()
        {
            var oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{AdvertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a PUT advertisement request to update a standout job to classic")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Put,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                        },
                        Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                            .WithAdvertisementType(AdvertisementType.Classic.ToString())
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 422,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8" }
                        },
                        Body = new
                        {
                            message = "Validation Failure",
                            errors = new[]
                            {
                                new { field = "advertisementType", code = "ChangeNotAllowed" }
                            }
                        }
                    });

            var requestModel = new AdvertisementModelBuilder(MinimumFieldsInitializer).WithAdvertisementType(AdvertisementType.Classic).Build();

            ValidationException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<ValidationException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link), requestModel));
            }

            var expectedException = new ValidationException(
                HttpMethod.Put,
                new ValidationMessage
                {
                    Message = "Validation Failure",
                    Errors = new[] { new ValidationData { Field = "advertisementType", Code = "ChangeNotAllowed" } }
                }
            );

            actualException.ShouldBeEquivalentToException(expectedException);
        }

        private AdPostingApiFixture Fixture { get; }
    }
}