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

        public Task<AdvertisementResource> GetAdvertisementByIdAsync(Guid id)
        {
            return this.GetResourceAsync<AdvertisementResource>("advertisement", new {advertisementId = id});
        }
        public Task<AdvertisementListResource> GetAllAdvertisements()
        {
            return this.GetResourceAsync<AdvertisementListResource>("advertisement");
        }

        public Task<AdvertisementResource> UpdateAdvertisementByIdAsync(Guid id, Advertisement advertisement)
        {
            return this.PutResourceAsync<AdvertisementResource, Advertisement>("advertisement", new { advertisementId = id }, advertisement);
        }

        public Task<Status> GetAdvertisementStatusByIdAsync(Guid id)
        {
            return base.HeadResourceAsync<Status, AdvertisementResource>("advertisement", new { advertisementId = id });
        }
    }
}
