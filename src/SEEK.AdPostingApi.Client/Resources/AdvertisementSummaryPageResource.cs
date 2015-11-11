using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Resources
{
    public class AdvertisementSummaryPageResource : IResource
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

        [Embedded(Rel = "advertisements")]
        public IList<AdvertisementSummaryResource> AdvertisementSummaries { get; set; }

        public async Task<AdvertisementSummaryPageResource> NextPageAsync()
        {
            if (this.Eof)
            {
                throw new NotSupportedException("There are no more results");
            }

            return await this._client.GetResourceAsync<AdvertisementSummaryPageResource>(this.Links.GenerateLink("next"));
        }

        public bool Eof => (this.Links == null || !this.Links.ContainsKey("next"));
    }
}