using System;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.Client
{
    public interface IAdPostingApiClient : IDisposable
    {
        Task<AdvertisementResource> CreateAdvertisementAsync(Advertisement advertisement);

        Task<GetAdvertisementResult> GetAdvertisementAsync(Uri uri);

        Task<ProcessingStatus> GetAdvertisementStatusAsync(Uri uri);

        Task<AdvertisementListResource> GetAllAdvertisementsAsync();

        Task<AdvertisementResource> ExpireAdvertisementAsync(Uri uri, AdvertisementPatch advertisementPatch);

        Task<AdvertisementResource> UpdateAdvertisementAsync(Uri uri, Advertisement advertisement);
    }
}