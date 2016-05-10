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

        public void Initialise(Hal.Client client)
        {
            this._client = client;
        }

        [JsonIgnore]
        public Uri Uri => this.Links.GenerateLink("self");

        [JsonIgnore]
        public Links Links { get; set; }

        [JsonIgnore]
        [FromHeader("Processing-Status")]
        public ProcessingStatus ProcessingStatus { get; set; }

        public AdvertisementState State { get; set; }

        public AdvertisementError[] Errors { get; set; }

        public async Task<AdvertisementResource> SaveAsync()
        {
            return await this._client.PutResourceAsync<AdvertisementResource, Advertisement>(this.Uri, this);
        }

        public async Task<AdvertisementResource> ExpireAsync()
        {
            return await this._client.PatchResourceAsync<AdvertisementResource, ExpireAdvertisementJsonPatch>(this.Uri, new ExpireAdvertisementJsonPatch());
        }

        public bool ShouldSerializeState()
        {
            return false;
        }

        public bool ShouldSerializeErrors()
        {
            return false;
        }
    }
}