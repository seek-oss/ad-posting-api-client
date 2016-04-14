using System;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.Client
{
    public interface IAdPostingApiClient : IDisposable
    {
        Task<AdvertisementResource> CreateAdvertisementAsync(Advertisement advertisement);

        Task<AdvertisementResource> GetAdvertisementAsync(Uri uri);

        Task<ProcessingStatus> GetAdvertisementStatusAsync(Uri uri);

        Task<AdvertisementSummaryPageResource> GetAllAdvertisementsAsync(string advertiserId = null);

        Task<AdvertisementResource> ExpireAdvertisementAsync(Uri uri, AdvertisementPatch advertisementPatch);

        Task<AdvertisementResource> UpdateAdvertisementAsync(Uri uri, Advertisement advertisement);
    }
}