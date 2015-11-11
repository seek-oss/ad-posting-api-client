using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    [TestFixture]
    public class ExpireAdTests
    {
        private const string AdvertisementLink = "/advertisement";

        private IBuilderInitializer AllFieldsInitializer => new AllFieldsInitializer();

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
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";

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
                            .WithResponseLink("view", viewRenderedAdvertisementLink)
                            .Build()
                    });

            AdvertisementResource result;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                result = await client.ExpireAdvertisementAsync(
                    new Uri(PactProvider.MockServiceUri, link), new AdvertisementPatch { State = AdvertisementState.Expired });
            }

            var expectedResult = new AdvertisementResource
            {
                Links = new Links(PactProvider.MockServiceUri)
                {
                    { "self", new Link { Href = link } },
                    { "view", new Link { Href = viewRenderedAdvertisementLink } }
                }
            };

            new AdvertisementModelBuilder(AllFieldsInitializer, expectedResult).WithAgentId(null).Build();

            result.ShouldBeEquivalentTo(expectedResult);
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

            ValidationException actualException;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<ValidationException>(
                    async () => await client.ExpireAdvertisementAsync(
                        new Uri(PactProvider.MockServiceUri, link), new AdvertisementPatch { State = AdvertisementState.Expired }));
            }

            var expectedException = new ValidationException(
                new HttpMethod("PATCH"),
                new ValidationMessage
                {
                    Message = "Validation Failure",
                    Errors = new[] { new ValidationData { Code = "InvalidState", Message = "Advertisement has already expired." } }
                });

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

            AdvertisementNotFoundException actualException;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<AdvertisementNotFoundException>(
                    async () => await client.ExpireAdvertisementAsync(
                        new Uri(PactProvider.MockServiceUri, link), new AdvertisementPatch { State = AdvertisementState.Expired }));
            }

            actualException.ShouldBeEquivalentToException(new AdvertisementNotFoundException());
        }

        [Test]
        public void ExpireAdvertisementNotPermitted()
        {
            var advertisementId = new Guid("8e2fde50-bc5f-4a12-9cfb-812e50500184");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_Disabled).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There is a pending standout advertisement with maximum data and id '{advertisementId}'")
                .UponReceiving("An unauthorised expire request for an advertisement")
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
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/json; charset=utf-8" }
                        },
                        Body = new { message = "Operation not permitted on advertisement with advertiser id: '9012'" }
                    });

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException = Assert.Throws<UnauthorizedException>(
                    async () => await client.ExpireAdvertisementAsync(
                        new Uri(PactProvider.MockServiceUri, link), new AdvertisementPatch { State = AdvertisementState.Expired }));
            }

            actualException.ShouldBeEquivalentToException(new UnauthorizedException("Operation not permitted on advertisement with advertiser id: '9012'"));
        }

        private AdPostingApiClient GetClient(OAuth2Token token)
        {
            var oAuthClient = Mock.Of<IOAuth2TokenClient>(c => c.GetOAuth2TokenAsync() == Task.FromResult(token));

            return new AdPostingApiClient(PactProvider.MockServiceUri, oAuthClient);
        }
    }
}