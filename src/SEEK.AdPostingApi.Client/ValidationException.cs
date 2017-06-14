using System;
using System.Net.Http;
using System.Runtime.Serialization;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class ValidationException : RequestException
    {
        public ValidationException(string requestId, HttpMethod method, AdvertisementErrorResponse errorResponse)
            : base(requestId, 422, $"{method:G} failed.{errorResponse?.Message.PadLeft(errorResponse.Message.Length + 1)}")
        {
            this.Errors = errorResponse?.Errors ?? new Error[0];
        }

        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Errors = (Error[])info.GetValue(nameof(this.Errors), typeof(Error[]));
        }

        public Error[] Errors { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.Errors), this.Errors);

            base.GetObjectData(info, context);
        }
    }
}