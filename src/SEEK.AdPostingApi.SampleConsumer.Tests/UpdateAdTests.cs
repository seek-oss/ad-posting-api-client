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
    public class UpdateAdTests : IDisposable
    {
        private readonly IOAuth2TokenClient _oauthClient;
        private const string AdvertisementLink = "/advertisement";

        private IBuilderInitializer MinimumFieldsInitializer => new MinimumFieldsInitializer();

        private IBuilderInitializer AllFieldsInitializer => new AllFieldsInitializer();

        public UpdateAdTests()
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
        public async Task UpdateExistingAdvertisement()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There is a pending standout advertisement with maximum data and id '{advertisementId}'")
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
                        .WithoutAgentId()
                        .WithJobTitle("Exciting Senior Developer role in a great CBD location. Great $$$ - updated")
                        .WithVideoUrl("https://www.youtube.com/v/dVDk7PXNXB8")
                        .WithApplicationFormUrl("http://FakeATS.com.au")
                        .WithStandoutBullets("new Uzi", "new Remington Model", "new AK-47")
                        .WithoutSeekCodes()
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
                        .WithoutAgentId()
                        .WithJobTitle("Exciting Senior Developer role in a great CBD location. Great $$$ - updated")
                        .WithVideoUrl("https://www.youtube.com/v/dVDk7PXNXB8")
                        .WithApplicationFormUrl("http://FakeATS.com.au")
                        .WithStandoutBullets("new Uzi", "new Remington Model", "new AK-47")
                        .WithoutSeekCodes()
                        .WithResponseLink("self", link)
                        .Build()
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            AdvertisementResource jobAd = await client.UpdateAdvertisementAsync(
                new Uri(PactProvider.MockServiceUri, link),
                new AdvertisementModelBuilder(AllFieldsInitializer)
                        .WithAgentId(null)
                        .WithJobTitle("Exciting Senior Developer role in a great CBD location. Great $$$ - updated")
                        .WithVideoUrl("https://www.youtube.com/v/dVDk7PXNXB8")
                        .WithApplicationFormUrl("http://FakeATS.com.au")
                        .WithStandoutBullets("new Uzi", "new Remington Model", "new AK-47")
                        .WithSeekCodes(null)
                        .Build()
                );

            Assert.AreEqual("Exciting Senior Developer role in a great CBD location. Great $$$ - updated", jobAd.Properties.JobTitle);
            Assert.AreEqual("https://www.youtube.com/v/dVDk7PXNXB8", jobAd.Properties.Video.Url);
            Assert.AreEqual("http://FakeATS.com.au", jobAd.Properties.ApplicationFormUrl);
            CollectionAssert.AreEqual(new[] { "new Uzi", "new Remington Model", "new AK-47" }, jobAd.Properties.Standout.Bullets);
        }

        [Test]
        public async Task UpdateNonExistentAdvertisement()
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
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                    },
                    Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                        .WithAdvertisementDetails("This advertisement should not exist.")
                        .Build()
                })
                .WillRespondWith(new ProviderServiceResponse { Status = 404 });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            ResourceActionException exception = Assert.Throws<ResourceActionException>(
                async () => await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link),
                    new AdvertisementModelBuilder(MinimumFieldsInitializer)
                        .WithAdvertisementDetails("This advertisement should not exist.")
                        .Build()));

            StringAssert.Contains("404", exception.Message);
        }

        [Test]
        public async Task UpdateWithBadAdvertisementData()
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
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                    },
                    Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                        .WithSalaryMinimum(0)
                        .WithVideoUrl("htp://www.youtube.com/v/abc".PadRight(260, '!'))
                        .WithVideoPosition(VideoPosition.Below.ToString())
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
                new ValidationData { Field = "applicationEmail", Code = "InvalidEmailAddress" },
                new ValidationData { Field = "applicationFormUrl", Code = "InvalidUrl" },
                new ValidationData { Field = "salary.minimum", Code = "ValueOutOfRange" },
                new ValidationData { Field = "template.items[1].name", Code = "Required" },
                new ValidationData { Field = "template.items[1].value", Code = "MaxLengthExceeded" },
                new ValidationData { Field = "video.url", Code = "MaxLengthExceeded" },
                new ValidationData { Field = "video.url", Code = "RegexPatternNotMatched" }
            };

            ValidationException exception = Assert.Throws<ValidationException>(
                async () => await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link),
                    new AdvertisementModelBuilder(MinimumFieldsInitializer)
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
    }
}