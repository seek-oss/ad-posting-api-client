using Xunit;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    [CollectionDefinition(Name)]
    public class AdPostingTemplateApiCollection : ICollectionFixture<AdPostingTemplateApiPactService>
    {
        public const string Name = "Ad Posting Template API Service Collection";
    }
}