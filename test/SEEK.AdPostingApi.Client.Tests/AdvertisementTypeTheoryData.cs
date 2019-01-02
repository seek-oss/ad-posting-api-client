using System.Collections.Generic;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Tests
{
    public class AdvertisementTypeTheoryData
    {
        public static IEnumerable<object[]> AdvertisementTypes => new[]
        {
            new object[] { AdvertisementType.StandOut },
            new object[] { AdvertisementType.Premium },
        };
    }
}