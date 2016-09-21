using System;
using System.Net;
using System.Runtime.Serialization;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class TooManyRequestsException : RequestException
    {
        public TooManyRequestsException(string requestId, TimeSpan? delta) : base(requestId, 429, "Too many requests have been sent in a given amount of time.")
        {
            this.RetryAfter = delta;
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