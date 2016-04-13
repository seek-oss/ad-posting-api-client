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
            const string advertisementId3 = "9141cf19-b8d7-4380-9e3f-3b5c22783bdc";
            const string advertisementId2 = "7bbe4318-fd3b-4d26-8384-d41489ff1dd0";
            const string advertisementId1 = "e6e31b9c-3c2c-4b85-b17f-babbf7da972b";
            const string advertisement3Title = "More Exciting Senior Developer role in a great CBD location. Great $$$";
            const string advertisement2Title = "More Exciting Senior Tester role in a great CBD location. Great $$$";
            const string advertisement1Title = "More Exciting Senior Developer role in a great CBD location. Great $$$";
            const string advertisement3Reference = "JOB4444";
            const string advertisement2Reference = "JOB3333";
            const string advertisement1Reference = "JOB12345";
            const string beforeJobId = "6";
            const string nextLink = "/advertisement?beforeId=" + beforeJobId;
            const string selfLink = "/advertisement";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .Given("A page size of 3, and there are more than 1 page worth of data")
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
                                    .WithAdvertiserId("456")
                                    .WithJobTitle(advertisement3Title)
                                    .WithJobReference(advertisement3Reference)
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId3))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId3))
                                    .Build(),
                                new AdvertisementSummaryContentBuilder()
                                    .WithAdvertiserId("456")
                                    .WithJobTitle(advertisement2Title)
                                    .WithJobReference(advertisement2Reference)
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId2))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId2))
                                    .Build(),
                                new AdvertisementSummaryContentBuilder()
                                    .WithAdvertiserId("345")
                                    .WithJobTitle(advertisement1Title)
                                    .WithJobReference(advertisement1Reference)
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId1))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId1))
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
                        AdvertiserId = "456",
                        JobReference = advertisement3Reference,
                        JobTitle = advertisement3Title,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                        {
                            { "self", new Link { Href = $"/advertisement/{advertisementId3}" } },
                            { "view", new Link { Href = $"/advertisement/{advertisementId3}/view" } }
                        }
                    },
                    new AdvertisementSummaryResource
                    {
                        AdvertiserId = "456",
                        JobReference = advertisement2Reference,
                        JobTitle = advertisement2Title,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                        {
                            { "self", new Link { Href = $"/advertisement/{advertisementId2}" } },
                            { "view", new Link { Href = $"/advertisement/{advertisementId2}/view" } }
                        }
                    },
                    new AdvertisementSummaryResource
                    {
                        AdvertiserId = "345",
                        JobReference = advertisement1Reference,
                        JobTitle = advertisement1Title,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                        {
                            { "self", new Link { Href = $"/advertisement/{advertisementId1}" } },
                            { "view", new Link { Href = $"/advertisement/{advertisementId1}/view" } }
                        }
                    }
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = "/advertisement" } },
                    { "next", new Link { Href = "/advertisement?beforeId=" + beforeJobId} }
                }
            };

            pageResource.ShouldBeEquivalentTo(expectedPageResource);
        }

        [Fact]
        public async Task GetAllAdvertisementsNextPage()
        {
            const string advertisementId1 = "fa6939b5-c91f-4f6a-9600-1ea74963fbb2";
            const string advertisementId2 = "f7302df2-704b-407c-a42a-62ff822b5461";
            const string advertisementId3 = "3b138935-f65b-4ec7-91d8-fc250757b53d";
            const string beforeJobId = "6";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.AdPostingApiService
                .Given("A page size of 3, and there are more than 1 page worth of data")
                .UponReceiving("GET request for the last page of data")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/advertisement",
                    Query = "beforeId=" + beforeJobId,
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
                                    .WithAdvertiserId("456")
                                    .WithJobTitle("Exciting tester role in a great CBD location. Great $$")
                                    .WithJobReference("JOB2222")
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId3))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId3))
                                    .Build(),
                                new AdvertisementSummaryContentBuilder()
                                    .WithAdvertiserId("456")
                                    .WithJobTitle("Exciting Developer role in a great CBD location. Great $$")
                                    .WithJobReference("JOB1111")
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
                            self = new { href = $"/advertisement?beforeId={beforeJobId}" }
                        }
                    }
                });

            AdvertisementSummaryPageResource pageResource = new AdvertisementSummaryPageResource
            {
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    {"self", new Link {Href = "/advertisement"}},
                    {"next", new Link {Href = $"/advertisement?beforeId={beforeJobId}"}}
                }
            };

            var oAuthClient = Mock.Of<IOAuth2TokenClient>(c => c.GetOAuth2TokenAsync() == Task.FromResult(oAuth2Token));
            AdvertisementSummaryPageResource nextPageResource;

            using (var client = new Hal.Client(new HttpClient(new OAuthMessageHandler(oAuthClient))))
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
                        AdvertiserId = "456",
                        JobReference = "JOB2222",
                        JobTitle = "Exciting tester role in a great CBD location. Great $$",
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                        {
                            {"self", new Link {Href = $"/advertisement/{advertisementId3}"}},
                            {"view", new Link {Href = $"/advertisement/{advertisementId3}/view"}}
                        }
                    },
                    new AdvertisementSummaryResource
                    {
                        AdvertiserId = "456",
                        JobReference = "JOB1111",
                        JobTitle = "Exciting Developer role in a great CBD location. Great $$",
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
                    {"self", new Link {Href = $"/advertisement?beforeId={beforeJobId}"}}
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

        [Fact]
        public async Task GetAllAdvertisementsByAdvertiserFirstPage()
        {
            const string advertiser = "456";
            const string advertisementId3 = "9141cf19-b8d7-4380-9e3f-3b5c22783bdc";
            const string advertisementId2 = "7bbe4318-fd3b-4d26-8384-d41489ff1dd0";
            const string advertisementId1 = "3b138935-f65b-4ec7-91d8-fc250757b53d";
            const string advertisement3Title = "More Exciting Senior Developer role in a great CBD location. Great $$$";
            const string advertisement2Title = "More Exciting Senior Tester role in a great CBD location. Great $$$";
            const string advertisement1Title = "Exciting tester role in a great CBD location. Great $$";
            const string advertisement3Reference = "JOB4444";
            const string advertisement2Reference = "JOB3333";
            const string advertisement1Reference = "JOB2222";
            const string advertisementJobId2 = "5";
            const string queryString = "advertiserId=" + advertiser;
            const string selfLink = "/advertisement?" + queryString;
            const string nextLink = "/advertisement?" + queryString + "&beforeId=" + advertisementJobId2;
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .Given("A page size of 3, and there are more than 1 page worth of data")
                .UponReceiving("GET request for the first page of advertisements belong to the advertiser")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/advertisement",
                    Query = queryString,
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
                                    .WithAdvertiserId(advertiser)
                                    .WithJobTitle(advertisement3Title)
                                    .WithJobReference(advertisement3Reference)
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId3))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId3))
                                    .Build(),
                               new AdvertisementSummaryContentBuilder()
                                    .WithAdvertiserId(advertiser)
                                    .WithJobTitle(advertisement2Title)
                                    .WithJobReference(advertisement2Reference)
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId2))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId2))
                                    .Build(),
                               new AdvertisementSummaryContentBuilder()
                                    .WithAdvertiserId(advertiser)
                                    .WithJobTitle(advertisement1Title)
                                    .WithJobReference(advertisement1Reference)
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId1))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId1))
                                    .Build(),
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
                pageResource = await client.GetAllAdvertisementsAsync(advertiser);
            }

            AdvertisementSummaryPageResource expectedPageResource = new AdvertisementSummaryPageResource
            {
                AdvertisementSummaries = new List<AdvertisementSummaryResource>
                {
                    new AdvertisementSummaryResource
                    {
                        AdvertiserId = advertiser,
                        JobReference = advertisement3Reference,
                        JobTitle = advertisement3Title,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                        {
                            { "self", new Link { Href = $"/advertisement/{advertisementId3}" } },
                            { "view", new Link { Href = $"/advertisement/{advertisementId3}/view" } }
                        }
                    },
                    new AdvertisementSummaryResource
                    {
                        AdvertiserId = advertiser,
                        JobReference = advertisement2Reference,
                        JobTitle = advertisement2Title,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                        {
                            { "self", new Link { Href = $"/advertisement/{advertisementId2}" } },
                            { "view", new Link { Href = $"/advertisement/{advertisementId2}/view" } }
                        }
                    },
                    new AdvertisementSummaryResource
                    {
                        AdvertiserId = advertiser,
                        JobReference = advertisement1Reference,
                        JobTitle = advertisement1Title,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                        {
                            { "self", new Link { Href = $"/advertisement/{advertisementId1}" } },
                            { "view", new Link { Href = $"/advertisement/{advertisementId1}/view" } }
                        }
                    },
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = selfLink } },
                    { "next", new Link { Href = nextLink } }
                }
            };

            pageResource.ShouldBeEquivalentTo(expectedPageResource);
        }

        [Fact]
        public async Task GetAllAdvertisementsByAdvertiserNextPage()
        {
            const string advertiserId = "456";
            const string advertisementId1 = "f7302df2-704b-407c-a42a-62ff822b5461";
            const string beforeJobId = "5";
            const string queryString = "advertiserId=" + advertiserId + "&beforeId=" + beforeJobId;
            const string selfLink = "/advertisement?" + queryString;
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.AdPostingApiService
                .Given("A page size of 3, and there are more than 1 page worth of data")
                .UponReceiving("GET request for the second page of advertisements belong to the advertiser")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/advertisement",
                    Query = queryString,
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
                                    .WithAdvertiserId(advertiserId)
                                    .WithJobTitle("Exciting Developer role in a great CBD location. Great $$")
                                    .WithJobReference("JOB1111")
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId1))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId1))
                                    .Build()
                            }
                        },
                        _links = new
                        {
                            self = new { href = selfLink }
                        }
                    }
                });

            AdvertisementSummaryPageResource pageResource = new AdvertisementSummaryPageResource
            {
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    {"self", new Link {Href = "/advertisement"}},
                    {"next", new Link {Href = $"/advertisement?advertiserId={advertiserId}&beforeId={beforeJobId}"}}
                }
            };

            var oAuthClient = Mock.Of<IOAuth2TokenClient>(c => c.GetOAuth2TokenAsync() == Task.FromResult(oAuth2Token));
            AdvertisementSummaryPageResource nextPageResource;

            using (var client = new Hal.Client(new HttpClient(new OAuthMessageHandler(oAuthClient))))
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
                        AdvertiserId = advertiserId,
                        JobReference = "JOB1111",
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
                    {"self", new Link {Href = selfLink}}
                }
            };

            nextPageResource.ShouldBeEquivalentTo(expectedNextPageResource);
        }

        [Fact]
        public async Task GetAllAdvertisementByAdvertiserWithNonExistentAdvertiserId()
        {
            string advertiser = Guid.NewGuid().ToString();
            string queryString = "advertiserId=" + advertiser;
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .UponReceiving("GET request to retrieve all advertisements for an advertiser not exists")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/advertisement",
                    Query = queryString,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Accept", "application/hal+json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 403,
                    Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8" }
                        },
                    Body = new
                    {
                        message = "Forbidden",
                        errors = new[]
                            {
                                new { code = "InvalidValue" }
                            }
                    }
                });

            UnauthorizedException actualException;
            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.GetAllAdvertisementsAsync(advertiser));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    new ForbiddenMessage
                    {
                        Message = "Forbidden",
                        Errors = new[] { new ForbiddenMessageData { Code = "InvalidValue" } }
                    }
                    ));
        }

        [Fact]
        public async Task GetAllAdvertisementsByAdvertiserReturnsRelationshipError()
        {
            var advertiserId = "874392";
            string queryString = "advertiserId=" + advertiserId;
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.Fixture.AdPostingApiService
                .Given("The advertiser account doesn't have relationship to any uploader")
                .UponReceiving("GET request to retrieve all advertisements for the advertiser not related to uploader")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/advertisement",
                    Query = queryString,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Accept", "application/hal+json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 403,
                    Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8" }
                        },
                    Body = new
                    {
                        message = "Forbidden",
                        errors = new[]
                            {
                                new { code = "RelationshipError" }
                            }
                    }
                });

            UnauthorizedException actualException;
            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.GetAllAdvertisementsAsync(advertiserId));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    new ForbiddenMessage
                    {
                        Message = "Forbidden",
                        Errors = new[] { new ForbiddenMessageData { Code = "RelationshipError" } }
                    }
                    ));
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