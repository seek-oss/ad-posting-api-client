using System;
using System.Net;
using System.Runtime.Serialization;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class CreationIdAlreadyExistsException : RequestException
    {
        public CreationIdAlreadyExistsException(string requestId, Uri advertisementLink, AdvertisementErrorResponse errorResponse)
             : base(requestId, (int)HttpStatusCode.Conflict, $"The {nameof(Advertisement.CreationId)} has already been used to create an advertisement. The {nameof(AdvertisementLink)} property provides the link to the conflicting advertisement.")
        {
            this.AdvertisementLink = advertisementLink;
            this.Errors = errorResponse?.Errors ?? new Error[0];
        }

        protected CreationIdAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.AdvertisementLink = (Uri)info.GetValue(nameof(this.AdvertisementLink), typeof(Uri));
            this.Errors = (Error[])info.GetValue(nameof(this.Errors), typeof(Error[]));
        }

        public Uri AdvertisementLink { get; }
        public Error[] Errors { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.AdvertisementLink), this.AdvertisementLink);
            info.AddValue(nameof(this.Errors), this.Errors);
            base.GetObjectData(info, context);
        }
    }
}