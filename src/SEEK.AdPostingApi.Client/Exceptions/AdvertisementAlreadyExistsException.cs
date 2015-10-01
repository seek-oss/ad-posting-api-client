using System;
using System.Runtime.Serialization;

namespace SEEK.AdPostingApi.Client.Exceptions
{
    [Serializable]
    public class AdvertisementAlreadyExistsException : Exception
    {
        public string CreationId { get; private set; }

        public Uri AdvertisementLink { get; private set; }

        public new ResourceActionException InnerException => base.InnerException as ResourceActionException;

        public AdvertisementAlreadyExistsException(string creationId, ResourceActionException innerException)
            : base($"Advertisement with creation Id {creationId} already exists.", innerException)
        {
            CreationId = creationId;
            AdvertisementLink = innerException.ResponseHeaders.Location;
        }

        protected AdvertisementAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            info.AddValue(nameof(CreationId), CreationId);
            info.AddValue(nameof(AdvertisementLink), AdvertisementLink);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            CreationId = info.GetString(nameof(CreationId));
            AdvertisementLink = (Uri)info.GetValue(nameof(AdvertisementLink), typeof(Uri));
        }
    }
}
