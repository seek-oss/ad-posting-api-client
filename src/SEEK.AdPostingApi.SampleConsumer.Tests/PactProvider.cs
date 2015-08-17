using PactNet;
using PactNet.Mocks.MockHttpService;
using System;

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
    }
}