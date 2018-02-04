using System;
using System.Net;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    public class CreationIdAlreadyExistsException : RequestException
    {
        public CreationIdAlreadyExistsException(string requestId, Uri advertisementLink, AdvertisementErrorResponse errorResponse)
             : base(requestId, (int)HttpStatusCode.Conflict, $"The {nameof(Advertisement.CreationId)} has already been used to create an advertisement. The {nameof(AdvertisementLink)} property provides the link to the conflicting advertisement.")
        {
            this.AdvertisementLink = advertisementLink;
            this.Errors = errorResponse?.Errors ?? new AdvertisementError[0];
        }

        public Uri AdvertisementLink { get; }
        public AdvertisementError[] Errors { get; }
    }
}