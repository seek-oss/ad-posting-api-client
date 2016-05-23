using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Tests
{
    public interface IBuilderInitializer
    {
        void Initialize(AdvertisementContentBuilder builder);

        void Initialize<TAdvertisement>(AdvertisementModelBuilder<TAdvertisement> builder) where TAdvertisement : Advertisement, new();
    }
}