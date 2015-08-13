using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
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
    public class PostAdTests : IDisposable
    {
        private readonly IMockProviderService _mockService;
        private readonly PactProvider _pactProvider;
        private readonly ISEEKOauth2TokenClient _mockOauthClient;

        public PostAdTests()
        {
            this._pactProvider = new PactProvider();
            this._mockService = _pactProvider.MockService;

            this._mockOauthClient = Substitute.For<ISEEKOauth2TokenClient>();
            this._mockOauthClient
                .GetOauth2Token(Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult(SetupAToken()));
        }

        public void Dispose()
        {
            this._pactProvider.Dispose();
            this._mockOauthClient.Dispose();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockService.VerifyInteractions();
        }

        private void SetupJobCreationWithMinimumData(string accessToken, Advertisement testAdvertisement)
        {
            const string advertisementLink = "/advertisement";

            _mockService
                .UponReceiving("a request to retrieve API links")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/",
                    Headers = new Dictionary<string, string>
                    {
                        {"Accept", "application/json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/json; charset=utf-8"}
                    },
                    Body = new
                    {
                        _links = new
                        {
                            advertisements = new
                            {
                                href = advertisementLink
                            },
                            advertisement = new
                            {
                                href = advertisementLink + "/{advertisementId}",
                                templated = true
                            }
                        }
                    }
                });

            _mockService
                .UponReceiving("a request to create a job ad with minimum required data")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = advertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + accessToken},
                            {"Content-Type", "application/json"}
                        },
                        Body = testAdvertisement
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

        private void SetupJobCreationWithMaximumData(string accessToken, Advertisement testAdvertisement)
        {
            const string advertisementLink = "/advertisement";

            _mockService
                .UponReceiving("a request to retrieve API links")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/",
                    Headers = new Dictionary<string, string>
                    {
                        {"Accept", "application/json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/json; charset=utf-8"}
                    },
                    Body = new
                    {
                        _links = new
                        {
                            advertisements = new
                            {
                                href = advertisementLink
                            },
                            advertisement = new
                            {
                                href = advertisementLink + "/{advertisementId}",
                                templated = true
                            }
                        }
                    }
                });

            _mockService
                .UponReceiving("a request to create a job ad with maximum required data")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = advertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                                        {"Authorization", "Bearer " + accessToken},
                                        {"Content-Type", "application/json"}
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
                            locationId = 1002,
                            subclassificationId = 6227,
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
            const string advertisementLink = "/advertisement";

            _mockService
                .UponReceiving("a request to retrieve API links")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/",
                    Headers = new Dictionary<string, string>
                    {
                        {"Accept", "application/json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/json; charset=utf-8"}
                    },
                    Body = new
                    {
                        _links = new
                        {
                            advertisements = new
                            {
                                href = advertisementLink
                            },
                            advertisement = new
                            {
                                href = advertisementLink + "/{advertisementId}",
                                templated = true
                            }
                        }
                    }
                });

            _mockService
                .UponReceiving("a request to create a job ad with bad data")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = advertisementLink,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + accessToken},
                            {"Content-Type", "application/json"}
                        },
                        Body = new
                        {
                            locationId = 0,
                            subclassificationId = 0,
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

        private Oauth2Token SetupAToken()
        {
            return new Oauth2Token
            {
                AccessToken = "b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
                ExpiresIn = 3600,
                Scope = "seek",
                TokenType = "Bearer"
            };
        }

        [TestMethod]
        public async Task PostAdWithMinimumRequiredData()
        {
            Oauth2Token oauth2Token = SetupAToken();

            SetupJobCreationWithMinimumData(oauth2Token.AccessToken, SetupJobAdWithMinimumRequiredData());

            var client = new AdPostingApiClient("testClientId", "testClientSecret", _pactProvider.MockServiceUri, _mockOauthClient);
            Uri jobAdLink = await client.CreateAdvertisementAsync(SetupJobAdWithMinimumRequiredData());

            StringAssert.StartsWith(jobAdLink.ToString(), "http://localhost/advertisement");
        }

        public Advertisement SetupJobAdWithMinimumRequiredData()
        {
            return new Advertisement
            {
                advertiserId = "advertiserA",
                jobTitle = "Bricklayer",
                jobSummary = "some text",
                advertisementDetails = "experience required",
                advertisementType = AdvertisementType.Classic.ToString(),
                workType = WorkType.Casual.ToString(),
                salaryType = SalaryType.HourlyRate.ToString(),
                locationId = 1002,
                subclassificationId = 6227,
                salaryMinimum = 20,
                salaryMaximum = 24
            };
        }

        [TestMethod]
        public async Task PostAdWithMaximumData()
        {
            Oauth2Token oauth2Token = SetupAToken();

            SetupJobCreationWithMaximumData(oauth2Token.AccessToken, SetupJobAdWithMaximumData());

            var client = new AdPostingApiClient("testClientId", "testClientSecret", _pactProvider.MockServiceUri, _mockOauthClient);
            Uri jobAdLink = await client.CreateAdvertisementAsync(SetupJobAdWithMaximumData());

            StringAssert.StartsWith(jobAdLink.ToString(), "http://localhost/advertisement");
        }

        [TestMethod]
        public async Task PostAdWithWrongData()
        {
            Oauth2Token oauth2Token = SetupAToken();

            SetupJobCreationWithBadData(oauth2Token.AccessToken);

            var client = new AdPostingApiClient("testClientId", "testClientSecret", _pactProvider.MockServiceUri, _mockOauthClient);

            try
            {
                await client.CreateAdvertisementAsync(new Advertisement());
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Response status code does not indicate success: 400 (Bad Request).", ex.Message);
            }
        }

        public Advertisement SetupJobAdWithMaximumData()
        {
            return new Advertisement
            {
                agentId = "agentA",
                advertiserId = "advertiserB",
                jobTitle = "Baker",
                jobSummary = "Fantastic opportunity for an awesome baker",
                advertisementDetails = "Baking experience required",
                advertisementType = AdvertisementType.StandOut.ToString(),
                workType = WorkType.Casual.ToString(),
                salaryType = SalaryType.HourlyRate.ToString(),
                locationId = 1002,
                subclassificationId = 6227,
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
                             new TemplateItemModel { Name = "template1", Value = "value1" },
                             new TemplateItemModel { Name = "template2", Value = "value2" }
                         },
                standoutLogoId = 39,
                standoutBullet1 = "standout bullet 1",
                standoutBullet2 = "standout bullet 2",
                standoutBullet3 = "standout bullet 3",
            };
        }
    }
}