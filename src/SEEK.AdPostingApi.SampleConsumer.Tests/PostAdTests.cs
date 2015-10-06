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
    public class PostAdTests : IDisposable
    {
        private readonly IOAuth2TokenClient _oauthClient;

        private const string AdvertisementLink = "/advertisement";
        private const string CreationIdForAdThatAlreadyExists = "20150914-134527-00042";
        private const string CreationIdForAdWithMinimumRequiredData = "20150914-134527-00012";
        private const string CreationIdForAdWithMaximumRequiredData = "20150914-134527-00097";

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

        private void SetupJobCreationWithMinimumData(string accessToken)
        {
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
                            {"Authorization", "Bearer " + accessToken},
                            {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                        },
                        Body = new
                        {
                            advertiserId = "advertiserA",
                            creationId = CreationIdForAdWithMinimumRequiredData,
                            advertisementType = AdvertisementType.Classic.ToString(),
                            jobTitle = "Bricklayer",
                            locationId = "1002",
                            subclassificationId = "6227",
                            workType = WorkType.Casual.ToString(),
                            salaryType = SalaryType.HourlyRate.ToString(),
                            salaryMinimum = 20,
                            salaryMaximum = 24,
                            jobSummary = "some text",
                            advertisementDetails = "experience required"
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 202,
                        Headers = new Dictionary<string, string>
                        {
                            {"Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8"},
                            {"Location", "http://localhost/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a"}
                        },
                        Body = new
                        {
                            advertiserId = "advertiserA",
                            advertisementType = AdvertisementType.Classic.ToString(),
                            jobTitle = "Bricklayer",
                            locationId = "1002",
                            subclassificationId = "6227",
                            workType = WorkType.Casual.ToString(),
                            salaryType = SalaryType.HourlyRate.ToString(),
                            salaryMinimum = 20,
                            salaryMaximum = 24,
                            jobSummary = "some text",
                            advertisementDetails = "experience required",
                            _links = new
                            {
                                self = new
                                {
                                    href = "/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a"
                                }
                            }
                        }
                    });
        }

        private void SetupJobCreationWithMaximumData(string accessToken)
        {
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
                            { "Authorization", "Bearer " + accessToken },
                            { "Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8" }
                        },
                        Body = new
                        {
                            agentId = "agentA",
                            advertiserId = "advertiserB",
                            creationId = CreationIdForAdWithMaximumRequiredData,
                            jobTitle = "Baker",
                            jobSummary = "Fantastic opportunity for an awesome baker",
                            advertisementDetails = "Baking experience required",
                            advertisementType = AdvertisementType.StandOut.ToString(),
                            workType = WorkType.Casual.ToString(),
                            salaryType = SalaryType.HourlyRate.ToString(),
                            locationId = "1002",
                            subclassificationId = "6227",
                            salaryMinimum = 20,
                            salaryMaximum = 24,
                            salaryDetails = "Huge bonus",
                            contactDetails = "0412345678",
                            videoUrl = "http://www.youtube.com/v/abc",
                            videoPosition = VideoPosition.Above.ToString(),
                            applicationEmail = "me@contactme.com.au",
                            applicationFormUrl = "http://FakeATS.com.au",
                            screenId = 100,
                            jobReference = "REF1234",
                            templateId = 43496,
                            templateItems = new[]
                            {
                                new { name = "template1", value = "value1" },
                                new { name = "template2", value = "value2" }
                            },
                            standoutLogoId = 39,
                            standoutBullet1 = "standout bullet 1",
                            standoutBullet2 = "standout bullet 2",
                            standoutBullet3 = "standout bullet 3",
                            seekCodes = new[]
                            {
                                "SK840239A",
                                "SK4232A",
                                "SK23894023A",
                                "SK23432A",
                                "SK238429A"
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
                            {"Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8"},
                            { "Location", "http://localhost/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a" }
                        },
                        Body = new
                        {
                            agentId = "agentA",
                            advertiserId = "advertiserB",
                            jobTitle = "Baker",
                            jobSummary = "Fantastic opportunity for an awesome baker",
                            advertisementDetails = "Baking experience required",
                            advertisementType = AdvertisementType.StandOut.ToString(),
                            workType = WorkType.Casual.ToString(),
                            salaryType = SalaryType.HourlyRate.ToString(),
                            locationId = "1002",
                            subclassificationId = "6227",
                            salaryMinimum = 20,
                            salaryMaximum = 24,
                            salaryDetails = "Huge bonus",
                            contactDetails = "0412345678",
                            videoUrl = "http://www.youtube.com/v/abc",
                            videoPosition = VideoPosition.Above.ToString(),
                            applicationEmail = "me@contactme.com.au",
                            applicationFormUrl = "http://FakeATS.com.au",
                            screenId = 100,
                            jobReference = "REF1234",
                            templateId = 43496,
                            templateItems = new[]
                            {
                                new { name = "template1", value = "value1" },
                                new { name = "template2", value = "value2" }
                            },
                            standoutLogoId = 39,
                            standoutBullet1 = "standout bullet 1",
                            standoutBullet2 = "standout bullet 2",
                            standoutBullet3 = "standout bullet 3",
                            seekCodes = new[]
                            {
                                "SK840239A",
                                "SK4232A",
                                "SK23894023A",
                                "SK23432A",
                                "SK238429A"
                            },
                            _links = new
                            {
                                self = new
                                {
                                    href = "/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a"
                                }
                            }
                        }
                    });
        }

        private void SetupJobCreationWithBadData(string accessToken)
        {
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
                            {"Authorization", "Bearer " + accessToken},
                            {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                        },
                        Body = new
                        {
                            creationId = "20150914-134527-00109",
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
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 400,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8" }
                        },
                        Body = new Dictionary<string, object[]>
                        {
                            { "advertiserId", new object[] { new { severity = "Error", code = "Required" } } },
                            { "salaryMinimum", new object[] { new { severity = "Error", code = "ValueOutOfRange" } } },
                            { "videoUrl", new object[] {
                                new { severity = "Error", code = "MaxLengthExceeded" },
                                new { severity = "Error", code = "RegexPatternNotMatched" } }
                            },
                            { "applicationEmail", new object[] { new { severity = "Error", code = "InvalidEmailAddress" } } },
                            { "applicationFormUrl", new object[] { new { severity = "Error", code = "InvalidUrl" } } },
                            { "templateItems[1].name", new object[] { new { severity = "Error", code = "Required" } } },
                            { "templateItems[1].value", new object[] { new { severity = "Error", code = "MaxLengthExceeded" } } }
                        }
                    });
        }

        private void SetupJobCreationWithNoCreationId(string accessToken)
        {
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
                            {"Authorization", "Bearer " + accessToken},
                            {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                        },
                        Body = new
                        {
                            advertiserId = "advertiserA",
                            advertisementType = AdvertisementType.Classic.ToString(),
                            jobTitle = "Bricklayer",
                            locationId = "1002",
                            subclassificationId = "6227",
                            workType = WorkType.Casual.ToString(),
                            salaryType = SalaryType.HourlyRate.ToString(),
                            salaryMinimum = 20,
                            salaryMaximum = 24,
                            jobSummary = "some text",
                            advertisementDetails = "experience required"
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 400,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8" }
                        },
                        Body = new Dictionary<string, object[]>
                        {
                            { "creationId", new object[] { new { severity = "Error", code = "Required" } } }
                        }
                    });
        }

        private void SetupJobCreationWithExistingCreationId(string accessToken)
        {
            PactProvider.MockLinks();

            PactProvider.MockService
                .Given($"a job ad with creation ID '{CreationIdForAdThatAlreadyExists}' already exists")
                .UponReceiving("a request to create a job ad")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = AdvertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + accessToken},
                            {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                        },
                        Body = new
                        {
                            advertiserId = "advertiserA",
                            creationId = CreationIdForAdThatAlreadyExists,
                            advertisementType = AdvertisementType.Classic.ToString(),
                            jobTitle = "Bricklayer",
                            locationId = "1002",
                            subclassificationId = "6227",
                            workType = WorkType.Casual.ToString(),
                            salaryType = SalaryType.HourlyRate.ToString(),
                            salaryMinimum = 20,
                            salaryMaximum = 24,
                            jobSummary = "some text",
                            advertisementDetails = "experience required"
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 409,
                        Headers = new Dictionary<string, string>
                        {
                            {"Location", "http://localhost/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a"}
                        }
                    });
        }

        [Test]
        public async Task PostAdWithMinimumRequiredData()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            SetupJobCreationWithMinimumData(oAuth2Token.AccessToken);

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);
            AdvertisementResource jobAd = await client.CreateAdvertisementAsync(SetupJobAdWithMinimumRequiredData(CreationIdForAdWithMinimumRequiredData));

            StringAssert.StartsWith("/advertisement/", jobAd.Links["self"].Href);
            Assert.AreEqual("advertiserA", jobAd.Properties.AdvertiserId);
        }

        public Advertisement SetupJobAdWithMinimumRequiredData(string creationId = null)
        {
            return new Advertisement
            {
                AdvertiserId = "advertiserA",
                CreationId = creationId,
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
            };
        }

        [Test]
        public async Task PostAdWithMaximumData()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            SetupJobCreationWithMaximumData(oAuth2Token.AccessToken);

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            AdvertisementResource jobAd = await client.CreateAdvertisementAsync(SetupJobAdWithMaximumData());

            StringAssert.StartsWith("/advertisement/", jobAd.Links["self"].Href);
            Assert.AreEqual("advertiserB", jobAd.Properties.AdvertiserId);
        }

        [Test]
        public async Task PostAdWithWrongData()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            SetupJobCreationWithBadData(oAuth2Token.AccessToken);

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            try
            {
                await client.CreateAdvertisementAsync(new Advertisement
                {
                    CreationId = "20150914-134527-00109",
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
                Assert.IsNotNull(ex.ValidationDataDictionary);
                Assert.AreEqual(7, ex.ValidationDataDictionary.Count);
                ex.ValidationDataDictionary.AssertValidationData("advertiserId", new ValidationData { Severity = ValidationSeverity.Error, Code = "Required" });
                ex.ValidationDataDictionary.AssertValidationData("salaryMinimum", new ValidationData { Severity = ValidationSeverity.Error, Code = "ValueOutOfRange" });
                ex.ValidationDataDictionary.AssertValidationData("videoUrl", new ValidationData { Severity = ValidationSeverity.Error, Code = "MaxLengthExceeded" }, new ValidationData { Severity = ValidationSeverity.Error, Code = "RegexPatternNotMatched" });
                ex.ValidationDataDictionary.AssertValidationData("applicationEmail", new ValidationData { Severity = ValidationSeverity.Error, Code = "InvalidEmailAddress" });
                ex.ValidationDataDictionary.AssertValidationData("applicationFormUrl", new ValidationData { Severity = ValidationSeverity.Error, Code = "InvalidUrl" });
                ex.ValidationDataDictionary.AssertValidationData("templateItems[1].name", new ValidationData { Severity = ValidationSeverity.Error, Code = "Required" });
                ex.ValidationDataDictionary.AssertValidationData("templateItems[1].value", new ValidationData { Severity = ValidationSeverity.Error, Code = "MaxLengthExceeded" });
            }
        }

        public Advertisement SetupJobAdWithMaximumData()
        {
            return new Advertisement
            {
                AgentId = "agentA",
                AdvertiserId = "advertiserB",
                CreationId = CreationIdForAdWithMaximumRequiredData,
                JobTitle = "Baker",
                JobSummary = "Fantastic opportunity for an awesome baker",
                AdvertisementDetails = "Baking experience required",
                AdvertisementType = AdvertisementType.StandOut,
                WorkType = WorkType.Casual,
                SalaryType = SalaryType.HourlyRate,
                LocationId = "1002",
                SubclassificationId = "6227",
                SalaryMinimum = 20,
                SalaryMaximum = 24,
                SalaryDetails = "Huge bonus",
                ContactDetails = "0412345678",
                VideoUrl = "http://www.youtube.com/v/abc",
                VideoPosition = VideoPosition.Above,
                ApplicationEmail = "me@contactme.com.au",
                ApplicationFormUrl = "http://FakeATS.com.au",
                ScreenId = 100,
                JobReference = "REF1234",
                TemplateId = 43496,
                TemplateItems = new[]
                {
                    new TemplateItemModel { Name = "template1", Value = "value1" },
                    new TemplateItemModel { Name = "template2", Value = "value2" }
                },
                StandoutLogoId = 39,
                StandoutBullet1 = "standout bullet 1",
                StandoutBullet2 = "standout bullet 2",
                StandoutBullet3 = "standout bullet 3",
                SeekCodes = new[]
                {
                    "SK840239A",
                    "SK4232A",
                    "SK23894023A",
                    "SK23432A",
                    "SK238429A"
                }
            };
        }

        [Test]
        public async Task PostAdWithNoCreationId()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            SetupJobCreationWithNoCreationId(oAuth2Token.AccessToken);

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            try
            {
                await client.CreateAdvertisementAsync(SetupJobAdWithMinimumRequiredData());
                Assert.Fail($"Should throw a '{typeof(ValidationException).FullName}' exception");
            }
            catch (ValidationException ex)
            {
                Assert.IsNotNull(ex.ValidationDataDictionary);
                Assert.AreEqual(1, ex.ValidationDataDictionary.Count);
                ex.ValidationDataDictionary.AssertValidationData("creationId", new ValidationData { Severity = ValidationSeverity.Error, Code = "Required" });
            }
        }

        [Test]
        public async Task PostAdWithExistingCreationId()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            SetupJobCreationWithExistingCreationId(oAuth2Token.AccessToken);

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            try
            {
                await client.CreateAdvertisementAsync(SetupJobAdWithMinimumRequiredData(CreationIdForAdThatAlreadyExists));
                Assert.Fail($"Should throw an '{typeof(AdvertisementAlreadyExistsException).FullName}' exception");
            }
            catch (AdvertisementAlreadyExistsException ex)
            {
                Assert.AreEqual(ex.CreationId, CreationIdForAdThatAlreadyExists);
            }
        }
    }
}