using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Resources
{
    [MediaType("application/vnd.seek.template-list+json;version=1")]
    public class TemplateSummaryListResource : IResource
    {
        private Hal.Client _client;

        [Embedded(Rel = "templates")]
        public IList<TemplateSummaryResource> Templates { get; set; }

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
    }
}