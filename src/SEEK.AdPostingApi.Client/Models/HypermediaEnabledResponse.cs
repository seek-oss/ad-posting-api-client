using System.Collections.Generic;
using Newtonsoft.Json;

namespace SEEK.AdPostingApi.Client.Models
{
    public class HypermediaEnabledResponse
    {
        [JsonProperty(PropertyName = "_links")]
        public IDictionary<string, HypermediaLink> Links { get; set; }
    }
}
