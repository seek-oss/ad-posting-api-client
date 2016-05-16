using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Tests
{
    public class MinimumFieldsInitializer : IBuilderInitializer
    {
        public void Initialize(AdvertisementContentBuilder builder)
        {
            builder
                .WithAdvertisementDetails(GetDefaultAdvertisementDetails())
                .WithAdvertiserId(GetDefaultAdvertiserId())
                .WithAdvertisementType(AdvertisementType.Classic.ToString())
                .WithJobSummary(GetDefaultJobSummary())
                .WithJobTitle(GetDefaultJobTitle())
                .WithLocationId(this.GetDefaultLocationId())
                .WithLocationAreaId(this.GetDefaultLocationAreaId())
                .WithSalaryMinimum(GetDefaultSalaryMinimum())
                .WithSalaryMaximum(GetDefaultSalaryMaximum())
                .WithSalaryType(GetDefaultSalaryType().ToString())
                .WithSubclassificationId(GetDefaultSubclassificationId())
                .WithWorkType(GetDefaultWorkType().ToString());
        }

        public void Initialize<TAdvertisement>(AdvertisementModelBuilder<TAdvertisement> builder) where TAdvertisement : Advertisement, new()
        {
            builder
                .WithAdvertiserId(GetDefaultAdvertiserId())
                .WithAdvertisementType(AdvertisementType.Classic)
                .WithJobTitle(GetDefaultJobTitle())
                .WithLocationArea(this.GetDefaultLocationId(), this.GetDefaultLocationAreaId())
                .WithSubclassificationId(GetDefaultSubclassificationId())
                .WithWorkType(GetDefaultWorkType())
                .WithSalaryType(GetDefaultSalaryType())
                .WithSalaryMinimum(GetDefaultSalaryMinimum())
                .WithSalaryMaximum(GetDefaultSalaryMaximum())
                .WithJobSummary(GetDefaultJobSummary())
                .WithAdvertisementDetails(GetDefaultAdvertisementDetails());
        }

        private string GetDefaultAdvertisementDetails()
        {
            return "Exciting, do I need to say more?";
        }

        private string GetDefaultAdvertiserId()
        {
            return "1";
        }

        private string GetDefaultJobSummary()
        {
            return "Developer job";
        }

        private string GetDefaultJobTitle()
        {
            return "Exciting Senior Developer role in a great CBD location. Great $$$";
        }

        private string GetDefaultLocationAreaId()
        {
            return "RussiaEasternEurope";
        }

        private string GetDefaultLocationId()
        {
            return "EuropeRussia";
        }

        private decimal GetDefaultSalaryMaximum()
        {
            return 119999;
        }

        private decimal GetDefaultSalaryMinimum()
        {
            return 100000;
        }

        private SalaryType GetDefaultSalaryType()
        {
            return SalaryType.AnnualPackage;
        }

        private string GetDefaultSubclassificationId()
        {
            return "AerospaceEngineering";
        }

        private WorkType GetDefaultWorkType()
        {
            return WorkType.FullTime;
        }
    }
}