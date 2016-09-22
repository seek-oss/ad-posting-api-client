using System;
using System.Runtime.CompilerServices;
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

        private static readonly RetryPolicy TransientErrorRetryPolicy = Policy
            .Handle<RequestException>(ex => ex.StatusCode >= 500)
            .WaitAndRetryAsync(5, attempt => TimeSpan.FromSeconds(BaseRetryIntervalSeconds * Math.Pow(2, attempt)),
                    (exception, waitInterval) =>
                    {
                        Console.WriteLine($"Unexpected error, waiting {waitInterval.TotalSeconds} seconds before retrying.");
                    });

        public static void Main()
        {
            Task.Run(ExampleAsync).Wait();
        }

        public static async Task ExampleAsync()
        {
            using (IAdPostingApiClient client = new AdPostingApiClient(ClientId, ClientSecret, Environment.Integration))
            {
                // Create a new advertisement
                Advertisement advertisement = GetExampleAdvertisementToCreate();
                AdvertisementResource createdAdvertisement = await CreateAdvertisementExampleAsync(advertisement, client);

                if (createdAdvertisement != null)
                {
                    // Poll and check the processing status of the create advertisement request
                    // Note that Update and Expire requests can still be sent when Processing Status of a previous request(eg: Create) is Pending.
                    ProcessingStatus processingStatus = await WaitForAdvertisementProcessingCompleteExampleAsync(createdAdvertisement.Id, client);
                    Console.WriteLine($"The Processing Status of the Create Advertisement request is '{processingStatus}'.");

                    // Retrieve advertisement
                    AdvertisementResource advertisementResource = await GetAdvertisementExampleAsync(createdAdvertisement.Id, client);

                    if (processingStatus == ProcessingStatus.Failed)
                    {
                        // The advertisement could not be processed, the Errors item on the retrieved advertisement lists the errors.
                        // Correct the errors then send an update request.
                        PrintAdvertisementErrors(advertisementResource.Errors);
                    }
                    else
                    {
                        // Modify details on the advertisement
                        advertisementResource.JobTitle = "Senior Dude";
                        AdvertisementResource updatedAdvertisementResource = await UpdateAdvertisementExampleAsync(advertisementResource);

                        processingStatus = await WaitForAdvertisementProcessingCompleteExampleAsync(createdAdvertisement.Id, client);
                        Console.WriteLine($"The Processing Status of the Update Advertisement request is '{processingStatus}'.");

                        // Expire the advertisement
                        await ExpireAdvertisementExampleAsync(updatedAdvertisementResource);
                    }
                }

                // Get paged summaries of created advertisements
                AdvertisementSummaryPageResource advertisementSummaryPage = await GetAllAdvertisementsExampleAsync(client);
                while (!advertisementSummaryPage.Eof)
                {
                    advertisementSummaryPage = await GetAllAdvertisementsNextPageExampleAsync(advertisementSummaryPage);
                }
            }

            Console.WriteLine("Finished Example. Press a key to exit.");
            Console.ReadKey(true);
        }

        private static async Task<ProcessingStatus> WaitForAdvertisementProcessingCompleteExampleAsync(Guid advertisementId, IAdPostingApiClient client)
        {
            ProcessingStatus status = ProcessingStatus.Unknown;
            try
            {
                await Policy
                    .Handle<RequestException>(ex => ex.StatusCode >= 500) // retry on transient errors
                    .OrResult<ProcessingStatus>(p => p == ProcessingStatus.Pending) // retry while status is Pending
                    .WaitAndRetryAsync(5, attempt => TimeSpan.FromSeconds(BaseRetryIntervalSeconds * Math.Pow(2, attempt)))
                    .ExecuteAsync(async () => status = await client.GetAdvertisementStatusAsync(advertisementId));
            }
            catch (RequestException ex)
            {
                LogException(ex);
            }

            return status;
        }

        public static async Task<AdvertisementResource> CreateAdvertisementExampleAsync(Advertisement advertisementToCreate, IAdPostingApiClient client)
        {
            AdvertisementResource advertisementResource = null;
            try
            {
                await TransientErrorRetryPolicy.ExecuteAsync(async () => advertisementResource = await client.CreateAdvertisementAsync(advertisementToCreate));
                Console.WriteLine($"Created Advertisement:\n{JsonConvert.SerializeObject(advertisementResource, Formatting.Indented)}");
            }
            catch (RequestException ex)
            {
                LogException(ex);
            }

            return advertisementResource;
        }

        public static async Task<AdvertisementResource> UpdateAdvertisementExampleAsync(AdvertisementResource advertisement)
        {
            AdvertisementResource advertisementResource = null;
            try
            {
                await TransientErrorRetryPolicy.ExecuteAsync(async () => advertisementResource = await advertisement.SaveAsync());
                Console.WriteLine($"Updated Advertisement:\n{JsonConvert.SerializeObject(advertisementResource, Formatting.Indented)}");
            }
            catch (RequestException ex)
            {
                LogException(ex);
            }

            return advertisementResource;
        }

        private static async Task<AdvertisementResource> ExpireAdvertisementExampleAsync(AdvertisementResource advertisement)
        {
            AdvertisementResource advertisementResource = null;
            try
            {
                await TransientErrorRetryPolicy.ExecuteAsync(async () => advertisementResource = await advertisement.ExpireAsync());
                Console.WriteLine($"Expired Advertisement:\n{JsonConvert.SerializeObject(advertisementResource, Formatting.Indented)}");
            }
            catch (RequestException ex)
            {
                LogException(ex);
            }

            return advertisementResource;
        }

        private static async Task<AdvertisementResource> GetAdvertisementExampleAsync(Guid advertisementId, IAdPostingApiClient client)
        {
            AdvertisementResource advertisementResource = null;
            try
            {
                await TransientErrorRetryPolicy.ExecuteAsync(async () => advertisementResource = await client.GetAdvertisementAsync(advertisementId));
                Console.WriteLine($"Retrieved Advertisement:\n{JsonConvert.SerializeObject(advertisementResource, Formatting.Indented)}");
            }
            catch (RequestException ex)
            {
                LogException(ex);
            }

            return advertisementResource;
        }

        private static async Task<AdvertisementSummaryPageResource> GetAllAdvertisementsExampleAsync(IAdPostingApiClient client)
        {
            AdvertisementSummaryPageResource advertisementSummaryPage = null;
            try
            {
                await TransientErrorRetryPolicy.ExecuteAsync(async () => advertisementSummaryPage = await client.GetAllAdvertisementsAsync());
                Console.WriteLine($"Retrieve all advertisements:{JsonConvert.SerializeObject(advertisementSummaryPage, Formatting.Indented)}");
            }
            catch (RequestException ex)
            {
                LogException(ex);
            }

            return advertisementSummaryPage;
        }

        private static async Task<AdvertisementSummaryPageResource> GetAllAdvertisementsNextPageExampleAsync(AdvertisementSummaryPageResource summaryPage)
        {
            try
            {
                await TransientErrorRetryPolicy.ExecuteAsync(async () => summaryPage = await summaryPage.NextPageAsync());
                Console.WriteLine($"Next page of advertisements:{JsonConvert.SerializeObject(summaryPage, Formatting.Indented)}");
            }
            catch (RequestException ex)
            {
                LogException(ex);
            }

            return summaryPage;
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
            Console.WriteLine($"Error (Status Code: {ex.StatusCode}) while performing `{callerName}`.\r\nMessage: {ex.Message}");

            switch (ex.GetType().Name)
            {
                case nameof(CreationIdAlreadyExistsException):
                    Uri advertisementLink = ((CreationIdAlreadyExistsException)ex).AdvertisementLink;
                    Console.WriteLine($"Advertisement Link:{advertisementLink}");
                    break;

                case nameof(ValidationException):
                    PrintAdvertisementErrors(((ValidationException)ex).Errors);
                    break;

                case nameof(UnauthorizedException):
                    PrintAdvertisementErrors(((UnauthorizedException)ex).Errors);
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

        private static void PrintAdvertisementErrors(AdvertisementError[] errors)
        {
            if (errors.Length < 1) return;

            Console.WriteLine("Advertisement Errors:");

            int counter = 1;
            foreach (AdvertisementError error in errors)
            {
                Console.WriteLine($"  [{counter:##}] Field: '{error.Field}' Code: '{error.Code}' Message: '{error.Message}'");
                counter++;
            }
        }
    }
}