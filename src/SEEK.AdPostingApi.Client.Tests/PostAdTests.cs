using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using SEEK.AdPostingApi.Client.Tests.Framework;
using Xunit;

namespace SEEK.AdPostingApi.Client.Tests
{
    [Collection(AdPostingApiCollection.Name)]
    public class PostAdTests : IDisposable
    {
        private const string AdvertisementLink = "/advertisement";
        private const string CreationIdForAdWithMinimumRequiredData = "20150914-134527-00012";
        private const string CreationIdForAdWithMaximumRequiredData = "20150914-134527-00097";
        private const string CreationIdForAdWithDuplicateTemplateCustomFields = "20160120-162020-00000";
        private const string RequestId = "PactRequestId";

        private IBuilderInitializer MinimumFieldsInitializer => new MinimumFieldsInitializer();

        private IBuilderInitializer AllFieldsInitializer => new AllFieldsInitializer();

        public PostAdTests(AdPostingApiPactService adPostingApiPactService)
        {
            this.Fixture = new AdPostingApiFixture(adPostingApiPactService);
        }

        public void Dispose()
        {
            this.Fixture.Dispose();
        }

        [Fact]
        public async Task PostAdWithRequiredFieldValuesOnly()
        {
            const string advertisementId = "75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";
            var location = $"http://localhost{link}";

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .UponReceiving("a POST advertisement request to create a job ad with required field values only")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementVersion1 },
                            { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                            { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                        },
                        Body = new AdvertisementContentBuilder(this.MinimumFieldsInitializer)
                            .WithRequestCreationId(CreationIdForAdWithMinimumRequiredData)
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 200,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementVersion1 },
                            { "Location", location },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new AdvertisementResponseContentBuilder(this.MinimumFieldsInitializer)
                            .WithState(AdvertisementState.Open.ToString())
                            .WithId(advertisementId)
                            .WithLink("self", link)
                            .WithLink("view", viewRenderedAdvertisementLink)
                            .Build()
                    });

            var requestModel = new AdvertisementModelBuilder(this.MinimumFieldsInitializer).WithRequestCreationId(CreationIdForAdWithMinimumRequiredData).Build();

            AdvertisementResource result;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                result = await client.CreateAdvertisementAsync(requestModel);
            }

            AdvertisementResource expectedResult = new AdvertisementResourceBuilder(this.MinimumFieldsInitializer)
                .WithId(new Guid(advertisementId))
                .WithLinks(advertisementId)
                .Build();

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task PostAdWithRequiredAndOptionalFieldValues()
        {
            const string advertisementId = "75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";
            var location = $"http://localhost{link}";

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .UponReceiving("a POST advertisement request to create a job ad with required and optional field values")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementVersion1 },
                            { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                            { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                        },
                        Body = new AdvertisementContentBuilder(this.AllFieldsInitializer)
                            .WithRequestCreationId(CreationIdForAdWithMaximumRequiredData)
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 200,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementVersion1 },
                            { "Location", location },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new AdvertisementResponseContentBuilder(this.AllFieldsInitializer)
                            .WithState(AdvertisementState.Open.ToString())
                            .WithId(advertisementId)
                            .WithLink("self", link)
                            .WithLink("view", viewRenderedAdvertisementLink)
                            .Build()
                    });

            var requestModel = new AdvertisementModelBuilder(this.AllFieldsInitializer).WithRequestCreationId(CreationIdForAdWithMaximumRequiredData).Build();

            AdvertisementResource result;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                result = await client.CreateAdvertisementAsync(requestModel);
            }

            AdvertisementResource expectedResult = new AdvertisementResourceBuilder(this.AllFieldsInitializer)
                .WithId(new Guid(advertisementId))
                .WithLinks(advertisementId)
                .Build();

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task PostAdWithInvalidFieldValues()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .UponReceiving("a POST advertisement request to create a job ad with invalid field values")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementVersion1 },
                            { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                            { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                        },
                        Body = new AdvertisementContentBuilder(this.MinimumFieldsInitializer)
                            .WithRequestCreationId("20150914-134527-00109")
                            .WithAdvertisementType(AdvertisementType.StandOut.ToString())
                            .WithSalaryMinimum(-1.0)
                            .WithStandoutBullets("new Uzi", "new Remington Model".PadRight(85, '!'), "new AK-47")
                            .WithApplicationEmail("someone(at)some.domain")
                            .WithApplicationFormUrl("htp://somecompany.domain/apply")
                            .WithTemplateItems(
                                new KeyValuePair<object, object>("Template Line 1", "Template Value 1"),
                                new KeyValuePair<object, object>("", "value2"))
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 422,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementErrorVersion1 },
                            { "X-Request-Id", RequestId }
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
                                new { field = "template.items[1].name", code = "Required" }
                            }
                        }
                    });

            ValidationException exception;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                exception = await Assert.ThrowsAsync<ValidationException>(
                    async () =>
                        await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(this.MinimumFieldsInitializer)
                            .WithRequestCreationId("20150914-134527-00109")
                            .WithAdvertisementType(AdvertisementType.StandOut)
                            .WithSalaryMinimum(-1)
                            .WithStandoutBullets("new Uzi", "new Remington Model".PadRight(85, '!'), "new AK-47")
                            .WithApplicationEmail("someone(at)some.domain")
                            .WithApplicationFormUrl("htp://somecompany.domain/apply")
                            .WithTemplateItems(
                                new TemplateItem { Name = "Template Line 1", Value = "Template Value 1" },
                                new TemplateItem { Name = "", Value = "value2" })
                            .Build()));
            }

            var expectedException =
                new ValidationException(
                    RequestId,
                    HttpMethod.Post,
                    new AdvertisementErrorResponse
                    {
                        Message = "Validation Failure",
                        Errors = new[]
                        {
                            new AdvertisementError { Field = "applicationEmail", Code = "InvalidEmailAddress" },
                            new AdvertisementError { Field = "applicationFormUrl", Code = "InvalidUrl" },
                            new AdvertisementError { Field = "salary.minimum", Code = "ValueOutOfRange" },
                            new AdvertisementError { Field = "standout.bullets[1]", Code = "MaxLengthExceeded" },
                            new AdvertisementError { Field = "template.items[1].name", Code = "Required" }
                        }
                    });

            exception.ShouldBeEquivalentToException(expectedException);
        }

        [Fact]
        public async Task PostAdWithInvalidSalaryData()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .UponReceiving("a POST advertisement request to create a job ad with invalid salary data")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementVersion1 },
                            { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                            { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                        },
                        Body = new AdvertisementContentBuilder(this.MinimumFieldsInitializer)
                            .WithRequestCreationId(CreationIdForAdWithMinimumRequiredData)
                            .WithSalaryMinimum(2.0)
                            .WithSalaryMaximum(1.0)
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 422,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementErrorVersion1 },
                            { "X-Request-Id", RequestId }
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

            ValidationException exception;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                exception = await Assert.ThrowsAsync<ValidationException>(
                    async () =>
                        await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(this.MinimumFieldsInitializer)
                            .WithRequestCreationId(CreationIdForAdWithMinimumRequiredData)
                            .WithSalaryMinimum(2)
                            .WithSalaryMaximum(1)
                            .Build()));
            }

            var expectedException =
                new ValidationException(
                    RequestId,
                    HttpMethod.Post,
                    new AdvertisementErrorResponse
                    {
                        Message = "Validation Failure",
                        Errors = new[] { new AdvertisementError { Field = "salary.maximum", Code = "InvalidValue" } }
                    });

            exception.ShouldBeEquivalentToException(expectedException);
        }

        [Fact]
        public async Task PostAdWithInvalidAdvertisementDetails()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .UponReceiving("a POST advertisement request to create a job ad with invalid advertisement details")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementVersion1 },
                            { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                            { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                        },
                        Body = new AdvertisementContentBuilder(this.MinimumFieldsInitializer)
                            .WithRequestCreationId("20150914-134527-00109")
                            .WithAdvertisementDetails("Ad details with <a href='www.youtube.com'>a link</a> and incomplete <h2> element")
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 422,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementErrorVersion1 },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new
                        {
                            message = "Validation Failure",
                            errors = new[]
                            {
                                new { field = "advertisementDetails", code = "InvalidFormat" }
                            }
                        }
                    });

            ValidationException exception;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                exception = await Assert.ThrowsAsync<ValidationException>(
                    async () =>
                        await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(this.MinimumFieldsInitializer)
                            .WithRequestCreationId("20150914-134527-00109")
                            .WithAdvertisementDetails("Ad details with <a href='www.youtube.com'>a link</a> and incomplete <h2> element")
                            .Build()));
            }

            var expectedException =
                new ValidationException(
                    RequestId,
                    HttpMethod.Post,
                    new AdvertisementErrorResponse
                    {
                        Message = "Validation Failure",
                        Errors = new[] { new AdvertisementError { Field = "advertisementDetails", Code = "InvalidFormat" } }
                    });

            exception.ShouldBeEquivalentToException(expectedException);
        }

        [Fact]
        public async Task PostAdWithInvalidAdvertisementDetailsWithCleanseJobAdDetailsOption()
        {
            const string advertisementId = "75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";
            var location = $"http://localhost{link}";
            var adDetailsBeforeCleanse = "<p style=\"text-align:justify; color:#FF00AA\">Colourful</p>";
            var adDetailsAfterCleanse = "<p style=\"text-align:justify\">Colourful</p>";

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .UponReceiving("a POST advertisement request to create a job ad with invalid advertisement details and with 'CleanseJobAdDetails' processing option")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementVersion1 },
                            { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                            { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                        },
                        Body = new AdvertisementContentBuilder(this.MinimumFieldsInitializer)
                            .WithRequestCreationId(CreationIdForAdWithMinimumRequiredData)
                            .WithAdvertisementDetails(adDetailsBeforeCleanse)
                            .WithProcessingOptions(ProcessingOptionsType.CleanseAdvertisementDetails.ToString())
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 200,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementVersion1 },
                            { "Location", location },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new AdvertisementResponseContentBuilder(this.MinimumFieldsInitializer)
                            .WithState(AdvertisementState.Open.ToString())
                            .WithId(advertisementId)
                            .WithLink("self", link)
                            .WithLink("view", viewRenderedAdvertisementLink)
                            .WithAdvertisementDetails(adDetailsAfterCleanse)
                            .Build()
                    });

            var requestModel = new AdvertisementModelBuilder(this.MinimumFieldsInitializer)
                .WithRequestCreationId(CreationIdForAdWithMinimumRequiredData)
                .WithAdvertisementDetails(adDetailsBeforeCleanse)
                .WithProcessingOptions(ProcessingOptionsType.CleanseAdvertisementDetails)
                .Build();

            AdvertisementResource result;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                result = await client.CreateAdvertisementAsync(requestModel);
            }

            AdvertisementResource expectedResult = new AdvertisementResourceBuilder(this.MinimumFieldsInitializer)
                .WithId(new Guid(advertisementId))
                .WithLinks(advertisementId)
                .WithAdvertisementDetails(adDetailsAfterCleanse)
                .Build();

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task PostAdWithNoCreationId()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .UponReceiving("a POST advertisement request to create a job ad without a creation id")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementVersion1 },
                            { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                            { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                        },
                        Body = new AdvertisementContentBuilder(this.MinimumFieldsInitializer).Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 422,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementErrorVersion1 },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new
                        {
                            message = "Validation Failure",
                            errors = new[]
                            {
                                new { field = "creationId", code = "Required" }
                            }
                        }
                    });

            ValidationException exception;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                exception = await Assert.ThrowsAsync<ValidationException>(
                    async () => await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(this.MinimumFieldsInitializer).Build()));
            }

            exception.ShouldBeEquivalentToException(
                new ValidationException(
                    RequestId,
                    HttpMethod.Post,
                    new AdvertisementErrorResponse
                    {
                        Message = "Validation Failure",
                        Errors = new[] { new AdvertisementError { Field = "creationId", Code = "Required" } }
                    }));
        }

        [Fact]
        public async Task PostAdWithExistingCreationId()
        {
            const string creationId = "CreationIdOf8e2fde50-bc5f-4a12-9cfb-812e50500184";
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var location = $"http://localhost{AdvertisementLink}/{advertisementId}";

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .Given("There is a standout advertisement with maximum data")
                .UponReceiving($"a POST advertisement request to create a job ad with the same creation id '{creationId}'")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementVersion1 },
                            { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                            { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                        },
                        Body = new AdvertisementContentBuilder(this.MinimumFieldsInitializer).WithRequestCreationId(creationId).Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 409,
                        Headers = new Dictionary<string, string>
                        {
                            { "Location", location },
                            { "Content-Type", ResponseContentTypes.AdvertisementErrorVersion1 },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new
                        {
                            message = "Conflict",
                            errors = new[] { new { field = "creationId", code = "AlreadyExists" } }
                        }
                    });

            CreationIdAlreadyExistsException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<CreationIdAlreadyExistsException>(
                    async () => await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(this.MinimumFieldsInitializer).WithRequestCreationId(creationId).Build()));
            }

            var expectedException = new CreationIdAlreadyExistsException(RequestId, new Uri(location),
                new AdvertisementErrorResponse
                {
                    Message = "Conflict",
                    Errors = new[] { new AdvertisementError { Field = "creationId", Code = "AlreadyExists" } }
                });

            actualException.ShouldBeEquivalentToException(expectedException);
        }

        [Fact]
        public async Task PostAdWithAnInvalidAdvertiserId()
        {
            var oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .UponReceiving("a POST advertisement request to create a job ad with an invalid advertiser id")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementVersion1 },
                            { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                            { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                        },
                        Body = new AdvertisementContentBuilder(this.MinimumFieldsInitializer)
                            .WithRequestCreationId(CreationIdForAdWithMinimumRequiredData)
                            .WithAdvertiserId("1234ABC")
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementErrorVersion1 },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new
                        {
                            message = "Forbidden",
                            errors = new[] { new { code = "InvalidValue" } }
                        }
                    });

            var requestModel = new AdvertisementModelBuilder(this.MinimumFieldsInitializer).WithRequestCreationId(CreationIdForAdWithMinimumRequiredData).WithAdvertiserId("1234ABC").Build();

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.CreateAdvertisementAsync(requestModel));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    RequestId,
                    403,
                    new AdvertisementErrorResponse
                    {
                        Message = "Forbidden",
                        Errors = new[] { new AdvertisementError { Code = "InvalidValue" } }
                    }));
        }

        [Fact]
        public async Task PostAdUsingDisabledRequestorAccount()
        {
            var oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .Given("The requestor's account is disabled")
                .UponReceiving("a POST advertisement request to create a job")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementVersion1 },
                            { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                            { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                        },
                        Body = new AdvertisementContentBuilder(this.MinimumFieldsInitializer)
                            .WithRequestCreationId(CreationIdForAdWithMinimumRequiredData)
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementErrorVersion1 },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new
                        {
                            message = "Forbidden",
                            errors = new[] { new { code = "AccountError" } }
                        }
                    });

            var requestModel = new AdvertisementModelBuilder(this.MinimumFieldsInitializer).WithRequestCreationId(CreationIdForAdWithMinimumRequiredData).Build();

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.CreateAdvertisementAsync(requestModel));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    RequestId,
                    403,
                    new AdvertisementErrorResponse
                    {
                        Message = "Forbidden",
                        Errors = new[] { new AdvertisementError { Code = "AccountError" } }
                    }));
        }

        [Fact]
        public async Task PostAdWhereAdvertiserNotRelatedToRequestor()
        {
            var oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .UponReceiving("a POST advertisement request to create a job for an advertiser not related to the requestor's account")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementVersion1 },
                            { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                            { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                        },
                        Body = new AdvertisementContentBuilder(this.MinimumFieldsInitializer)
                            .WithRequestCreationId(CreationIdForAdWithMinimumRequiredData)
                            .WithAdvertiserId("999888777")
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementErrorVersion1 },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new
                        {
                            message = "Forbidden",
                            errors = new[] { new { code = "RelationshipError" } }
                        }
                    });

            var requestModel = new AdvertisementModelBuilder(this.MinimumFieldsInitializer).WithRequestCreationId(CreationIdForAdWithMinimumRequiredData).WithAdvertiserId("999888777").Build();

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.CreateAdvertisementAsync(requestModel));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    RequestId,
                    403,
                    new AdvertisementErrorResponse
                    {
                        Message = "Forbidden",
                        Errors = new[] { new AdvertisementError { Code = "RelationshipError" } }
                    }));
        }

        [Fact]
        public async Task PostAdWithDuplicateTemplateCustomFieldNames()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .UponReceiving("a POST advertisement request to create a job ad with duplicated names for template custom fields")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementVersion1 },
                            { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                            { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                        },
                        Body = new AdvertisementContentBuilder(this.MinimumFieldsInitializer)
                            .WithRequestCreationId(CreationIdForAdWithDuplicateTemplateCustomFields)
                            .WithTemplateItems(
                                new KeyValuePair<object, object>("FieldNameA", "Template Value 1"),
                                new KeyValuePair<object, object>("FieldNameB", "Template Value 2"),
                                new KeyValuePair<object, object>("FieldNameA", "Template Value 3"))
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 422,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementErrorVersion1 },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new
                        {
                            message = "Validation Failure",
                            errors = new[]
                            {
                                new { field = "template.items[0]", code = "AlreadySpecified" },
                                new { field = "template.items[2]", code = "AlreadySpecified" }
                            }
                        }
                    });
            ValidationException exception;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                exception = await Assert.ThrowsAsync<ValidationException>(
                    async () =>
                        await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(this.MinimumFieldsInitializer)
                            .WithRequestCreationId(CreationIdForAdWithDuplicateTemplateCustomFields)
                            .WithTemplateItems(
                                new TemplateItem { Name = "FieldNameA", Value = "Template Value 1" },
                                new TemplateItem { Name = "FieldNameB", Value = "Template Value 2" },
                                new TemplateItem { Name = "FieldNameA", Value = "Template Value 3" })
                            .Build()));
            }

            var expectedException = new ValidationException(
                RequestId,
                HttpMethod.Post,
                new AdvertisementErrorResponse
                {
                    Message = "Validation Failure",
                    Errors = new[]
                    {
                        new AdvertisementError { Field = "template.items[0]", Code = "AlreadySpecified" },
                        new AdvertisementError { Field = "template.items[2]", Code = "AlreadySpecified" }
                    }
                });

            exception.ShouldBeEquivalentToException(expectedException);
        }

        [Fact]
        public async Task PostAdWithGranularLocation()
        {
            const string advertisementId = "75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";
            var location = $"http://localhost{link}";

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            var allFieldsWithGranularLocationInitializer = new AllFieldsInitializer(LocationType.UseGranularLocation);

            this.Fixture.AdPostingApiService
                .UponReceiving("a POST advertisement request to create a job ad with granular location")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementVersion1 },
                            { "Accept", $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}" },
                            { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                        },
                        Body = new AdvertisementContentBuilder(allFieldsWithGranularLocationInitializer)
                            .WithRequestCreationId(CreationIdForAdWithMinimumRequiredData)
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 200,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementVersion1 },
                            { "Location", location },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new AdvertisementResponseContentBuilder(allFieldsWithGranularLocationInitializer)
                            .WithId(advertisementId)
                            .WithState(AdvertisementState.Open.ToString())
                            .WithLink("self", link)
                            .WithLink("view", viewRenderedAdvertisementLink)
                            .WithGranularLocationState(null)
                            .Build()
                    });

            var requestModel = new AdvertisementModelBuilder(allFieldsWithGranularLocationInitializer)
                .WithRequestCreationId(CreationIdForAdWithMinimumRequiredData)
                .Build();

            AdvertisementResource result;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                result = await client.CreateAdvertisementAsync(requestModel);
            }

            AdvertisementResource expectedResult = new AdvertisementResourceBuilder(allFieldsWithGranularLocationInitializer)
                .WithId(new Guid(advertisementId))
                .WithLinks(advertisementId)
                .WithGranularLocationState(null)
                .Build();

            result.ShouldBeEquivalentTo(expectedResult);
        }

        private AdPostingApiFixture Fixture { get; }
    }
}