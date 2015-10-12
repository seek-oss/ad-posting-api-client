using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    public class MinimumFieldsInitializer : IBuilderInitializer
    {
        public void Initialize(AdvertisementContentBuilder builder)
        {
            builder
                .WithAdvertiserId(GetDefaultAdvertiserId())
                .WithAdvertisementType(AdvertisementType.Classic.ToString())
                .WithJobTitle(GetDefaultJobTitle())
                .WithLocationId(GetDefaultLocationId())
                .WithSubclassificationId(GetDefaultSubclassificationId())
                .WithWorkType(GetDefaultWorkType().ToString())
                .WithSalaryType(GetDefaultSalaryType().ToString())
                .WithSalaryMinimum(GetDefaultSalaryMinimum())
                .WithSalaryMaximum(GetDefaultSalaryMaximum())
                .WithJobSummary(GetDefaultJobSummary())
                .WithAdvertisementDetails(GetDefaultAdvertisementDetails());
        }

        public void Initialize(AdvertisementModelBuilder builder)
        {
            builder
                .WithAdvertiserId(GetDefaultAdvertiserId())
                .WithAdvertisementType(AdvertisementType.Classic)
                .WithJobTitle(GetDefaultJobTitle())
                .WithLocationId(GetDefaultLocationId())
                .WithSubclassificationId(GetDefaultSubclassificationId())
                .WithWorkType(GetDefaultWorkType())
                .WithSalaryType(GetDefaultSalaryType())
                .WithSalaryMinimum(GetDefaultSalaryMinimum())
                .WithSalaryMaximum(GetDefaultSalaryMaximum())
                .WithJobSummary(GetDefaultJobSummary())
                .WithAdvertisementDetails(GetDefaultAdvertisementDetails());
        }

        private string GetDefaultAdvertiserId()
        {
            return "advertiserB";
        }

        private string GetDefaultJobTitle()
        {
            return "Baker";
        }

        private string GetDefaultLocationId()
        {
            return "1002";
        }

        private string GetDefaultSubclassificationId()
        {
            return "6227";
        }

        private WorkType GetDefaultWorkType()
        {
            return WorkType.Casual;
        }

        private SalaryType GetDefaultSalaryType()
        {
            return SalaryType.HourlyRate;
        }

        private int GetDefaultSalaryMinimum()
        {
            return 20;
        }

        private int GetDefaultSalaryMaximum()
        {
            return 24;
        }

        private string GetDefaultJobSummary()
        {
            return "Fantastic opportunity for an awesome baker";
        }

        private string GetDefaultAdvertisementDetails()
        {
            return "Baking experience required";
        }
    }
}
