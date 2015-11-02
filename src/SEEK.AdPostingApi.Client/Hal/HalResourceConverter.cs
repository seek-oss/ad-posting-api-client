using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SEEK.AdPostingApi.Client.Hal
{
    public class HalResourceConverter : JsonConverter
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _baseUri;

        public HalResourceConverter(HttpClient httpClient, Uri baseUri)
        {
            _httpClient = httpClient;
            _baseUri = baseUri;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = (HalResource)Activator.CreateInstance(objectType);

            obj.Initialise(this._httpClient, this._baseUri);
            obj.PopulateResource(JToken.ReadFrom(reader));

            return obj;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(HalResource).IsAssignableFrom(objectType);
        }
    }
}