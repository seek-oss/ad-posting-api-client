using System;
using System.Collections.Generic;
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
    public class GetAdTests
    {
        private const string AdvertisementLink = "/advertisement";

        private IBuilderInitializer MinimumFieldsInitializer => new MinimumFieldsInitializer();

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
        public async Task GetExistingAdvertisement()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";

            PactProvider.MockService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("GET request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", "application/vnd.seek.advertisement+json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8" },
                        { "Processing-Status", "Pending" }
                    },
                    Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                        .WithAgentId(null)
                        .WithState(AdvertisementState.Open.ToString())
                        .WithAdditionalProperties(AdditionalPropertyType.ResidentsOnly.ToString(), AdditionalPropertyType.Graduate.ToString())
                        .WithResponseLink("self", link)
                        .WithResponseLink("view", viewRenderedAdvertisementLink)
                        .Build()
                });

            AdvertisementResource result;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                result = await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link));
            }

            var expectedResult = new AdvertisementResource
            {
                Links = new Links(PactProvider.MockServiceUri)
                {
                    { "self", new Link { Href = link } },
                    { "view", new Link { Href = viewRenderedAdvertisementLink } }
                },
                ProcessingStatus = ProcessingStatus.Pending
            };

            new AdvertisementModelBuilder(AllFieldsInitializer, expectedResult).WithAgentId(null).Build();

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GetExistingAdvertisementWithWarnings()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";

            PactProvider.MockService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("GET request for advertisement with warnings")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", "application/vnd.seek.advertisement+json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8" },
                        { "Processing-Status", "Pending" }
                    },
                    Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                        .WithAgentId(null)
                        .WithState(AdvertisementState.Open.ToString())
                        .WithAdditionalProperties(AdditionalPropertyType.ResidentsOnly.ToString(), AdditionalPropertyType.Graduate.ToString())
                        .WithResponseWarnings(
                            new { field = "standout.logoId", code = "missing" },
                            new { field = "standout.bullets", code = "missing" })
                        .Build()
                });

            AdvertisementResource result;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                result = await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link));
            }

            ValidationData[] expectedWarnings =
            {
                new ValidationData { Field = "standout.logoId", Code = "missing" },
                new ValidationData { Field = "standout.bullets", Code = "missing" }
            };

            var expectedResult = new AdvertisementResource
            {
                Links = new Links(PactProvider.MockServiceUri)
                {
                    { "self", new Link { Href = link } },
                    { "view", new Link { Href = viewRenderedAdvertisementLink } }
                },
                Warnings = expectedWarnings,
                ProcessingStatus = ProcessingStatus.Pending
            };

            new AdvertisementModelBuilder(AllFieldsInitializer, expectedResult).WithAgentId(null).Build();

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GetExistingAdvertisementWithErrors()
        {
            const string advertisementId = "448b8474-6165-4eed-a5b5-d2bb52e471ef";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";

            PactProvider.MockService
                .Given("There is a failed classic advertisement")
                .UponReceiving("GET request for advertisement with errors")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", "application/vnd.seek.advertisement+json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8" },
                        { "Processing-Status", "Failed" }
                    },
                    Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                        .WithAgentId(null)
                        .WithState(AdvertisementState.Open.ToString())
                        .WithResponseLink("self", link)
                        .WithResponseLink("view", viewRenderedAdvertisementLink)
                        .WithResponseErrors(new { code = "Unauthorised", message = "Unauthorised" })
                        .Build()
                });

            AdvertisementResource result;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                result = await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link));
            }

            AdvertisementError[] expectedErrors = { new AdvertisementError { Code = "Unauthorised", Message = "Unauthorised" } };
            var expectedResult = new AdvertisementResource
            {
                Links = new Links(PactProvider.MockServiceUri)
                {
                    { "self", new Link { Href = link } },
                    { "view", new Link { Href = viewRenderedAdvertisementLink } }
                },
                Errors = expectedErrors,
                ProcessingStatus = ProcessingStatus.Failed
            };

            new AdvertisementModelBuilder(MinimumFieldsInitializer, expectedResult).WithAgentId(null).Build();

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetNonExistentAdvertisement()
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
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", "application/vnd.seek.advertisement+json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse { Status = 404 });

            AdvertisementNotFoundException actualException;

            using (AdPostingApiClient client = this.GetClient(oAuth2Token))
            {
                actualException =
                    Assert.Throws<AdvertisementNotFoundException>(
                        async () => await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link)));
            }

            actualException.ShouldBeEquivalentToException(new AdvertisementNotFoundException());
        }

        [Test]
        [Ignore("To be implemented")]
        public void GetExistingAdvertisementNotPermitted()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_Disabled).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given("There is a pending standout advertisement")
                .UponReceiving("Unauthorised GET request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", "application/vnd.seek.advertisement+json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
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
                    async () => await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link)));
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