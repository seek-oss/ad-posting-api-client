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
    [Collection(AdPostingTemplateApiCollection.Name)]
    public class GetAllTemplatesTests : IDisposable
    {
        private const string RequestId = "PactRequestId";

        #region Provider States

        private readonly OAuth2Token _oAuth2TokenRequestorA = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken).Build();
        private readonly OAuth2Token _oAuth2TokenRequestorB = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.OtherThirdPartyUploader).Build();

        private const string TemplateId1 = "8059016";
        private const string TemplateId2 = "65146183";
        private const string TemplateId3 = "9874198";
        private const string TemplateId4 = "892138";
        private const string TemplateId5 = "1132687";
        private const string AdvertiserId1 = "456";
        private const string AdvertiserId2 = "3214";
        private const string Template1Name = "The blue template";
        private const string Template2Name = "The template with a round logo";
        private const string Template3Name = "Testing template";
        private const string Template4Name = "Inactive template";
        private const string Template5Name = "Our first template";

        private const string TemplateUpdateDateTimeString1 = "2017-01-03T11:45:44Z";
        private const string TemplateUpdateDateTimeString2 = "2016-11-03T13:11:11Z";
        private const string TemplateUpdateDateTimeString3 = "2017-05-07T09:45:43Z";
        private const string TemplateUpdateDateTimeString4 = "2015-10-13T03:41:21Z";
        private const string TemplateUpdateDateTimeString5 = "2017-03-23T11:12:10Z";
        private const int TemplateWriteSequence1 = 7001;
        private const int TemplateWriteSequence2 = 6011;
        private const int TemplateWriteSequence3 = 7005;

        private readonly TemplateSummaryResponseContentBuilder _template1 = new TemplateSummaryResponseContentBuilder()
            .WithId(TemplateId1)
            .WithAdvertiserId(AdvertiserId1)
            .WithName(Template1Name)
            .WithUpdatedDateTime(DateTimeOffset.Parse(TemplateUpdateDateTimeString1))
            .WithTemplateState("Active");

        private readonly TemplateSummaryResponseContentBuilder _template2 = new TemplateSummaryResponseContentBuilder()
            .WithId(TemplateId2)
            .WithAdvertiserId(AdvertiserId1)
            .WithName(Template2Name)
            .WithUpdatedDateTime(DateTimeOffset.Parse(TemplateUpdateDateTimeString2))
            .WithTemplateState("Active");

        private readonly TemplateSummaryResponseContentBuilder _template3 = new TemplateSummaryResponseContentBuilder()
            .WithId(TemplateId3)
            .WithAdvertiserId(AdvertiserId2)
            .WithName(Template3Name)
            .WithUpdatedDateTime(DateTimeOffset.Parse(TemplateUpdateDateTimeString3))
            .WithTemplateState("Active");

        private readonly TemplateSummaryResponseContentBuilder _template4 = new TemplateSummaryResponseContentBuilder()
            .WithId(TemplateId4)
            .WithAdvertiserId(AdvertiserId2)
            .WithName(Template4Name)
            .WithUpdatedDateTime(DateTimeOffset.Parse(TemplateUpdateDateTimeString4))
            .WithTemplateState("Inactive");

        private readonly TemplateSummaryResponseContentBuilder _template5 = new TemplateSummaryResponseContentBuilder()
            .WithId(TemplateId5)
            .WithAdvertiserId(AdvertiserId2)
            .WithName(Template5Name)
            .WithUpdatedDateTime(DateTimeOffset.Parse(TemplateUpdateDateTimeString5))
            .WithTemplateState("Active");

        private readonly TemplateSummaryResource _expectedTemplateResource1 = new TemplateSummaryResource
        {
            Id = TemplateId1,
            AdvertiserId = AdvertiserId1,
            Name = Template1Name,
            UpdatedDateTime = DateTimeOffset.Parse(TemplateUpdateDateTimeString1),
            State = TemplateStatus.Active,
            Links = new Links(AdPostingTemplateApiPactService.MockProviderServiceBaseUri)
        };

        private readonly TemplateSummaryResource _expectedTemplateResource2 = new TemplateSummaryResource
        {
            Id = TemplateId2,
            AdvertiserId = AdvertiserId1,
            Name = Template2Name,
            UpdatedDateTime = DateTimeOffset.Parse(TemplateUpdateDateTimeString2),
            State = TemplateStatus.Active,
            Links = new Links(AdPostingTemplateApiPactService.MockProviderServiceBaseUri)
        };

        private readonly TemplateSummaryResource _expectedTemplateResource3 = new TemplateSummaryResource
        {
            Id = TemplateId3,
            AdvertiserId = AdvertiserId2,
            Name = Template3Name,
            UpdatedDateTime = DateTimeOffset.Parse(TemplateUpdateDateTimeString3),
            State = TemplateStatus.Active,
            Links = new Links(AdPostingTemplateApiPactService.MockProviderServiceBaseUri)
        };

        private readonly TemplateSummaryResource _expectedTemplateResource4 = new TemplateSummaryResource
        {
            Id = TemplateId4,
            AdvertiserId = AdvertiserId2,
            Name = Template4Name,
            UpdatedDateTime = DateTimeOffset.Parse(TemplateUpdateDateTimeString4),
            State = TemplateStatus.Inactive,
            Links = new Links(AdPostingTemplateApiPactService.MockProviderServiceBaseUri)
        };

        private readonly TemplateSummaryResource _expectedTemplateResource5 = new TemplateSummaryResource
        {
            Id = TemplateId5,
            AdvertiserId = AdvertiserId2,
            Name = Template5Name,
            UpdatedDateTime = DateTimeOffset.Parse(TemplateUpdateDateTimeString5),
            State = TemplateStatus.Active,
            Links = new Links(AdPostingTemplateApiPactService.MockProviderServiceBaseUri)
        };

        #endregion

        public GetAllTemplatesTests(AdPostingTemplateApiPactService adPostingTemplateApiPactService)
        {
            this.Fixture = new AdPostingTemplateApiFixture(adPostingTemplateApiPactService);
        }

        public void Dispose()
        {
            this.Fixture.Dispose();
        }

        [Fact]
        public async Task GetAllTemplatesForPartnerMultipleTemplatesReturned()
        {
            this.Fixture.MockProviderService
                .Given("There are multiple templates for multiple advertisers related to the requestor")
                .UponReceiving("a GET templates request to retrieve all templates")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorA.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.TemplateListVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        _embedded = new
                        {
                            // sorted by WriteSequence
                            templates = new[]
                            {
                                this._template4.Build(),
                                this._template2.Build(),
                                this._template1.Build(),
                                this._template5.Build(),
                                this._template3.Build()
                            }
                        },
                        _links = new
                        {
                            self = new { href = AdPostingTemplateApiFixture.TemplateApiBasePath },
                            next = new { href = AdPostingTemplateApiFixture.TemplateApiBasePath + $"?after={TemplateWriteSequence3}" }
                        }
                    }
                });

            TemplateSummaryListResource templatesSummary;

            using (AdPostingApiClient client = this.Fixture.GetClient(this._oAuth2TokenRequestorA))
            {
                templatesSummary = await client.GetAllTemplatesAsync();
            }

            TemplateSummaryListResource expectedTemplates = new TemplateSummaryListResource
            {
                Templates = new List<TemplateSummaryResource>
                {
                    // sorted by WriteSequence
                    this._expectedTemplateResource4,
                    this._expectedTemplateResource2,
                    this._expectedTemplateResource1,
                    this._expectedTemplateResource5,
                    this._expectedTemplateResource3
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = AdPostingTemplateApiFixture.TemplateApiBasePath } },
                    { "next", new Link { Href = AdPostingTemplateApiFixture.TemplateApiBasePath + $"?after={TemplateWriteSequence3}"} }
                },
                RequestId = RequestId
            };

            templatesSummary.ShouldBeEquivalentTo(expectedTemplates);
        }

        [Fact]
        public async Task GetAllTemplatesForPartnerNoTemplatesReturned()
        {
            this.Fixture.MockProviderService
                .Given("There are no templates for any advertiser related to the requestor")
                .UponReceiving("a GET templates request to retrieve all templates")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorB.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.TemplateListVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        _embedded = new { templates = new List<TemplateSummaryResource>() },
                        _links = new { self = new { href = "/template" } }
                    }
                });

            TemplateSummaryListResource templatesSummary;

            using (AdPostingApiClient client = this.Fixture.GetClient(this._oAuth2TokenRequestorB))
            {
                templatesSummary = await client.GetAllTemplatesAsync();
            }

            TemplateSummaryListResource expectedTemplates = new TemplateSummaryListResource
            {
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri) { { "self", new Link { Href = "/template" } } },
                Templates = new List<TemplateSummaryResource>(),
                RequestId = RequestId
            };

            templatesSummary.ShouldBeEquivalentTo(expectedTemplates);
        }

        [Fact]
        public async Task GetAllTemplatesForAdvertiserMultipleTemplatesReturned()
        {
            string queryString = "advertiserId=" + AdvertiserId1;

            this.Fixture.MockProviderService
                .Given("There are multiple templates for multiple advertisers related to the requestor")
                .UponReceiving("a GET templates request to retrieve all templates for an advertiser")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorA.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.TemplateListVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        _embedded = new
                        {
                            templates = new[]
                            {
                                // sorted by WriteSequence
                                this._template2.Build(),
                                this._template1.Build()
                            }
                        },
                        _links = new
                        {
                            self = new { href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" },
                            next = new { href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}&after={TemplateWriteSequence1}" }
                        }
                    }
                });

            TemplateSummaryListResource templatesSummary;

            using (AdPostingApiClient client = this.Fixture.GetClient(this._oAuth2TokenRequestorA))
            {
                templatesSummary = await client.GetAllTemplatesAsync(AdvertiserId1);
            }

            TemplateSummaryListResource expectedTemplates = new TemplateSummaryListResource
            {
                Templates = new List<TemplateSummaryResource>
                {
                    // sorted by WriteSequence
                    this._expectedTemplateResource2,
                    this._expectedTemplateResource1
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" } },
                    { "next", new Link { Href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}&after={TemplateWriteSequence1}" } },
                },
                RequestId = RequestId
            };

            templatesSummary.ShouldBeEquivalentTo(expectedTemplates);
        }

        [Fact]
        public async Task GetAllTemplatesForAdvertiserNonExistentAdvertiserId()
        {
            string advertiserId = "654321";
            string queryString = "advertiserId=" + advertiserId;

            this.Fixture.MockProviderService
                .UponReceiving("a GET templates request to retrieve all templates for an advertiser that doesn't exist")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorA.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 403,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.TemplateErrorVersion1 },
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
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(async () => await client.GetAllTemplatesAsync(advertiserId));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    RequestId,
                    403,
                    new TemplateErrorResponse
                    {
                        Message = "Forbidden",
                        Errors = new[] { new Error { Code = "RelationshipError" } }
                    }));
        }

        [Fact]
        public async Task GetAllTemplatesForAdvertiserReturnsRelationshipError()
        {
            string queryString = "advertiserId=" + AdvertiserId1;

            this.Fixture.MockProviderService
                .UponReceiving("a GET templates request to retrieve all templates for an advertiser not related to requestor")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorB.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 403,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.TemplateErrorVersion1 },
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
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(async () => await client.GetAllTemplatesAsync(AdvertiserId1));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    RequestId,
                    403,
                    new TemplateErrorResponse
                    {
                        Message = "Forbidden",
                        Errors = new[] { new Error { Code = "RelationshipError" } }
                    }));
        }

        [Fact]
        public async Task GetAllTemplatesForPartnerAndAfterSequenceIdentifierMultipleTemplatesReturned()
        {
            string queryString = "after=" + TemplateWriteSequence2;

            this.Fixture.MockProviderService
                .Given("There are multiple templates for multiple advertisers related to the requestor")
                .UponReceiving("a GET templates request to retrieve all templates after specified sequence identifier")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorA.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.TemplateListVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        _embedded = new
                        {
                            templates = new[]
                            {
                                // sorted by WriteSequence
                                this._template1.Build(),
                                this._template5.Build(),
                                this._template3.Build()
                            }
                        },
                        _links = new
                        {
                            self = new { href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" },
                            next = new { href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?after={TemplateWriteSequence3}" }
                        }
                    }
                });

            TemplateSummaryListResource templatesSummary;

            using (AdPostingApiClient client = this.Fixture.GetClient(this._oAuth2TokenRequestorA))
            {
                templatesSummary = await client.GetAllTemplatesAsync(after: TemplateWriteSequence2);
            }

            TemplateSummaryListResource expectedTemplates = new TemplateSummaryListResource
            {
                Templates = new List<TemplateSummaryResource>
                {
                    // sorted by WriteSequence
                    this._expectedTemplateResource1,
                    this._expectedTemplateResource5,
                    this._expectedTemplateResource3
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" } },
                    { "next", new Link { Href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?after={TemplateWriteSequence3}" } }
                },
                RequestId = RequestId
            };

            templatesSummary.ShouldBeEquivalentTo(expectedTemplates);
        }

        [Fact]
        public async Task GetAllTemplatesWithInvalidRequestFieldValuesReturnsError()
        {
            const string invalidSequenceIdentifier = "not-an-accepted-sequence-format";
            string queryString = "after=" + invalidSequenceIdentifier;

            this.Fixture.MockProviderService
                .Given("There are multiple templates for multiple advertisers related to the requestor")
                .UponReceiving("a GET templates request to retrieve all templates with invalid request field values")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorA.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 422,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.TemplateErrorVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        message = "Validation Failure",
                        errors = new[]
                        {
                            new { field = "after", code = "InvalidValue" }
                        }
                    }
                });

            using (var client = new HttpClient())
            {
                {
                    using (HttpRequestMessage request = this.Fixture.CreateGetTemplatesRequest(queryString, this._oAuth2TokenRequestorA.AccessToken))
                    {
                        using (HttpResponseMessage response = await client.SendAsync(request))
                        {
                            Assert.Equal(422, (int)response.StatusCode);
                        }
                    }
                }
            }
        }

        [Fact]
        public async Task GetAllTemplatesForAdvertiserAndAfterSequenceIdentifierMultipleTemplatesReturned()
        {
            string queryString = "advertiserId=" + AdvertiserId2 + "&after=" + TemplateWriteSequence2;

            this.Fixture.MockProviderService
                .Given("There are multiple templates for multiple advertisers related to the requestor")
                .UponReceiving("a GET templates request to retrieve all templates for an advertiser after specified sequence identifier")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorA.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.TemplateListVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        _embedded = new
                        {
                            templates = new[]
                            {
                                // sorted by WriteSequence
                                this._template5.Build(),
                                this._template3.Build()
                            }
                        },
                        _links = new
                        {
                            self = new { href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" },
                            next = new { href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?advertiserId={AdvertiserId2}&after={TemplateWriteSequence3}" }
                        }
                    }
                });

            TemplateSummaryListResource templatesSummary;

            using (AdPostingApiClient client = this.Fixture.GetClient(this._oAuth2TokenRequestorA))
            {
                templatesSummary = await client.GetAllTemplatesAsync(AdvertiserId2, TemplateWriteSequence2);
            }

            TemplateSummaryListResource expectedTemplates = new TemplateSummaryListResource
            {
                Templates = new List<TemplateSummaryResource>
                {
                    // sorted by WriteSequence
                    this._expectedTemplateResource5,
                    this._expectedTemplateResource3
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" } },
                    { "next", new Link { Href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?advertiserId={AdvertiserId2}&after={TemplateWriteSequence3}" } }
                },
                RequestId = RequestId
            };

            templatesSummary.ShouldBeEquivalentTo(expectedTemplates);
        }

        [Fact]
        public async Task GetAllTemplatesForAdvertiserAndAfterSequenceIdentifierNoTemplatesReturned()
        {
            string queryString = "advertiserId=" + AdvertiserId2 + "&after=" + TemplateWriteSequence3;

            this.Fixture.MockProviderService
                .Given("There are no templates after given sequence identifier for all advertisers related to the requestor")
                .UponReceiving("a GET templates request to retrieve all templates for an advertiser after given sequence identifier")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer " + this._oAuth2TokenRequestorA.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", ResponseContentTypes.TemplateListVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        _embedded = new
                        {
                            templates = new object[0]
                        },
                        _links = new
                        {
                            self = new { href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" }
                        }
                    }
                });

            TemplateSummaryListResource templateSummary;

            using (AdPostingApiClient client = this.Fixture.GetClient(this._oAuth2TokenRequestorA))
            {
                templateSummary = await client.GetAllTemplatesAsync(AdvertiserId2, TemplateWriteSequence3);
            }

            TemplateSummaryListResource expectedTemplates = new TemplateSummaryListResource
            {
                Templates = new List<TemplateSummaryResource>(),
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" } }
                },
                RequestId = RequestId
            };

            templateSummary.ShouldBeEquivalentTo(expectedTemplates);
        }

        private AdPostingTemplateApiFixture Fixture { get; }
    }
}
