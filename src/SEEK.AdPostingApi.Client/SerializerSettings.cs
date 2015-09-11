using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SEEK.AdPostingApi.Client
{
    internal class SerializerSettings : JsonSerializerSettings
    {
        public SerializerSettings()
        {
            this.ContractResolver = new CamelCasePropertyNamesContractResolver();
            this.NullValueHandling = NullValueHandling.Ignore;
            this.Converters.Add(new StringEnumConverter());
        }
    }
}
