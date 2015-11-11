using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    public class AdvertisementModelBuilder
    {
        private readonly Advertisement _advertisementModel;

        public AdvertisementModelBuilder(IBuilderInitializer initializer = null, Advertisement advertisement = null)
        {
            this._advertisementModel = advertisement ?? new Advertisement();
            initializer?.Initialize(this);
        }

        public AdvertisementModelBuilder WithAgentId(string agentId)
        {
            this._advertisementModel.AgentId = agentId;
            return this;
        }

        public AdvertisementModelBuilder WithAdvertiserId(string advertiserId)
        {
            this._advertisementModel.AdvertiserId = advertiserId;
            return this;
        }

        public AdvertisementModelBuilder WithRequestCreationId(string creationId)
        {
            this._advertisementModel.CreationId = creationId;
            return this;
        }

        public AdvertisementModelBuilder WithJobTitle(string jobTitle)
        {
            this._advertisementModel.JobTitle = jobTitle;
            return this;
        }

        public AdvertisementModelBuilder WithJobSummary(string jobSummary)
        {
            this._advertisementModel.JobSummary = jobSummary;
            return this;
        }

        public AdvertisementModelBuilder WithAdvertisementDetails(string advertisementDetails)
        {
            this._advertisementModel.AdvertisementDetails = advertisementDetails;
            return this;
        }

        public AdvertisementModelBuilder WithAdvertisementType(AdvertisementType advertisementType)
        {
            this._advertisementModel.AdvertisementType = advertisementType;
            return this;
        }

        public AdvertisementModelBuilder WithWorkType(WorkType workType)
        {
            this._advertisementModel.WorkType = workType;
            return this;
        }

        public AdvertisementModelBuilder WithLocationId(string locationId)
        {
            this._advertisementModel.LocationId = locationId;
            return this;
        }

        public AdvertisementModelBuilder WithSubclassificationId(string subclassificationId)
        {
            this._advertisementModel.SubclassificationId = subclassificationId;
            return this;
        }

        public AdvertisementModelBuilder WithSalaryType(SalaryType salaryType)
        {
            this._advertisementModel.Salary = this._advertisementModel.Salary ?? new Salary();
            this._advertisementModel.Salary.Type = salaryType;
            return this;
        }

        public AdvertisementModelBuilder WithSalaryMinimum(int minimum)
        {
            this._advertisementModel.Salary = this._advertisementModel.Salary ?? new Salary();
            this._advertisementModel.Salary.Minimum = minimum;
            return this;
        }

        public AdvertisementModelBuilder WithSalaryMaximum(int maximum)
        {
            this._advertisementModel.Salary = this._advertisementModel.Salary ?? new Salary();
            this._advertisementModel.Salary.Maximum = maximum;
            return this;
        }

        public AdvertisementModelBuilder WithSalaryDetails(string details)
        {
            this._advertisementModel.Salary = this._advertisementModel.Salary ?? new Salary();
            this._advertisementModel.Salary.Details = details;
            return this;
        }

        public AdvertisementModelBuilder WithContactDetails(string contactDetails)
        {
            this._advertisementModel.ContactDetails = contactDetails;
            return this;
        }

        public AdvertisementModelBuilder WithVideoUrl(string url)
        {
            this._advertisementModel.Video = this._advertisementModel.Video ?? new Video();
            this._advertisementModel.Video.Url = url;
            return this;
        }

        public AdvertisementModelBuilder WithVideoPosition(VideoPosition videoPosition)
        {
            this._advertisementModel.Video = this._advertisementModel.Video ?? new Video();
            this._advertisementModel.Video.Position = videoPosition;
            return this;
        }

        public AdvertisementModelBuilder WithApplicationEmail(string applicationEmail)
        {
            this._advertisementModel.ApplicationEmail = applicationEmail;
            return this;
        }

        public AdvertisementModelBuilder WithApplicationFormUrl(string applicationFormUrl)
        {
            this._advertisementModel.ApplicationFormUrl = applicationFormUrl;
            return this;
        }

        public AdvertisementModelBuilder WithScreenId(int? screenId)
        {
            this._advertisementModel.ScreenId = screenId;
            return this;
        }

        public AdvertisementModelBuilder WithJobReference(string jobReference)
        {
            this._advertisementModel.JobReference = jobReference;
            return this;
        }

        public AdvertisementModelBuilder WithTemplateId(int? id)
        {
            this._advertisementModel.Template = this._advertisementModel.Template ?? new Template();

            this._advertisementModel.Template.Id = id;
            return this;
        }

        public AdvertisementModelBuilder WithTemplateItems(params TemplateItemModel[] templateItems)
        {
            this._advertisementModel.Template = this._advertisementModel.Template ?? new Template();

            this._advertisementModel.Template.Items = templateItems?.Clone<TemplateItemModel[]>();
            return this;
        }

        public AdvertisementModelBuilder WithStandoutLogoId(int? logoId)
        {
            this._advertisementModel.Standout = this._advertisementModel.Standout ?? new StandoutAdvertisement();

            this._advertisementModel.Standout.LogoId = logoId;
            return this;
        }

        public AdvertisementModelBuilder WithStandoutBullets(params string[] bullets)
        {
            this._advertisementModel.Standout = this._advertisementModel.Standout ?? new StandoutAdvertisement();

            this._advertisementModel.Standout.Bullets = bullets?.Clone<string[]>();
            return this;
        }

        public AdvertisementModelBuilder WithSeekCodes(params string[] seekCodes)
        {
            this._advertisementModel.SeekCodes = seekCodes?.Clone<string[]>();
            return this;
        }

        public AdvertisementModelBuilder WithAdditionalProperties(params AdditionalPropertyType[] additionalPropertyTypes)
        {
            this._advertisementModel.AdditionalProperties = additionalPropertyTypes.Clone<AdditionalPropertyType[]>();
            return this;
        }

        public Advertisement Build()
        {
            return this._advertisementModel;
        }
    }
}