using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GetAllAdTests : IDisposable
    {
        private readonly IOAuth2TokenClient _oauthClient;

        public GetAllAdTests()
        {
            this._oauthClient = Mock.Of<IOAuth2TokenClient>(
                c => c.GetOAuth2TokenAsync() == Task.FromResult(new OAuth2TokenBuilder().Build()));
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
        public async Task GetAllAdvertisementsWithNoAdvertisementReturns()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.RegisterIndexPageInteractions();

            PactProvider.MockService
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
                        _embedded = new
                        {
                            advertisements = new AdvertisementSummary[] { }
                        }
                    }
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            var advertisements = await client.GetAllAdvertisementsAsync();

            Assert.IsEmpty(advertisements);
        }

        [Test]
        public async Task GetAllAdvertisementsByFollowingNextLink()
        {
            const string advertisementId1 = "fa6939b5-c91f-4f6a-9600-1ea74963fbb2";
            const string advertisementId2 = "e6e31b9c-3c2c-4b85-b17f-babbf7da972b";
            const string advertisementJobId2 = "4";
            const string nextLink = "/advertisement?beforeId=" + advertisementJobId2;
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.RegisterIndexPageInteractions();

            PactProvider.MockService
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
                                new
                                {
                                    advertiserId = "0004",
                                    jobTitle = "More Exciting Senior Developer role in a great CBD location. Great $$$",
                                    jobReference = "JOB12347",
                                    _links = new
                                    {
                                        self = new
                                        {
                                            href = "/advertisement/9141cf19-b8d7-4380-9e3f-3b5c22783bdc"
                                        },
                                        view = new
                                        {
                                            href = "/advertisement/9141cf19-b8d7-4380-9e3f-3b5c22783bdc/view"
                                        }
                                    }
                                },
                                new
                                {
                                    advertiserId = "0003",
                                    jobTitle = "Exciting Developer role in a great CBD location. Great $$",
                                    jobReference = "JOB1236",
                                    _links = new
                                    {
                                        self = new
                                        {
                                            href = "/advertisement/7bbe4318-fd3b-4d26-8384-d41489ff1dd0"
                                        },
                                        view = new
                                        {
                                            href = "/advertisement/7bbe4318-fd3b-4d26-8384-d41489ff1dd0/view"
                                        }
                                    }
                                }
                            }
                        },
                        _links = new
                        {
                            self = new { href = "/advertisement" },
                            next = new { href = nextLink }
                        }
                    }
                });

            PactProvider.MockService
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
                                new
                                {
                                    advertiserId = "0002",
                                    jobTitle = "More Exciting Senior Developer role in a great CBD location. Great $$$",
                                    jobReference = "JOB12345",
                                    _links = new
                                    {
                                        self = new
                                        {
                                            href = "/advertisement/" + advertisementId2
                                        },
                                        view = new
                                        {
                                            href = "/advertisement/" + advertisementId2 + "/view"
                                        }
                                    }
                                },
                                new
                                {
                                    advertiserId = "0001",
                                    jobTitle = "Exciting Developer role in a great CBD location. Great $$",
                                    jobReference = "JOB1234",
                                    _links = new
                                    {
                                        self = new
                                        {
                                            href = "/advertisement/" + advertisementId1
                                        },
                                        view = new
                                        {
                                            href = "/advertisement/" + advertisementId1 + "/view"
                                        }
                                    }
                                }
                            }
                        },
                        _links = new
                        {
                            self = new { href = nextLink }
                        }
                    }
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            var allAdvertisements = new List<AdvertisementSummaryResource>();
            AdvertisementSummaryPageResource pageResource = await client.GetAllAdvertisementsAsync();

            Assert.AreEqual(2, pageResource.Count());

            allAdvertisements.AddRange(pageResource);

            while (!pageResource.Eof)
            {
                pageResource = await pageResource.NextPageAsync();
                allAdvertisements.AddRange(pageResource);
            }

            Assert.AreEqual(4, allAdvertisements.Count);

            var actualException = Assert.Throws<NotSupportedException>(async () => await pageResource.NextPageAsync());
            var expectedException = new NotSupportedException("There are no more results");

            actualException.ShouldBeEquivalentToException(expectedException);
        }
    }
}