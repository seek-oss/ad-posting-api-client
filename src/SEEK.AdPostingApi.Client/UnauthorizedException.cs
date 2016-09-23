using System;
using System.Runtime.Serialization;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class UnauthorizedException : RequestException
    {
        public UnauthorizedException(string requestId, int httpStatusCode, string message) : base(requestId, httpStatusCode, message)
        {
            this.Errors = new AdvertisementError[0];
        }

        public UnauthorizedException(string requestId, int httpStatusCode, AdvertisementErrorResponse errorResponse) : base(requestId, httpStatusCode, errorResponse?.Message)
        {
            this.Errors = errorResponse?.Errors ?? new AdvertisementError[0];
        }

        protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Errors = (AdvertisementError[])info.GetValue(nameof(this.Errors), typeof(AdvertisementError[]));
        }

        public AdvertisementError[] Errors { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.Errors), this.Errors);

            base.GetObjectData(info, context);
        }
    }
}