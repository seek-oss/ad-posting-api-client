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
    public class GetAdTests : IDisposable
    {
        private readonly IOAuth2TokenClient _oauthClient;
        private const string AdvertisementLink = "/advertisement";

        private IBuilderInitializer MinimumFieldsInitializer => new MinimumFieldsInitializer();

        private IBuilderInitializer AllFieldsInitializer => new AllFieldsInitializer();

        public GetAdTests()
        {
            this._oauthClient = Mock.Of<IOAuth2TokenClient>(c => c.GetOAuth2TokenAsync() == Task.FromResult(new OAuth2TokenBuilder().Build()));
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
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";

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
                        {"Processing-Status", "Pending"}
                    },
                    Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                        .WithoutAgentId()
                        .WithState(AdvertisementState.Open.ToString())
                        .WithAdditionalProperties(AdditionalPropertyType.ResidentsOnly.ToString())
                        .WithResponseLink("self", link)
                        .WithResponseLink("view", viewRenderedAdvertisementLink)
                        .Build()
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            GetAdvertisementResult result = await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link));

            var expectedResult = new GetAdvertisementResult(
                new AdvertisementResource
                {
                    Links = new Dictionary<string, Link>
                    {
                        { "self", new Link { Href = link } },
                        { "view", new Link { Href = viewRenderedAdvertisementLink } }
                    },
                    Properties = new AdvertisementModelBuilder(AllFieldsInitializer).WithAgentId(null).Build(),
                    ResponseHeaders = new HttpResponseMessage().Headers
                });

            expectedResult.AdvertisementResource.ResponseHeaders.Add("Processing-Status", "Pending");
            expectedResult.AdvertisementResource.ResponseHeaders.Add("Date", result.AdvertisementResource.ResponseHeaders.GetValues("Date"));
            expectedResult.AdvertisementResource.ResponseHeaders.Add("Server", result.AdvertisementResource.ResponseHeaders.GetValues("Server"));

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
                .Given($"There is a pending standout advertisement with maximum data and id '{advertisementId}'")
                .UponReceiving("GET request for advertisement with warnings")
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
                        {"Processing-Status", "Pending"}
                    },
                    Body = new AdvertisementContentBuilder(AllFieldsInitializer)
                        .WithoutAgentId()
                        .WithState(AdvertisementState.Open.ToString())
                        .WithAdditionalProperties(AdditionalPropertyType.ResidentsOnly.ToString())
                        .WithResponseLink("self", link)
                        .WithResponseLink("view", viewRenderedAdvertisementLink)
                        .WithResponseWarnings(
                            new { field = "standout.logoId", code = "missing" },
                            new { field = "standout.bullets", code = "missing" })
                        .Build()
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            GetAdvertisementResult result = await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link));

            ValidationData[] expectedWarnings = { new ValidationData { Field = "standout.logoId", Code = "missing" }, new ValidationData { Field = "standout.bullets", Code = "missing" } };
            var expectedResult = new GetAdvertisementResult(
                new AdvertisementResource
                {
                    Links = new Dictionary<string, Link>
                    {
                        { "self", new Link { Href = link } },
                        { "view", new Link { Href = viewRenderedAdvertisementLink } }
                    },
                    Properties = new AdvertisementModelBuilder(AllFieldsInitializer).WithAgentId(null).WithWarnings(expectedWarnings).Build(),
                    ResponseHeaders = new HttpResponseMessage().Headers
                });

            expectedResult.AdvertisementResource.ResponseHeaders.Add("Processing-Status", "Pending");
            expectedResult.AdvertisementResource.ResponseHeaders.Add("Date", result.AdvertisementResource.ResponseHeaders.GetValues("Date"));
            expectedResult.AdvertisementResource.ResponseHeaders.Add("Server", result.AdvertisementResource.ResponseHeaders.GetValues("Server"));
        }

        [Test]
        public async Task GetExistingAdvertisementWithErrors()
        {
            const string advertisementId = "448b8474-6165-4eed-a5b5-d2bb52e471ef";

            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";
            var viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";

            PactProvider.MockService
                .Given($"There is a failed classic advertisement with id '{advertisementId}'")
                .UponReceiving("GET request for advertisement with errors")
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
                        {"Processing-Status", "Failed"}
                    },
                    Body = new AdvertisementContentBuilder(MinimumFieldsInitializer)
                        .WithoutAgentId()
                        .WithState(AdvertisementState.Open.ToString())
                        .WithResponseLink("self", link)
                        .WithResponseLink("view", viewRenderedAdvertisementLink)
                        .WithResponseErrors(new { code = "Unauthorised", message = "Unauthorised" })
                        .Build()
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            GetAdvertisementResult result = await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link));

            AdvertisementError[] expectedErrors = { new AdvertisementError { Code = "Unauthorised", Message = "Unauthorised" } };
            var expectedResult = new GetAdvertisementResult(
                new AdvertisementResource
                {
                    Links = new Dictionary<string, Link>
                    {
                        { "self", new Link { Href = link } },
                        { "view", new Link { Href = viewRenderedAdvertisementLink } }
                    },
                    Properties = new AdvertisementModelBuilder(MinimumFieldsInitializer).WithAgentId(null).WithErrors(expectedErrors).Build(),
                    ResponseHeaders = new HttpResponseMessage().Headers
                });

            expectedResult.AdvertisementResource.ResponseHeaders.Add("Processing-Status", "Failed");
            expectedResult.AdvertisementResource.ResponseHeaders.Add("Date", result.AdvertisementResource.ResponseHeaders.GetValues("Date"));
            expectedResult.AdvertisementResource.ResponseHeaders.Add("Server", result.AdvertisementResource.ResponseHeaders.GetValues("Server"));
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
                        {"Processing-Status", "Pending"}
                    }
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            ProcessingStatus status = await client.GetAdvertisementStatusAsync(new Uri(PactProvider.MockServiceUri, link));

            Assert.AreEqual(ProcessingStatus.Pending, status);
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

            var expectedException = new AdvertisementNotFoundException();
            var actualException = Assert.Throws<AdvertisementNotFoundException>(async () => await client.GetAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link)));

            actualException.ShouldBeEquivalentToException(expectedException);
        }
    }
}