namespace SEEK.AdPostingApi.Client.Tests
{
    public interface IBuilderInitializer
    {
        void Initialize(AdvertisementContentBuilder builder);

        void Initialize(AdvertisementModelBuilder builder);
    }
}
