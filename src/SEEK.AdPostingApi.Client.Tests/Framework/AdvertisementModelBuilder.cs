using System.Linq;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class AdvertisementModelBuilder : AdvertisementModelBuilder<Advertisement>
    {
        public AdvertisementModelBuilder(IBuilderInitializer initializer = null) : base(initializer)
        {
            initializer?.Initialize(this);
        }
    }

    public class AdvertisementModelBuilder<TAdvertisement> where TAdvertisement : Advertisement, new()
    {
        private string _agentId;
        private string _advertiserId;
        private string _creationId;
        private string _jobTitle;
        private string _searchJobTitle;
        private string _jobSummary;
        private string _advertisementDetails;
        private AdvertisementType _advertisementType;
        private WorkType _workType;
        private string _locationId;
        private string _areaId;
        private string _subclassificationId;
        private SalaryType _salaryType;
        private decimal _salaryMinimum;
        private decimal _salaryMaximum;
        private string _salaryDetails;
        private string _contactName;
        private string _contactPhone;
        private string _contactEmail;
        private string _videoUrl;
        private VideoPosition? _videoPosition;
        private string _applicationEmail;
        private string _applicationFormUrl;
        private string _endApplicationUrl;
        private int? _screenId;
        private string _jobReference;
        private string _agentJobReference;
        private int? _templateId;
        private TemplateItem[] _templateItems;
        private int? _standoutLogoId;
        private string[] _standoutBullets;
        private AdditionalPropertyType[] _additionalPropertyTypes;

        protected AdvertisementModelBuilder(IBuilderInitializer initializer = null)
        {
            initializer?.Initialize(this);
        }

        public AdvertisementModelBuilder<TAdvertisement> WithAgentId(string agentId)
        {
            this._agentId = agentId;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithAdvertiserId(string advertiserId)
        {
            this._advertiserId = advertiserId;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithRequestCreationId(string creationId)
        {
            this._creationId = creationId;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithJobTitle(string jobTitle)
        {
            this._jobTitle = jobTitle;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithSearchJobTitle(string searchJobTitle)
        {
            this._searchJobTitle = searchJobTitle;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithJobSummary(string jobSummary)
        {
            this._jobSummary = jobSummary;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithAdvertisementDetails(string advertisementDetails)
        {
            this._advertisementDetails = advertisementDetails;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithAdvertisementType(AdvertisementType advertisementType)
        {
            this._advertisementType = advertisementType;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithWorkType(WorkType workType)
        {
            this._workType = workType;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithLocationArea(string locationId, string areaId = null)
        {
            this._locationId = locationId;
            this._areaId = areaId;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithSubclassificationId(string subclassificationId)
        {
            this._subclassificationId = subclassificationId;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithSalaryType(SalaryType salaryType)
        {
            this._salaryType = salaryType;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithSalaryMinimum(decimal minimum)
        {
            this._salaryMinimum = minimum;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithSalaryMaximum(decimal maximum)
        {
            this._salaryMaximum = maximum;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithSalaryDetails(string details)
        {
            this._salaryDetails = details;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithContactName(string contactName)
        {
            this._contactName = contactName;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithContactPhone(string contactPhone)
        {
            this._contactPhone = contactPhone;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithContactEmail(string contactEmail)
        {
            this._contactEmail = contactEmail;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithVideoUrl(string url)
        {
            this._videoUrl = url;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithVideoPosition(VideoPosition? videoPosition)
        {
            this._videoPosition = videoPosition;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithApplicationEmail(string applicationEmail)
        {
            this._applicationEmail = applicationEmail;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithApplicationFormUrl(string applicationFormUrl)
        {
            this._applicationFormUrl = applicationFormUrl;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithEndApplicationUrl(string endApplicationUrl)
        {
            this._endApplicationUrl = endApplicationUrl;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithScreenId(int? screenId)
        {
            this._screenId = screenId;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithJobReference(string jobReference)
        {
            this._jobReference = jobReference;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithAgentJobReference(string agentJobReference)
        {
            this._agentJobReference = agentJobReference;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithTemplateId(int? id)
        {
            this._templateId = id;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithTemplateItems(params TemplateItem[] templateItems)
        {
            this._templateItems = templateItems?.Select(itm => itm == null ? null : new TemplateItem { Name = itm.Name, Value = itm.Value }).ToArray();

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithStandoutLogoId(int? logoId)
        {
            this._standoutLogoId = logoId;

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithStandoutBullets(params string[] bullets)
        {
            this._standoutBullets = bullets?.ToArray();

            return this;
        }

        public AdvertisementModelBuilder<TAdvertisement> WithAdditionalProperties(params AdditionalPropertyType[] additionalPropertyTypes)
        {
            this._additionalPropertyTypes = additionalPropertyTypes.ToArray();

            return this;
        }

        public virtual TAdvertisement Build()
        {
            return new TAdvertisement
            {
                ThirdParties = this._advertiserId == null && this._agentId == null
                    ? null
                    : new ThirdParties { AdvertiserId = this._advertiserId, AgentId = this._agentId },
                CreationId = this._creationId,
                AdvertisementType = this._advertisementType,
                JobTitle = this._jobTitle,
                SearchJobTitle = this._searchJobTitle,
                Location = this._locationId == null && this._areaId == null
                    ? null
                    : new Location { Id = this._locationId, AreaId = this._areaId },
                SubclassificationId = this._subclassificationId,
                WorkType = this._workType,
                JobSummary = this._jobSummary,
                AdvertisementDetails = this._advertisementDetails,
                ApplicationEmail = this._applicationEmail,
                ApplicationFormUrl = this._applicationFormUrl,
                EndApplicationUrl = this._endApplicationUrl,
                ScreenId = this._screenId,
                JobReference = this._jobReference,
                AgentJobReference = this._agentJobReference,
                Salary = new Salary
                {
                    Type = this._salaryType,
                    Minimum = this._salaryMinimum,
                    Maximum = this._salaryMaximum,
                    Details = this._salaryDetails
                },
                Contact = this._contactName == null && this._contactPhone == null && this._contactEmail == null
                    ? null
                    : new Contact { Name = this._contactName, Phone = this._contactPhone, Email = this._contactEmail },
                Template = this._templateId == null && this._templateItems == null
                    ? null
                    : new Template { Id = this._templateId, Items = this._templateItems?.ToArray() },
                Video = this._videoUrl == null && this._videoPosition == null
                    ? null
                    : new Video { Url = this._videoUrl, Position = this._videoPosition ?? default(VideoPosition) },
                Standout = this._standoutLogoId == null && this._standoutBullets == null
                    ? null
                    : new StandoutAdvertisement { LogoId = this._standoutLogoId, Bullets = this._standoutBullets?.ToArray() },
                AdditionalProperties = this._additionalPropertyTypes?.ToArray()
            };
        }
    }
}