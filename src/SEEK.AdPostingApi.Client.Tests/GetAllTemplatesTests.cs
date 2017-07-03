using System;
using System.Collections.Generic;
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
            const string templateId1 = "8059016";
            const string templateId2 = "65146183";
            const string templateId3 = "9874198";
            const string templateId4 = "892138";
            const string templateId5 = "1132687";
            const string advertiserId1 = "456";
            const string advertiserId2 = "3214";
            const string template1Name = "The blue template";
            const string template2Name = "The template with a round logo";
            const string template3Name = "Testing template";
            const string template4Name = "Inactive template";
            const string template5Name = "Our first template";
            DateTimeOffset template1UpdateDateTime = DateTimeOffset.Parse("2017-01-03T11:45:44Z");
            DateTimeOffset template2UpdateDateTime = DateTimeOffset.Parse("2016-11-03T13:11:11Z");
            DateTimeOffset template3UpdateDateTime = DateTimeOffset.Parse("2017-05-07T09:45:43Z");
            DateTimeOffset template4UpdateDateTime = DateTimeOffset.Parse("2015-10-13T03:41:21Z");
            DateTimeOffset template5UpdateDateTime = DateTimeOffset.Parse("2017-03-23T11:12:10Z");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.MockProviderService
                .Given("Multiple templates exist for multiple advertisers related to the requestor")
                .UponReceiving("a GET templates request")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
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
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId1)
                                    .WithAdvertiserId(advertiserId1)
                                    .WithName(template1Name)
                                    .WithUpdateDateTime(template1UpdateDateTime)
                                    .WithTemplateState("Active")
                                    .Build(),
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId2)
                                    .WithAdvertiserId(advertiserId1)
                                    .WithName(template2Name)
                                    .WithUpdateDateTime(template2UpdateDateTime)
                                    .WithTemplateState("Active")
                                    .Build(),
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId3)
                                    .WithAdvertiserId(advertiserId2)
                                    .WithName(template3Name)
                                    .WithUpdateDateTime(template3UpdateDateTime)
                                    .WithTemplateState("Active")
                                    .Build(),
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId4)
                                    .WithAdvertiserId(advertiserId2)
                                    .WithName(template4Name)
                                    .WithUpdateDateTime(template4UpdateDateTime)
                                    .WithTemplateState("Inactive")
                                    .Build(),
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId5)
                                    .WithAdvertiserId(advertiserId2)
                                    .WithName(template5Name)
                                    .WithUpdateDateTime(template5UpdateDateTime)
                                    .WithTemplateState("Active")
                                    .Build()
                            }
                        },
                        _links = new
                        {
                            self = new { href = AdPostingTemplateApiFixture.TemplateApiBasePath }
                        }
                    }
                });

            TemplateSummaryListResource listResource;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                listResource = await client.GetAllTemplatesAsync();
            }

            TemplateSummaryListResource expectedListResource = new TemplateSummaryListResource
            {
                Templates = new List<TemplateSummaryResource>
                {
                    new TemplateSummaryResource
                    {
                        Id = templateId1,
                        AdvertiserId = advertiserId1,
                        Name = template1Name,
                        UpdateDateTime = template1UpdateDateTime,
                        State = TemplateStatus.Active,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    },
                    new TemplateSummaryResource
                    {
                        Id = templateId2,
                        AdvertiserId = advertiserId1,
                        Name = template2Name,
                        UpdateDateTime = template2UpdateDateTime,
                        State = TemplateStatus.Active,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    },
                    new TemplateSummaryResource
                    {
                        Id = templateId3,
                        AdvertiserId = advertiserId2,
                        Name = template3Name,
                        UpdateDateTime = template3UpdateDateTime,
                        State = TemplateStatus.Active,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    },
                    new TemplateSummaryResource
                    {
                        Id = templateId4,
                        AdvertiserId = advertiserId2,
                        Name = template4Name,
                        UpdateDateTime = template4UpdateDateTime,
                        State = TemplateStatus.Inactive,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    },
                    new TemplateSummaryResource
                    {
                        Id = templateId5,
                        AdvertiserId = advertiserId2,
                        Name = template5Name,
                        UpdateDateTime = template5UpdateDateTime,
                        State = TemplateStatus.Active,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    }
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = AdPostingTemplateApiFixture.TemplateApiBasePath } }
                },
                RequestId = RequestId
            };

            listResource.ShouldBeEquivalentTo(expectedListResource);
        }

        [Fact]
        public async Task GetAllTemplatesForPartnerNoTemplatesReturned()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.MockProviderService
                .Given("There are no templates")
                .UponReceiving("a GET templates request for all templates")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
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

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                templatesSummary = await client.GetAllTemplatesAsync();
            }

            TemplateSummaryListResource expectedtemplates = new TemplateSummaryListResource
            {
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri) { { "self", new Link { Href = "/template" } } },
                Templates = new List<TemplateSummaryResource>(),
                RequestId = RequestId
            };

            templatesSummary.ShouldBeEquivalentTo(expectedtemplates);
        }

        [Fact]
        public async Task GetAllTemplatesForAdvertiserMultipleTemplatesReturned()
        {
            const string templateId1 = "8059016";
            const string templateId2 = "65146183";
            const string advertiserId1 = "456";
            const string template1Name = "The blue template";
            const string template2Name = "The template with a round logo";
            DateTimeOffset template1UpdateDateTime = DateTimeOffset.Parse("2017-01-03T11:45:44Z");
            DateTimeOffset template2UpdateDateTime = DateTimeOffset.Parse("2016-11-03T13:11:11Z");

            string queryString = "advertiserId=" + advertiserId1;
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.MockProviderService
                .Given("Multiple templates exist for an advertiser related to the requestor")
                .UponReceiving("a GET templates request to retrieve all templates for an advertiser")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
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
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId1)
                                    .WithAdvertiserId(advertiserId1)
                                    .WithName(template1Name)
                                    .WithUpdateDateTime(template1UpdateDateTime)
                                    .WithTemplateState("Active")
                                    .Build(),
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId2)
                                    .WithAdvertiserId(advertiserId1)
                                    .WithName(template2Name)
                                    .WithUpdateDateTime(template2UpdateDateTime)
                                    .WithTemplateState("Active")
                                    .Build()
                            }
                        },
                        _links = new
                        {
                            self = new { href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" }
                        }
                    }
                });

            TemplateSummaryListResource listResource;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                listResource = await client.GetAllTemplatesAsync(advertiserId1);
            }

            TemplateSummaryListResource expectedListResource = new TemplateSummaryListResource
            {
                Templates = new List<TemplateSummaryResource>
                {
                    new TemplateSummaryResource
                    {
                        Id = templateId1,
                        AdvertiserId = advertiserId1,
                        Name = template1Name,
                        UpdateDateTime = template1UpdateDateTime,
                        State = TemplateStatus.Active,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    },
                    new TemplateSummaryResource
                    {
                        Id = templateId2,
                        AdvertiserId = advertiserId1,
                        Name = template2Name,
                        UpdateDateTime = template2UpdateDateTime,
                        State = TemplateStatus.Active,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    }
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" } }
                },
                RequestId = RequestId
            };

            listResource.ShouldBeEquivalentTo(expectedListResource);
        }

        [Fact]
        public async Task GetAllTemplatesForAdvertiserNoTemplatesReturned()
        {
            string advertiserId = "111222";
            string queryString = "advertiserId=" + advertiserId;
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.MockProviderService
                .Given("There are no templates for an advertiser related to the requestor")
                .UponReceiving("a GET templates request to retrieve all templates for an advertiser")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", ResponseContentTypes.TemplateListVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        _embedded = new { templates = new List<TemplateSummaryResource>() },
                        _links = new { self = new { href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" } }
                    }
                });

            TemplateSummaryListResource templatesSummary;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                templatesSummary = await client.GetAllTemplatesAsync(advertiserId);
            }

            TemplateSummaryListResource expectedtemplates = new TemplateSummaryListResource
            {
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" } }
                },
                Templates = new List<TemplateSummaryResource>(),
                RequestId = RequestId
            };

            templatesSummary.ShouldBeEquivalentTo(expectedtemplates);
        }

        [Fact]
        public async Task GetAllTemplatesForAdvertiserNonExistentAdvertiserId()
        {
            string advertiserId = "654321";
            string queryString = "advertiserId=" + advertiserId;
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.MockProviderService
                .UponReceiving("a GET templates request to retrieve all templates for an advertiser that doesn't exist")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 403,
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", ResponseContentTypes.TemplateErrorVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        message = "Forbidden",
                        errors = new[] { new { code = "InvalidValue" } }
                    }
                });

            UnauthorizedException actualException;
            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
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
                        Errors = new[] { new Error { Code = "InvalidValue" } }
                    }));
        }

        [Fact]
        public async Task GetAllTemplatesForAdvertiserReturnsRelationshipError()
        {
            string advertiserId = "874392";
            string queryString = "advertiserId=" + advertiserId;
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.OtherThirdPartyUploader).Build();

            this.Fixture.MockProviderService
                .UponReceiving("a GET templates request to retrieve all templates for an advertiser not related to requestor")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 403,
                    Headers = new Dictionary<string, string>
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
            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
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
        public async Task GetAllTemplatesForPartnerAndFromDateTimeUtcMultipleTemplatesReturned()
        {
            const string templateId1 = "8059016";
            const string templateId2 = "65146183";
            const string templateId3 = "9874198";
            const string templateId4 = "892138";
            const string templateId5 = "1132687";
            const string advertiserId1 = "456";
            const string advertiserId2 = "3214";
            const string template1Name = "The blue template";
            const string template2Name = "The template with a round logo";
            const string template3Name = "Testing template";
            const string template4Name = "Inactive template";
            const string template5Name = "Our first template";
            DateTimeOffset template1UpdateDateTime = DateTimeOffset.Parse("2017-01-03T11:45:44Z");
            DateTimeOffset template2UpdateDateTime = DateTimeOffset.Parse("2016-11-03T13:11:11Z");
            DateTimeOffset template3UpdateDateTime = DateTimeOffset.Parse("2017-05-07T09:45:43Z");
            DateTimeOffset template4UpdateDateTime = DateTimeOffset.Parse("2015-10-13T03:41:21Z");
            DateTimeOffset template5UpdateDateTime = DateTimeOffset.Parse("2017-03-23T11:12:10Z");

            string fromDateTimeUtcString = "2015-10-13T03:41:21Z"; // inclusive search
            DateTimeOffset fromDateTimeUtc = DateTimeOffset.Parse(fromDateTimeUtcString);
            string queryString = "fromDateTimeUtc=" + fromDateTimeUtcString;
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.MockProviderService
                .Given("Multiple templates updated after fromDateTimeUtc exist for multiple advertisers related to the requestor")
                .UponReceiving("a GET templates request to retrieve all templates updated after a specified time")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
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
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId1)
                                    .WithAdvertiserId(advertiserId1)
                                    .WithName(template1Name)
                                    .WithUpdateDateTime(template1UpdateDateTime)
                                    .WithTemplateState("Active")
                                    .Build(),
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId2)
                                    .WithAdvertiserId(advertiserId1)
                                    .WithName(template2Name)
                                    .WithUpdateDateTime(template2UpdateDateTime)
                                    .WithTemplateState("Active")
                                    .Build(),
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId3)
                                    .WithAdvertiserId(advertiserId2)
                                    .WithName(template3Name)
                                    .WithUpdateDateTime(template3UpdateDateTime)
                                    .WithTemplateState("Active")
                                    .Build(),
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId4)
                                    .WithAdvertiserId(advertiserId2)
                                    .WithName(template4Name)
                                    .WithUpdateDateTime(template4UpdateDateTime)
                                    .WithTemplateState("Inactive")
                                    .Build(),
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId5)
                                    .WithAdvertiserId(advertiserId2)
                                    .WithName(template5Name)
                                    .WithUpdateDateTime(template5UpdateDateTime)
                                    .WithTemplateState("Active")
                                    .Build()
                            }
                        },
                        _links = new
                        {
                            self = new { href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" }
                        }
                    }
                });

            TemplateSummaryListResource listResource;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                listResource = await client.GetAllTemplatesAsync(fromDateTimeUtc: fromDateTimeUtc);
            }

            TemplateSummaryListResource expectedListResource = new TemplateSummaryListResource
            {
                Templates = new List<TemplateSummaryResource>
                {
                    new TemplateSummaryResource
                    {
                        Id = templateId1,
                        AdvertiserId = advertiserId1,
                        Name = template1Name,
                        UpdateDateTime = template1UpdateDateTime,
                        State = TemplateStatus.Active,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    },
                    new TemplateSummaryResource
                    {
                        Id = templateId2,
                        AdvertiserId = advertiserId1,
                        Name = template2Name,
                        UpdateDateTime = template2UpdateDateTime,
                        State = TemplateStatus.Active,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    },
                    new TemplateSummaryResource
                    {
                        Id = templateId3,
                        AdvertiserId = advertiserId2,
                        Name = template3Name,
                        UpdateDateTime = template3UpdateDateTime,
                        State = TemplateStatus.Active,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    },
                    new TemplateSummaryResource
                    {
                        Id = templateId4,
                        AdvertiserId = advertiserId2,
                        Name = template4Name,
                        UpdateDateTime = template4UpdateDateTime,
                        State = TemplateStatus.Inactive,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    },
                    new TemplateSummaryResource
                    {
                        Id = templateId5,
                        AdvertiserId = advertiserId2,
                        Name = template5Name,
                        UpdateDateTime = template5UpdateDateTime,
                        State = TemplateStatus.Active,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    }
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" } }
                },
                RequestId = RequestId
            };

            listResource.ShouldBeEquivalentTo(expectedListResource);
        }

        [Fact]
        public async Task GetAllTemplatesForPartnerAndFromDateTimeUtcNoTemplatesReturned()
        {
            string fromDateTimeUtcString = "2011-01-01T00:00:00Z";
            DateTimeOffset fromDateTimeUtc = DateTimeOffset.Parse(fromDateTimeUtcString);
            string queryString = "fromDateTimeUtc=" + fromDateTimeUtcString;
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.MockProviderService
                .Given("There are no templates updated since fromDateTimeUtc for any advertiser related to the requestor")
                .UponReceiving("a GET templates request to retrieve all templates updated after a specified time")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", ResponseContentTypes.TemplateListVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        _embedded = new { templates = new List<TemplateSummaryResource>() },
                        _links = new
                        {
                            self = new { href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" }
                        }
                    }
                });

            TemplateSummaryListResource listResource;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                listResource = await client.GetAllTemplatesAsync(fromDateTimeUtc: fromDateTimeUtc);
            }

            TemplateSummaryListResource expectedListResource = new TemplateSummaryListResource
            {
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" } }
                },
                Templates = new List<TemplateSummaryResource>(),
                RequestId = RequestId
            };

            listResource.ShouldBeEquivalentTo(expectedListResource);
        }

        [Fact]
        public async Task GetAllTemplatesForAdvertiserAndFromDateTimeUtcMultipleTemplatesReturned()
        {
            const string templateId1 = "8059016";
            const string templateId2 = "65146183";
            const string advertiserId1 = "456";

            const string template1Name = "The blue template";
            const string template2Name = "The template with a round logo";
            DateTimeOffset template1UpdateDateTime = DateTimeOffset.Parse("2017-01-03T11:45:44Z");
            DateTimeOffset template2UpdateDateTime = DateTimeOffset.Parse("2016-11-03T13:11:11Z");

            string fromDateTimeUtcString = "2015-10-13T03:41:21Z"; // inclusive search
            DateTimeOffset fromDateTimeUtc = DateTimeOffset.Parse(fromDateTimeUtcString);
            string queryString = "advertiserId=" + advertiserId1 + "&fromDateTimeUtc=" + fromDateTimeUtcString;
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.MockProviderService
                .Given("Multiple templates updated after fromDateTimeUtc exist for an advertiser related to the requestor")
                .UponReceiving("a GET templates request to retrieve all templates for an advertiser updated after a specified time")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
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
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId1)
                                    .WithAdvertiserId(advertiserId1)
                                    .WithName(template1Name)
                                    .WithUpdateDateTime(template1UpdateDateTime)
                                    .WithTemplateState("Active")
                                    .Build(),
                                new TemplateSummaryResponseContentBuilder()
                                    .WithId(templateId2)
                                    .WithAdvertiserId(advertiserId1)
                                    .WithName(template2Name)
                                    .WithUpdateDateTime(template2UpdateDateTime)
                                    .WithTemplateState("Active")
                                    .Build()
                            }
                        },
                        _links = new
                        {
                            self = new { href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" }
                        }
                    }
                });

            TemplateSummaryListResource listResource;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                listResource = await client.GetAllTemplatesAsync(advertiserId1, fromDateTimeUtc);
            }

            TemplateSummaryListResource expectedListResource = new TemplateSummaryListResource
            {
                Templates = new List<TemplateSummaryResource>
                {
                    new TemplateSummaryResource
                    {
                        Id = templateId1,
                        AdvertiserId = advertiserId1,
                        Name = template1Name,
                        UpdateDateTime = template1UpdateDateTime,
                        State = TemplateStatus.Active,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    },
                    new TemplateSummaryResource
                    {
                        Id = templateId2,
                        AdvertiserId = advertiserId1,
                        Name = template2Name,
                        UpdateDateTime = template2UpdateDateTime,
                        State = TemplateStatus.Active,
                        Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                    }
                },
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" } }
                },
                RequestId = RequestId
            };

            listResource.ShouldBeEquivalentTo(expectedListResource);
        }

        [Fact]
        public async Task GetAllTemplatesForAdvertiserAndFromDateTimeUtcNoTemplatesReturned()
        {
            string advertiserId = "111222";
            string fromDateTimeUtcString = "2011-01-01T00:00:00Z";
            DateTimeOffset fromDateTimeUtc = DateTimeOffset.Parse(fromDateTimeUtcString);
            string queryString = "advertiserId=" + advertiserId + "&fromDateTimeUtc=" + fromDateTimeUtcString;
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            this.Fixture.MockProviderService
                .Given("There are no templates updated since fromDateTimeUtc for an advertiser related to the requestor")
                .UponReceiving("a GET templates request to retrieve all templates for an advertiser updated after a specified time")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = AdPostingTemplateApiFixture.TemplateApiBasePath,
                    Query = queryString,
                    Headers = new Dictionary<string, string>
                    {
                        { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                        { "Accept", $"{ResponseContentTypes.TemplateListVersion1}, {ResponseContentTypes.TemplateErrorVersion1}" },
                        { "User-Agent", AdPostingApiFixture.UserAgentHeaderValue }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", ResponseContentTypes.TemplateListVersion1 },
                        { "X-Request-Id", RequestId }
                    },
                    Body = new
                    {
                        _embedded = new { templates = new List<TemplateSummaryResource>() },
                        _links = new
                        {
                            self = new { href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" }
                        }
                    }
                });

            TemplateSummaryListResource listResource;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                listResource = await client.GetAllTemplatesAsync(advertiserId, fromDateTimeUtc);
            }

            TemplateSummaryListResource expectedListResource = new TemplateSummaryListResource
            {
                Links = new Links(this.Fixture.AdPostingApiServiceBaseUri)
                {
                    { "self", new Link { Href = $"{AdPostingTemplateApiFixture.TemplateApiBasePath}?{queryString}" } }
                },
                Templates = new List<TemplateSummaryResource>(),
                RequestId = RequestId
            };

            listResource.ShouldBeEquivalentTo(expectedListResource);
        }

        private AdPostingTemplateApiFixture Fixture { get; }
    }
}