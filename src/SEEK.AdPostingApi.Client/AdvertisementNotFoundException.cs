using System;
using System.Runtime.Serialization;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class AdvertisementNotFoundException : RequestException
    {
        public AdvertisementNotFoundException(string requestId) : base(requestId, "The advertisement does not exist.")
        {
        }

        protected AdvertisementNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}