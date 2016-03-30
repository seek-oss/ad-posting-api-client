using Xunit;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    [CollectionDefinition(Name)]
    public class AdPostingApiCollection : ICollectionFixture<AdPostingApiPactService>
    {
        public const string Name = "Ad Posting API Service Collection";
    }
}