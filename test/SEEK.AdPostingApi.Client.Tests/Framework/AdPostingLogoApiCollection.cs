using Xunit;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    [CollectionDefinition(Name)]
    public class AdPostingLogoApiCollection : ICollectionFixture<AdPostingLogoApiPactService>
    {
        public const string Name = "Ad Posting Logo API Service Collection";
    }
}