using System;
using System.Linq;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Resources
{
    public class GetAdvertisementResult
    {
        public GetAdvertisementResult(AdvertisementResource resource)
        {
            AdvertisementResource = resource;
        }

        public AdvertisementResource AdvertisementResource { get; }

        public ProcessingStatus ProcessingStatus => (ProcessingStatus)Enum.Parse(typeof(ProcessingStatus), this.AdvertisementResource.ResponseHeaders.GetValues("Processing-Status").First());
    }
}