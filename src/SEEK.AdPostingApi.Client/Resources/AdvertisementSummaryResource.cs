using System;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Resources
{
    public class AdvertisementSummaryResource : AdvertisementSummary, IResource
    {
        public void Initialise(Hal.Client client)
        {
        }

        [JsonIgnore]
        public Uri Uri => this.Links.GenerateLink("self");

        [JsonIgnore]
        public Links Links { get; set; }
    }
}