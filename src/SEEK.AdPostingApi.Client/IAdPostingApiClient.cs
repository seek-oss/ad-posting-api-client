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

        [Obsolete("The returned status will always be completed. All validation is done upfront and the advertisement will not fail once successfully submitted.")]
        Task<ProcessingStatus> GetAdvertisementStatusAsync(Guid id);

        [Obsolete("The returned status will always be completed. All validation is done upfront and the advertisement will not fail once successfully submitted.")]
        Task<ProcessingStatus> GetAdvertisementStatusAsync(Uri uri);

        Task<AdvertisementSummaryPageResource> GetAllAdvertisementsAsync(string advertiserId = null);

        Task<AdvertisementResource> UpdateAdvertisementAsync(Guid id, Advertisement advertisement);

        Task<AdvertisementResource> UpdateAdvertisementAsync(Uri uri, Advertisement advertisement);
    }
}