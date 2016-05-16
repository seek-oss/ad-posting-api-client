using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Resources
{
    [MediaType("application/vnd.seek.advertisement+json")]
    public class AdvertisementResource : Advertisement, IResource
    {
        private Hal.Client _client;

        public AdvertisementError[] Errors { get; set; }

        [JsonIgnore]
        public Links Links { get; set; }

        [JsonIgnore]
        [FromHeader("Processing-Status")]
        public ProcessingStatus ProcessingStatus { get; set; }

        public AdvertisementState State { get; set; }

        [JsonIgnore]
        public Uri Uri => this.Links.GenerateLink("self");

        public async Task<AdvertisementResource> ExpireAsync()
        {
            return await this._client.PatchResourceAsync<AdvertisementResource, ExpireAdvertisementJsonPatch>(this.Uri, new ExpireAdvertisementJsonPatch());
        }

        public void Initialise(Hal.Client client)
        {
            this._client = client;
        }

        public async Task<AdvertisementResource> SaveAsync()
        {
            return await this._client.PutResourceAsync<AdvertisementResource, Advertisement>(this.Uri, this);
        }

        public bool ShouldSerializeErrors()
        {
            return false;
        }

        public bool ShouldSerializeState()
        {
            return false;
        }
    }
}