using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Resources
{
    public class AdvertisementSummary: HalResource<object>
    {
        public string AdvertiserId { get; set; }
        public string JobTitle { get; set; }
        public string JobReference { get; set; }
    }
}
