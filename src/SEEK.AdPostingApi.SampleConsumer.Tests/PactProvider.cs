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
        }

        public IMockProviderService MockService { get; }

        public string MockServiceUri
        {
            get { return "http://localhost:" + MockProviderServicePort; }
        }

        public void Dispose()
        {
            this._pactBuilder.Build();
        }
    }
}