using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class AdPostingTemplateApiPactService : IPactService, IDisposable
    {
        public AdPostingTemplateApiPactService()
        {
            this.PactBuilder = new PactBuilder(new PactConfig { PactDir = @"..\..\..\..\pact", LogDir = @"..\..\logs" })
                .ServiceConsumer("Ad Posting API Client")
                .HasPactWith("Ad Posting Template API");

            this.MockProviderService = this.PactBuilder.MockService(MockServerPort);
        }

        public IMockProviderService MockProviderService { get; }

        public static Uri _MockProviderServiceBaseUri => new Uri($"http://localhost:{_MockServerPort}");

        public Uri MockProviderServiceBaseUri => _MockProviderServiceBaseUri;

        public static int _MockServerPort => 8894;

        private int MockServerPort => _MockServerPort;

        private IPactBuilder PactBuilder { get; }

        public void Dispose()
        {
            this.PactBuilder.Build();
        }
    }
}