using System;
using System.Runtime.Serialization;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class TooManyRequestsException : RequestException
    {
        public TooManyRequestsException(string requestId, TimeSpan? delta) : base(requestId, 429, "Too many requests have been sent in a given amount of time.")
        {
            this.RetryAfter = delta;
        }

        protected TooManyRequestsException(SerializationInfo info) : base(info)
        {
            this.RetryAfter = (TimeSpan?)info.GetValue(nameof(this.RetryAfter), typeof(TimeSpan?));
        }

        public TimeSpan? RetryAfter { get; }

        public void GetObjectData(SerializationInfo info)
        {
            info.AddValue(nameof(this.RetryAfter), this.RetryAfter);

            base.GetObjectData(info);
        }
    }
}