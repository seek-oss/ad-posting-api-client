using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using System;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client
{
    public interface IAdPostingApiClient : IDisposable
    {
        Task<Uri> CreateAdvertisementAsync(Advertisement advertisement);

        Task<AdvertisementResource> GetAdvertisementAsync(Guid id);

        Task<AdvertisementResource> GetAdvertisementAsync(Uri uri);
    }
}