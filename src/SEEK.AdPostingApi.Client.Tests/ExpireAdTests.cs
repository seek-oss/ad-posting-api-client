using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using SEEK.AdPostingApi.Client.Tests.Framework;
using Xunit;

namespace SEEK.AdPostingApi.Client.Tests
{
    [Collection(AdPostingApiCollection.Name)]
    public class ExpireAdTests : IDisposable
    {
        private const string AdvertisementLink = "/advertisement";
        private const string RequestId = "PactRequestId";

        private readonly string _acceptHeader = $"{ResponseContentTypes.AdvertisementVersion1}, {ResponseContentTypes.AdvertisementErrorVersion1}";

        private readonly dynamic _expireRequest = new[]
        {
            new
            {
                op = "replace",
                path = "state",
                value = "Expired"
            }
        };

        private IBuilderInitializer AllFieldsInitializer => new AllFieldsInitializer();

        public ExpireAdTests(AdPostingApiPactService adPostingApiPactService)
        {
            this.Fixture = new AdPostingApiFixture(adPostingApiPactService);
        }

        public void Dispose()
        {
            this.Fixture.Dispose();
        }

        [Fact]
        public async Task ExpireAdvertisementUsingHalSelfLink()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            string link = $"{AdvertisementLink}/{advertisementId}";
            string viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";
            DateTime expiryDate = new DateTime(2015, 10, 7, 21, 19, 00, DateTimeKind.Utc);

            this.SetupPactForExpiringExistingAdvertisement(link, oAuth2Token, advertisementId, viewRenderedAdvertisementLink, expiryDate);

