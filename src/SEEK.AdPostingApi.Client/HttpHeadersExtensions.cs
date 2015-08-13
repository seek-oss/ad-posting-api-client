using System.Net;
using System.Net.Http.Headers;

namespace SEEK.AdPostingApi.Client
{
    public static class HttpHeadersExtensions
    {
        public static void AddAccessToken(this HttpRequestHeaders headers, string accessToken)
        {
            headers.Add(HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
        }
    }
}