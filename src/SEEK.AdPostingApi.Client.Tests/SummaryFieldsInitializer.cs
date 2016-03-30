using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    public class SummaryFieldsInitializer : IBuilderInitializer
    {
        public void Initialize(AdvertisementContentBuilder builder)
        {
            builder
                .WithAdvertiserId(GetDefaultAdvertiserId())
                .WithJobTitle(GetDefaultJobTitle())
                .WithJobReference(GetDefaultjobReference());
        }

        public void Initialize(AdvertisementModelBuilder builder)
        {
            builder
                .WithAdvertiserId(GetDefaultAdvertiserId())
                .WithJobTitle(GetDefaultJobTitle())
                .WithJobReference(GetDefaultjobReference());
        }

        private string GetDefaultAdvertiserId()
        {
            return "1";
        }

        private string GetDefaultJobTitle()
        {
            return "Exciting Senior Developer role in a great CBD location. Great $$$";
        }

        private string GetDefaultjobReference()
        {
            return "JobReference1";
        }
    }
}
