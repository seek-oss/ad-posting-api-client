using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client.Hal
{
    internal static class TypeExtensions
    {
        public static string GetMediaType(this Type type, string defaultMediaType)
        {
            var mediaTypeAttribute = type.GetCustomAttributes(typeof(MediaTypeAttribute), true)
                .Cast<MediaTypeAttribute>()
                .SingleOrDefault();

            return mediaTypeAttribute == null ? defaultMediaType : mediaTypeAttribute.MediaType;
        }
    }
}
