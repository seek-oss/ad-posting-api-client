using System;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Resources
{
    public class AdvertisementSummaryResource : AdvertisementSummary, IResource
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public Links Links { get; set; }

        [JsonIgnore]
        public Uri Uri => this.Links.GenerateLink("self");

        public void Initialise(Hal.Client client)
        {
        }
    }
}