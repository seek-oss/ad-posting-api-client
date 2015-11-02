using System;
using System.Linq;
using System.Net.Http.Headers;

namespace SEEK.AdPostingApi.Client.Hal
{
    public class FromHeaderAttribute : Attribute
    {
        private readonly string _header;

        public FromHeaderAttribute(string header)
        {
            this._header = header;
        }

        public T GetValue<T>(HttpResponseHeaders headers)
        {
            if (typeof(T).IsEnum)
                return (T)Enum.Parse(typeof(T), headers.GetValues(this._header).Single());

            throw new NotSupportedException();
        }
    }
}