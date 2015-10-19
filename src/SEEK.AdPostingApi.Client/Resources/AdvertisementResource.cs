using System;
using System.Linq;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Resources
{
    [MediaType("application/vnd.seek.advertisement+json")]
    public class AdvertisementResource : HalResource<Advertisement>
    {
        public ProcessingStatus ProcessingStatus => (ProcessingStatus)Enum.Parse(typeof(ProcessingStatus), this.ResponseHeaders.GetValues("Processing-Status").First());

        public async Task<AdvertisementResource> SaveAsync()
        {
            return await this.PutResourceAsync("self", this);
        }

        public async Task<AdvertisementResource> ExpireAsync()
        {
            return await this.PatchResourceAsync<AdvertisementResource, AdvertisementPatch>("self", this, new AdvertisementPatch { State = AdvertisementState.Expired });
        }
    }
}