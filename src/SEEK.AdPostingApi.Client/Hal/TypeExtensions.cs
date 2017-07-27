using System;
using System.Linq;
using System.Reflection;

namespace SEEK.AdPostingApi.Client.Hal
{
    internal static class TypeExtensions
    {
        public static string GetMediaType(this Type type)
        {
            var mediaTypeAttribute = type.GetTypeInfo().GetCustomAttributes(typeof(MediaTypeAttribute), true)
                .Cast<MediaTypeAttribute>()
                .Single();

            return mediaTypeAttribute.MediaType;
        }
    }
}