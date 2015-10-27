using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    [TestFixture]
    public class ExpireAdTests : IDisposable
    {
        private readonly IOAuth2TokenClient _oauthClient;

        private const string AdvertisementLink = "/advertisement";

        private IBuilderInitializer MinimumFieldsInitializer => new MinimumFieldsInitializer();

        private IBuilderInitializer AllFieldsInitializer => new AllFieldsInitializer();

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
        public async Task ExpireAdvertisement()
        {
            var advertisementId = new Guid("8e2fde50-bc5f-4a12-9cfb-812e50500184");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There is a pending standout advertisement with maximum data and id '{advertisementId}'")
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
                        Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                            .WithoutAgentId()
                            .WithState(AdvertisementState.Expired.ToString())
                            .WithAdditionalProperties(AdditionalPropertyType.ResidentsOnly.ToString())
                            .WithResponseLink("self", link)
                            .Build()
                    });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);
            AdvertisementResource jobAd = await client.ExpireAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), new AdvertisementPatch { State = AdvertisementState.Expired });

            Assert.AreEqual("9012", jobAd.Properties.AdvertiserId);
        }

        [Test]
        public void ExpireAlreadyExpiredAdvertisement()
        {
            var advertisementId = new Guid("dfd944df-e17d-45b5-8c86-0af43f9bae5d");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

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

            var expectedException = new ValidationException(
                new HttpMethod("PATCH"),
                new ValidationMessage()
                {
                    Message = "Validation Failure",
                    Errors = new[] { new ValidationData { Code = "InvalidState", Message = "Advertisement has already expired." } }
                });
            var actualException = Assert.Throws<ValidationException>(
                async () => await client.ExpireAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), new AdvertisementPatch { State = AdvertisementState.Expired }));

            actualException.ShouldBeEquivalentToException(expectedException);
        }

        [Test]
        public void ExpireNonExistentAdvertisment()
        {
            var advertisementId = new Guid("9b650105-7434-473f-8293-4e23b7e0e064");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .UponReceiving("An expire request for a non-existent advertisement")
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
            var expectedException = new AdvertisementNotFoundException();
            var actualException = Assert.Throws<AdvertisementNotFoundException>(
                async () => await client.ExpireAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), new AdvertisementPatch { State = AdvertisementState.Expired }));

            actualException.ShouldBeEquivalentToException(expectedException);
        }
    }
}