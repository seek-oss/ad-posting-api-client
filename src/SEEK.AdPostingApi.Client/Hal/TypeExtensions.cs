using System;
using System.Linq;

namespace SEEK.AdPostingApi.Client.Hal
{
    internal static class TypeExtensions
    {
        public static string GetMediaType(this Type type)
        {
            var mediaTypeAttribute = type.GetCustomAttributes(typeof(MediaTypeAttribute), true)
                .Cast<MediaTypeAttribute>()
                .Single();

            return mediaTypeAttribute.MediaType;
        }
    }
}