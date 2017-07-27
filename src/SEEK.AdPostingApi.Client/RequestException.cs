using System;
using System.Runtime.Serialization;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class RequestException : Exception
    {
        public RequestException(string requestId, int statusCode, string message, string responseContent = null, string responseContentType = null) : base(message)
        {
            this.RequestId = requestId;
            this.ResponseContent = responseContent;
            this.ResponseContentType = responseContentType;
            this.StatusCode = statusCode;
        }
        
        protected RequestException(SerializationInfo info)
        {
            this.RequestId = (string)info.GetValue(nameof(this.RequestId), typeof(string));
            this.ResponseContent = (string)info.GetValue(nameof(this.ResponseContent), typeof(string));
            this.ResponseContentType = (string)info.GetValue(nameof(this.ResponseContentType), typeof(string));
            this.StatusCode = (int)info.GetValue(nameof(this.StatusCode), typeof(int));
        }

        public string RequestId { get; }
        public string ResponseContent { get; }
        public string ResponseContentType { get; }
        public int StatusCode { get; }

        public void GetObjectData(SerializationInfo info)
        {
            info.AddValue(nameof(this.RequestId), this.RequestId);
            info.AddValue(nameof(this.StatusCode), this.StatusCode);
            info.AddValue(nameof(this.ResponseContent), this.ResponseContent);
            info.AddValue(nameof(this.ResponseContentType), this.ResponseContentType);
        }
    }
}