using System;
using System.Runtime.Serialization;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public abstract class RequestException : Exception
    {
        protected RequestException(string requestId, string message) : base(message)
        {
            this.RequestId = requestId;
        }

        protected RequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.RequestId = (string)info.GetValue(nameof(this.RequestId), typeof(string));
        }

        public string RequestId { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.RequestId), this.RequestId);

            base.GetObjectData(info, context);
        }
    }
}