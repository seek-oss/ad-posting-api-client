using System;
using System.Runtime.Serialization;

namespace SEEK.AdPostingApi.Client.Exceptions
{
    [Serializable]
    public class AdvertisementAlreadyExistsException : Exception
    {
        public string CreationId { get; private set; }

        public AdvertisementAlreadyExistsException(string creationId)
            : this(creationId, null)
        {
        }

        public AdvertisementAlreadyExistsException(string creationId, Exception innerException)
            : base($"Advertisement with creation Id {creationId} already exists.", innerException)
        {
            CreationId = creationId;
        }

        protected AdvertisementAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            info.AddValue("CreationId", CreationId);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            CreationId = info.GetString("CreationId");
        }
    }
}
