using Newtonsoft.Json;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Exceptions;
using Environment = SEEK.AdPostingApi.Client.Environment;

namespace SEEK.AdPostingApi.SampleConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(() => MainAsync()).Wait();
        }

        private enum CreateResult
        {
            Unknown,
            Created,
            AlreadyExists,
            ValidationErrors,
            Timeout
        }

        private static async Task MainAsync()
        {
            IAdPostingApiClient postingClient = new AdPostingApiClient("<client id>", "<client secret>", Environment.Integration);

            // An example advertisement with a unique creation ID that ensures multiple create retries will not create duplicate advertisements.
            var ad = new Advertisement
            {
                CreationId = "Sample Consumer 20151001 114732 1234567",
                AdvertiserId = "Advertiser Id",
                JobTitle = "A Job Title",
                JobSummary = "Job summary of the job ad",
                AdvertisementDetails = "Experience Required",
                AdvertisementType = AdvertisementType.Classic,
                WorkType = WorkType.Casual,
                Salary = new Salary() { Type = SalaryType.HourlyRate,
                                        Minimum = 20,
                                        Maximum = 24},
                LocationId = "1002",
                SubclassificationId = "6227",
               
            };

            // Example of creating the advertisement using a simple retry loop.
            ValidationData[] validationDataItems = null;
            var createResult = CreateResult.Unknown;
            AdvertisementResource advertisement = null;
            Uri advertisementLink = null;
            var maxAttempts = 5;
            while (maxAttempts > 0)
            {
                try
                {
                    advertisement = await postingClient.CreateAdvertisementAsync(ad);
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
                    AdvertisementResource content = await postingClient.GetAdvertisementAsync(advertisementLink);
                    Console.WriteLine(JsonConvert.SerializeObject(content, Formatting.Indented));

                    // Update the advertisement.
                    content.Properties.JobTitle = "New job title";
                    await content.SaveAsync();
            //Thread.Sleep(3600000);

                    AdvertisementResource newContent = await postingClient.GetAdvertisementAsync(advertisementLink);
                    Console.WriteLine();
                    Console.WriteLine("Updated job advertisement.");
                    Console.WriteLine(JsonConvert.SerializeObject(newContent, Formatting.Indented));
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
                    Console.WriteLine("Job not created. Maximum attempts reached.");
                    break;
                default:
                    Console.WriteLine($"Job not created. Unexpected createResult {createResult}.");
                    break;
            }

            Console.WriteLine("Finished Example. Press a key to exit.");
            Console.ReadKey(true);
        }
    }
}