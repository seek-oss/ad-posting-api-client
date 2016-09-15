using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.SampleConsumer
{
    public class Program
    {
        public static void Main()
        {
            Task.Run(MainAsync).Wait();
        }

        private enum CreateResult
        {
            AlreadyExists,
            Created,
            Failed,
            Unauthorized,
            Unknown,
            ValidationErrors
        }

        private static async Task MainAsync()
        {
            IAdPostingApiClient postingClient = new AdPostingApiClient("<client id>", "<client secret>", Client.Environment.Integration);

            var ad = GetExampleAdvertisementDetails();

            // Example of creating the advertisement using a simple exponential retry loop.
            AdvertisementError[] errors = null;
            var createResult = CreateResult.Unknown;
            Guid? advertisementId = null;
            Uri advertisementLink = null;
            var retryAttempts = 0;
            var maxRetryAttempts = 5;
            var baseRetryIntervalSeconds = 2;
            while (true)
            {
                try
                {
                    //Create a new advertisement
                    var advertisement = await postingClient.CreateAdvertisementAsync(ad);

                    advertisementId = advertisement.Id;
                    advertisementLink = advertisement.Uri;
                    createResult = CreateResult.Created;
                    break;
                }
                catch (CreationIdAlreadyExistsException ex)
                {
                    advertisementLink = ex.AdvertisementLink;
                    createResult = CreateResult.AlreadyExists;
                    break;
                }
                catch (ValidationException ex)
                {
                    errors = ex.Errors;
                    createResult = CreateResult.ValidationErrors;
                    break;
                }
                catch (UnauthorizedException ex)
                {
                    LogException(ex);
                    createResult = CreateResult.Unauthorized;
                    break;
                }
                catch (RequestException ex)
                {
                    LogException(ex);

                    if (ex.StatusCode >= 500)
                    {
                        if (retryAttempts == maxRetryAttempts)
                        {
                            createResult = CreateResult.Failed;
                            break;
                        }

                        var waitInterval = (int)Math.Pow(3, retryAttempts++) * baseRetryIntervalSeconds;
                        Console.WriteLine("Waiting {0} seconds before retrying.", waitInterval);
                        await Task.Delay(TimeSpan.FromSeconds(waitInterval));
                        continue;
                    }

                    break;
                }
            }

            Console.WriteLine("Advertisement creation result: {0}", createResult);
            switch (createResult)
            {
                case CreateResult.Created:
                case CreateResult.AlreadyExists:
                    if (createResult == CreateResult.Created)
                    {
                        Console.WriteLine($"Advertisement Id: {advertisementId}");
                    }
                    Console.WriteLine($"Advertisement Link: {advertisementLink}");

                    await GetUpdateAndExpireAdvertisement(postingClient, advertisementLink);

                    break;

                case CreateResult.ValidationErrors:
                    PrintValidationErrors(errors);
                    break;

                case CreateResult.Failed:
                    Console.WriteLine("Advertisement failed to be created, contact SEEK.");
                    break;

                default:
                    Console.WriteLine($"Advertisement not created. Unexpected createResult {createResult}.");
                    break;
            }

            await GetAllAdvertisements(postingClient);

            Console.WriteLine("Finished Example. Press a key to exit.");
            Console.ReadKey(true);
        }

        private static void LogException(RequestException ex)
        {
            Console.WriteLine("Error (Status Code: {0}) while creating advertisement.\r\n{1}", ex.StatusCode, ex.Message);
        }

        private static void PrintValidationErrors(AdvertisementError[] errors)
        {
            Console.WriteLine("Advertisement creation failed. Validation errors:");
            var counter = 1;
            foreach (var error in errors)
            {
                Console.WriteLine($"  [{counter:##}] Field: '{error.Field}' Code: '{error.Code}' Message: '{error.Message}'");
                counter++;
            }
        }

        /// <summary>
        /// Example on get all advertisement for the advertiser
        /// </summary>
        /// <param name="postingClient"></param>
        /// <returns></returns>
        private static async Task GetAllAdvertisements(IAdPostingApiClient postingClient)
        {
            var advertisementList = await postingClient.GetAllAdvertisementsAsync();
            Console.WriteLine("\nRetrieve all advertisements.{0}",
                JsonConvert.SerializeObject(advertisementList, Formatting.Indented));
        }

        /// <summary>
        /// Example on Get, Update and Expire an advertisement.
        /// </summary>
        /// <param name="postingClient"></param>
        /// <param name="advertisementLink"></param>
        /// <returns></returns>
        private static async Task GetUpdateAndExpireAdvertisement(IAdPostingApiClient postingClient, Uri advertisementLink)
        {
            // Use the advertisement link to get the advertisement.
            AdvertisementResource advertisementResource = await postingClient.GetAdvertisementAsync(advertisementLink);
            Console.WriteLine(JsonConvert.SerializeObject(advertisementResource, Formatting.Indented));

            // Modify and update the advertisement
            advertisementResource.JobTitle = "New job title";
            advertisementResource = await advertisementResource.SaveAsync();
            Console.WriteLine("\nUpdated advertisement:\n{0}", JsonConvert.SerializeObject(advertisementResource, Formatting.Indented));

            //Expire the advertisement
            var expiredAdvertisementContent = await advertisementResource.ExpireAsync();
            Console.WriteLine("\nExpired advertisement:\n{0}", JsonConvert.SerializeObject(expiredAdvertisementContent, Formatting.Indented));
        }

        /// <summary>
        /// Get a Sample advertisement to be created
        /// </summary>
        /// <returns></returns>
        private static Advertisement GetExampleAdvertisementDetails()
        {
            return new Advertisement
            {
                CreationId = "Sample Consumer 2575274f-7526-455d-a2a3-32447e40733d",
                ThirdParties = new ThirdParties { AdvertiserId = "<advertiser id>" },
                JobTitle = "A Job Title",
                SearchJobTitle = "A Job Title for Searching",
                JobSummary = "Job summary of the job ad",
                AdvertisementDetails = "Experience Required",
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
    }
}