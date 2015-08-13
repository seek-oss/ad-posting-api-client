using Newtonsoft.Json;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using SEEK.AdPostingApi.Configuration;
using System;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.SampleConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(() => MainAsync()).Wait();
        }

        private static async Task MainAsync()
        {
            IConfiguration configuration = new JsonConfiguration();

            IAdPostingApiClient postingClient = new AdPostingApiClient(configuration.ClientKey, configuration.ClientSecret, configuration.Environment);

            var ad = new Advertisement
            {
                AdvertiserId = "Advertiser Id",
                JobTitle = "A Job Title",
                JobSummary = "Job summary of the job ad",
                AdvertisementDetails = "Experience Required",
                AdvertisementType = AdvertisementType.Classic,
                WorkType = WorkType.Casual,
                SalaryType = SalaryType.HourlyRate,
                LocationId = "1002",
                SubclassificationId = "6227",
                SalaryMinimum = 20,
                SalaryMaximum = 24
            };

            Uri jobAdLink = await postingClient.CreateAdvertisementAsync(ad);

            Console.WriteLine(jobAdLink.ToString());

            AdvertisementResource content = await postingClient.GetAdvertisementAsync(jobAdLink);

            Console.WriteLine(JsonConvert.SerializeObject(content, Formatting.Indented));

            Console.ReadLine();
        }
    }
}