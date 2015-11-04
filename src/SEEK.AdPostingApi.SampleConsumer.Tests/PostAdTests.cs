using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
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
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";
            var location = $"http://localhost{link}";

            PactProvider.RegisterIndexPageInteractions();

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
                            .WithResponseLink("view", viewRenderedAdvertisementLink)
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
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";
            var location = $"http://localhost{link}";

            PactProvider.RegisterIndexPageInteractions();

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
                            .WithResponseLink("view", viewRenderedAdvertisementLink)
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

            PactProvider.RegisterIndexPageInteractions();

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

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);
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

            ValidationException exception = Assert.Throws<ValidationException>(
                async () => await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(MinimumFieldsInitializer)
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

            exception.ShouldBeEquivalentToException(expectedException);
        }

        [Test]
        public async Task PostAdWithNoCreationId()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.RegisterIndexPageInteractions();

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

            exception.ShouldBeEquivalentToException(
                new ValidationException(
                    HttpMethod.Post,
                    new ValidationMessage
                    {
                        Message = "Validation Failure",
                        Errors = new[]
                        {
                            new ValidationData { Field = "creationId", Code = "Required" }
                        }
                    }));
        }

        [Test]
        public async Task PostAdWithExistingCreationId()
        {
            const string creationId = "CreationIdOf8e2fde50-bc5f-4a12-9cfb-812e50500184";
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var location = $"http://localhost{AdvertisementLink}/{advertisementId}";

            PactProvider.RegisterIndexPageInteractions();

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

            var expectedException = new AdvertisementAlreadyExistsException(new Uri(location));
            var actualException = Assert.Throws<AdvertisementAlreadyExistsException>(
                async () => await client.CreateAdvertisementAsync(new AdvertisementModelBuilder(MinimumFieldsInitializer).WithRequestCreationId(creationId).Build()));

            actualException.ShouldBeEquivalentToException(expectedException);
        }
    }
}