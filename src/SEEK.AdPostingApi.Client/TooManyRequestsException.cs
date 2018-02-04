using System;

namespace SEEK.AdPostingApi.Client
{
    public class TooManyRequestsException : RequestException
    {
        public TooManyRequestsException(string requestId, TimeSpan? delta) : base(requestId, 429, "Too many requests have been sent in a given amount of time.")
        {
            this.RetryAfter = delta;
        }

        public TimeSpan? RetryAfter { get; }
    }
}