using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using Xunit;

namespace SEEK.AdPostingApi.Client.Tests
{
    [Collection(AdPostingApiCollection.Name)]
    public class GetAllAdTests : IDisposable
    {
        public GetAllAdTests(AdPostingApiPactService adPostingApiPactService)
        {
            this.Fixture = new AdPostingApiFixture(adPostingApiPactService);
        }

        public void Dispose()
        {
            this.Fixture.Dispose();
        }

        [Fact]
        public async Task GetAllAdvertisementsWithNoAdvertisementReturns()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .Given("There are no advertisements")
                .UponReceiving("GET request for all advertisements")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/advertisement",
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Accept", "application/hal+json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8"}
                    },
                    Body = new
                    {
                        _embedded = new { advertisements = new AdvertisementSummary[] { } },
                        _links = new { self = new { href = "/advertisement" } }
                    }
                });

            AdvertisementSummaryPageResource advertisements;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                advertisements = await client.GetAllAdvertisementsAsync();
            }

            AdvertisementSummaryPageResource expectedAdvertisements = new AdvertisementSummaryPageResource
            {
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri) { { "self", new Link { Href = "/advertisement" } } },
                AdvertisementSummaries = new List<AdvertisementSummaryResource>()
            };

            advertisements.ShouldBeEquivalentTo(expectedAdvertisements);
        }

        [Fact]
        public async Task GetAllAdvertisementsFirstPage()
        {
            const string advertisementId3 = "7bbe4318-fd3b-4d26-8384-d41489ff1dd0";
            const string advertisementId4 = "9141cf19-b8d7-4380-9e3f-3b5c22783bdc";
            const string advertisementJobId2 = "4";
            const string nextLink = "/advertisement?beforeId=" + advertisementJobId2;
            const string selfLink = "/advertisement";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .Given("A page size of 2, and there are 2 pages worth of data")
                .UponReceiving("GET request for first page of data")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/advertisement",
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Accept", "application/hal+json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8"}
                    },
                    Body = new
                    {
                        _embedded = new
                        {
                            advertisements = new[]
                            {
                                new AdvertisementSummaryContentBuilder()
                                    .WithAdvertiserId("678")
                                    .WithJobTitle("More Exciting Senior Developer role in a great CBD location. Great $$$")
                                    .WithJobReference("JOB12347")
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId4))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId4))
                                    .Build(),
                                new AdvertisementSummaryContentBuilder()
                                    .WithAdvertiserId("456")
                                    .WithJobTitle("Exciting Developer role in a great CBD location. Great $$")
                                    .WithJobReference("JOB1236")
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId3))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId3))
                                    .Build()
                            }
                        },
                        _links = new
                        {
                            self = new { href = selfLink },
                            next = new { href = nextLink }
                        }
                    }
                });

            AdvertisementSummaryPageResource pageResource;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                pageResource = await client.GetAllAdvertisementsAsync();
            }

            AdvertisementSummaryPageResource expectedPageResource = new AdvertisementSummaryPageResource
            {
                AdvertisementSummaries = new List<AdvertisementSummaryResource>
                {
                    new AdvertisementSummaryResource
                    {
                        AdvertiserId = "678",
                        JobReference = "JOB12347",
                        JobTitle = "More Exciting Senior Developer role in a great CBD location. Great $$$",
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                        {
                            { "self", new Link { Href = $"/advertisement/{advertisementId4}" } },
                            { "view", new Link { Href = $"/advertisement/{advertisementId4}/view" } }
                        }
                    },
                    new AdvertisementSummaryResource
                    {
                        AdvertiserId = "456",
                        JobReference = "JOB1236",
                        JobTitle = "Exciting Developer role in a great CBD location. Great $$",
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                        {
                            { "self", new Link { Href = $"/advertisement/{advertisementId3}" } },
                            { "view", new Link { Href = $"/advertisement/{advertisementId3}/view" } }
                        }
                    }
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = "/advertisement" } },
                    { "next", new Link { Href = "/advertisement?beforeId=4"} }
                }
            };

            pageResource.ShouldBeEquivalentTo(expectedPageResource);
        }

        [Fact]
        public async Task GetAllAdvertisementsNextPage()
        {
            const string advertisementId1 = "fa6939b5-c91f-4f6a-9600-1ea74963fbb2";
            const string advertisementId2 = "e6e31b9c-3c2c-4b85-b17f-babbf7da972b";
            const string advertisementJobId2 = "4";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.AdPostingApiService
                .Given("A page size of 2, and there are 2 pages worth of data")
                .UponReceiving("GET request for second page of data")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/advertisement",
                    Query = "beforeId=" + advertisementJobId2,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Accept", "application/hal+json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8"}
                    },
                    Body = new
                    {
                        _embedded = new
                        {
                            advertisements = new[]
                            {
                                new AdvertisementSummaryContentBuilder()
                                    .WithAdvertiserId("345")
                                    .WithJobTitle(
                                        "More Exciting Senior Developer role in a great CBD location. Great $$$")
                                    .WithJobReference("JOB12345")
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId2))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId2))
                                    .Build(),
                                new AdvertisementSummaryContentBuilder()
                                    .WithAdvertiserId("123")
                                    .WithJobTitle("Exciting Developer role in a great CBD location. Great $$")
                                    .WithJobReference("JOB1234")
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId1))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId1))
                                    .Build()
                            }
                        },
                        _links = new
                        {
                            self = new { href = $"/advertisement?beforeId={advertisementJobId2}" }
                        }
                    }
                });

            AdvertisementSummaryPageResource pageResource = new AdvertisementSummaryPageResource
            {
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    {"self", new Link {Href = "/advertisement"}},
                    {"next", new Link {Href = $"/advertisement?beforeId={advertisementJobId2}"}}
                }
            };

            var oAuthClient = Mock.Of<IOAuth2TokenClient>(c => c.GetOAuth2TokenAsync() == Task.FromResult(oAuth2Token));
            AdvertisementSummaryPageResource nextPageResource;

            using (var client = new Client.Hal.Client(new HttpClient(new OAuthMessageHandler(oAuthClient))))
            {
                pageResource.Initialise(client);

                nextPageResource = await pageResource.NextPageAsync();
            }

            AdvertisementSummaryPageResource expectedNextPageResource = new AdvertisementSummaryPageResource
            {
                AdvertisementSummaries = new List<AdvertisementSummaryResource>
                {
                    new AdvertisementSummaryResource
                    {
                        AdvertiserId = "345",
                        JobReference = "JOB12345",
                        JobTitle = "More Exciting Senior Developer role in a great CBD location. Great $$$",
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                        {
                            {"self", new Link {Href = $"/advertisement/{advertisementId2}"}},
                            {"view", new Link {Href = $"/advertisement/{advertisementId2}/view"}}
                        }
                    },
                    new AdvertisementSummaryResource
                    {
                        AdvertiserId = "123",
                        JobReference = "JOB1234",
                        JobTitle = "Exciting Developer role in a great CBD location. Great $$",
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                        {
                            {"self", new Link {Href = $"/advertisement/{advertisementId1}"}},
                            {"view", new Link {Href = $"/advertisement/{advertisementId1}/view"}}
                        }
                    }
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    {"self", new Link {Href = $"/advertisement?beforeId={advertisementJobId2}"}}
                }
            };

            nextPageResource.ShouldBeEquivalentTo(expectedNextPageResource);
        }

        [Fact]
        public async Task GetAllAdvertisementsNoNextPage()
        {
            AdvertisementSummaryPageResource pageResource = new AdvertisementSummaryPageResource
            {
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = "/advertisement" } }
                }
            };

            var actualException = await Assert.ThrowsAsync<NotSupportedException>(async () => await pageResource.NextPageAsync());
            var expectedException = new NotSupportedException("There are no more results");

            actualException.ShouldBeEquivalentToException(expectedException);
        }

        private string GenerateSelfLink(string advertisementId)
        {
            return "/advertisement/" + advertisementId;
        }

        private string GenerateViewLink(string advertisementId)
        {
            return "/advertisement/" + advertisementId + "/view";
        }

        private AdPostingApiFixture Fixture { get; }
    }
}