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
        public async Task ExpireActiveAdvertisement()
        {
            var advertisementId = new Guid("8e2fde50-bc5f-4a12-9cfb-812e50500184");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.MockLinks();

            PactProvider.MockService
                .Given($"There is an advertisement with id: '{advertisementId}'")
                .UponReceiving("An expire request for advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = $"{AdvertisementLink}/{advertisementId}",
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
                            state = AdvertisementState.Expired.ToString(),
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
                            expiredDate = new DateTime(2015, 10, 7, 21, 19, 00, DateTimeKind.Utc),
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
            AdvertisementResource jobAd = await client.ExpireAdvertisementAsync(advertisementId, new AdvertisementPatch { State = AdvertisementState.Expired });

            Assert.AreEqual("9012", jobAd.Properties.AdvertiserId);
        }

        [Test]
        public void ExpireAlreadyExpiredAdvertisement()
        {
            var advertisementId = new Guid("4efd0bbd-c0b7-4c59-8b3d-4e5d97546181");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.MockLinks();

            PactProvider.MockService
                .Given($"There is an expired advertisement with id: '{advertisementId}'")
                .UponReceiving("An expire request for advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = $"{AdvertisementLink}/{advertisementId}",
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
                async () => { await client.ExpireAdvertisementAsync(advertisementId, new AdvertisementPatch { State = AdvertisementState.Expired }); });

            exception.ValidationDataItems.ShouldBe(new ValidationData { Code = "InvalidState" });
        }

        [Test]
        public void ExpireNonExistentAdvertisment()
        {
            var advertisementId = new Guid("9b650105-7434-473f-8293-4e23b7e0e064");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.MockLinks();

            PactProvider.MockService
                .Given($"There isn't an advertisement with id: '{advertisementId}'")
                .UponReceiving("An expire request for advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = $"{AdvertisementLink}/{advertisementId}",
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
                async () => await client.ExpireAdvertisementAsync(advertisementId, new AdvertisementPatch { State = AdvertisementState.Expired }));

            Assert.AreEqual(HttpStatusCode.NotFound, exception.StatusCode);
        }
    }
}