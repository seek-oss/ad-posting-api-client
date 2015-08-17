using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Resources
{
    [MediaType("application/vnd.seek.advertisement+json")]
    public class AdvertisementResource : HalResource
    {
        public string AdvertiserId { get; set; }
        public string AgentId { get; set; }
        public AdvertisementType AdvertisementType { get; set; }
        public string JobTitle { get; set; }
        public string LocationId { get; set; }
        public string SubclassificationId { get; set; }
        public WorkType WorkType { get; set; }
        public SalaryType SalaryType { get; set; }
        public int SalaryMinimum { get; set; }
        public int SalaryMaximum { get; set; }
        public string SalaryDetails { get; set; }
        public string JobSummary { get; set; }
        public string AdvertisementDetails { get; set; }
        public string ContactDetails { get; set; }
        public string VideoUrl { get; set; }
        public VideoPosition? VideoPosition { get; set; }
        public string ApplicationEmail { get; set; }
        public string ApplicationFormUrl { get; set; }
        public int? ScreenId { get; set; }
        public string JobReference { get; set; }
        public int? TemplateId { get; set; }
        public TemplateItemModel[] TemplateItems { get; set; }
        public int? StandoutLogoId { get; set; }
        public string StandoutBullet1 { get; set; }
        public string StandoutBullet2 { get; set; }
        public string StandoutBullet3 { get; set; }
        public AdditionalPropertyType[] AdditionalProperties { get; set; }
        public string Status { get; set; }
    }
}