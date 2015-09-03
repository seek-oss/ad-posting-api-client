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
        private readonly IOAuth2TokenClient _tokenClient;

        public HalResourceConverter(HttpClient httpClient, Uri baseUri, IOAuth2TokenClient tokenClient)
        {
            _httpClient = httpClient;
            _baseUri = baseUri;
            _tokenClient = tokenClient;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = (HalResource)Activator.CreateInstance(objectType);
            obj.InitialiseEmbedded(this._httpClient, this._baseUri, this._tokenClient);
            obj.PopulateResource(JToken.ReadFrom(reader));
            return obj;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(HalResource).IsAssignableFrom(objectType);
        }
    }
}