using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    static class AssertExtensions
    {
        public static void AssertValidationData(this Dictionary<string, ValidationData[]> actualValidationDataDictionary, string expectedKey,
            params ValidationData[] expectedValidationData)
        {
            Assert.IsTrue(actualValidationDataDictionary.ContainsKey(expectedKey), $"validationDataDictionary does not contain key {expectedKey}.");
            var validationDataArray = actualValidationDataDictionary[expectedKey];
            Assert.AreEqual(expectedValidationData.Length, validationDataArray.Length, $"Key {expectedKey} ValidationDataArray count does not match expected.");
            foreach (var expectedValidationDataItem in validationDataArray)
            {
                Assert.IsTrue(validationDataArray.Any(v => v.Severity == expectedValidationDataItem.Severity && v.Code == expectedValidationDataItem.Code), $"Key {expectedKey} does not have any ValidationData object with severity {expectedValidationDataItem.Severity:G} and code {expectedValidationDataItem.Code}.");
            }
        }
    }
}
