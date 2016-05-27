using System;
using System.Runtime.Serialization;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class AdvertisementAlreadyExistsException : RequestException
    {
        public AdvertisementAlreadyExistsException(string requestId, Uri advertisementLink) : base(requestId, "Advertisement already exists.")
        {
            this.AdvertisementLink = advertisementLink;
        }

        protected AdvertisementAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.AdvertisementLink = (Uri)info.GetValue(nameof(this.AdvertisementLink), typeof(Uri));
        }

        public Uri AdvertisementLink { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.AdvertisementLink), this.AdvertisementLink);

            base.GetObjectData(info, context);
        }
    }
}