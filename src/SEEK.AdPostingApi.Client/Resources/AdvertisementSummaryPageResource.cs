using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Resources
{
    public class AdvertisementSummaryPageResource : HalResource, IEnumerable<AdvertisementSummaryResource>
    {
        private class EmbeddedResourceList
        {
            public IEnumerable<AdvertisementSummaryResource> Advertisements { get; set; }
        }

        private EmbeddedResourceList Embedded { get; set; }

        public IEnumerator<AdvertisementSummaryResource> GetEnumerator()
        {
            return this.Embedded.Advertisements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Embedded.Advertisements.GetEnumerator();
        }

        public async Task<AdvertisementSummaryPageResource> NextPageAsync()
        {
            if (Eof)
            {
                throw new NotSupportedException("There are no more results");
            }

            return await this.GetResourceAsync<AdvertisementSummaryPageResource>(this.GenerateLink("next"));
        }

        public bool Eof => (this.Links == null) || !this.Links.ContainsKey("next");
    }
}