using System;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.Client
{
    public interface IAdPostingApiClient : IDisposable
    {
        Task<AdvertisementResource> CreateAdvertisementAsync(Advertisement advertisement);

        Task<AdvertisementResource> ExpireAdvertisementAsync(Guid id);

        Task<AdvertisementResource> ExpireAdvertisementAsync(Uri uri);

        Task<AdvertisementResource> GetAdvertisementAsync(Guid id);

        Task<AdvertisementResource> GetAdvertisementAsync(Uri uri);

        Task<ProcessingStatus> GetAdvertisementStatusAsync(Guid id);

        Task<ProcessingStatus> GetAdvertisementStatusAsync(Uri uri);

        Task<AdvertisementSummaryPageResource> GetAllAdvertisementsAsync(string advertiserId = null);

        Task<AdvertisementResource> UpdateAdvertisementAsync(Guid id, Advertisement advertisement);

        Task<AdvertisementResource> UpdateAdvertisementAsync(Uri uri, Advertisement advertisement);
    }
}