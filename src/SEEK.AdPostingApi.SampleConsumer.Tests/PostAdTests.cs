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
    public class PostAdTests
    {
        private const string AdvertisementLink = "/advertisement";
        private const string CreationIdForAdWithMinimumRequiredData = "20150914-134527-00012";
        private const string CreationIdForAdWithMaximumRequiredData = "20150914-134527-00097";

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
        public async Task PostAdWithMinimumRequiredData()
        {
            const string advertisementId = "75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";
            var location = $"http://localhost{link}";

            PactProvider.RegisterIndexPageInteractions(oAuth2Token);

            PactProvider.MockService
                .UponReceiving("a request to create a job ad with minimum required data")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                        },
                        Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                            .WithRequestCreationId(CreationIdForAdWithMinimumRequiredData)
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 202,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8" },
                            { "Location", location }
                        },
                        Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                            .WithResponseLink("self", link)
                            .WithResponseLink("view", viewRenderedAdvertisementLink)
                            .Build()
                    });

            var requestModel = new AdvertisementModelBuilder(MinimumFieldsInitializer).WithRequestCreationId(CreationIdForAdWithMinimumRequiredData).Build();

            AdvertisementResource result;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                result = await client.CreateAdvertisementAsync(requestModel);
            }

            var expectedResult = new AdvertisementResource
            {
                Links = new Dictionary<string, Link>
                {
                    { "self", new Link { Href = link } },
                    { "view", new Link { Href = viewRenderedAdvertisementLink } }
                },
                Properties = new AdvertisementModelBuilder(MinimumFieldsInitializer).Build(),
                ResponseHeaders = new HttpResponseMessage().Headers
            };

            expectedResult.ResponseHeaders.Add("Date", result.ResponseHeaders.GetValues("Date"));
            expectedResult.ResponseHeaders.Add("Server", result.ResponseHeaders.GetValues("Server"));
            expectedResult.ResponseHeaders.Add("Location", location);

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task PostAdWithMaximumData()
        {
            const string advertisementId = "75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";
            var location = $"http://localhost{link}";

            PactProvider.RegisterIndexPageInteractions(oAuth2Token);

            PactProvider.MockService
                .UponReceiving("a request to create a job ad with maximum required data")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                        },
                        Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                            .WithRequestCreationId(CreationIdForAdWithMaximumRequiredData)
                            .Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 202,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8" },
                            { "Location", location }
                        },
                        Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                            .WithState(AdvertisementState.Open.ToString())
                            .WithResponseLink("self", link)
                            .WithResponseLink("view", viewRenderedAdvertisementLink)
                            .Build()
                    });

            var requestModel = new AdvertisementModelBuilder(AllFieldsInitializer).WithRequestCreationId(CreationIdForAdWithMaximumRequiredData).Build();

            AdvertisementResource result;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                result = await client.CreateAdvertisementAsync(requestModel);
            }

            var expectedResult = new AdvertisementResource
            {
                Links = new Dictionary<string, Link>
                {
                    { "self", new Link { Href = link } },
                    { "view", new Link { Href = viewRenderedAdvertisementLink } }
                },
                Properties = new AdvertisementModelBuilder(AllFieldsInitializer).Build(),
                ResponseHeaders = new HttpResponseMessage().Headers
            };

            expectedResult.ResponseHeaders.Add("Date", result.ResponseHeaders.GetValues("Date"));
            expectedResult.ResponseHeaders.Add("Server", result.ResponseHeaders.GetValues("Server"));
            expectedResult.ResponseHeaders.Add("Location", location);

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public void PostAdWithWrongData()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.RegisterIndexPageInteractions(oAuth2Token);

            PactProvider.MockService
                .UponReceiving("a request to create a job ad with bad data")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                        },
                        Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                            .WithRequestCreationId("20150914-134527-00109")
                            .WithoutAdvertiserId()
                            .WithAdvertisementType(AdvertisementType.StandOut.ToString())
                            .WithSalaryMinimum(0)
                            .WithVideoUrl("htp://www.youtube.com/v/abc".PadRight(260, '!'))
                            .WithVideoPosition(VideoPosition.Below.ToString())
                            .WithStandoutBullets("new Uzi", "new Remington Model".PadRight(85, '!'), "new AK-47")
                            .WithApplicationEmail("someone(at)some.domain")
                            .WithApplicationFormUrl("htp://somecompany.domain/apply")
                            .WithTemplateItems(
                                new KeyValuePair<object, object>("Template Line 1", "Template Value 1"),
                                new KeyValuePair<object, object>("", "value2".PadRight(3010, '!')))
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
                                new { field = "advertiserId", code = "Required" },
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

            ValidationException exception;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                exception = Assert.Throws<ValidationException>(
                    async () =>
                        await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(MinimumFieldsInitializer)
                            .WithRequestCreationId("20150914-134527-00109")
                            .WithAdvertiserId(null)
                            .WithAdvertisementType(AdvertisementType.StandOut)
                            .WithSalaryMinimum(0)
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
                HttpMethod.Post,
                new ValidationMessage
                {
                    Message = "Validation Failure",
                    Errors = new[]
                    {
                        new ValidationData { Field = "advertiserId", Code = "Required" },
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

            exception.ShouldBeEquivalentToException(expectedException);
        }

        [Test]
        public void PostAdWithNoCreationId()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.RegisterIndexPageInteractions(oAuth2Token);

            PactProvider.MockService
                .UponReceiving("a request to create a job ad without a creation id")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                        },
                        Body = new AdvertisementContentBuilder(MinimumFieldsInitializer).Build()
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
                                new { field = "creationId", code = "Required" }
                            }
                        }
                    });

            ValidationException exception;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                exception = Assert.Throws<ValidationException>(
                    async () => await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(MinimumFieldsInitializer).Build()));
            }

            exception.ShouldBeEquivalentToException(
                new ValidationException(
                    HttpMethod.Post,
                    new ValidationMessage
                    {
                        Message = "Validation Failure",
                        Errors = new[] { new ValidationData { Field = "creationId", Code = "Required" } }
                    }));
        }

        [Test]
        public void PostAdWithExistingCreationId()
        {
            const string creationId = "CreationIdOf8e2fde50-bc5f-4a12-9cfb-812e50500184";
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var location = $"http://localhost{AdvertisementLink}/{advertisementId}";

            PactProvider.RegisterIndexPageInteractions(oAuth2Token);

            PactProvider.MockService
                .Given($"There is a pending standout advertisement with maximum data and id '{advertisementId}'")
                .UponReceiving($"a request to create a job ad with the same creation id '{creationId}'")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                        },
                        Body = new AdvertisementContentBuilder(MinimumFieldsInitializer).WithRequestCreationId(creationId).Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 409,
                        Headers = new Dictionary<string, string> { { "Location", location } }
                    });

            AdvertisementAlreadyExistsException actualException;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<AdvertisementAlreadyExistsException>(
                    async () => await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(MinimumFieldsInitializer).WithRequestCreationId(creationId).Build()));
            }

            var expectedException = new AdvertisementAlreadyExistsException(new Uri(location));

            actualException.ShouldBeEquivalentToException(expectedException);
        }

        [Test]
        public void PostAdNotPermitted()
        {
            const string advertisementId = "75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.RegisterIndexPageInteractions(oAuth2Token);

            PactProvider.MockService
                .UponReceiving("an unauthorised request to create a job ad")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                        },
                        Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
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
                            { "Content-Type", "application/json; charset=utf-8" }
                        },
                        Body = new { message = "Operation not permitted on advertisement with advertiser id: '9012'" }
                    });

            var requestModel = new AdvertisementModelBuilder(MinimumFieldsInitializer).WithRequestCreationId(CreationIdForAdWithMinimumRequiredData).Build();

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<UnauthorizedException>(
                    async () => await client.CreateAdvertisementAsync(requestModel));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException("Operation not permitted on advertisement with advertiser id: '9012'"));
        }

        private AdPostingApiClient GetClient(OAuth2Token token)
        {
            var oAuthClient = Mock.Of<IOAuth2TokenClient>(c => c.GetOAuth2TokenAsync() == Task.FromResult(token));

            return new AdPostingApiClient(PactProvider.MockServiceUri, oAuthClient);
        }
    }
}