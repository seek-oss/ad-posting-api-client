using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using Environment = SEEK.AdPostingApi.Client.Environment;

namespace SEEK.AdPostingApi.SampleConsumer
{
    public class Program
    {
        public static void Main(string[] args)
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
            IAdPostingApiClient postingClient = new AdPostingApiClient("<client id>", "<client secret>", Environment.Integration);

            // An example advertisement with a unique creation ID that ensures multiple create retries will not create duplicate advertisements.
            var ad = new Advertisement
            {
                CreationId = "Sample Consumer 2575274f-7526-455d-a2a3-32447e40733d",
                AdvertiserId = "Advertiser Id",
                JobTitle = "A Job Title",
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

            // Example of creating the advertisement using a simple retry loop.
            ValidationData[] validationDataItems = null;
            var createResult = CreateResult.Unknown;
            Uri advertisementLink = null;
            var maxAttempts = 2;
            while (maxAttempts > 0)
            {
                try
                {
                    var advertisement = await postingClient.CreateAdvertisementAsync(ad);

                    advertisementLink = advertisement.Uri;
                    createResult = CreateResult.Created;
                    break;
                }
                catch (AdvertisementAlreadyExistsException ex)
                {
                    advertisementLink = ex.AdvertisementLink;
                    createResult = CreateResult.AlreadyExists;
                    break;
                }
                catch (ValidationException ex)
                {
                    validationDataItems = ex.ValidationDataItems;
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
                    maxAttempts--;
                    Console.WriteLine("Unexpected exception while creating advertisement.\r\n{0}", ex);
                    if (maxAttempts == 0)
                    {
                        createResult = CreateResult.Timeout;
                        break;
                    }
                    await Task.Delay(2000);
                }
            }

            Console.WriteLine("Advertisement creation result: {0}", createResult);
            switch (createResult)
            {
                case CreateResult.Created:
                case CreateResult.AlreadyExists:
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
                    foreach (var item in validationDataItems)
                    {
                        Console.WriteLine($"  [{counter:##}] Field: '{item.Field}' Code: '{item.Code}' Message: '{item.Message}'");
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