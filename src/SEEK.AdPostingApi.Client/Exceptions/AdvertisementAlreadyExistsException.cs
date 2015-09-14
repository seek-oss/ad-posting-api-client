using System;
using System.Runtime.Serialization;

namespace SEEK.AdPostingApi.Client.Exceptions
{
    [Serializable]
    public class AdvertisementAlreadyExistsException : Exception
    {
        public string CorrelationId { get; private set; }

        public AdvertisementAlreadyExistsException(string correlationId)
            : this(correlationId, null)
        {
        }

        public AdvertisementAlreadyExistsException(string correlationId, Exception innerException)
            : base($"Advertisement with correlation Id {correlationId} already exists.", innerException)
        {
            CorrelationId = correlationId;
        }

        protected AdvertisementAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            info.AddValue("CorrelationId", CorrelationId);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            CorrelationId = info.GetString("CorrelationId");
        }
    }
}
