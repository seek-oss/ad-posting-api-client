using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    [TestFixture]
    public class UpdateAdTests
    {
        private const string AdvertisementLink = "/advertisement";
        private const string AdvertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

        private IBuilderInitializer MinimumFieldsInitializer => new MinimumFieldsInitializer();

        private IBuilderInitializer AllFieldsInitializer => new AllFieldsInitializer();

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
        public async Task UpdateExistingAdvertisement()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{AdvertisementId}";
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{AdvertisementId}/view";

            PactProvider.MockService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("Update request for advertisement")
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
                    Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                        .WithAgentId(null)
                        .WithJobTitle("Exciting Senior Developer role in a great CBD location. Great $$$ - updated")
                        .WithVideoUrl("https://www.youtube.com/v/dVDk7PXNXB8")
                        .WithApplicationFormUrl("http://FakeATS.com.au")
                        .WithEndApplicationUrl("http://endform.com/updated")
                        .WithStandoutBullets("new Uzi", "new Remington Model", "new AK-47")
                        .WithResponseLink("self", link)
                        .WithResponseLink("view", viewRenderedAdvertisementLink)
                        .Build()
                });

            AdvertisementResource expectedResult = new AdvertisementResource();

            new AdvertisementModelBuilder(AllFieldsInitializer, expectedResult)
                .WithAgentId(null)
                .WithJobTitle("Exciting Senior Developer role in a great CBD location. Great $$$ - updated")
                .WithVideoUrl("https://www.youtube.com/v/dVDk7PXNXB8")
                .WithApplicationFormUrl("http://FakeATS.com.au")
                .WithEndApplicationUrl("http://endform.com/updated")
                .WithStandoutBullets("new Uzi", "new Remington Model", "new AK-47")
                .Build();

            AdvertisementResource result;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                result = await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), expectedResult);
            }

            expectedResult.Links = new Links(PactProvider.MockServiceUri)
            {
                { "self", new Link { Href = link } },
                { "view", new Link { Href = viewRenderedAdvertisementLink } }
            };

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateNonExistentAdvertisement()
        {
            const string advertisementId = "9b650105-7434-473f-8293-4e23b7e0e064";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .UponReceiving("Update request for a non-existent advertisement")
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

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<AdvertisementNotFoundException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link),
                        new AdvertisementModelBuilder(MinimumFieldsInitializer)
                            .WithAdvertisementDetails("This advertisement should not exist.")
                            .Build()));
            }

            actualException.ShouldBeEquivalentToException(new AdvertisementNotFoundException());
        }

        [Test]
        public void UpdateWithBadAdvertisementData()
        {
            const string advertisementId = "7e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .UponReceiving("Update request for advertisement with bad data")
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
                        .WithAdvertisementType(AdvertisementType.StandOut.ToString())
                        .WithVideoUrl("htp://www.youtube.com/v/abc".PadRight(260, '!'))
                        .WithVideoPosition(VideoPosition.Below.ToString())
                        .WithStandoutBullets("new Uzi", "new Remington Model".PadRight(85, '!'), "new AK-47")
                        .WithApplicationEmail("someone(at)some.domain")
                        .WithApplicationFormUrl("htp://somecompany.domain/apply")
                        .WithTemplateItems(
                            new KeyValuePair<object, object>("Template Line 1", "Template Value 1"),
                            new KeyValuePair<object, object>("", "value2".PadRight(3010, '!')))
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
                                new { field = "template.items[1].value", code = "MaxLengthExceeded" },
                                new { field = "video.url", code = "MaxLengthExceeded" },
                                new { field = "video.url", code = "RegexPatternNotMatched" }
                            }
                        }
                    });

            ValidationException actualException;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<ValidationException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link),
                        new AdvertisementModelBuilder(MinimumFieldsInitializer)
                            .WithAdvertisementType(AdvertisementType.StandOut)
                            .WithSalaryMinimum(-1)
                            .WithVideoUrl("htp://www.youtube.com/v/abc".PadRight(260, '!'))
                            .WithVideoPosition(VideoPosition.Below)
                            .WithStandoutBullets("new Uzi", "new Remington Model".PadRight(85, '!'), "new AK-47")
                            .WithApplicationEmail("someone(at)some.domain")
                            .WithApplicationFormUrl("htp://somecompany.domain/apply")
                            .WithTemplateItems(
                                new TemplateItemModel { Name = "Template Line 1", Value = "Template Value 1" },
                                new TemplateItemModel { Name = "", Value = "value2".PadRight(3010, '!') })
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
                        new ValidationData { Field = "template.items[1].value", Code = "MaxLengthExceeded" },
                        new ValidationData { Field = "video.url", Code = "MaxLengthExceeded" },
                        new ValidationData { Field = "video.url", Code = "RegexPatternNotMatched" }
                    }
                });

            actualException.ShouldBeEquivalentToException(expectedException);
        }

        [Test]
        [Ignore("To be implemented")]
        public void UpdateExistingAdvertisementNotPermitted()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_Disabled).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("Unauthorised update request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Put,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                    },
                    Body = new AdvertisementContentBuilder(AllFieldsInitializer).Build()
                })
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/json; charset=utf-8" }
                        },
                        Body = new { message = "Operation not permitted on advertisement with advertiser id: '9012'" }
                    });

            var requestModel = new AdvertisementModelBuilder(AllFieldsInitializer).Build();

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<UnauthorizedException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), requestModel));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException("Operation not permitted on advertisement with advertiser id: '9012'"));
        }

        [Test]
        public void UpdateAdWithADifferentAdvertiserToTheOneOwningTheJob()
        {
            var oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{AdvertisementId}";

            PactProvider.MockService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a request to update a job ad with a different advertiser from the one owning the job")
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

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<UnauthorizedException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), requestModel));
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

        [Test]
        public void UpdateAdWithArchivedThirdPartyUploader()
        {
            var oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ArchivedThirdPartyUploader).Build();
            var link = $"{AdvertisementLink}/{AdvertisementId}";

            PactProvider.MockService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a request to update a job with an archived third party uploader")
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

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<UnauthorizedException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), requestModel));
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

        [Test]
        public void UpdateAdWhereAdvertiserNotRelatedToThirdPartyUploader()
        {
            var oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.OtherThirdPartyUploader).Build();
            var link = $"{AdvertisementLink}/{AdvertisementId}";

            PactProvider.MockService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a request to update a job for an advertiser not related to the third party uploader")
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

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<UnauthorizedException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), requestModel));
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

        [Test]
        public void UpdateAgentAdWithEmptyAgentId()
        {
            var oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAgentAccessToken).Build();
            var link = $"{AdvertisementLink}/{AdvertisementId}";

            PactProvider.MockService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a request to update an agent job where the agent id is not supplied")
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
                            .WithAgentId("")
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
                                new { code = "Required" }
                            }
                        }
                    });

            var requestModel = new AdvertisementModelBuilder(AllFieldsInitializer).WithAgentId("").Build();

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<UnauthorizedException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), requestModel));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    new ForbiddenMessage
                    {
                        Message = "Forbidden",
                        Errors = new[] { new ForbiddenMessageData { Code = "Required" } }
                    }
                    ));
        }

        [Test]
        public void UpdateStandoutJobToClassic()
        {
            var oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{AdvertisementId}";

            PactProvider.MockService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a request to update a standout job to classic")
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

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<ValidationException>(
                    async () => await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), requestModel));
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

        private AdPostingApiClient GetClient(OAuth2Token token)
        {
            var oAuthClient = Mock.Of<IOAuth2TokenClient>(c => c.GetOAuth2TokenAsync() == Task.FromResult(token));

            return new AdPostingApiClient(PactProvider.MockServiceUri, oAuthClient);
        }
    }
}