using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Resources
{
    public class IndexResource : IResource
    {
        private Hal.Client _client;

        public void Initialise(Hal.Client client)
        {
            this._client = client;
        }

        [JsonIgnore]
        public Uri Uri => this.Links.GenerateLink("self");

        [JsonIgnore]
        public Links Links { get; set; }

        public async Task<AdvertisementResource> CreateAdvertisementAsync(Advertisement advertisement)
        {
            return await this._client.PostResourceAsync<AdvertisementResource, Advertisement>(this.Links.GenerateLink("advertisements"), advertisement);
        }

        public async Task<AdvertisementSummaryPageResource> GetAllAdvertisements(string advertiserIdentifier = null)
        {
            return string.IsNullOrWhiteSpace(advertiserIdentifier)
                ? await this._client.GetResourceAsync<AdvertisementSummaryPageResource>(
                    this.Links.GenerateLink("advertisements"))
                : await this._client.GetResourceAsync<AdvertisementSummaryPageResource>(
                    this.Links.GenerateLink("advertisements", new { advertiserId = advertiserIdentifier }));
        }
    }
}