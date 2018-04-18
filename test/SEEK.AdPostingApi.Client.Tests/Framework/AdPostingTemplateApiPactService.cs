using System;
using Newtonsoft.Json;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class AdPostingTemplateApiPactService : IDisposable
    {
        public AdPostingTemplateApiPactService()
        {
            this.PactBuilder = new PactBuilder(new PactConfig { PactDir = @"..\..\..\..\..\pact", LogDir = @"..\..\..\..\..\logs" })
                .ServiceConsumer("Ad Posting API Client")
                .HasPactWith("Ad Posting Template API");

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };

            this.MockProviderService = this.PactBuilder.MockService(MockServerPort, serializerSettings);
        }

        public IMockProviderService MockProviderService { get; }

        public static Uri MockProviderServiceBaseUri => new Uri($"http://localhost:{MockServerPort}");

        private static int MockServerPort => 8894;

        private IPactBuilder PactBuilder { get; }

        public void Dispose()
        {
            this.PactBuilder.Build();
        }
    }
}