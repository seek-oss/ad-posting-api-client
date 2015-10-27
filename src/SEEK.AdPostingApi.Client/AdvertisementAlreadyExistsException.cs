using System;
using System.Runtime.Serialization;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class AdvertisementAlreadyExistsException : Exception
    {
        public Uri AdvertisementLink { get; private set; }

        public AdvertisementAlreadyExistsException(Uri advertisementLink)
            : base("Advertisement already exists.")
        {
            AdvertisementLink = advertisementLink;
        }

        protected AdvertisementAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            info.AddValue(nameof(AdvertisementLink), AdvertisementLink);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            AdvertisementLink = (Uri)info.GetValue(nameof(AdvertisementLink), typeof(Uri));
        }
    }
}
