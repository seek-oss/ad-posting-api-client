using System;
using System.Net;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class AdvertisementNotFoundException : RequestException
    {
        public AdvertisementNotFoundException(string requestId) : base(requestId, (int)HttpStatusCode.NotFound, "The advertisement does not exist.")
        {
        }
    }
}