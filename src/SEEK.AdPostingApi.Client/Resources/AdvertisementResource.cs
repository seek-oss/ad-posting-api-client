using System;
using System.Linq;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client.Resources
{
    [MediaType("application/vnd.seek.advertisement+json")]
    public class AdvertisementResource : HalResource<Advertisement>
    {
        public Status Status => (Status)Enum.Parse(typeof(Status), this.ResponseHeaders.GetValues("Status").First());

        public async Task SaveAsync()
        {
            await this.PutResourceAsync("self", this);
        }
    }
}