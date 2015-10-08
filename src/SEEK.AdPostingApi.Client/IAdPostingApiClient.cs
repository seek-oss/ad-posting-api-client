using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using System;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client
{
    public interface IAdPostingApiClient : IDisposable
    {
        Task<AdvertisementResource> CreateAdvertisementAsync(Advertisement advertisement);

        Task<AdvertisementResource> GetAdvertisementAsync(Uri uri);
    }
}