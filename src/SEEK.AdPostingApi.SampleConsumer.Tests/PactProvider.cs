using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet;
using PactNet.Mocks.MockHttpService;
using System;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    [TestClass]
    public static class PactProvider
    {
        private const int MockProviderServicePort = 8893;

        private static readonly IPactBuilder PactBuilder;

        static PactProvider()
        {
            PactBuilder = new PactBuilder()
                .ServiceConsumer("AdPostingApi SampleConsumer")
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

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            PactBuilder.Build();
        }
    }
}