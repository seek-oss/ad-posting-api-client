using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class MinimumFieldsInitializer : IBuilderInitializer
    {
        public void Initialize(AdvertisementContentBuilder builder)
        {
            builder
                .WithAdvertisementDetails(this.GetDefaultAdvertisementDetails())
                .WithAdvertiserId(this.GetDefaultAdvertiserId())
                .WithAdvertisementType(AdvertisementType.Classic.ToString())
                .WithJobSummary(this.GetDefaultJobSummary())
                .WithJobTitle(this.GetDefaultJobTitle())
                .WithLocationId(this.GetDefaultLocationId())
                .WithLocationAreaId(this.GetDefaultLocationAreaId())
                .WithSalaryMinimum(this.GetDefaultSalaryMinimum())
                .WithSalaryMaximum(this.GetDefaultSalaryMaximum())
                .WithSalaryType(this.GetDefaultSalaryType().ToString())
                .WithSubclassificationId(this.GetDefaultSubclassificationId())
                .WithWorkType(this.GetDefaultWorkType().ToString())
                .WithRecruiterFullName(this.GetDefaultRecruiterFullName())
                .WithRecruiterEmail(this.GetDefaultRecruiterEmail());
        }

        public void Initialize<TAdvertisement>(AdvertisementModelBuilder<TAdvertisement> builder) where TAdvertisement : Advertisement, new()
        {
            builder
                .WithAdvertiserId(this.GetDefaultAdvertiserId())
                .WithAdvertisementType(AdvertisementType.Classic)
                .WithJobTitle(this.GetDefaultJobTitle())
                .WithLocationArea(this.GetDefaultLocationId(), this.GetDefaultLocationAreaId())
                .WithSubclassificationId(this.GetDefaultSubclassificationId())
                .WithWorkType(this.GetDefaultWorkType())
                .WithSalaryType(this.GetDefaultSalaryType())
                .WithSalaryMinimum(this.GetDefaultSalaryMinimum())
                .WithSalaryMaximum(this.GetDefaultSalaryMaximum())
                .WithJobSummary(this.GetDefaultJobSummary())
                .WithAdvertisementDetails(this.GetDefaultAdvertisementDetails())
                .WithRecruiterFullName(this.GetDefaultRecruiterFullName())
                .WithRecruiterEmail(this.GetDefaultRecruiterEmail());
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

        private string GetDefaultRecruiterFullName()
        {
            return "John Smith";
        }

        private string GetDefaultRecruiterEmail()
        {
            return "smithy@recruiter.com";
        }
    }
}