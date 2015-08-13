using System;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    public interface IAdPostingApiClient : IDisposable
    {
        Task<Uri> CreateAdvertisementAsync(Advertisement advertisement);
    }
}
