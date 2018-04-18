using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Resources
{
    [MediaType("application/vnd.seek.advertisement-list+json;version=1")]
    public class AdvertisementSummaryPageResource : IResource
    {
        private Hal.Client _client;

        [Embedded(Rel = "advertisements")]
        public IList<AdvertisementSummaryResource> AdvertisementSummaries { get; set; }

        public bool Eof => (this.Links == null || !this.Links.ContainsKey("next"));

        [JsonIgnore]
        public Links Links { get; set; }

        [JsonIgnore]
        [FromHeader("X-Request-Id")]
        public string RequestId { get; set; }

        [JsonIgnore]
        public Uri Uri => this.Links.GenerateLink("self");

        public void Initialise(Hal.Client client)
        {
            this._client = client;
        }

        public async Task<AdvertisementSummaryPageResource> NextPageAsync()
        {
            if (this.Eof)
            {
                throw new NotSupportedException("There are no more results");
            }

            return await this._client.GetResourceAsync<AdvertisementSummaryPageResource, AdvertisementErrorResponse>(this.Links.GenerateLink("next"));
        }
    }
}