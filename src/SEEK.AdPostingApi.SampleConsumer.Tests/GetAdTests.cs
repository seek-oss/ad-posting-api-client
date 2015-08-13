using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    [TestClass]
    public class GetAdTests : IDisposable
    {
        private readonly IMockProviderService _mockService;
        private readonly PactProvider _pactProvider;
        private readonly IOAuth2TokenClient _oauthClient;

        public GetAdTests()
        {
            this._pactProvider = new PactProvider();
            this._mockService = _pactProvider.MockService;
            this._oauthClient = Mock.Of<IOAuth2TokenClient>(
                c => c.GetOAuth2TokenAsync(It.IsAny<string>(), It.IsAny<string>()) == Task.FromResult(new OAuth2TokenBuilder().Build()));
        }

        public void Dispose()
        {
            this._pactProvider.Dispose();
            this._oauthClient.Dispose();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockService.VerifyInteractions();
        }

        [TestMethod]
        public async Task GetExistingAdvertisement()
        {
            const string advertisementId = "8E2FDE50-BC5F-4A12-9CFB-812E50500184";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this._mockService
                .Given(string.Format("There is an advertisement with id: '{0}'", advertisementId))
                .UponReceiving("GET request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/advertisement/" + advertisementId,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Accept", "application/vnd.seek.advertisement+json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/vnd.seek.advertisement+json"},
                        {"Status", "Pending"}
                    },
                    Body = new
                    {
                        _links = new
                        {
                            self = new
                            {
                                href = "/advertisement/" + advertisementId
                            }
                        },
                        agentId = (object)null,
                        advertiserId = "9012",
                        advertisementType = AdvertisementType.StandOut,
                        jobTitle = "Exciting Senior Developer role in a great CBD location. Great $$$",
                        locationId = "378",
                        subclassificationId = "734",
                        workType = WorkType.FullTime,
                        salaryType = SalaryType.AnnualPackage,
                        salaryMinimum = 100000,
                        salaryMaximum = 200000,
                        salaryDetails = "We will pay you",
                        jobSummary = "It's a job",
                        advertisementDetails = "Exciting, do I need to say more?",
                        contactDetails = "Call me baby",
                        videoUrl = "https://www.youtube.com/watch?v=4r7wHMg5Yjg",
                        videoPosition = VideoPosition.Above,
                        applicationEmail = "asdf@asdf.com",
                        applicationFormUrl = "http://applicationform/",
                        screenId = 20,
                        jobReference = "JOB1234",
                        templateId = 99,
                        templateItems = new[]
                        {
                            new TemplateItemModel {Name = "Template Line 1", Value = "Template Value 1"},
                            new TemplateItemModel {Name = "Template Line 2", Value = "Template Value 2"}
                        },
                        standoutLogoId = 333,
                        standoutBullet1 = "Uzi",
                        standoutBullet2 = "Remington Model",
                        standoutBullet3 = "AK-47",
                        additionalProperties = new[] { AdditionalPropertyType.ResidentsOnly }
                    }
                });

            var client = new AdPostingApiClient("testClientId", "testClientSecret", _oauthClient, _pactProvider.MockServiceUri);

            await client.GetAdvertisementAsync(new Uri(this._pactProvider.MockServiceUri, "advertisement/" + advertisementId));
        }

        [TestMethod]
        public async Task GetNonExistentAdvertisement()
        {
            const string advertisementId = "No Advertisement";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this._mockService
                .Given(string.Format("There isn't an advertisement with id: '{0}'", advertisementId))
                .UponReceiving("GET request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/advertisement/" + advertisementId,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Accept", "application/vnd.seek.advertisement+json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse { Status = 404 });

            var client = new AdPostingApiClient("testClientId", "testClientSecret", _oauthClient, _pactProvider.MockServiceUri);

            try
            {
                await client.GetAdvertisementAsync(new Uri(this._pactProvider.MockServiceUri, "advertisement/" + advertisementId));
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Response status code does not indicate success: 404 (Not Found).", ex.Message);
            }
        }
    }
}