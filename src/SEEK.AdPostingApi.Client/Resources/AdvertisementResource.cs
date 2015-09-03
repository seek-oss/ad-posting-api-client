using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client.Resources
{
    [MediaType("application/vnd.seek.advertisement+json")]
    public class AdvertisementResource : HalResource<Advertisement>
    {
        public string Status { get; set; }

        public async Task SaveAsync()
        {
            await this.PutResourceAsync("self", this);
        }
    }
}