using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    public class AdvertisementContentBuilder
    {
        private readonly dynamic _advertisementModel = new ExpandoObject();

        public AdvertisementContentBuilder(IBuilderInitializer initializer)
        {
            initializer?.Initialize(this);
        }

        private void TryRemoveProperty(dynamic model, string propertyName)
        {
            var dictionary = model as IDictionary<string, object>;
            if (dictionary == null) return;

            if (dictionary.ContainsKey(propertyName))
            {
                dictionary.Remove(propertyName);
            }
        }

        public AdvertisementContentBuilder WithAgentId(object agentId)
        {
            _advertisementModel.agentId = agentId;
            return this;
        }

        public AdvertisementContentBuilder WithoutAgentId()
        {
            TryRemoveProperty(_advertisementModel, "agentId");
            return this;
        }

        public AdvertisementContentBuilder WithAdvertiserId(object advertiserId)
        {
            _advertisementModel.advertiserId = advertiserId;
            return this;
        }

        public AdvertisementContentBuilder WithoutAdvertiserId()
        {
            TryRemoveProperty(_advertisementModel, "advertiserId");
            return this;
        }

        public AdvertisementContentBuilder WithRequestCreationId(object creationId)
        {
            _advertisementModel.creationId = creationId;
            return this;
        }

        public AdvertisementContentBuilder WithJobTitle(object jobTitle)
        {
            _advertisementModel.jobTitle = jobTitle;
            return this;
        }

        public AdvertisementContentBuilder WithJobSummary(object jobSummary)
        {
            _advertisementModel.jobSummary = jobSummary;
            return this;
        }

        public AdvertisementContentBuilder WithAdvertisementDetails(object advertisementDetails)
        {
            _advertisementModel.advertisementDetails = advertisementDetails;
            return this;
        }

        public AdvertisementContentBuilder WithAdvertisementType(object advertisementType)
        {
            _advertisementModel.advertisementType = advertisementType;
            return this;
        }

        public AdvertisementContentBuilder WithWorkType(object workType)
        {
            _advertisementModel.workType = workType;
            return this;
        }

        public AdvertisementContentBuilder WithLocationId(object locationId)
        {
            _advertisementModel.locationId = locationId;
            return this;
        }

        public AdvertisementContentBuilder WithSubclassificationId(object subclassificationId)
        {
            _advertisementModel.subclassificationId = subclassificationId;
            return this;
        }

        private void EnsureSalaryPropertyExists()
        {
            if (!((IDictionary<string, object>) _advertisementModel).ContainsKey("salary"))
            {
                _advertisementModel.salary = new ExpandoObject();
            }
        }

        public AdvertisementContentBuilder WithSalaryType(object salaryType)
        {
            EnsureSalaryPropertyExists();
            _advertisementModel.salary.type = salaryType;
            return this;
        }

        public AdvertisementContentBuilder WithSalaryMinimum(object minimum)
        {
            EnsureSalaryPropertyExists();
            _advertisementModel.salary.minimum = minimum;
            return this;
        }

        public AdvertisementContentBuilder WithSalaryMaximum(object maximum)
        {
            EnsureSalaryPropertyExists();
            _advertisementModel.salary.maximum = maximum;
            return this;
        }

        public AdvertisementContentBuilder WithSalaryDetails(object details)
        {
            EnsureSalaryPropertyExists();
            _advertisementModel.salary.details = details;
            return this;
        }

        public AdvertisementContentBuilder WithContactDetails(object contactDetails)
        {
            _advertisementModel.contactDetails = contactDetails;
            return this;
        }

        private void EnsureVideoPropertyExists()
        {
            if (!((IDictionary<string, object>) _advertisementModel).ContainsKey("video"))
            {
                _advertisementModel.video = new ExpandoObject();
            }
        }

        public AdvertisementContentBuilder WithVideoUrl(object url)
        {
            EnsureVideoPropertyExists();
            _advertisementModel.video.url = url;
            return this;
        }

        public AdvertisementContentBuilder WithVideoPosition(object videoPosition)
        {
            EnsureVideoPropertyExists();
            _advertisementModel.video.position = videoPosition;
            return this;
        }

        public AdvertisementContentBuilder WithApplicationEmail(object applicationEmail)
        {
            _advertisementModel.applicationEmail = applicationEmail;
            return this;
        }

        public AdvertisementContentBuilder WithApplicationFormUrl(object applicationFormUrl)
        {
            _advertisementModel.applicationFormUrl = applicationFormUrl;
            return this;
        }

        public AdvertisementContentBuilder WithScreenId(object screenId)
        {
            _advertisementModel.screenId = screenId;
            return this;
        }

        public AdvertisementContentBuilder WithJobReference(object jobReference)
        {
            _advertisementModel.jobReference = jobReference;
            return this;
        }

        private void EnsureTemplatePropertyExists()
        {
            if (!((IDictionary<string, object>)_advertisementModel).ContainsKey("template"))
            {
                _advertisementModel.template = new ExpandoObject();
            }
        }

        public AdvertisementContentBuilder WithTemplateId(object id)
        {
            EnsureTemplatePropertyExists();

            _advertisementModel.template.id = id;
            return this;
        }

        public AdvertisementContentBuilder WithTemplateItems(params KeyValuePair<object, object>[] templateItems)
        {
            EnsureTemplatePropertyExists();

            _advertisementModel.template.items = templateItems?.Select(t => new { name = t.Key, value = t.Value }).ToArray();
            return this;
        }

        private void EnsureStandoutPropertyExists()
        {
            if (!((IDictionary<string, object>) _advertisementModel).ContainsKey("standout"))
            {
                _advertisementModel.standout = new ExpandoObject();
            }
        }

        public AdvertisementContentBuilder WithStandoutLogoId(object logoId)
        {
            EnsureStandoutPropertyExists();

            _advertisementModel.standout.logoId = logoId;
            return this;
        }

        public AdvertisementContentBuilder WithStandoutBullets(params object[] bullets)
        {
            EnsureStandoutPropertyExists();

            _advertisementModel.standout.bullets = bullets?.Clone();
            return this;
        }

        public AdvertisementContentBuilder WithSeekCodes(params object[] seekCodes)
        {
            _advertisementModel.seekCodes = seekCodes?.Clone();
            return this;
        }

        public AdvertisementContentBuilder WithoutSeekCodes()
        {
            TryRemoveProperty(_advertisementModel, "seekCodes");
            return this;
        }

        public AdvertisementContentBuilder WithAdditionalProperties(params object[] additionalPropertyTypes)
        {
            _advertisementModel.additionalProperties = additionalPropertyTypes?.Clone<object[]>();
            return this;
        }

        public AdvertisementContentBuilder WithResponseLink(string linkName, object linkRef)
        {
            if (!((IDictionary<string, object>)_advertisementModel).ContainsKey("_links"))
            {
                _advertisementModel._links = new ExpandoObject();
            }

            dynamic href = new ExpandoObject();
            href.href = linkRef;
            ((IDictionary<string, object>)_advertisementModel._links).Add(linkName, href);

            return this;
        }

        public AdvertisementContentBuilder WithState(string state)
        {
            _advertisementModel.state = state;

            return this;
        }

        public dynamic Build()
        {
            return ((IDictionary<string, object>)_advertisementModel).Clone();
        }
    }
}