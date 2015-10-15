using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Models
{
    [MediaType("application/vnd.seek.advertisement+json")]
    public class Advertisement
    {
        public string AdvertiserId { get; set; }
        public string AgentId { get; set; }
        public AdvertisementState State { get; internal set; }
        public string CreationId { get; set; }
        public AdvertisementType AdvertisementType { get; set; }
        public string JobTitle { get; set; }
        public string LocationId { get; set; }
        public string SubclassificationId { get; set; }
        public WorkType WorkType { get; set; }
        public string JobSummary { get; set; }
        public string AdvertisementDetails { get; set; }
        public string ContactDetails { get; set; }
        public string ApplicationEmail { get; set; }
        public string ApplicationFormUrl { get; set; }
        public int? ScreenId { get; set; }
        public string JobReference { get; set; }
        public string[] SeekCodes { get; set; }
        public Salary Salary { get; set; }
        public Template Template { get; set; }
        public Video Video { get; set; }
        public StandoutAdvertisement Standout { get; set; }
        public AdditionalPropertyType[] AdditionalProperties { get; set; }
        public ValidationData[] Warnings { get; set; }
        public AdvertisementError[] Errors { get; set; }

        public bool ShouldSerializeState()
        {
            return false;
        }

        public bool ShouldSerializeErrors()
        {
            return false;
        }
    }
}