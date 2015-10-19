using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Exceptions;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    [TestFixture]
    public class PostAdTests : IDisposable
    {
        private readonly IOAuth2TokenClient _oauthClient;

        private const string AdvertisementLink = "/advertisement";
        private const string CreationIdForAdWithMinimumRequiredData = "20150914-134527-00012";
        private const string CreationIdForAdWithMaximumRequiredData = "20150914-134527-00097";

        private IBuilderInitializer MinimumFieldsInitializer => new MinimumFieldsInitializer();

        private IBuilderInitializer AllFieldsInitializer => new AllFieldsInitializer();

        public PostAdTests()
        {
            this._oauthClient = Mock.Of<IOAuth2TokenClient>(
                c => c.GetOAuth2TokenAsync() == Task.FromResult(new OAuth2TokenBuilder().Build()));
            this._oauthClient.AccessToken = "b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e";
        }

        public void Dispose()
        {
            this._oauthClient.Dispose();
        }

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
            var location = $"http://localhost{link}";

            PactProvider.MockLinks();

            PactProvider.MockService
                .UponReceiving("a request to create a job ad with minimum required data")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
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
                            {"Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8"},
                            {"Location", location}
                        },
                        Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                            .WithResponseLink("self", link)
                            .Build()
                    });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);
            var requestModel = new AdvertisementModelBuilder(MinimumFieldsInitializer).WithRequestCreationId(CreationIdForAdWithMinimumRequiredData).Build();
            AdvertisementResource jobAd = await client.CreateAdvertisementAsync(requestModel);

            Assert.AreEqual(link, jobAd.Links["self"].Href);
            Assert.IsNull(jobAd.Properties.CreationId);
            Assert.AreEqual(requestModel.AdvertiserId, jobAd.Properties.AdvertiserId);
        }

        [Test]
        public async Task PostAdWithMaximumData()
        {
            const string advertisementId = "75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";
            var location = $"http://localhost{link}";

            PactProvider.MockLinks();

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
                            {"Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8"},
                            {"Location", location}
                        },
                        Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                            .WithState(AdvertisementState.Open.ToString())
                            .WithResponseLink("self", link)
                            .Build()
                    });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            var requestModel = new AdvertisementModelBuilder(AllFieldsInitializer).WithRequestCreationId(CreationIdForAdWithMaximumRequiredData).Build();
            AdvertisementResource jobAd = await client.CreateAdvertisementAsync(requestModel);

            Assert.AreEqual(link, jobAd.Links["self"].Href);
            Assert.IsNull(jobAd.Properties.CreationId);
            Assert.AreEqual(requestModel.AdvertiserId, jobAd.Properties.AdvertiserId);
        }

        [Test]
        public async Task PostAdWithWrongData()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.MockLinks();

            PactProvider.MockService
                .UponReceiving("a request to create a job ad with bad data")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                        },
                        Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                            .WithRequestCreationId("20150914-134527-00109")
                            .WithoutAdvertiserId()
                            .WithSalaryMinimum(0)
                            .WithVideoUrl("htp://www.youtube.com/v/abc".PadRight(260, '!'))
                            .WithVideoPosition(VideoPosition.Below.ToString())
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
                            errors = new[] {
                                new { field = "advertiserId", code = "Required" },
                                new { field = "applicationEmail", code = "InvalidEmailAddress" },
                                new { field = "applicationFormUrl", code = "InvalidUrl" },
                                new { field = "salary.minimum", code = "ValueOutOfRange" },
                                new { field = "template.items[1].name", code = "Required" },
                                new { field = "template.items[1].value", code = "MaxLengthExceeded" },
                                new { field = "video.url", code = "MaxLengthExceeded" },
                                new { field = "video.url", code = "RegexPatternNotMatched" }
                            }
                        }
                    });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);
            var expectedValidationDataItems = new[]
            {
                new ValidationData { Field = "advertiserId", Code = "Required" },
                new ValidationData { Field = "applicationEmail", Code = "InvalidEmailAddress" },
                new ValidationData { Field = "applicationFormUrl", Code = "InvalidUrl" },
                new ValidationData { Field = "salary.minimum", Code = "ValueOutOfRange" },
                new ValidationData { Field = "template.items[1].name", Code = "Required" },
                new ValidationData { Field = "template.items[1].value", Code = "MaxLengthExceeded" },
                new ValidationData { Field = "video.url", Code = "MaxLengthExceeded" },
                new ValidationData { Field = "video.url", Code = "RegexPatternNotMatched" }
            };

            ValidationException exception = Assert.Throws<ValidationException>(
                async () => await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(MinimumFieldsInitializer)
                    .WithRequestCreationId("20150914-134527-00109")
                    .WithAdvertiserId(null)
                    .WithSalaryMinimum(0)
                    .WithVideoUrl("htp://www.youtube.com/v/abc".PadRight(260, '!'))
                    .WithVideoPosition(VideoPosition.Below)
                    .WithApplicationEmail("someone(at)some.domain")
                    .WithApplicationFormUrl("htp://somecompany.domain/apply")
                    .WithTemplateItems(
                        new TemplateItemModel { Name = "Template Line 1", Value = "Template Value 1" },
                        new TemplateItemModel { Name = "", Value = "value2".PadRight(3010, '!') })
                    .Build()));

            exception.ValidationDataItems.ShouldBeEquivalentTo(expectedValidationDataItems);
        }

        [Test]
        public async Task PostAdWithNoCreationId()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.MockLinks();

            PactProvider.MockService
                .UponReceiving("a request to create a job ad without a creation id")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
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

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            ValidationException exception = Assert.Throws<ValidationException>(
                async () => await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(MinimumFieldsInitializer).Build()));

            exception.ValidationDataItems.ShouldBeEquivalentTo(new[] { new ValidationData { Field = "creationId", Code = "Required" } });
        }

        [Test]
        public async Task PostAdWithExistingCreationId()
        {
            const string creationId = "Creation Id 123";
            const string advertisementId = "66fb4361-c97c-4833-a46f-3606a703a65e";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var location = $"http://localhost{AdvertisementLink}/{advertisementId}";

            PactProvider.MockLinks();

            PactProvider.MockService
                .Given($"There is a classic advertisement with id: '{advertisementId}'")
                .UponReceiving($"a request to create a job ad with the same creation id '{creationId}'")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                        },
                        Body = new AdvertisementContentBuilder(MinimumFieldsInitializer).WithRequestCreationId(creationId).Build()
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 409,
                        Headers = new Dictionary<string, string>
                        {
                            {"Location", location}
                        }
                    });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            AdvertisementAlreadyExistsException exception = Assert.Throws<AdvertisementAlreadyExistsException>(
                async () => await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(MinimumFieldsInitializer).WithRequestCreationId(creationId).Build()));

            Assert.AreEqual(exception.CreationId, creationId);
            Assert.AreEqual(location, exception.AdvertisementLink.AbsoluteUri);
        }
    }
}