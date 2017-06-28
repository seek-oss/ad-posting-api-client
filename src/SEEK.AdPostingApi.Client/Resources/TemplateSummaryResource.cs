using System;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Resources
{
    [MediaType("application/vnd.seek.template+json;version=1")]
    public class TemplateSummaryResource : TemplateSummary, IResource
    {
        private Hal.Client _client;

        [JsonIgnore]
        public Links Links { get; set; }

        [JsonIgnore]
        public Uri Uri => null;

        public void Initialise(Hal.Client client)
        {
            this._client = client;
        }
    }
}