using Newtonsoft.Json;

namespace SEEK.AdPostingApi.Client.Models
{
    public class JsonPatchOperation
    {
        [JsonProperty("op")]
        public string Operation { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}