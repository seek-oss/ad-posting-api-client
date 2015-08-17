using System;
using System.Linq;

namespace SEEK.AdPostingApi.Client
{
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum @enum) where T : Attribute
        {
            return @enum.GetType().GetMember(@enum.ToString())[0]
                .GetCustomAttributes(typeof (T), false)
                .Cast<T>()
                .SingleOrDefault();
        }
    }
}
