using System;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Configuration;

namespace SEEK.AdPostingApi.SampleConsumer
{
    class Program
    {
        private static void Main(string[] args)
        {
            Task.Run(() => MainAsync()).Wait();
        }

        private static async Task MainAsync()
        {
            IConfiguration configuration = new JsonConfiguration();

            var postingClient = new AdPostingApiClient(configuration.ClientId, configuration.ClientSecret, configuration.Environment);

            var ad = new Advertisement
            {
                advertiserId = "Advertiser Id",
                jobTitle = "A Job Title",
                jobSummary = "Job summary of the job ad",
                advertisementDetails = "Experience Required",
                advertisementType = AdvertisementType.Classic.ToString(),
                workType = WorkType.Casual.ToString(),
                salaryType = SalaryType.HourlyRate.ToString(),
                locationId = 1002,
                subclassificationId = 6227,
                salaryMinimum = 20,
                salaryMaximum = 24
            };
            var jobAdLink = await postingClient.CreateAdvertisementAsync(ad);
            Console.WriteLine(jobAdLink.ToString());
            Console.ReadLine();
        }

    }
}
