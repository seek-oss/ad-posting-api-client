using System;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Resources
{
    public class IndexResource : HalResource
    {
        public Task<AdvertisementResource> CreateAdvertisementAsync(Advertisement advertisement)
        {
            return this.PostResourceAsync<AdvertisementResource, Advertisement>("advertisements", advertisement);
        }

        public Task<AdvertisementListResource> GetAllAdvertisements()
        {
            return this.GetResourceAsync<AdvertisementListResource>("advertisement");
        }
    }
}