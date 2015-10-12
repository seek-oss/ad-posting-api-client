using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Exceptions;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    [TestFixture]
    public class UpdateAdTests : IDisposable
    {
        private readonly IOAuth2TokenClient _oauthClient;
        private const string AdvertisementLink = "/advertisement";

        public UpdateAdTests()
        {
            this._oauthClient = Mock.Of<IOAuth2TokenClient>(
                c => c.GetOAuth2TokenAsync() == Task.FromResult(new OAuth2TokenBuilder().Build()));
            this._oauthClient.AccessToken = "b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e";
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
            const string advertisementId = "8e2fde50-bc5f-4a12-9cfb-812e50500184";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There is an advertisement with id: '{advertisementId}'")
                .UponReceiving("Update request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Put,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                    },
                    Body = new
                    {
                        agentId = (object)null,
                        advertiserId = "9012",
                        advertisementType = AdvertisementType.StandOut.ToString(),
                        jobTitle = "Exciting Senior Developer role in a great CBD location. Great $$$ - updated",
                        locationId = "378",
                        subclassificationId = "734",
                        workType = WorkType.FullTime.ToString(),
                        salary = new
                        {
                            type = SalaryType.AnnualPackage.ToString(),
                            minimum = 100000,
                            maximum = 200000,
                            details = "We will pay you"
                        },
                        jobSummary = "Developer job",
                        advertisementDetails = "Exciting, do I need to say more?",
                        contactDetails = "Call me",
                        video = new
                        {
                            url = "https://www.youtube.com/v/abc",
                            position = VideoPosition.Above.ToString()
                        },
                        applicationEmail = "asdf@asdf.com",
                        applicationFormUrl = "http://FakeATS.com.au",
                        screenId = 20,
                        jobReference = "JOB1234",
                        template = new
                        {
                            id = 99,
                            items = new[]
                            {
                                new { name = "Template Line 1", value = "Template Value 1" },
                                new { name = "Template Line 2", value = "Template Value 2" }
                            }
                        },
                        standout = new
                        {
                            logoId = 333,
                            bullets = new[] { "new Uzi", "new Remington Model", "new AK-47" }
                        },
                        additionalProperties = new[] { AdditionalPropertyType.ResidentsOnly.ToString() },
                    }
                })
                .WillRespondWith(
                new ProviderServiceResponse
                {
                    Status = 202,
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/vnd.seek.advertisement+json; version=1; charset=utf-8"}
                    },
                    Body = new
                    {
                        agentId = (object)null,
                        advertiserId = "9012",
                        advertisementType = AdvertisementType.StandOut.ToString(),
                        state = AdvertisementState.Pending.ToString(),
                        jobTitle = "Exciting Senior Developer role in a great CBD location. Great $$$ - updated",
                        locationId = "378",
                        subclassificationId = "734",
                        workType = WorkType.FullTime.ToString(),
                        salary = new
                        {
                            type = SalaryType.AnnualPackage.ToString(),
                            minimum = 100000,
                            maximum = 200000,
                            details = "We will pay you"
                        },
                        jobSummary = "Developer job",
                        advertisementDetails = "Exciting, do I need to say more?",
                        contactDetails = "Call me",
                        video = new
                        {
                            url = "https://www.youtube.com/v/abc",
                            position = VideoPosition.Above.ToString()
                        },
                        applicationEmail = "asdf@asdf.com",
                        applicationFormUrl = "http://FakeATS.com.au",
                        screenId = 20,
                        jobReference = "JOB1234",
                        template = new
                        {
                            id = 99,
                            items = new[]
                            {
                                new { name = "Template Line 1", value = "Template Value 1" },
                                new { name = "Template Line 2", value = "Template Value 2" }
                            }
                        },
                        standout = new
                        {
                            logoId = 333,
                            bullets = new[] { "new Uzi", "new Remington Model", "new AK-47" }
                        },
                        additionalProperties = new[] { AdditionalPropertyType.ResidentsOnly.ToString() },
                        _links = new
                        {
                            self = new
                            {
                                href = link
                            }
                        }
                    }
                });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            AdvertisementResource jobAd = await client.UpdateAdvertisementAsync(
                new Uri(PactProvider.MockServiceUri, link),
                new Advertisement
                {
                    AdvertiserId = "9012",
                    AdvertisementType = AdvertisementType.StandOut,
                    JobTitle = "Exciting Senior Developer role in a great CBD location. Great $$$ - updated",
                    LocationId = "378",
                    SubclassificationId = "734",
                    WorkType = WorkType.FullTime,
                    Salary = new Salary
                    {
                        Type = SalaryType.AnnualPackage,
                        Minimum = 100000,
                        Maximum = 200000,
                        Details = "We will pay you"
                    },
                    JobSummary = "Developer job",
                    AdvertisementDetails = "Exciting, do I need to say more?",
                    ContactDetails = "Call me",
                    Video = new Video
                    {
                        Url = "https://www.youtube.com/v/abc",
                        Position = VideoPosition.Above
                    },
                    ApplicationEmail = "asdf@asdf.com",
                    ApplicationFormUrl = "http://FakeATS.com.au",
                    ScreenId = 20,
                    JobReference = "JOB1234",
                    Template = new Template
                    {
                        Id = 99,
                        Items = new[]
                        {
                            new TemplateItemModel{Name = "Template Line 1", Value = "Template Value 1"},
                            new TemplateItemModel{Name = "Template Line 2", Value = "Template Value 2"}
                        }
                    },
                    Standout = new StandoutAdvertisement
                    {
                        LogoId = 333,
                        Bullets = new[] { "new Uzi", "new Remington Model", "new AK-47" }
                    },

                    AdditionalProperties = new[] { AdditionalPropertyType.ResidentsOnly },
                });

            Assert.AreEqual("Exciting Senior Developer role in a great CBD location. Great $$$ - updated", jobAd.Properties.JobTitle);
        }

        [Test]
        public async Task UpdateNonExistentAdvertisement()
        {
            const string advertisementId = "9b650105-7434-473f-8293-4e23b7e0e064";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There isn't an advertisement with id: '{advertisementId}'")
                .UponReceiving("Update request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Put,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                    },
                    Body = new
                    {
                        advertiserId = "advertiserA",
                        jobTitle = "Bricklayer",
                        jobSummary = "some text",
                        advertisementDetails = "experience required",
                        advertisementType = AdvertisementType.Classic.ToString(),
                        workType = WorkType.Casual.ToString(),
                        salary = new
                        {
                            type = SalaryType.HourlyRate.ToString(),
                            minimum = 20,
                            maximum = 24
                        },
                        locationId = "1002",
                        subclassificationId = "6227",
                    }
                })
                .WillRespondWith(new ProviderServiceResponse { Status = 404 });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);

            try
            {
                await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), new Advertisement
                {
                    AdvertiserId = "advertiserA",
                    JobTitle = "Bricklayer",
                    JobSummary = "some text",
                    AdvertisementDetails = "experience required",
                    AdvertisementType = AdvertisementType.Classic,
                    WorkType = WorkType.Casual,
                    Salary = new Salary
                    {
                        Type = SalaryType.HourlyRate,
                        Minimum = 20,
                        Maximum = 24
                    },
                    LocationId = "1002",
                    SubclassificationId = "6227",
                });
            }
            catch (ResourceActionException ex)
            {
                StringAssert.Contains("404", ex.Message);
            }
        }

        [Test]
        public async Task UpdateWithBadAdvertisementData()
        {
            const string advertisementId = "7e2fde50-bc5f-4a12-9cfb-812e50500184";
            OAuth2Token oAuth2Token = new OAuth2TokenBuilder().Build();
            var link = $"{AdvertisementLink}/{advertisementId}";

            PactProvider.MockService
                .Given($"There is an advertisement with id: '{advertisementId}'")
                .UponReceiving("Update request for advertisement")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Put,
                    Path = link,
                    Headers = new Dictionary<string, string>
                    {
                        {"Authorization", "Bearer " + oAuth2Token.AccessToken},
                        {"Content-Type", "application/vnd.seek.advertisement+json; charset=utf-8"}
                    },
                    Body = new
                    {
                        advertiserId = "advertiserA",
                        advertisementType = AdvertisementType.Classic.ToString(),
                        workType = WorkType.Casual.ToString(),
                        jobTitle = "Candle Stick Maker",
                        locationId = "1002",
                        subclassificationId = "6227",
                        salary = new
                        {
                            type = SalaryType.HourlyRate.ToString(),
                            minimum = 0,
                            maximum = 24
                        },
                        jobSummary = "some text",
                        advertisementDetails = "experience required",
                        video = new
                        {
                            url = "htp://www.youtube.com/v/abc".PadRight(260, '!'),
                            position = VideoPosition.Below.ToString()
                        },
                        applicationEmail = "someone(at)some.domain",
                        applicationFormUrl = "htp://somecompany.domain/apply",
                        template = new
                        {
                            items = new[]
                            {
                                new { name = "template1", value = "value1" },
                                new { name = "", value = "value2".PadRight(260, '!') }
                            }
                        }
                    }
                })
                .WillRespondWith(
                    new ProviderServiceResponse
                    {
                        Status = 422,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8" }
                        },
                        Body = new
                        {
                            message = "Validation Failure",
                            errors = new[]
                            {
                                new { field = "applicationEmail", code = "InvalidEmailAddress" },
                                new { field = "applicationFormUrl", code = "InvalidUrl" },
                                new { field = "salary.minimum", code = "ValueOutOfRange" },
                                new { field = "template.items[1].name", code = "Required" },
                                new { field = "template.items[1].value", code = "MaxLengthExceeded" },
                                new { field = "video.url", code = "MaxLengthExceeded" },
                                new { field = "video.url", code = "RegexPatternNotMatched" }
                            }
                        }
                    });

            var client = new AdPostingApiClient(PactProvider.MockServiceUri, _oauthClient);
            var expectedValidationDataItems = new[]
            {
                new ValidationData { Field = "applicationEmail", Code = "InvalidEmailAddress" },
                new ValidationData { Field = "applicationFormUrl", Code = "InvalidUrl" },
                new ValidationData { Field = "salary.minimum", Code = "ValueOutOfRange" },
                new ValidationData { Field = "template.items[1].name", Code = "Required" },
                new ValidationData { Field = "template.items[1].value", Code = "MaxLengthExceeded" },
                new ValidationData { Field = "video.url", Code = "MaxLengthExceeded" },
                new ValidationData { Field = "video.url", Code = "RegexPatternNotMatched" }
            };

            try
            {
                await client.UpdateAdvertisementAsync(new Uri(PactProvider.MockServiceUri, link), new Advertisement
                {
                    AdvertiserId = "advertiserA",
                    AdvertisementType = AdvertisementType.Classic,
                    WorkType = WorkType.Casual,
                    JobTitle = "Candle Stick Maker",
                    LocationId = "1002",
                    SubclassificationId = "6227",
                    Salary = new Salary
                    {
                        Type = SalaryType.HourlyRate,
                        Minimum = 0,
                        Maximum = 24
                    },
                    JobSummary = "some text",
                    AdvertisementDetails = "experience required",
                    Video = new Video
                    {
                        Url = "htp://www.youtube.com/v/abc".PadRight(260, '!'),
                        Position = VideoPosition.Below
                    },
                    ApplicationEmail = "someone(at)some.domain",
                    ApplicationFormUrl = "htp://somecompany.domain/apply",
                    Template = new Template
                    {
                        Items = new[]
                        {
                            new TemplateItemModel { Name = "template1", Value = "value1" },
                            new TemplateItemModel { Name = "", Value = "value2".PadRight(260, '!') }
                        }
                    }
                });
                Assert.Fail($"Should throw a '{typeof(ValidationException).FullName}' exception");
            }
            catch (ValidationException ex)
            {
                Assert.IsNotNull(ex.ValidationDataItems);
                ex.ValidationDataItems.ShouldBe(expectedValidationDataItems);
            }
        }
    }
}