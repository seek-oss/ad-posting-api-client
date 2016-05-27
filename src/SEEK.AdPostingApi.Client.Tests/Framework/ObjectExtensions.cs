using System.Dynamic;
using Newtonsoft.Json;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public static class ObjectExtensions
    {
        public static T Clone<T>(this T source)
        {
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            var jsonSerializerSettings = source is ExpandoObject
                ? null
                : new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source, jsonSerializerSettings));
        }
    }
}