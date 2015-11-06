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
    public class UpdateAdTests : IDisposable
    {
        private readonly IOAuth2TokenClient _oauthClient;
        private const string AdvertisementLink = "/advertisement";

        private IBuilderInitializer MinimumFieldsInitializer => new MinimumFieldsInitializer();

        private IBuilderInitializer AllFieldsInitializer => new AllFieldsInitializer();

        public UpdateAdTests()
        {
            this._oauthClient = Mock.Of<IOAuth2TokenClient>(c => c.GetOAuth2TokenAsync() == Task.FromResult(new OAuth2TokenBuilder().Build()));
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
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";

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
                        .WithResponseLink("view", viewRenderedAdvertisementLink)
                        .Build()
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);
            var requestModel = new AdvertisementModelBuilder(AllFieldsInitializer)
                .WithAgentId(null)
                .WithJobTitle("Exciting Senior Developer role in a great CBD location. Great $$$ - updated")
                .WithVideoUrl("https://www.youtube.com/v/dVDk7PXNXB8")
                .WithApplicationFormUrl("http://FakeATS.com.au")
                .WithStandoutBullets("new Uzi", "new Remington Model", "new AK-47")
                .WithSeekCodes(null)
                .Build();

            AdvertisementResource result = await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), requestModel);

            var expectedResult = new AdvertisementResource
            {
                Links = new Dictionary<string, Link>
                {
                    { "self", new Link { Href = link } },
                    { "view", new Link { Href = viewRenderedAdvertisementLink } }
                },
                Properties = requestModel,
                ResponseHeaders = new HttpResponseMessage().Headers
            };

            expectedResult.ResponseHeaders.Add("Date", result.ResponseHeaders.GetValues("Date"));
            expectedResult.ResponseHeaders.Add("Server", result.ResponseHeaders.GetValues("Server"));

            result.ShouldBeEquivalentTo(expectedResult);
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
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                    },
                    Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                        .WithAdvertisementDetails("This advertisement should not exist.")
                        .Build()
                })
                .WillRespondWith(new ProviderServiceResponse { Status = 404 });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            var actualException = Assert.Throws<AdvertisementNotFoundException>(
                async () => await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link),
                    new AdvertisementModelBuilder(MinimumFieldsInitializer)
                        .WithAdvertisementDetails("This advertisement should not exist.")
                        .Build()));
            var expectedException = new AdvertisementNotFoundException();

            actualException.ShouldBeEquivalentToException(expectedException);
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
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                    },
                    Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                        .WithSalaryMinimum(0)
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

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            var actualException = Assert.Throws<ValidationException>(
                async () => await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link),
                    new AdvertisementModelBuilder(MinimumFieldsInitializer)
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
    }
}