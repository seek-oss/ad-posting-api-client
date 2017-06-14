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

            this.MockProviderService = this.PactBuilder.MockService(this.MockServerPort);
        }

        public IMockProviderService MockProviderService { get; }

        public Uri MockProviderServiceBaseUri => new Uri($"http://localhost:{this.MockServerPort}");

        private int MockServerPort => 8894;

        private IPactBuilder PactBuilder { get; }

        public void Dispose()
        {
            this.PactBuilder.Build();
        }
    }
}