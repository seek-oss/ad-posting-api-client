using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;

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
                        additionalProperties = new[] { AdditionalPropertyType.ResidentsOnly.ToString() }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse { Status = 204 });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, "advertisement/" + advertisementId), 
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
                        AdditionalProperties = new[] { AdditionalPropertyType.ResidentsOnly }
                });

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
            catch (Exception ex)
            {
                StringAssert.Contains("Not Found", ex.Message);
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
                        jobTitle = "A very longgggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg title",
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
                .WillRespondWith(new ProviderServiceResponse { Status = 400 });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            try
            {
                await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, "advertisement/" + advertisementId), new Advertisement
                {
                    AdvertiserId = "advertiserA",
                    JobTitle = "A very longgggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg title",
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
            catch (Exception ex)
            {
                StringAssert.Contains("Bad Request", ex.Message);
            }
        }

    }

}
