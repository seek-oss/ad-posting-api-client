using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            PactProvider.MockService
                .Given(string.Format("There is an advertisement with id: '{0}'", advertisementId))
                .UponReceiving("Update request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Put,
                    Path = "/advertisement/" + advertisementId,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                    },
                    Body = new
                    {
                        agentId = (object)null,
                        advertiserId = "9012",
                        advertisementType = AdvertisementType.StandOut.ToString(),
                        jobTitle = "Exciting Senior Developer role in a great CBD location. Great $$$ - updated",
                        locationId = "378",
                        subclassificationId = "734",
                        workType = WorkType.FullTime.ToString(),
                        salaryType = SalaryType.AnnualPackage.ToString(),
                        salaryMinimum = 100000,
                        salaryMaximum = 200000,
                        salaryDetails = "We will pay you",
                        jobSummary = "Developer job",
                        advertisementDetails = "Exciting, do I need to say more?",
                        contactDetails = "Call me",
                        videoUrl = "https://www.youtube.com/v/abc",
                        videoPosition = VideoPosition.Above.ToString(),
                        applicationEmail = "asdf@asdf.com",
                        applicationFormUrl = "http://FakeATS.com.au",
                        screenId = 20,
                        jobReference = "JOB1234",
                        templateId = 99,
                        templateItems = new[]
                        {
                            new { name = "Template Line 1", value = "Template Value 1" },
                            new { name = "Template Line 2", value = "Template Value 2" }
                        },
                        standoutLogoId = 333,
                        standoutBullet1 = "new Uzi",
                        standoutBullet2 = "new Remington Model",
                        standoutBullet3 = "new AK-47",
                        additionalProperties = new[] { AdditionalPropertyType.ResidentsOnly.ToString() },
                    }
                })
                .WillRespondWith(
                new ProviderServiceResponse
                {
                    Status = 202,
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8"}
                    },
                    Body = new
                    {
                        agentId = (object)null,
                        advertiserId = "9012",
                        advertisementType = AdvertisementType.StandOut.ToString(),
                        jobTitle = "Exciting Senior Developer role in a great CBD location. Great $$$ - updated",
                        locationId = "378",
                        subclassificationId = "734",
                        workType = WorkType.FullTime.ToString(),
                        salaryType = SalaryType.AnnualPackage.ToString(),
                        salaryMinimum = 100000,
                        salaryMaximum = 200000,
                        salaryDetails = "We will pay you",
                        jobSummary = "Developer job",
                        advertisementDetails = "Exciting, do I need to say more?",
                        contactDetails = "Call me",
                        videoUrl = "https://www.youtube.com/v/abc",
                        videoPosition = VideoPosition.Above.ToString(),
                        applicationEmail = "asdf@asdf.com",
                        applicationFormUrl = "http://FakeATS.com.au",
                        screenId = 20,
                        jobReference = "JOB1234",
                        templateId = 99,
                        templateItems = new[]
                        {
                            new { name = "Template Line 1", value = "Template Value 1" },
                            new { name = "Template Line 2", value = "Template Value 2" }
                        },
                        standoutLogoId = 333,
                        standoutBullet1 = "new Uzi",
                        standoutBullet2 = "new Remington Model",
                        standoutBullet3 = "new AK-47",
                        additionalProperties = new[] { AdditionalPropertyType.ResidentsOnly.ToString() },
                        _links = new
                        {
                            self = new
                            {
                                href = "/advertisement/" + advertisementId
                            }
                        }
                    }
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            AdvertisementResource jobAd = await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, "advertisement/" + advertisementId),
                new Advertisement
                {
                    AdvertiserId = "9012",
                    AdvertisementType = AdvertisementType.StandOut,
                    JobTitle = "Exciting Senior Developer role in a great CBD location. Great $$$ - updated",
                    LocationId = "378",
                    SubclassificationId = "734",
                    WorkType = WorkType.FullTime,
                    SalaryType = SalaryType.AnnualPackage,
                    SalaryMinimum = 100000,
                    SalaryMaximum = 200000,
                    SalaryDetails = "We will pay you",
                    JobSummary = "Developer job",
                    AdvertisementDetails = "Exciting, do I need to say more?",
                    ContactDetails = "Call me",
                    VideoUrl = "https://www.youtube.com/v/abc",
                    VideoPosition = VideoPosition.Above,
                    ApplicationEmail = "asdf@asdf.com",
                    ApplicationFormUrl = "http://FakeATS.com.au",
                    ScreenId = 20,
                    JobReference = "JOB1234",
                    TemplateId = 99,
                    TemplateItems = new[]
                        {
                            new TemplateItemModel{Name = "Template Line 1", Value = "Template Value 1"},
                            new TemplateItemModel{Name = "Template Line 2", Value = "Template Value 2"}
                        },
                    StandoutLogoId = 333,
                    StandoutBullet1 = "new Uzi",
                    StandoutBullet2 = "new Remington Model",
                    StandoutBullet3 = "new AK-47",
                    AdditionalProperties = new[] { AdditionalPropertyType.ResidentsOnly },
                });

            Assert.AreEqual("Exciting Senior Developer role in a great CBD location. Great $$$ - updated", jobAd.Properties.JobTitle);
        }

        [Test]
        public async Task UpdateNonExistentAdvertisement()
        {
            const string advertisementId = "9b650105-7434-473f-8293-4e23b7e0e064";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.MockService
                .Given(string.Format("There isn't an advertisement with id: '{0}'", advertisementId))
                .UponReceiving("Update request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Put,
                    Path = "/advertisement/" + advertisementId,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                    },
                    Body = new
                    {
                        advertiserId = "advertiserA",
                        jobTitle = "Bricklayer",
                        jobSummary = "some text",
                        advertisementDetails = "experience required",
                        advertisementType = AdvertisementType.Classic.ToString(),
                        workType = WorkType.Casual.ToString(),
                        salaryType = SalaryType.HourlyRate.ToString(),
                        locationId = "1002",
                        subclassificationId = "6227",
                        salaryMinimum = 20,
                        salaryMaximum = 24
                    }
                })
                .WillRespondWith(new ProviderServiceResponse { Status = 404 });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            try
            {
                await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, "advertisement/" + advertisementId), new Advertisement
                {
                    AdvertiserId = "advertiserA",
                    JobTitle = "Bricklayer",
                    JobSummary = "some text",
                    AdvertisementDetails = "experience required",
                    AdvertisementType = AdvertisementType.Classic,
                    WorkType = WorkType.Casual,
                    SalaryType = SalaryType.HourlyRate,
                    LocationId = "1002",
                    SubclassificationId = "6227",
                    SalaryMinimum = 20,
                    SalaryMaximum = 24
                });
            }
            catch (ResourceActionException ex)
            {
                StringAssert.Contains("404", ex.Message);
            }
        }

        [Test]
        public async Task UpdateWithBadAdvertisementData()
        {
            const string advertisementId = "7e2fde50-bc5f-4a12-9cfb-812e50500184";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.MockService
                .Given(string.Format("There is an advertisement with id: '{0}'", advertisementId))
                .UponReceiving("Update request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Put,
                    Path = "/advertisement/" + advertisementId,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                    },
                    Body = new
                    {
                        advertiserId = "advertiserA",
                        advertisementType = AdvertisementType.Classic.ToString(),
                        workType = WorkType.Casual.ToString(),
                        jobTitle = "Candle Stick Maker",
                        locationId = "1002",
                        subclassificationId = "6227",
                        salaryType = SalaryType.HourlyRate.ToString(),
                        salaryMinimum = 0,
                        salaryMaximum = 24,
                        jobSummary = "some text",
                        advertisementDetails = "experience required",
                        videoUrl = "htp://www.youtube.com/v/abc".PadRight(260, '!'),
                        videoPosition = VideoPosition.Below.ToString(),
                        applicationEmail = "someone(at)some.domain",
                        applicationFormUrl = "htp://somecompany.domain/apply",
                        templateItems = new[]
                            {
                                new { name = "template1", value = "value1" },
                                new { name = "", value = "value2".PadRight(260, '!') }
                            }
                    }
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
                                new { field = "salaryMinimum", code = "ValueOutOfRange" },
                                new { field = "templateItems[1].name", code = "Required" },
                                new { field = "templateItems[1].value", code = "MaxLengthExceeded" },
                                new { field = "videoUrl", code = "MaxLengthExceeded" },
                                new { field = "videoUrl", code = "RegexPatternNotMatched" }
                            }
                        }
                    });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);
            var expectedValidationDataItems = new[]
            {
                new ValidationData { Field = "applicationEmail", Code = "InvalidEmailAddress" },
                new ValidationData { Field = "applicationFormUrl", Code = "InvalidUrl" },
                new ValidationData { Field = "salaryMinimum", Code = "ValueOutOfRange" },
                new ValidationData { Field = "templateItems[1].name", Code = "Required" },
                new ValidationData { Field = "templateItems[1].value", Code = "MaxLengthExceeded" },
                new ValidationData { Field = "videoUrl", Code = "MaxLengthExceeded" },
                new ValidationData { Field = "videoUrl", Code = "RegexPatternNotMatched" }
            };

            try
            {
                await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, "advertisement/" + advertisementId), new Advertisement
                {
                    AdvertiserId = "advertiserA",
                    AdvertisementType = AdvertisementType.Classic,
                    WorkType = WorkType.Casual,
                    JobTitle = "Candle Stick Maker",
                    LocationId = "1002",
                    SubclassificationId = "6227",
                    SalaryType = SalaryType.HourlyRate,
                    SalaryMinimum = 0,
                    SalaryMaximum = 24,
                    JobSummary = "some text",
                    AdvertisementDetails = "experience required",
                    VideoUrl = "htp://www.youtube.com/v/abc".PadRight(260, '!'),
                    VideoPosition = VideoPosition.Below,
                    ApplicationEmail = "someone(at)some.domain",
                    ApplicationFormUrl = "htp://somecompany.domain/apply",
                    TemplateItems = new[]
                        {
                            new TemplateItemModel { Name = "template1", Value = "value1" },
                            new TemplateItemModel { Name = "", Value = "value2".PadRight(260, '!') }
                        }
                });
                Assert.Fail($"Should throw a '{typeof(ValidationException).FullName}' exception");
            }
            catch (ValidationException ex)
            {
                Assert.IsNotNull(ex.ValidationDataItems);
                ex.ValidationDataItems.ShouldBe(expectedValidationDataItems);
            }
        }
    }
}