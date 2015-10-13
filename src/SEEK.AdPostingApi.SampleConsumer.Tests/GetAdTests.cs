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
    public class GetAdTests : IDisposable
    {
        private readonly IOAuth2TokenClient _oauthClient;
        private const string AdvertisementLink = "/advertisement";

        private AllFieldsInitializer AllFieldsInitializer => new AllFieldsInitializer();

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
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There is an advertisement with id: '{advertisementId}'")
                .UponReceiving("GET request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = link,
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
                    Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                        .WithoutAgentId()
                        .WithState(AdvertisementState.Pending.ToString())
                        .WithAdditionalProperties(AdditionalPropertyType.ResidentsOnly.ToString())
                        .WithResponseLink("self", link)
                        .Build()
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            var jobAd = await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link));
            Assert.AreEqual("Exciting Senior Developer role in a great CBD location. Great $$$", jobAd.Properties.JobTitle, "Wrong job title returned!");
            Assert.AreEqual(Status.Pending, jobAd.Status);
        }

        [Test]
        public async Task GetExistingAdvertisementStatus()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            var oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There is an advertisement with id: '{advertisementId}'")
                .UponReceiving("HEAD request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Head,
                    Path = link,
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

            var status = await client.GetAdvertisementStatusAsync(new Uri(PactProvider.MockServiceUri, link));
            Assert.AreEqual(Status.Pending, status);
        }

        [Test]
        public async Task GetExistingAdvertisementStatusUsingUri()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            var oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There is an advertisement with id: '{advertisementId}'")
                .UponReceiving("HEAD request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Head,
                    Path = link,
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

            var status = await client.GetAdvertisementStatusAsync(new Uri(PactProvider.MockServiceUri, link));
            Assert.AreEqual(Status.Pending, status);
        }

        [Test]
        public async Task GetNonExistentAdvertisement()
        {
            const string advertisementId = "9b650105-7434-473f-8293-4e23b7e0e064";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There isn't an advertisement with id: '{advertisementId}'")
                .UponReceiving("GET request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = link,
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
                await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link));
            }
            catch (Exception ex)
            {
                StringAssert.Contains("404 (Not Found)", ex.Message);
            }
        }
    }
}