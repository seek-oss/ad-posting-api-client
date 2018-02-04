using System;

namespace SEEK.AdPostingApi.Client
{
    public class RequestException : Exception
    {
        public RequestException(string requestId, int statusCode, string message, string responseContent = null, string responseContentType = null) : base(message)
        {
            this.RequestId = requestId;
            this.ResponseContent = responseContent;
            this.ResponseContentType = responseContentType;
            this.StatusCode = statusCode;
        }

        public string RequestId { get; }
        public string ResponseContent { get; }
        public string ResponseContentType { get; }
        public int StatusCode { get; }
    }
}