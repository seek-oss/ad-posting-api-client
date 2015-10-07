using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Models
{
    [MediaType("application/vnd.seek.advertisement-patch+json")]
    public class AdvertisementPatch
    {
        public AdvertisementState State { get; set; }
    }
}