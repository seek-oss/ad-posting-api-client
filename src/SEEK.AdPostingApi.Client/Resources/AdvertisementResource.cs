using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Resources
{
    [MediaType("application/vnd.seek.advertisement+json")]
    public class AdvertisementResource : HalResource<Advertisement>
    {
        public async Task<AdvertisementResource> SaveAsync()
        {
            return await this.PutResourceAsync<AdvertisementResource, Advertisement>(this.GetUri(), this.Properties);
        }

        public async Task<AdvertisementResource> ExpireAsync()
        {
            return await this.PatchResourceAsync<AdvertisementResource, AdvertisementPatch>(this.GetUri(), new AdvertisementPatch { State = AdvertisementState.Expired });
        }
    }
}