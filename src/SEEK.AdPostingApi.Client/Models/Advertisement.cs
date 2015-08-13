using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Models
{
    public class Advertisement
    {
        public string postingRequestorId { get; set; }

        public string advertiserId { get; set; }

        public string jobTitle { get; set; }

        public string jobSummary { get; set; }

        public string advertisementDetails { get; set; }

        public string advertisementType { get; set; }

        public string workType { get; set; }

        public string salaryType { get; set; }

        public int locationId { get; set; }

        public int subclassificationId { get; set; }

        public int salaryMinimum { get; set; }

        public int salaryMaximum { get; set; }

        public string agentId { get; set; }

        public string salaryDetails { get; set; }

        public string contactDetails { get; set; }

        public string videoUrl { get; set; }

        public string videoPosition { get; set; }

        public string applicationEmail { get; set; }

        public string applicationFormUrl { get; set; }

        public int? screenId { get; set; }

        public string jobReference { get; set; }

        public int? templateId { get; set; }

        public TemplateItemModel[] templateItems { get; set; }

        public int? standoutLogoId { get; set; }

        public string standoutBullet1 { get; set; }

        public string standoutBullet2 { get; set; }

        public string standoutBullet3 { get; set; }

        public AdditionalPropertyType[] additionalProperties { get; set; }
    }
}
