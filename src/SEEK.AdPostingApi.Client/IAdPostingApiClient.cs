using SEEK.AdPostingApi.Client.Models;
using System;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client
{
    public interface IAdPostingApiClient : IDisposable
    {
        Task<Uri> CreateAdvertisementAsync(Advertisement advertisement);

        Task<string> GetAdvertisementAsync(Uri advertisementLocation);
    }
}