            AdvertisementResource result;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                result = await client.ExpireAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link));
            }

            this.AssertReturnedAdvertisementMatchesExpectedExpiredAdvertisement(advertisementId, expiryDate, result);
        }

        [Fact]
        public async Task ExpireAdvertisementUsingHalTemplateWithAdvertisementId()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            string link = $"{AdvertisementLink}/{advertisementId}";
            string viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";
            DateTime expiryDate = new DateTime(2015, 10, 7, 21, 19, 00, DateTimeKind.Utc);

            this.Fixture.RegisterIndexPageInteractions(oAuth2Token);

            this.SetupPactForExpiringExistingAdvertisement(link, oAuth2Token, advertisementId, viewRenderedAdvertisementLink, expiryDate);

            AdvertisementResource result;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                result = await client.ExpireAdvertisementAsync(new Guid(advertisementId));
            }

            this.AssertReturnedAdvertisementMatchesExpectedExpiredAdvertisement(advertisementId, expiryDate, result);
        }

        private void SetupPactForExpiringExistingAdvertisement(
            string link, OAuth2Token oAuth2Token, string advertisementId, string viewRenderedAdvertisementLink, DateTime expiryDate)
        {
            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a PATCH advertisement request to expire an advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                            {"Content-Type", RequestContentTypes.AdvertisementPatchVersion1},
                            {"Accept", this._acceptHeader}
                        },
                        Body = new[]
                        {
                            new
                            {
                                op = "replace",
                                path = "state",
                                value = AdvertisementState.Expired.ToString()
                            }
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 202,
                        Headers = new Dictionary<string, string>
                        {
                            {"Content-Type", ResponseContentTypes.AdvertisementVersion1},
                            {"X-Request-Id", RequestId}
                        },
                        Body = new AdvertisementResponseContentBuilder(this.AllFieldsInitializer)
                            .WithId(advertisementId)
                            .WithState(AdvertisementState.Expired.ToString())
                            .WithLink("self", link)
                            .WithLink("view", viewRenderedAdvertisementLink)
                            .WithExpiryDate(expiryDate)
                            .WithAgentId(null)
                            .WithAdditionalProperties(AdditionalPropertyType.ResidentsOnly.ToString(), AdditionalPropertyType.Graduate.ToString())
                            .Build()
                    });
        }

        private void AssertReturnedAdvertisementMatchesExpectedExpiredAdvertisement(string advertisementId, DateTime expiryDate, AdvertisementResource result)
        {
            AdvertisementResource expectedResult = new AdvertisementResourceBuilder(this.AllFieldsInitializer)
                .WithId(new Guid(advertisementId))
                .WithLinks(advertisementId)
                .WithState(AdvertisementState.Expired)
                .WithExpiryDate(expiryDate)
                .WithAgentId(null)
                .Build();

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ExpireAdvertisementWithPostVerb()
        {
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            string link = $"{AdvertisementLink}/{advertisementId}";
            string viewRenderedAdvertisementLink = $"{AdvertisementLink}/{advertisementId}/view";
            DateTime expiryDate = new DateTime(2015, 10, 7, 21, 19, 00, DateTimeKind.Utc);

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a POST advertisement request to expire an advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Post,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementPatchVersion1 },
                            { "Accept", this._acceptHeader }
                        },
                        Body = this._expireRequest
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 202,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementVersion1 },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new AdvertisementResponseContentBuilder(this.AllFieldsInitializer)
                            .WithId(advertisementId)
                            .WithState(AdvertisementState.Expired.ToString())
                            .WithLink("self", link)
                            .WithLink("view", viewRenderedAdvertisementLink)
                            .WithExpiryDate(expiryDate)
                            .WithAgentId(null)
                            .WithAdditionalProperties(AdditionalPropertyType.ResidentsOnly.ToString(), AdditionalPropertyType.Graduate.ToString())
                            .Build()
                    });

            AdvertisementResource result;
            var requestUri = new Uri(this.Fixture.AdPostingApiServiceBaseUri, link);

            using (var client = new HttpClient())
            {
                using (HttpRequestMessage request = this.CreatePatchRequest(requestUri, this._expireRequest, oAuth2Token.AccessToken, "POST"))
                {
                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();

                        result = JsonConvert.DeserializeObject<AdvertisementResource>(
                            await response.Content.ReadAsStringAsync(), new ResourceConverter(null, requestUri, response.Headers));
                    }
                }
            }

            AdvertisementResource expectedResult = new AdvertisementResourceBuilder(this.AllFieldsInitializer)
                .WithId(new Guid(advertisementId))
                .WithLinks(advertisementId)
                .WithState(AdvertisementState.Expired)
                .WithExpiryDate(expiryDate)
                .WithAgentId(null)
                .Build();

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ExpireAlreadyExpiredAdvertisement()
        {
            var advertisementId = new Guid("c294088d-ff50-4374-bc38-7fa805790e3e");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is an expired advertisement")
                .UponReceiving("a PATCH advertisement request to expire an advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementPatchVersion1 },
                            { "Accept", this._acceptHeader }
                        },
                        Body = new[]
                        {
                            new
                            {
                                op = "replace",
                                path = "state",
                                value = AdvertisementState.Expired.ToString()
                            }
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementErrorVersion1 },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new
                        {
                            message = "Forbidden",
                            errors = new[] { new { code = "Expired" } }
                        }
                    });

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.ExpireAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            var expectedException =
                new UnauthorizedException(
                    RequestId,
                    new AdvertisementErrorResponse
                    {
                        Message = "Forbidden",
                        Errors = new[] { new AdvertisementError { Code = "Expired" } }
                    });

            actualException.ShouldBeEquivalentToException(expectedException);
        }

        [Fact]
        public async Task ExpireNonExistentAdvertisment()
        {
            var advertisementId = new Guid("9b650105-7434-473f-8293-4e23b7e0e064");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .UponReceiving("a PATCH advertisement request to expire a non-existent advertisement")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementPatchVersion1 },
                            { "Accept", this._acceptHeader }
                        },
                        Body = new[]
                        {
                            new
                            {
                                op = "replace",
                                path = "state",
                                value = AdvertisementState.Expired.ToString()
                            }
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 404,
                        Headers = new Dictionary<string, string> { { "X-Request-Id", RequestId } }
                    });

            AdvertisementNotFoundException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<AdvertisementNotFoundException>(
                    async () => await client.ExpireAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            actualException.ShouldBeEquivalentToException(new AdvertisementNotFoundException(RequestId));
        }

        [Fact]
        public async Task ExpireAdvertisementUsingDisabledRequestorAccount()
        {
            var advertisementId = new Guid("8e2fde50-bc5f-4a12-9cfb-812e50500184");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.ValidAccessToken_Disabled).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a PATCH advertisement request to expire a job using a disabled requestor account")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementPatchVersion1 },
                            { "Accept", this._acceptHeader }
                        },
                        Body = new[]
                        {
                            new
                            {
                                op = "replace",
                                path = "state",
                                value = AdvertisementState.Expired.ToString()
                            }
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementErrorVersion1 },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new
                        {
                            message = "Forbidden",
                            errors = new[] { new { code = "AccountError" } }
                        }
                    });

            UnauthorizedException actualException;

            using (AdPostingApiClient client = this.Fixture.GetClient(oAuth2Token))
            {
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.ExpireAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    RequestId,
                    new AdvertisementErrorResponse
                    {
                        Message = "Forbidden",
                        Errors = new[] { new AdvertisementError { Code = "AccountError" } }
                    }));
        }

        [Fact]
        public async Task ExpireAdvertisementWhereAdvertiserNotRelatedToRequestor()
        {
            var advertisementId = new Guid("8e2fde50-bc5f-4a12-9cfb-812e50500184");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.OtherThirdPartyUploader).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a PATCH advertisement request to expire a job for an advertiser not related to the requestor's account")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementPatchVersion1 },
                            { "Accept", this._acceptHeader }
                        },
                        Body = new[]
                        {
                            new
                            {
                                op = "replace",
                                path = "state",
                                value = AdvertisementState.Expired.ToString()
                            }
                        }
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 403,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementErrorVersion1 },
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
                actualException = await Assert.ThrowsAsync<UnauthorizedException>(
                    async () => await client.ExpireAdvertisementAsync(new Uri(this.Fixture.AdPostingApiServiceBaseUri, link)));
            }

            actualException.ShouldBeEquivalentToException(
                new UnauthorizedException(
                    RequestId,
                    new AdvertisementErrorResponse
                    {
                        Message = "Forbidden",
                        Errors = new[] { new AdvertisementError { Code = "RelationshipError" } }
                    }));
        }

        [Fact]
        public async Task ExpireAdvertisementUsingInvalidRequestContent()
        {
            var advertisementId = new Guid("8e2fde50-bc5f-4a12-9cfb-812e50500184");
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().WithAccessToken(AccessTokens.OtherThirdPartyUploader).Build();
            var link = $"{AdvertisementLink}/{advertisementId}";
            var acceptHeader = this._acceptHeader;
            var requestBody = new[]
            {
                new
                {
                    op = "add",
                    path = "state",
                    value = "open"
                }
            };

            this.Fixture.AdPostingApiService
                .Given("There is a pending standout advertisement with maximum data")
                .UponReceiving("a PATCH advertisement request to expire a job using invalid request content")
                .With(
                    new ProviderServiceRequest
                    {
                        Method = HttpVerb.Patch,
                        Path = link,
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + oAuth2Token.AccessToken },
                            { "Content-Type", RequestContentTypes.AdvertisementPatchVersion1 },
                            { "Accept", acceptHeader }
                        },
                        Body = requestBody
                    }
                )
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 422,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", ResponseContentTypes.AdvertisementErrorVersion1 },
                            { "X-Request-Id", RequestId }
                        },
                        Body = new
                        {
                            message = "Validation Failure",
                            errors = new[] { new { code = "InvalidRequestContent" } }
                        }
                    });

            using (var client = new HttpClient())
            {
                using (HttpRequestMessage request = this.CreatePatchRequest(
                    new Uri(this.Fixture.AdPostingApiServiceBaseUri, link), requestBody, AccessTokens.OtherThirdPartyUploader))
                {
                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        Assert.Equal(422, (int)response.StatusCode);
                    }
                }
            }
        }

        private AdPostingApiFixture Fixture { get; }

        private HttpRequestMessage CreatePatchRequest(Uri requestUri, object requestBody, string accessToken, string verb = "PATCH")
        {
            var request = new HttpRequestMessage(new HttpMethod(verb), requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(requestBody))
            };

            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(RequestContentTypes.AdvertisementPatchVersion1);
            request.Headers.Accept.Clear();
            request.Headers.Accept.ParseAdd(this._acceptHeader);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return request;
        }
    }
}