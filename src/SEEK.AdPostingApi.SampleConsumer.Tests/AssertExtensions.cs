using System.Linq;
using NUnit.Framework;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    static class AssertExtensions
    {
        public static void ShouldBe(this ValidationData[] actualValidationDataItems, params ValidationData[] expectedValidationDataItems)
        {
            Assert.AreEqual(expectedValidationDataItems.Length, actualValidationDataItems.Length, "ValidationData array count does not match expected.");
            foreach (var expectedValidationDataItem in expectedValidationDataItems)
            {
                Assert.IsTrue(actualValidationDataItems.Any(v => v.Field == expectedValidationDataItem.Field && v.Code == expectedValidationDataItem.Code), $"Field '{expectedValidationDataItem.Field}' with Code '{expectedValidationDataItem.Code}' not found in actual ValidationData array.");
            }
        }
    }
}
