using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    [TestFixture]
    public class UpdateAdTests : IDisposable
    {
        private readonly IOAuth2TokenClient _oauthClient;

        public UpdateAdTests()
        {
            this._oauthClient = Mock.Of<IOAuth2TokenClient>(
                c => c.GetOAuth2TokenAsync() == Task.FromResult(new OAuth2TokenBuilder().Build()));
        }

        public void Dispose()
        {
            this._oauthClient.Dispose();
        }

        [SetUp]
        public void TestInitialize()
        {
            PactProvider.ClearInteractions();
        }

        [TearDown]
        public void TestCleanup()
        {
            PactProvider.VerifyInteractions();
        }

        [Test]
        public async Task UpdateExistingAdvertisement()
        {
        }

        [Test]
        public async Task UpdateNonExistentAdvertisement()
        {
            const string advertisementId = "f945e101-7226-43cd-a358-26aeacbc1e26";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();

            PactProvider.MockService
                .Given(string.Format("There isn't an advertisement with id: '{0}'", advertisementId))
                .UponReceiving("Update request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Put,
                    Path = "/advertisement/" + advertisementId,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse { Status = 404 });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            try
            {
                await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, "advertisement/" + advertisementId), new Advertisement
                {
                    AdvertiserId = "advertiserA",
                    JobTitle = "Bricklayer",
                    JobSummary = "some text",
                    AdvertisementDetails = "experience required",
                    AdvertisementType = AdvertisementType.Classic,
                    WorkType = WorkType.Casual,
                    SalaryType = SalaryType.HourlyRate,
                    LocationId = "1002",
                    SubclassificationId = "6227",
                    SalaryMinimum = 20,
                    SalaryMaximum = 24
                });
            }
            catch (Exception ex)
            {
                StringAssert.Contains("Not Found", ex.Message);
            }
        }

    }

}
