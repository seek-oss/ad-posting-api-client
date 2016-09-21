using System;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using Environment = SEEK.AdPostingApi.Client.Environment;

namespace SEEK.AdPostingApi.SampleConsumer
{
    public class Program
    {
        private const string AdvertiserId = "AdvertiserId";
        private const int BaseRetryIntervalSeconds = 2;
        private const string ClientId = "ClientId";
        private const string ClientSecret = "ClientSecret";

        public static void Main()
        {
            Task.Run(ExampleAsync).Wait();
        }

        public static async Task ExampleAsync()
        {
            RetryPolicy retryPolicy = Policy
                .Handle<RequestException>(ex => ex.StatusCode >= 500)
                .WaitAndRetryAsync(5, attempt => TimeSpan.FromSeconds(BaseRetryIntervalSeconds * Math.Pow(2, attempt)),
                    (exception, waitInterval) =>
                    {
                        Console.WriteLine($"Unexpected error, waiting {waitInterval.TotalSeconds} seconds before retrying.");
                    });

            using (var client = new AdPostingApiClient(ClientId, ClientSecret, Environment.Integration))
            {
                // Create a new advertisement
                Advertisement advertisement = GetExampleAdvertisementToCreate();
                AdvertisementResource createdAdvertisement = await CreateAdvertisementExampleAsync(advertisement, client, retryPolicy);

                if (createdAdvertisement != null)
                {
                    // Retrieve advertisement
                    AdvertisementResource advertisementResource = await GetAdvertisementExampleAsync(createdAdvertisement.Uri, client, retryPolicy);

                    // Modify details on the advertisement
                    advertisementResource.JobTitle = "Senior Dude";
                    AdvertisementResource updatedAdvertisement = await UpdateAdvertisementExampleAsync(advertisementResource, retryPolicy);

                    // Expire the advertisement
                    await ExpireAdvertisementExampleAsync(updatedAdvertisement, retryPolicy);
                }

                // Get a summarised page of created advertisements
                AdvertisementSummaryPageResource advertisementSummaryPage = await GetAllAdvertisementsExampleAsync(client, retryPolicy);
            }

            Console.WriteLine("Finished Example. Press a key to exit.");
            Console.ReadKey(true);
        }

        public static async Task<AdvertisementResource> CreateAdvertisementExampleAsync(Advertisement advertisementToCreate, IAdPostingApiClient client, Policy retryPolicy)
        {
            AdvertisementResource advertisementResource = null;
            try
            {
                await retryPolicy.ExecuteAsync(async () => { advertisementResource = await client.CreateAdvertisementAsync(advertisementToCreate); });
                Console.WriteLine($"Created Advertisement:\n{JsonConvert.SerializeObject(advertisementResource, Formatting.Indented)}");
            }
            catch (RequestException ex)
            {
                LogException(ex);
            }

            return advertisementResource;
        }

        public static async Task<AdvertisementResource> UpdateAdvertisementExampleAsync(AdvertisementResource advertisement, Policy retryPolicy)
        {
            AdvertisementResource advertisementResource = null;
            try
            {
                await retryPolicy.ExecuteAsync(async () => { advertisementResource = await advertisement.SaveAsync(); });
                Console.WriteLine($"Updated Advertisement:\n{JsonConvert.SerializeObject(advertisementResource, Formatting.Indented)}");
            }
            catch (RequestException ex)
            {
                LogException(ex);
            }

            return advertisementResource;
        }

        private static async Task<AdvertisementResource> ExpireAdvertisementExampleAsync(AdvertisementResource advertisement, RetryPolicy retryPolicy)
        {
            AdvertisementResource advertisementResource = null;
            try
            {
                await retryPolicy.ExecuteAsync(async () => { advertisementResource = await advertisement.ExpireAsync(); });
                Console.WriteLine($"Expired Advertisement:\n{JsonConvert.SerializeObject(advertisementResource, Formatting.Indented)}");
            }
            catch (RequestException ex)
            {
                LogException(ex);
            }

            return advertisementResource;
        }

