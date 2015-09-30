using Moq;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    [TestFixture]
    public class GetAdTests : IDisposable
    {
        private readonly IOAuth2TokenClient _oauthClient;

        public GetAdTests()
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
        public async Task GetExistingAdvertisement()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.MockService
                .Given($"There is an advertisement with id: '{advertisementId}'")
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
                        {"Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8"},
                        {"Status", "Pending"}
                    },
                    Body = new
                    {
                        agentId = (object)null,
                        advertiserId = "9012",
                        advertisementType = AdvertisementType.StandOut.ToString(),
                        jobTitle = "Exciting Senior Developer role in a great CBD location. Great $$$",
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
                        videoUrl = "https://www.youtube.com/watch?v=dVDk7PXNXB8",
                        videoPosition = VideoPosition.Above.ToString(),
                        applicationEmail = "asdf@asdf.com",
                        applicationFormUrl = "http://applicationform/",
                        screenId = 20,
                        jobReference = "JOB1234",
                        templateId = 99,
                        templateItems = new[]
                        {
                            new { name = "Template Line 1", value = "Template Value 1" },
                            new { name = "Template Line 2", value = "Template Value 2" }
                        },
                        standoutLogoId = 333,
                        standoutBullet1 = "Uzi",
                        standoutBullet2 = "Remington Model",
                        standoutBullet3 = "AK-47",
                        seekCodes = new[]
                        {
                            "SK010001Z",
                            "SK010010z",
                            "SK0101OOZ",
                            "SK910101A"
                        },
                        additionalProperties = new[] { AdditionalPropertyType.ResidentsOnly.ToString() },
                        _links = new
                        {
                            self = new
                            {
                                href = "/advertisement/" + advertisementId
                            },
                            expire = new {
                                href = "/advertisement/" + advertisementId + "/expire"
                            }
                        }
                    }
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            var jobAd = await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, "advertisement/" + advertisementId));
            Assert.AreEqual("Exciting Senior Developer role in a great CBD location. Great $$$", jobAd.Properties.JobTitle, "Wrong job titile returned!");
            Assert.AreEqual(Status.Pending, jobAd.Status);
        }

        [Test]
        public async Task GetExistingAdvertisementStatus()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            var oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.MockLinks();

            PactProvider.MockService
                .Given($"There is an advertisement with id: '{advertisementId}'")
                .UponReceiving("HEAD request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Head,
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
                        {"Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8"},
                        {"Status", "Pending"}
                    }
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            var status = await client.GetAdvertisementStatusAsync(new Guid(advertisementId));
            Assert.AreEqual(Status.Pending, status);
        }

        [Test]
        public async Task GetExistingAdvertisementStatusUsingUri()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            var oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.MockService
                .Given($"There is an advertisement with id: '{advertisementId}'")
                .UponReceiving("HEAD request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Head,
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
                        {"Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8"},
                        {"Status", "Pending"}
                    }
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            var status = await client.GetAdvertisementStatusAsync(new Uri(PactProvider.MockServiceUri, "advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184"));
            Assert.AreEqual(Status.Pending, status);
        }

        [Test]
        public async Task GetNonExistentAdvertisement()
        {
            const string advertisementId = "9b650105-7434-473f-8293-4e23b7e0e064";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.MockService
                .Given($"There isn't an advertisement with id: '{advertisementId}'")
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

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            try
            {
                await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, "advertisement/" + advertisementId));
            }
            catch (Exception ex)
            {
                StringAssert.Contains("404 (Not Found)", ex.Message);
            }
        }
    }
}