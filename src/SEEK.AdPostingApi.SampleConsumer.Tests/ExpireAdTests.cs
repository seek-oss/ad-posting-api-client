using System;
using System.Collections.Generic;
using System.Net;
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
    public class ExpireAdTests : IDisposable
    {
        private readonly IOAuth2TokenClient _oauthClient;

        private const string AdvertisementLink = "/advertisement";

        public ExpireAdTests()
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
        public async Task ExpirePendingAdvertisement()
        {
            var advertisementId = new Guid("8e2fde50-bc5f-4a12-9cfb-812e50500184");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockLinks();

            PactProvider.MockService
                .Given($"There is an advertisement with id: '{advertisementId}'")
                .UponReceiving("An expire request for advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", "application/vnd.seek.advertisement-patch+json; charset=utf-8"}
                        },
                        Body = new
                        {
                            state = AdvertisementState.Expired.ToString()
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 202,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8" }
                        },
                        Body = new
                        {
                            advertiserId = "9012",
                            state = AdvertisementState.Pending.ToString(),
                            advertisementType = AdvertisementType.StandOut.ToString(),
                            jobTitle = "Exciting Senior Developer role in a great CBD location. Great $$$",
                            locationId = "378",
                            subclassificationId = "734",
                            workType = WorkType.FullTime.ToString(),
                            salary = new
                            {
                                type = SalaryType.AnnualPackage.ToString(),
                                minimum = 100000,
                                maximum = 200000,
                                details = "We will pay you"
                            },
                            jobSummary = "Developer job",
                            advertisementDetails = "Exciting, do I need to say more?",
                            contactDetails = "Call me",
                            video = new
                            {
                                url = "https://www.youtube.com/watch?v=dVDk7PXNXB8",
                                position = VideoPosition.Above.ToString()
                            },
                            applicationEmail = "asdf@asdf.com",
                            applicationFormUrl = "http://applicationform/",
                            screenId = 20,
                            jobReference = "JOB1234",
                            template = new
                            {
                                id = 99,
                                items = new[]
                            {
                                new { name = "Template Line 1", value = "Template Value 1" },
                                new { name = "Template Line 2", value = "Template Value 2" }
                            }
                            },
                            standout = new
                            {
                                logoId = 333,
                                bullets = new[] { "Uzi", "Remington Model", "AK-47" }
                            },
                            seekCodes = new[]
                            {
                                "SK010001Z",
                                "SK010010z",
                                "SK0101OOZ",
                                "SK910101A"
                            },
                            additionalProperties = new[] { AdditionalPropertyType.ResidentsOnly.ToString() },
                            expiryDate = new DateTime(2015, 10, 7, 21, 19, 00, DateTimeKind.Utc),
                            _links = new
                            {
                                self = new
                                {
                                    href = link
                                }
                            }
                        }
                    });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);
            AdvertisementResource jobAd = await client.ExpireAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), new AdvertisementPatch { State = AdvertisementState.Expired });

            Assert.AreEqual("9012", jobAd.Properties.AdvertiserId);
        }

        [Test]
        public async Task ExpireActiveAdvertisement()
        {
            var advertisementId = new Guid("66fb4361-c97c-4833-a46f-3606a703a65e");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockLinks();

            PactProvider.MockService
                .Given($"There is an active advertisement with id: '{advertisementId}'")
                .UponReceiving("An expire request for advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", "application/vnd.seek.advertisement-patch+json; charset=utf-8"}
                        },
                        Body = new
                        {
                            state = AdvertisementState.Expired.ToString()
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 202,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8" }
                        },
                        Body = new
                        {
                            advertiserId = "13",
                            state = AdvertisementState.Expired.ToString(),
                            advertisementType = "Classic",
                            jobTitle = "Baker",
                            workType = "FullTime",
                            salary = new
                            {
                                type = SalaryType.AnnualPackage.ToString(),
                                minimum = 10000,
                                maximum = 20000,
                            },
                            jobSummary = "Fantastic opportunity for an awesome baker",
                            expiryDate = new DateTime(2015, 10, 7, 21, 19, 00, DateTimeKind.Utc),
                            _links = new
                            {
                                self = new
                                {
                                    href = link
                                }
                            }
                        }
                    });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);
            AdvertisementResource jobAd = await client.ExpireAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), new AdvertisementPatch { State = AdvertisementState.Expired });

            Assert.AreEqual("13", jobAd.Properties.AdvertiserId);
        }

        [Test]
        public void ExpireAlreadyExpiredAdvertisement()
        {
            var advertisementId = new Guid("dfd944df-e17d-45b5-8c86-0af43f9bae5d");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockLinks();

            PactProvider.MockService
                .Given($"There is an expired advertisement with id: '{advertisementId}'")
                .UponReceiving("An expire request for advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", "application/vnd.seek.advertisement-patch+json; charset=utf-8"}
                        },
                        Body = new
                        {
                            state = AdvertisementState.Expired.ToString()
                        }
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
                            errors = new[] {
                                new { code = "InvalidState", message = "Advertisement has already expired." }
                            }
                        }
                    });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);
            ValidationException exception = Assert.Throws<ValidationException>(
                async () => { await client.ExpireAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), new AdvertisementPatch { State = AdvertisementState.Expired }); });

            exception.ValidationDataItems.ShouldBe(new ValidationData { Code = "InvalidState" });
        }

        [Test]
        public void ExpireNonExistentAdvertisment()
        {
            var advertisementId = new Guid("9b650105-7434-473f-8293-4e23b7e0e064");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockLinks();

            PactProvider.MockService
                .Given($"There isn't an advertisement with id: '{advertisementId}'")
                .UponReceiving("An expire request for advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", "application/vnd.seek.advertisement-patch+json; charset=utf-8"}
                        },
                        Body = new
                        {
                            state = AdvertisementState.Expired.ToString()
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 404
                    });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);
            var exception = Assert.Throws<ResourceActionException>(
                async () => await client.ExpireAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), new AdvertisementPatch { State = AdvertisementState.Expired }));

            Assert.AreEqual(HttpStatusCode.NotFound, exception.StatusCode);
        }
    }
}