        private static async Task<AdvertisementResource> GetAdvertisementExampleAsync(Uri advertisementUri, AdPostingApiClient client, RetryPolicy retryPolicy)
        {
            AdvertisementResource advertisementResource = null;
            try
            {
                await retryPolicy.ExecuteAsync(async () => { advertisementResource = await client.GetAdvertisementAsync(advertisementUri); });
                Console.WriteLine($"Retrieved Advertisement:\n{JsonConvert.SerializeObject(advertisementResource, Formatting.Indented)}");
            }
            catch (RequestException ex)
            {
                LogException(ex);
            }

            return advertisementResource;
        }

        private static async Task<AdvertisementSummaryPageResource> GetAllAdvertisementsExampleAsync(AdPostingApiClient client, RetryPolicy retryPolicy)
        {
            AdvertisementSummaryPageResource advertisementSummaryPage = null;
            try
            {
                await retryPolicy.ExecuteAsync(async () => { advertisementSummaryPage = await client.GetAllAdvertisementsAsync(); });
                Console.WriteLine($"Retrieved all advertisements:{JsonConvert.SerializeObject(advertisementSummaryPage, Formatting.Indented)}");
            }
            catch (RequestException ex)
            {
                LogException(ex);
            }

            return advertisementSummaryPage;
        }

        private static Advertisement GetExampleAdvertisementToCreate()
        {
            return new Advertisement
            {
                CreationId = "Sample Consumer " + Guid.NewGuid(),
                ThirdParties = new ThirdParties { AdvertiserId = AdvertiserId },
                JobTitle = "A Job for a Dude",
                SearchJobTitle = "Dudes find job best when they search on this title",
                JobSummary = "Things a dude should know",
                AdvertisementDetails = "Things the dude should have done and will need to do",
                AdvertisementType = AdvertisementType.Classic,
                WorkType = WorkType.Casual,
                Salary = new Salary
                {
                    Type = SalaryType.HourlyRate,
                    Minimum = 20,
                    Maximum = 24
                },
                Location = new Location
                {
                    Id = "Melbourne",
                    AreaId = "MelbourneNorthernSuburbs"
                },
                Recruiter = new Recruiter
                {
                    FullName = "Recruiter full name",
                    Email = "recruiter@email.com"
                },
                SubclassificationId = "AerospaceEngineering"
            };
        }

        private static void LogException(RequestException ex, [CallerMemberName] string callerName = "")
        {
            LogException(ex.Message, ex.StatusCode, callerName);

            switch (ex.GetType().Name)
            {
                case nameof(CreationIdAlreadyExistsException):
                    Uri advertisementLink = ((CreationIdAlreadyExistsException)ex).AdvertisementLink;
                    Console.WriteLine($"Advertisement Link:{advertisementLink}");
                    break;

                case nameof(ValidationException):
                    PrintValidationErrors(((ValidationException)ex).Errors);
                    break;

                case nameof(UnauthorizedException):
                    PrintValidationErrors(((UnauthorizedException)ex).Errors);
                    break;

                case nameof(TooManyRequestsException):
                    TimeSpan? retryAfter = ((TooManyRequestsException)ex).RetryAfter;
                    if (retryAfter != null)
                    {
                        Console.WriteLine($"A Retry-After period of {retryAfter.Value.TotalSeconds} seconds is provided.");
                    }
                    break;

                case nameof(RequestException):
                    LogException(ex.ResponseContentType, ex.ResponseContent);
                    break;
            }
        }

        private static void LogException(string message, int statusCode, [CallerMemberName] string callerName = "")
        {
            Console.WriteLine($"Error (Status Code: {statusCode}) while performing `{callerName}`.\r\nMessage: {message}");
        }

        private static void LogException(string responseContentType, string responseContent)
        {
            if (responseContentType != null)
            {
                Console.WriteLine($"Response Content-Type: {responseContentType}");
            }
            if (responseContent != null)
            {
                Console.WriteLine($"Response Content: {responseContent}");
            }
        }

        private static void PrintValidationErrors(AdvertisementError[] errors)
        {
            Console.WriteLine("Advertisement creation failed. Validation errors:");
            int counter = 1;
            foreach (AdvertisementError error in errors)
            {
                Console.WriteLine($"  [{counter:##}] Field: '{error.Field}' Code: '{error.Code}' Message: '{error.Message}'");
                counter++;
            }
        }
    }
}