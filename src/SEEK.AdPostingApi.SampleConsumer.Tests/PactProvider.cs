using PactNet;
using PactNet.Mocks.MockHttpService;
using System;
using System.Collections.Generic;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    public static class PactProvider
    {
        private const int MockProviderServicePort = 8893;

        private static readonly IPactBuilder PactBuilder;

        static PactProvider()
        {
            PactBuilder = new PactBuilder(new PactConfig
            {
                PactDir = "../../pacts/",
                LogDir = "../../logs/"

            }).ServiceConsumer("AdPostingApi SampleConsumer")
              .HasPactWith("AdPostingApi");

            MockService = PactBuilder.MockService(MockProviderServicePort);
            MockServiceUri = new Uri("http://localhost:" + MockProviderServicePort);
        }

        public static IMockProviderService MockService { get; private set; }

        public static Uri MockServiceUri { get; private set; }

        public static void ClearInteractions()
        {
            MockService.ClearInteractions();
        }

        public static void VerifyInteractions()
        {
            MockService.VerifyInteractions();
        }

        public static void AssemblyCleanup()
        {
            PactBuilder.Build();
        }

        public static void MockLinks()
        {
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            const string advertisementLink = "/advertisement";
            MockService
                .UponReceiving("a request to retrieve API links")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/",
                    Headers = new Dictionary<string, string>
                    {
                        {"Accept", "application/hal+json"},
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/hal+json; charset=utf-8"}
                    },
                    Body = new
                    {
                        _links = new
                        {
                            advertisements = new
                            {
                                href = advertisementLink
                            },
                            advertisement = new
                            {
                                href = advertisementLink + "/{advertisementId}",
                                templated = true
                            }
                        }
                    }
                });
        }
    }
}