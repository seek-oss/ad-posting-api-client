using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GetAllAdTests : IDisposable
    {
        private readonly IOAuth2TokenClient _oauthClient;
        private IBuilderInitializer SummaryFieldsInitializer => new SummaryFieldsInitializer();

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

        private string GenerateSelfLink(string advertisementId)
        {
            return "/advertisement/" + advertisementId;
        }

        private string GenerateViewLink(string advertisementId)
        {
            return "/advertisement/" + advertisementId + "/view";
        }

        private void AssertAdvertisementLinks(string advertisementId, AdvertisementSummaryResource advertisementSummaryResult)
        {
            StringAssert.EndsWith(GenerateSelfLink(advertisementId), advertisementSummaryResult.Uri.ToString());
            Assert.AreEqual(GenerateSelfLink(advertisementId), advertisementSummaryResult.Links["self"].Href);
            Assert.AreEqual(GenerateViewLink(advertisementId), advertisementSummaryResult.Links["view"].Href);
        }

        private void AssertAdvertisementSummaryAndLinks(AdvertisementModelBuilder expectedAdFirst, AdvertisementModelBuilder expectedAdSecond, AdvertisementSummaryPageResource advertisementSummaryPage, string advertisementIdFirst, string advertisementIdSecond)
        {
            var advertisementSummaryResult = advertisementSummaryPage.GetEnumerator().Current;
            advertisementSummaryResult.Properties.ShouldBeEquivalentTo(expectedAdFirst);
            AssertAdvertisementLinks(advertisementIdFirst, advertisementSummaryResult);
            advertisementSummaryPage.GetEnumerator().MoveNext();
            advertisementSummaryResult = advertisementSummaryPage.GetEnumerator().Current;
            advertisementSummaryResult.Properties.ShouldBeEquivalentTo(expectedAdSecond);
            AssertAdvertisementLinks(advertisementIdSecond, advertisementSummaryResult);
        }

        [Test]
        [Explicit("Known issue with populating AdvertisementSummary object")]
        public async Task GetAllAdvertisementsByFollowingNextLink()
        {
            const string advertisementId1 = "fa6939b5-c91f-4f6a-9600-1ea74963fbb2";
            const string advertisementId2 = "e6e31b9c-3c2c-4b85-b17f-babbf7da972b";
            const string advertisementId3 = "7bbe4318-fd3b-4d26-8384-d41489ff1dd0";
            const string advertisementId4 = "9141cf19-b8d7-4380-9e3f-3b5c22783bdc";
            const string advertisementJobId2 = "4";
            const string nextLink = "/advertisement?beforeId=" + advertisementJobId2;
            const string selfLink = "/advertisement";
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
                                new AdvertisementContentBuilder(SummaryFieldsInitializer)
                                    .WithAdvertiserId("0004")
                                    .WithJobTitle("More Exciting Senior Developer role in a great CBD location. Great $$$")
                                    .WithJobReference("JOB12347")
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId4))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId4))
                                    .Build(),
                                new AdvertisementContentBuilder(SummaryFieldsInitializer)
                                    .WithAdvertiserId("0003")
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
                                new AdvertisementContentBuilder(SummaryFieldsInitializer)
                                    .WithAdvertiserId("0002")
                                    .WithJobTitle("More Exciting Senior Developer role in a great CBD location. Great $$$")
                                    .WithJobReference("JOB12345")
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId2))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId2))
                                    .Build(),
                                new AdvertisementContentBuilder(SummaryFieldsInitializer)
                                    .WithAdvertiserId("0001")
                                    .WithJobTitle("Exciting Developer role in a great CBD location. Great $$")
                                    .WithJobReference("JOB1234")
                                    .WithResponseLink("self", GenerateSelfLink(advertisementId1))
                                    .WithResponseLink("view", GenerateViewLink(advertisementId1))
                                    .Build()
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
            
            StringAssert.EndsWith(selfLink, pageResource.Uri.ToString());
            Assert.AreEqual(2, pageResource.Count());

            var expectedAdvertisement4 = new AdvertisementModelBuilder(SummaryFieldsInitializer)
                .WithAdvertiserId("0004")
                .WithJobTitle("More Exciting Senior Developer role in a great CBD location. Great $$$")
                .WithJobReference("JOB12347");
            var expectedAdvertisement3 = new AdvertisementModelBuilder(SummaryFieldsInitializer)
                .WithAdvertiserId("0003")
                .WithJobTitle("Exciting Developer role in a great CBD location. Great $$")
                .WithJobReference("JOB1236");

            AssertAdvertisementSummaryAndLinks(expectedAdvertisement4, expectedAdvertisement3, pageResource, advertisementId4, advertisementId3);

            allAdvertisements.AddRange(pageResource);

            while (!pageResource.Eof)
            {
                pageResource = await pageResource.NextPageAsync();
                allAdvertisements.AddRange(pageResource);
            }

            Assert.AreEqual(4, allAdvertisements.Count);
            StringAssert.EndsWith(nextLink, pageResource.Uri.ToString());

            var expectedAdvertisement2 = new AdvertisementModelBuilder(SummaryFieldsInitializer)
                .WithAdvertiserId("0002")
                .WithJobTitle("More Exciting Senior Developer role in a great CBD location. Great $$$")
                .WithJobReference("JOB12345");
            var expectedAdvertisement1 = new AdvertisementModelBuilder(SummaryFieldsInitializer)
                .WithAdvertiserId("0001")
                .WithJobTitle("Exciting Developer role in a great CBD location. Great $$")
                .WithJobReference("JOB1234");

            AssertAdvertisementSummaryAndLinks(expectedAdvertisement2, expectedAdvertisement1, pageResource, advertisementId2, advertisementId1);

            var actualException = Assert.Throws<NotSupportedException>(async () => await pageResource.NextPageAsync());
            var expectedException = new NotSupportedException("There are no more results");

            actualException.ShouldBeEquivalentToException(expectedException);
        }
    }
}