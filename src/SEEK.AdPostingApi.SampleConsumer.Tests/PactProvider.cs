using PactNet;
using PactNet.Mocks.MockHttpService;
using System;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    public class PactProvider : IDisposable
    {
        private const int MockProviderServicePort = 8893;

        private readonly IPactBuilder _pactBuilder;

        public PactProvider()
        {
            this._pactBuilder = new PactBuilder()
                .ServiceConsumer("AdPostingApi SampleConsumer")
                .HasPactWith("AdPostingApi");

            this.MockService = _pactBuilder.MockService(MockProviderServicePort);
            this.MockServiceUri = new Uri("http://localhost:" + MockProviderServicePort);
        }

        public IMockProviderService MockService { get; private set; }

        public Uri MockServiceUri { get; private set; }

        public void Dispose()
        {
            this._pactBuilder.Build();
        }
    }
}