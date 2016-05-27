using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SEEK.AdPostingApi.Client.Hal
{
    internal class ResourceConverter : JsonConverter
    {
        private readonly Client _client;
        private readonly Uri _uri;
        private readonly HttpResponseHeaders _headers;

        public ResourceConverter(Client client, Uri uri, HttpResponseHeaders headers)
        {
            this._client = client;
            this._uri = uri;
            this._headers = headers;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.ReadFrom(reader);
            object result = JsonConvert.DeserializeObject(token.ToString(), objectType);

            if (token["_embedded"] != null && token["_embedded"].HasValues)
            {
                foreach (JProperty embedded in token["_embedded"].Cast<JProperty>())
                {
                    foreach (var property in objectType.GetProperties())
                    {
                        var attribute = (EmbeddedAttribute)property.GetCustomAttributes(true)
                            .FirstOrDefault(attr => attr is EmbeddedAttribute && ((EmbeddedAttribute)attr).Rel == embedded.Name);

                        if (attribute != null)
                        {
                            property.SetValue(
                                result,
                                JsonConvert.DeserializeObject(
                                    embedded.Value.ToString(), property.PropertyType, new ResourceConverter(this._client, this._uri, this._headers)), null);
                        }
                    }
                }
            }

            foreach (var property in objectType.GetProperties())
            {
                var attribute = (FromHeaderAttribute)property.GetCustomAttributes(true).FirstOrDefault(attr => attr is FromHeaderAttribute);

                if (attribute != null && this._headers.Contains(attribute.Header))
                {
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(property.PropertyType);

                    property.SetValue(result, typeConverter.ConvertFromString(this._headers.GetValues(attribute.Header).First()));
                }
            }

            if (typeof(IResource).IsAssignableFrom(objectType))
            {
                ((IResource)result).Links = (token["_links"] == null)
                    ? new Links()
                    : JsonConvert.DeserializeObject<Links>(token["_links"].ToString());

                ((IResource)result).Links.BaseUri = this._uri;
                ((IResource)result).Initialise(this._client);
            }

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IResource).IsAssignableFrom(objectType);
        }
    }
}