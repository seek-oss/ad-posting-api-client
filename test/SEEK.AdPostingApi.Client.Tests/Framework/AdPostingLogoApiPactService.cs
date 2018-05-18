using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class AdPostingLogoApiPactService : IDisposable
    {
        public AdPostingLogoApiPactService()
        {
            this.PactBuilder = new PactBuilder(new PactConfig { PactDir = @"..\..\..\..\..\pact", LogDir = @"..\..\..\..\..\logs" })
                .ServiceConsumer("Ad Posting API Client")
                .HasPactWith("Ad Posting Logo API");

            this.MockProviderService = this.PactBuilder.MockService(MockServerPort);
        }

        public IMockProviderService MockProviderService { get; }

        public static Uri MockProviderServiceBaseUri => new Uri($"http://localhost:{MockServerPort}");

        private static int MockServerPort => 8895;

        private IPactBuilder PactBuilder { get; }

        public void Dispose()
        {
            this.PactBuilder.Build();
        }
    }
}