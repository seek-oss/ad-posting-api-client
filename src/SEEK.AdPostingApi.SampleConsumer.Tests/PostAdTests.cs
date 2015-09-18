using Moq;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using SEEK.AdPostingApi.Client.Exceptions;

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
                        Status = 204,
                        Headers = new Dictionary<string, string>
                        {
                            {"Location", "http://localhost/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a"}
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
                                        {"Authorization", "Bearer " + accessToken},
                                        {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
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
                            standoutBullet3 = "standout bullet 3"
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 204,
                        Headers = new Dictionary<string, string>
                        {
                            { "Location", "http://localhost/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a" }
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
                            advertisementType = 0,
                            workType = 0,
                            salaryType = 0,
                            salaryMinimum = 0,
                            salaryMaximum = 0
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 400,
                        //Body = new Dictionary<string, string>
                        //{
                        //    {"salaryType", "Error parsing boolean value. Path 'salaryType', line 8, position 19."}
                        //}
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
                        //Body = new Dictionary<string, string>
                        //{
                        //    {"creationId", "Creation ID is missing. Path 'creationId'"}
                        //}
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
            Uri jobAdLink = await client.CreateAdvertisementAsync(SetupJobAdWithMinimumRequiredData(CreationIdForAdWithMinimumRequiredData));

            StringAssert.StartsWith("http://localhost/advertisement", jobAdLink.ToString());
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

            Uri jobAdLink = await client.CreateAdvertisementAsync(SetupJobAdWithMaximumData());

            StringAssert.StartsWith("http://localhost/advertisement", jobAdLink.ToString());
        }

        [Test]
        public async Task PostAdWithWrongData()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            SetupJobCreationWithBadData(oAuth2Token.AccessToken);

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            try
            {
                await client.CreateAdvertisementAsync(new Advertisement() { CreationId = "20150914-134527-00109" });
                Assert.Fail($"Should throw a '{typeof(ResourceActionException).FullName}' exception");
            }
            catch (ResourceActionException ex)
            {
                StringAssert.Contains($"{HttpStatusCode.BadRequest:G}", ex.Message);
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
                Assert.Fail($"Should throw a '{typeof(ResourceActionException).FullName}' exception");
            }
            catch (ResourceActionException ex)
            {
                StringAssert.Contains($"{HttpStatusCode.BadRequest:G}", ex.Message);
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