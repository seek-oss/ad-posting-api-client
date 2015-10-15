using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
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

        private IBuilderInitializer MinimumFieldsInitializer => new MinimumFieldsInitializer();

        private IBuilderInitializer AllFieldsInitializer => new AllFieldsInitializer();

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
                .Given($"There is a pending standout advertisement with maximum data and id '{advertisementId}'")
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
                        .WithResponseErrors(new { code = "Unauthorised", message = "Unauthorised" })
                        .Build()
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            var jobAd = await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link));

            Assert.AreEqual("Exciting Senior Developer role in a great CBD location. Great $$$", jobAd.Properties.JobTitle, "Wrong job title returned!");
            Assert.AreEqual(Status.Pending, jobAd.Status);
            jobAd.Properties.Errors.ShouldAllBeEquivalentTo(new[] { new AdvertisementError { Code = "Unauthorised", Message = "Unauthorised" } });
        }

        [Test]
        public async Task GetExistingAdvertisementWithWarnings()
        {
            const string advertisementId = "448b8474-6165-4eed-a5b5-d2bb52e471ef";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There is an advertisement with warnings and id '{advertisementId}'")
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
                    Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                        .WithState(AdvertisementState.Pending.ToString())
                        .WithResponseLink("self", link)
                        .WithResponseWarnings(
                            new { field = "standout.logoId", code = "missing" },
                            new { field = "standout.bullets", code = "missing" })
                        .Build()
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            var jobAd = await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link));

            Assert.AreEqual("Exciting Senior Developer role in a great CBD location. Great $$$", jobAd.Properties.JobTitle, "Wrong job title returned!");
            jobAd.Properties.Warnings.ShouldAllBeEquivalentTo(new[] { new ValidationData { Field = "standout.logoId", Code = "missing" }, new ValidationData { Field = "standout.bullets", Code = "missing" } });
        }

        [Test]
        public async Task GetExistingAdvertisementStatus()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            var oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There is a pending standout advertisement with maximum data and id '{advertisementId}'")
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
                .Given($"There is a pending standout advertisement with maximum data and id '{advertisementId}'")
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
                .UponReceiving("GET request for a non-existent advertisement")
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

            HttpRequestException exception = Assert.Throws<HttpRequestException>(
                async () => await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link)));

            StringAssert.Contains("404 (Not Found)", exception.Message);
        }
    }
}