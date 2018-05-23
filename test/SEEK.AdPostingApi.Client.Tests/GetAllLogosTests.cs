using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using SEEK.AdPostingApi.Client.Tests.Framework;
using Xunit;

namespace SEEK.AdPostingApi.Client.Tests
{
    [Collection(AdPostingLogoApiCollection.Name)]
    public class GetAllLogosTests : IDisposable
    {
        private const string RequestId = "PactRequestId";

        #region Provider States

        private readonly OAuth2Token _oAuth2TokenRequestorA = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken).Build();
        private readonly OAuth2Token _oAuth2TokenRequestorB = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.OtherThirdPartyUploader).Build();

        private const string LogoId1 = "18475";
        private const string LogoId2 = "781312";
        private const string LogoId3 = "129301";
        private const string LogoId4 = "129";
        private const string LogoId5 = "5818341";
        private const string AdvertiserId1 = "456";
        private const string AdvertiserId2 = "3214";
        private const string Logo1Name = "The red logo";
        private const string Logo2Name = "The blue logo";
        private const string Logo3Name = "The transparent logo";
        private const string Logo4Name = "Old logo";
        private const string Logo5Name = "New logo";
        private const string LogoUpdateDateTimeString1 = "2018-01-03T11:55:55Z";
        private const string LogoUpdateDateTimeString2 = "2016-08-23T11:55:55Z";
        private const string LogoUpdateDateTimeString3 = "2015-11-11T11:55:55Z";
        private const string LogoUpdateDateTimeString4 = "2017-03-30T11:55:55Z";
        private const string LogoUpdateDateTimeString5 = "2017-05-05T11:55:55Z";
        private const string Logo1ViewLink = "https://www.seek.com.au/logos/jobseeker/thumbnail/18475.png";
        private const string Logo2ViewLink = "https://www.seek.com.au/logos/jobseeker/thumbnail/781312.png";
        private const string Logo3ViewLink = "https://www.seek.com.au/logos/jobseeker/thumbnail/129301.jpg";
        private const string Logo4ViewLink = "https://www.seek.com.au/logos/jobseeker/thumbnail/129.jpg";
        private const string Logo5ViewLink = "https://www.seek.com.au/logos/jobseeker/thumbnail/5818341.png";

        private readonly LogoSummaryResponseContentBuilder _logo1 = new LogoSummaryResponseContentBuilder()
            .WithId(LogoId1)
            .WithAdvertiserId(AdvertiserId1)
            .WithName(Logo1Name)
            .WithUpdatedDateTime(DateTime.Parse(LogoUpdateDateTimeString1))
            .WithLogoState("Active")
            .WithViewLink("view", Logo1ViewLink);

        private readonly LogoSummaryResponseContentBuilder _logo2 = new LogoSummaryResponseContentBuilder()
            .WithId(LogoId2)
            .WithAdvertiserId(AdvertiserId1)
            .WithName(Logo2Name)
            .WithUpdatedDateTime(DateTime.Parse(LogoUpdateDateTimeString2))
            .WithLogoState("Active")
            .WithViewLink("view", Logo2ViewLink);

        private readonly LogoSummaryResponseContentBuilder _logo3 = new LogoSummaryResponseContentBuilder()
            .WithId(LogoId3)
            .WithAdvertiserId(AdvertiserId1)
            .WithName(Logo3Name)
            .WithUpdatedDateTime(DateTime.Parse(LogoUpdateDateTimeString3))
            .WithLogoState("Active")
            .WithViewLink("view", Logo3ViewLink);

        private readonly LogoSummaryResponseContentBuilder _logo4 = new LogoSummaryResponseContentBuilder()
            .WithId(LogoId4)
            .WithAdvertiserId(AdvertiserId2)
            .WithName(Logo4Name)
            .WithUpdatedDateTime(DateTime.Parse(LogoUpdateDateTimeString4))
            .WithLogoState("Inactive")
            .WithViewLink("view", Logo4ViewLink);

        private readonly LogoSummaryResponseContentBuilder _logo5 = new LogoSummaryResponseContentBuilder()
            .WithId(LogoId5)
            .WithAdvertiserId(AdvertiserId2)
            .WithName(Logo5Name)
            .WithUpdatedDateTime(DateTime.Parse(LogoUpdateDateTimeString5))
            .WithLogoState("Active")
            .WithViewLink("view", Logo5ViewLink);

        private readonly LogoSummaryResource _expectedLogoResource1 = new LogoSummaryResource
        {
            LogoId = LogoId1,
            AdvertiserId = AdvertiserId1,
            Name = Logo1Name,
            UpdatedDateTime = DateTime.Parse(LogoUpdateDateTimeString1),
            State = LogoStatus.Active,
            Links = new Links()
            {
                { "view", new Link { Href = Logo1ViewLink } }
            }
        };

        private readonly LogoSummaryResource _expectedLogoResource2 = new LogoSummaryResource
        {
            LogoId = LogoId2,
            AdvertiserId = AdvertiserId1,
            Name = Logo2Name,
            UpdatedDateTime = DateTime.Parse(LogoUpdateDateTimeString2),
            State = LogoStatus.Active,
            Links = new Links()
            {
                { "view", new Link { Href = Logo2ViewLink } }
            }
        };

        private readonly LogoSummaryResource _expectedLogoResource3 = new LogoSummaryResource
        {
            LogoId = LogoId3,
            AdvertiserId = AdvertiserId1,
            Name = Logo3Name,
            UpdatedDateTime = DateTime.Parse(LogoUpdateDateTimeString3),
            State = LogoStatus.Active,
            Links = new Links()
            {
                { "view", new Link { Href = Logo3ViewLink } }
            }
        };

        private readonly LogoSummaryResource _expectedLogoResource4 = new LogoSummaryResource
        {
            LogoId = LogoId4,
            AdvertiserId = AdvertiserId2,
            Name = Logo4Name,
            UpdatedDateTime = DateTime.Parse(LogoUpdateDateTimeString4),
            State = LogoStatus.Inactive,
            Links = new Links()
            {
                { "view", new Link { Href = Logo4ViewLink } }
            }
        };

        private readonly LogoSummaryResource _expectedLogoResource5 = new LogoSummaryResource
        {
            LogoId = LogoId5,
            AdvertiserId = AdvertiserId2,
            Name = Logo5Name,
            UpdatedDateTime = DateTime.Parse(LogoUpdateDateTimeString5),
            State = LogoStatus.Active,
            Links = new Links()
            {
                { "view", new Link { Href = Logo5ViewLink } }
            }
        };

        #endregion

        public GetAllLogosTests(AdPostingLogoApiPactService adPostingLogoApiPactService)
        {
            this.Fixture = new AdPostingLogoApiFixture(adPostingLogoApiPactService);
        }

        public void Dispose()
        {
            this.Fixture.Dispose();
        }

        [Fact]
        public async Task GetAllLogosForPartnerMultipleLogosReturned()
        {
            this.Fixture.MockProviderService
                .Given("There are multiple logos for multiple advertisers related to the requestor")
                .UponReceiving("a GET logos request to retrieve all logos")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingLogoApiFixture.LogoApiBasePath,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorA.AccessToken },
                        { "Accept", $"{ResponseContentTypes.LogoListVersion1}, {ResponseContentTypes.LogoErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.LogoListVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        _embedded = new
                        {
                            logos = new[]
                            {
                                this._logo1.Build(),
                                this._logo2.Build(),
                                this._logo3.Build(),
                                this._logo4.Build(),
                                this._logo5.Build()
                            }
                        },
                        _links = new
                        {
                            self = new { href = AdPostingLogoApiFixture.LogoApiBasePath }
                        }
                    }
                });

            LogoSummaryListResource logosSummary;

            using (AdPostingApiClient client = this.Fixture.GetClient(this._oAuth2TokenRequestorA))
            {
                logosSummary = await client.GetAllLogosAsync();
            }

            LogoSummaryListResource expectedLogos = new LogoSummaryListResource
            {
                Logos = new List<LogoSummaryResource>
                {
                    this._expectedLogoResource1,
                    this._expectedLogoResource2,
                    this._expectedLogoResource3,
                    this._expectedLogoResource4,
                    this._expectedLogoResource5
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = AdPostingLogoApiFixture.LogoApiBasePath } }
                },
                RequestId = RequestId,
            };

            logosSummary.ShouldBeEquivalentTo(expectedLogos);
        }

        [Fact]
        public async Task GetAllLogosForPartnerNoLogosReturned()
        {
            this.Fixture.MockProviderService
                .Given("There are no logos for any advertiser related to the requestor")
                .UponReceiving("a GET logos request to retrieve all logos")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingLogoApiFixture.LogoApiBasePath,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorB.AccessToken },
                        { "Accept", $"{ResponseContentTypes.LogoListVersion1}, {ResponseContentTypes.LogoErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.LogoListVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        _embedded = new { logos = new List<LogoSummaryResource>() },
                        _links = new
                        {
                            self = new { href = AdPostingLogoApiFixture.LogoApiBasePath }
                        }
                    }
                });

            LogoSummaryListResource logosSummary;

            using (AdPostingApiClient client = this.Fixture.GetClient(this._oAuth2TokenRequestorB))
            {
                logosSummary = await client.GetAllLogosAsync();

                var expectedLogos = new LogoSummaryListResource
                {
                    Logos = new List<LogoSummaryResource>(),
                    Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    {
                        { "self", new Link { Href = AdPostingLogoApiFixture.LogoApiBasePath } }
                    },
                    RequestId = RequestId
                };

                logosSummary.ShouldBeEquivalentTo(expectedLogos);
            }
        }

        [Fact]
        public async Task GetAllLogosForAdvertiserMultipleLogosReturned()
        {
            string queryString = "advertiserId=" + AdvertiserId1;

            this.Fixture.MockProviderService
                .Given("There are multiple logos for multiple advertisers related to the requestor")
                .UponReceiving("a GET logos request to retrieve all logos for an advertiser")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingLogoApiFixture.LogoApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorA.AccessToken },
                        { "Accept", $"{ResponseContentTypes.LogoListVersion1}, {ResponseContentTypes.LogoErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.LogoListVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        _embedded = new
                        {
                            logos = new[]
                            {
                                this._logo1.Build(),
                                this._logo2.Build(),
                                this._logo3.Build()
                            }
                        },
                        _links = new
                        {
                            self = new { href = $"{AdPostingLogoApiFixture.LogoApiBasePath}?{queryString}" }
                        }
                    }
                });

            LogoSummaryListResource logosSummary;

            using (AdPostingApiClient client = this.Fixture.GetClient(this._oAuth2TokenRequestorA))
            {
                logosSummary = await client.GetAllLogosAsync(AdvertiserId1);
            }

            LogoSummaryListResource expectedLogos = new LogoSummaryListResource
            {
                Logos = new List<LogoSummaryResource>
                {
                    this._expectedLogoResource1,
                    this._expectedLogoResource2,
                    this._expectedLogoResource3
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = $"{AdPostingLogoApiFixture.LogoApiBasePath}?{queryString}" } }
                },
                RequestId = RequestId
            };

            logosSummary.ShouldBeEquivalentTo(expectedLogos);
        }

        [Fact]
        public async Task GetAllLogosForAdvertiserNonExistentAdvertiserId()
        {
            string advertiserId = "654321";
            string queryString = "advertiserId=" + advertiserId;

            this.Fixture.MockProviderService
                .UponReceiving("a GET logos request to retrieve all logos for an advertiser that doesn't exist")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingLogoApiFixture.LogoApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorA.AccessToken },
                        { "Accept", $"{ResponseContentTypes.LogoListVersion1}, {ResponseContentTypes.LogoErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 403,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.LogoErrorVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        message = "Forbidden",
                        errors = new[] { new { code = "RelationshipError" } }
                    }
                });

            UnauthorizedException actualException;
            using (AdPostingApiClient client = this.Fixture.GetClient(this._oAuth2TokenRequestorA))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(async () => await client.GetAllLogosAsync(advertiserId));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    RequestId,
                    403,
                    new LogoErrorResponse
                    {
                        Message = "Forbidden",
                        Errors = new[] { new Error { Code = "RelationshipError" } }
                    }));
        }

        [Fact]
        public async Task GetAllLogosForAdvertiserReturnsRelationshipError()
        {
            string queryString = "advertiserId=" + AdvertiserId1;

            this.Fixture.MockProviderService
                .UponReceiving("a GET logos request to retrieve all logos for an advertiser not related to requestor")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingLogoApiFixture.LogoApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorB.AccessToken },
                        { "Accept", $"{ResponseContentTypes.LogoListVersion1}, {ResponseContentTypes.LogoErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 403,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.LogoErrorVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        message = "Forbidden",
                        errors = new[] { new { code = "RelationshipError" } }
                    }
                });

            UnauthorizedException actualException;
            using (AdPostingApiClient client = this.Fixture.GetClient(this._oAuth2TokenRequestorB))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(async () => await client.GetAllLogosAsync(AdvertiserId1));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    RequestId,
                    403,
                    new LogoErrorResponse
                    {
                        Message = "Forbidden",
                        Errors = new[] { new Error { Code = "RelationshipError" } }
                    }));
        }

        [Fact]
        public async Task GetAllLogosWithInvalidRequestFieldValuesReturnsError()
        {
            const string invalidAdvertiserId = "asdf";
            string queryString = "advertiserId=" + invalidAdvertiserId;

            this.Fixture.MockProviderService
                .Given("There are multiple logos for multiple advertisers related to the requestor")
                .UponReceiving("a GET logos request to retrieve all logos with invalid request field values")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingLogoApiFixture.LogoApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorA.AccessToken },
                        { "Accept", $"{ResponseContentTypes.LogoListVersion1}, {ResponseContentTypes.LogoErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 403,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.LogoErrorVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        message = "Forbidden",
                        errors = new[]
                        {
                            new { message = "Invalid value 'asdf' in field 'AdvertiserPublicId'", code = "InvalidValue" }
                        }
                    }
                });

            using (var client = new HttpClient())
            {
                {
                    using (HttpRequestMessage request = this.Fixture.CreateGetLogoRequest(queryString, this._oAuth2TokenRequestorA.AccessToken))
                    {
                        using (HttpResponseMessage response = await client.SendAsync(request))
                        {
                            Assert.Equal(403, (int)response.StatusCode);
                        }
                    }
                }
            }
        }
        private AdPostingLogoApiFixture Fixture { get; }
    }
}
