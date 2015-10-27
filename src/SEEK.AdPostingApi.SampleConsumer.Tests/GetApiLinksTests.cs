﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    [TestFixture]
    public class GetApiLinksTests : IDisposable
    {
        public void Dispose()
        {
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

        private const string InvalidLayer7AccessToken = "ca11ab1e-c0de-b10b-feed-faceb0bb1e";
        private const string ValidLayer7AccessTokenInvalidForApi = "ca11ab1e-c0de-b10b-feed-f00db0bb1e";

        [Test]
        public async Task GetApiLinksWithInvalidAccessToken()
        {
            var testOAuth2Token = new OAuth2TokenBuilder().WithAccessToken(InvalidLayer7AccessToken).Build();

            using (var oauthClient = Mock.Of<IOAuth2TokenClient>())
            {
                Mock.Get(oauthClient).Setup(c => c.GetOAuth2TokenAsync()).ReturnsAsync(testOAuth2Token).Callback(() => testOAuth2Token.AccessToken = ValidLayer7AccessTokenInvalidForApi);
                oauthClient.AccessToken = InvalidLayer7AccessToken;

                PactProvider.MockService
                    .UponReceiving("a request to retrieve API links with an invalid access token")
                    .With(new ProviderServiceRequest
                    {
                        Method = HttpVerb.Get,
                        Path = "/",
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + InvalidLayer7AccessToken },
                            { "Accept", "application/hal+json" }
                        }
                    })
                    .WillRespondWith(new ProviderServiceResponse
                    {
                        Status = (int)HttpStatusCode.Unauthorized,
                        Headers = new Dictionary<string, string>
                        {
                            { "WWW-Authenticate", "Bearer error=\"Invalid request\"" }
                        }
                    });

                PactProvider.MockService
                    .UponReceiving("a request to retrieve API links with a valid access token that is not authorised for use with the Ad Posting API")
                    .With(new ProviderServiceRequest
                    {
                        Method = HttpVerb.Get,
                        Path = "/",
                        Headers = new Dictionary<string, string>
                        {
                            { "Authorization", "Bearer " + ValidLayer7AccessTokenInvalidForApi },
                            { "Accept", "application/hal+json" }
                        }
                    })
                    .WillRespondWith(new ProviderServiceResponse { Status = (int)HttpStatusCode.Unauthorized });

                var client = new AdPostingApiClient(PactProvider.MockServiceUri, oauthClient);

                var expectedException = new UnauthorizedException($"[GET] {PactProvider.MockServiceUri} is not authorized.");
                var actualException = Assert.Throws<UnauthorizedException>(async () => await client.GetAllAdvertisementsAsync());

                actualException.ShouldBeEquivalentToException(expectedException);
            }
        }
    }
}
