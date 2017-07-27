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
            this.Errors = errorResponse?.Errors ?? new AdvertisementError[0];
        }

        protected ValidationException(SerializationInfo info) : base(info)
        {
            this.Errors = (AdvertisementError[])info.GetValue(nameof(this.Errors), typeof(AdvertisementError[]));
        }

        public AdvertisementError[] Errors { get; }

        public void GetObjectData(SerializationInfo info)
        {
            info.AddValue(nameof(this.Errors), this.Errors);

            base.GetObjectData(info);
        }
    }
}