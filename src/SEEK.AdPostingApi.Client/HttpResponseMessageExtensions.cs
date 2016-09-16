using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    public static class HttpResponseMessageExtensions
    {
        public static string GetHeaderValue(this HttpResponseMessage httpResponse, string headerKey)
        {
            IEnumerable<string> headerValues;
            string headerValue = httpResponse.Headers.TryGetValues(headerKey, out headerValues)
                ? headerValues.First()
                : null;

            return headerValue;
        }
    }
}