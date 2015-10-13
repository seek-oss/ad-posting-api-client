using System.Collections.Generic;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    public class AdvertisementModelBuilder
    {
        private readonly Advertisement _advertisementModel = new Advertisement();

        public AdvertisementModelBuilder(IBuilderInitializer initializer = null)
        {
            initializer?.Initialize(this);
        }

        public AdvertisementModelBuilder WithAgentId(string agentId)
        {
            _advertisementModel.AgentId = agentId;
            return this;
        }

        public AdvertisementModelBuilder WithAdvertiserId(string advertiserId)
        {
            _advertisementModel.AdvertiserId = advertiserId;
            return this;
        }

        public AdvertisementModelBuilder WithRequestCreationId(string creationId)
        {
            _advertisementModel.CreationId = creationId;
            return this;
        }

        public AdvertisementModelBuilder WithJobTitle(string jobTitle)
        {
            _advertisementModel.JobTitle = jobTitle;
            return this;
        }

        public AdvertisementModelBuilder WithJobSummary(string jobSummary)
        {
            _advertisementModel.JobSummary = jobSummary;
            return this;
        }

        public AdvertisementModelBuilder WithAdvertisementDetails(string advertisementDetails)
        {
            _advertisementModel.AdvertisementDetails = advertisementDetails;
            return this;
        }

        public AdvertisementModelBuilder WithAdvertisementType(AdvertisementType advertisementType)
        {
            _advertisementModel.AdvertisementType = advertisementType;
            return this;
        }

        public AdvertisementModelBuilder WithWorkType(WorkType workType)
        {
            _advertisementModel.WorkType = workType;
            return this;
        }

        public AdvertisementModelBuilder WithLocationId(string locationId)
        {
            _advertisementModel.LocationId = locationId;
            return this;
        }

        public AdvertisementModelBuilder WithSubclassificationId(string subclassificationId)
        {
            _advertisementModel.SubclassificationId = subclassificationId;
            return this;
        }

        public AdvertisementModelBuilder WithSalaryType(SalaryType salaryType)
        {
            _advertisementModel.Salary = _advertisementModel.Salary ?? new Salary();
            _advertisementModel.Salary.Type = salaryType;
            return this;
        }

        public AdvertisementModelBuilder WithSalaryMinimum(int minimum)
        {
            _advertisementModel.Salary = _advertisementModel.Salary ?? new Salary();
            _advertisementModel.Salary.Minimum = minimum;
            return this;
        }

        public AdvertisementModelBuilder WithSalaryMaximum(int maximum)
        {
            _advertisementModel.Salary = _advertisementModel.Salary ?? new Salary();
            _advertisementModel.Salary.Maximum = maximum;
            return this;
        }

        public AdvertisementModelBuilder WithSalaryDetails(string details)
        {
            _advertisementModel.Salary = _advertisementModel.Salary ?? new Salary();
            _advertisementModel.Salary.Details = details;
            return this;
        }

        public AdvertisementModelBuilder WithContactDetails(string contactDetails)
        {
            _advertisementModel.ContactDetails = contactDetails;
            return this;
        }

        public AdvertisementModelBuilder WithVideoUrl(string url)
        {
            _advertisementModel.Video = _advertisementModel.Video ?? new Video();
            _advertisementModel.Video.Url = url;
            return this;
        }

        public AdvertisementModelBuilder WithVideoPosition(VideoPosition? videoPosition)
        {
            _advertisementModel.Video = _advertisementModel.Video ?? new Video();
            _advertisementModel.Video.Position = videoPosition;
            return this;
        }

        public AdvertisementModelBuilder WithApplicationEmail(string applicationEmail)
        {
            _advertisementModel.ApplicationEmail = applicationEmail;
            return this;
        }

        public AdvertisementModelBuilder WithApplicationFormUrl(string applicationFormUrl)
        {
            _advertisementModel.ApplicationFormUrl = applicationFormUrl;
            return this;
        }

        public AdvertisementModelBuilder WithScreenId(int? screenId)
        {
            _advertisementModel.ScreenId = screenId;
            return this;
        }

        public AdvertisementModelBuilder WithJobReference(string jobReference)
        {
            _advertisementModel.JobReference = jobReference;
            return this;
        }

        public AdvertisementModelBuilder WithTemplateId(int? id)
        {
            _advertisementModel.Template = _advertisementModel.Template ?? new Template();

            _advertisementModel.Template.Id = id;
            return this;
        }

        public AdvertisementModelBuilder WithTemplateItem(string name, string value)
        {
            _advertisementModel.Template = _advertisementModel.Template ?? new Template();

            var templateItemModels = _advertisementModel.Template.Items == null ? new List<TemplateItemModel>() : new List<TemplateItemModel>(_advertisementModel.Template.Items);

            templateItemModels.Add(new TemplateItemModel { Name = name, Value = value });
            _advertisementModel.Template.Items = templateItemModels.ToArray();
            return this;
        }

        public AdvertisementModelBuilder WithStandoutLogoId(int? logoId)
        {
            _advertisementModel.Standout = _advertisementModel.Standout ?? new StandoutAdvertisement();

            _advertisementModel.Standout.LogoId = logoId;
            return this;
        }

        public AdvertisementModelBuilder WithStandoutBullets(params string[] bullets)
        {
            _advertisementModel.Standout = _advertisementModel.Standout ?? new StandoutAdvertisement();

            _advertisementModel.Standout.Bullets = bullets?.Clone<string[]>();
            return this;
        }

        public AdvertisementModelBuilder WithSeekCodes(params string[] seekCodes)
        {
            _advertisementModel.SeekCodes = seekCodes?.Clone<string[]>();
            return this;
        }

        public AdvertisementModelBuilder WithAdditionalProperties(params AdditionalPropertyType[] additionalPropertyTypes)
        {
            _advertisementModel.AdditionalProperties = additionalPropertyTypes.Clone<AdditionalPropertyType[]>();
            return this;
        }

        public AdvertisementModelBuilder WithState(AdvertisementState state)
        {
            _advertisementModel.State = state;

            return this;
        }

        public Advertisement Build()
        {
            return _advertisementModel;
        }
    }
}