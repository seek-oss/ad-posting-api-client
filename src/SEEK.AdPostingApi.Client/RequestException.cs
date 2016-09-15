using System;
using System.Runtime.Serialization;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class RequestException : Exception
    {
        public RequestException(string requestId, int statusCode, string message) : base(message)
        {
            this.RequestId = requestId;
            this.StatusCode = statusCode;
        }

        public RequestException(SerializationInfo info, StreamingContext context) : base()
        {
            this.RequestId = (string)info.GetValue(nameof(this.RequestId), typeof(string));
            this.StatusCode = (int)info.GetValue(nameof(this.StatusCode), typeof(int));
        }

        public string RequestId { get; }
        public int StatusCode { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.RequestId), this.RequestId);
            info.AddValue(nameof(this.StatusCode), this.StatusCode);

            base.GetObjectData(info, context);
        }
    }
}