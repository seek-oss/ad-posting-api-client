using System;

namespace SEEK.AdPostingApi.Client
{
    public class AdvertisementNotFoundException : Exception
    {
        public AdvertisementNotFoundException()
            : base("The advertisement does not exist.")
        {
        }
    }
}
