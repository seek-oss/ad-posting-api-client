using System;
using System.Net;
using System.Runtime.Serialization;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class TooManyRequestsException : RequestException
    {
        public TooManyRequestsException(string requestId, int? retryAfterSeconds) : base(requestId, 429, "Too many requests have been sent in a given amount of time.")
        {
            this.RetryAfter = retryAfterSeconds == null
                ? (TimeSpan?)null
                : TimeSpan.FromSeconds(retryAfterSeconds.Value);
        }

        public TooManyRequestsException(string requestId) : this(requestId, null)
        {
        }

        protected TooManyRequestsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.RetryAfter = (TimeSpan)info.GetValue(nameof(this.RetryAfter), typeof(TimeSpan));
        }

        public TimeSpan? RetryAfter { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.RetryAfter), this.RetryAfter);

            base.GetObjectData(info, context);
        }
    }
}