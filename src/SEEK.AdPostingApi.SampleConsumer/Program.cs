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
            Unknown,
            Created,
            AlreadyExists,
            ValidationErrors,
            Timeout,
            Unauthorized
        }

        private static async Task MainAsync()
        {
            IAdPostingApiClient postingClient = new AdPostingApiClient("<client id>", "<client secret>", Client.Environment.Integration);

            // An example advertisement with a unique creation ID that ensures multiple create retries will not create duplicate advertisements.
            var ad = new Advertisement
            {
                CreationId = "Sample Consumer 2575274f-7526-455d-a2a3-32447e40733d",
                ThirdParties = new ThirdParties { AdvertiserId = "Advertiser Id" },
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
                SubclassificationId = "AerospaceEngineering"
            };

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
                    Console.WriteLine("Unauthorized exception while creating advertisement.\r\n{0}", ex.Message);
                    createResult = CreateResult.Unauthorized;
                    break;
                }
                catch (Exception ex)
                {
                    if (retryAttempts == maxRetryAttempts)
                    {
                        createResult = CreateResult.Timeout;
                        break;
                    }

                    var waitInterval = (int)Math.Pow(3, retryAttempts++) * baseRetryIntervalSeconds;
                    Console.WriteLine("Unexpected exception while creating advertisement, waiting {0} seconds before retrying.\r\n{1}", waitInterval, ex);
                    await Task.Delay(TimeSpan.FromSeconds(waitInterval));
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

                    // Use the returned advertisement link to get the advertisement.
                    AdvertisementResource advertisementResource = await postingClient.GetAdvertisementAsync(advertisementLink);
                    Console.WriteLine(JsonConvert.SerializeObject(advertisementResource, Formatting.Indented));

                    // Update the advertisement.
                    advertisementResource.JobTitle = "New job title";
                    advertisementResource = await advertisementResource.SaveAsync();

                    Console.WriteLine();
                    Console.WriteLine("Updated advertisement.");
                    Console.WriteLine(JsonConvert.SerializeObject(advertisementResource, Formatting.Indented));

                    var expiredAdvertisementContent = await advertisementResource.ExpireAsync();
                    Console.WriteLine();
                    Console.WriteLine("Expired advertisement.");
                    Console.WriteLine(JsonConvert.SerializeObject(expiredAdvertisementContent, Formatting.Indented));

                    var advertisementList = await postingClient.GetAllAdvertisementsAsync();
                    Console.WriteLine();
                    Console.WriteLine("Retrieve all advertisements.");
                    Console.WriteLine(JsonConvert.SerializeObject(advertisementList, Formatting.Indented));
                    break;

                case CreateResult.ValidationErrors:
                    // There were validation errors; show the errors.
                    Console.WriteLine("Advertisement creation failed. Validation errors:");
                    var counter = 1;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"  [{counter:##}] Field: '{error.Field}' Code: '{error.Code}' Message: '{error.Message}'");
                        counter++;
                    }
                    break;

                case CreateResult.Timeout:
                    Console.WriteLine("Advertisement not created. Maximum attempts reached.");
                    break;

                default:
                    Console.WriteLine($"Advertisement not created. Unexpected createResult {createResult}.");
                    break;
            }

            Console.WriteLine("Finished Example. Press a key to exit.");
            Console.ReadKey(true);
        }
    }
}