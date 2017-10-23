using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class AdvertisementContentBuilder
    {
        protected readonly dynamic AdvertisementModel = new ExpandoObject();

        public AdvertisementContentBuilder(IBuilderInitializer initializer)
        {
            initializer?.Initialize(this);
        }

        public dynamic Build()
        {
            return ((IDictionary<string, object>)this.AdvertisementModel).Clone();
        }

        public AdvertisementContentBuilder WithAdditionalProperties(params object[] additionalPropertyTypes)
        {
            this.AdvertisementModel.additionalProperties = additionalPropertyTypes?.Clone<object[]>();
            return this;
        }

        public AdvertisementContentBuilder WithProcessingOptions(params object[] processingOptionTypes)
        {
            this.AdvertisementModel.processingOptions = processingOptionTypes?.Clone<object[]>();
            return this;
        }

        public AdvertisementContentBuilder WithAdvertisementDetails(object advertisementDetails)
        {
            this.AdvertisementModel.advertisementDetails = advertisementDetails;
            return this;
        }

        public AdvertisementContentBuilder WithAdvertisementType(object advertisementType)
        {
            this.AdvertisementModel.advertisementType = advertisementType;
            return this;
        }

        public AdvertisementContentBuilder WithAdvertiserId(object advertiserId)
        {
            object agentId = PropertyExists(this.AdvertisementModel, "thirdParties") &&
                             PropertyExists(this.AdvertisementModel.thirdParties, "agentId")
                ? this.AdvertisementModel.thirdParties.agentId
                : null;

            this.CreateOrRemoveThirdParties(advertiserId, agentId);

            return this;
        }

        public AdvertisementContentBuilder WithAgentId(object agentId)
        {
            object advertiserId = PropertyExists(this.AdvertisementModel, "thirdParties") &&
                                  PropertyExists(this.AdvertisementModel.thirdParties, "advertiserId")
                ? this.AdvertisementModel.thirdParties.advertiserId
                : null;

            this.CreateOrRemoveThirdParties(advertiserId, agentId);

            return this;
        }

        public AdvertisementContentBuilder WithAgentJobReference(object agentJobReference)
        {
            if (string.IsNullOrWhiteSpace((string)agentJobReference))
            {
                TryRemoveProperty(this.AdvertisementModel, "agentJobReference");
            }
            else
            {
                this.AdvertisementModel.agentJobReference = agentJobReference;
            }
            return this;
        }

        public AdvertisementContentBuilder WithApplicationEmail(object applicationEmail)
        {
            this.AdvertisementModel.applicationEmail = applicationEmail;
            return this;
        }

        public AdvertisementContentBuilder WithApplicationFormUrl(object applicationFormUrl)
        {
            this.AdvertisementModel.applicationFormUrl = applicationFormUrl;
            return this;
        }

        public AdvertisementContentBuilder WithContactEmail(object contactEmail)
        {
            this.EnsureContactPropertyExists();
            this.AdvertisementModel.contact.email = contactEmail;
            return this;
        }

        public AdvertisementContentBuilder WithContactName(object contactName)
        {
            this.EnsureContactPropertyExists();
            this.AdvertisementModel.contact.name = contactName;
            return this;
        }

        public AdvertisementContentBuilder WithContactPhone(object contactPhone)
        {
            this.EnsureContactPropertyExists();
            this.AdvertisementModel.contact.phone = contactPhone;
            return this;
        }

        public AdvertisementContentBuilder WithEndApplicationUrl(object endApplicationUrl)
        {
            this.AdvertisementModel.endApplicationUrl = endApplicationUrl;
            return this;
        }

        public AdvertisementContentBuilder WithJobReference(object jobReference)
        {
            this.AdvertisementModel.jobReference = jobReference;
            return this;
        }

        public AdvertisementContentBuilder WithJobSummary(object jobSummary)
        {
            this.AdvertisementModel.jobSummary = jobSummary;
            return this;
        }

        public AdvertisementContentBuilder WithJobTitle(object jobTitle)
        {
            this.AdvertisementModel.jobTitle = jobTitle;
            return this;
        }

        public AdvertisementContentBuilder WithLocationAreaId(object areaId)
        {
            if (areaId == null)
            {
                if (PropertyExists(this.AdvertisementModel, "location"))
                {
                    TryRemoveProperty(this.AdvertisementModel.location, "areaId");
                }
            }
            else
            {
                this.EnsureLocationPropertyExists();
                this.AdvertisementModel.location.areaId = areaId;
            }
            return this;
        }

        public AdvertisementContentBuilder WithLocationId(object locationId)
        {
            if (locationId == null)
            {
                if (PropertyExists(this.AdvertisementModel, "location"))
                {
                    TryRemoveProperty(this.AdvertisementModel.location, "id");
                }
            }
            else
            {
                this.EnsureLocationPropertyExists();
                this.AdvertisementModel.location.id = locationId;
            }
            return this;
        }

        public AdvertisementContentBuilder WithGranularLocationCountry(object country)
        {
            this.EnsureGranularLocationPropertyExists();

            TryRemoveProperty(this.AdvertisementModel, "location");

            this.AdvertisementModel.granularLocation.country = country;
            return this;
        }

        public AdvertisementContentBuilder WithGranularLocationState(object state)
        {
            if (state == null)
            {
                if (PropertyExists(this.AdvertisementModel, "granularLocation"))
                {
                    TryRemoveProperty(this.AdvertisementModel.granularLocation, "state");
                }
            }
            else
            {
                this.EnsureGranularLocationPropertyExists();

                this.AdvertisementModel.granularLocation.state = state;
            }
            return this;
        }

        public AdvertisementContentBuilder WithGranularLocationCity(object city)
        {
            if (city == null)
            {
                if (PropertyExists(this.AdvertisementModel, "granularLocation"))
                {
                    TryRemoveProperty(this.AdvertisementModel.granularLocation, "city");
                }
            }
            else
            {
                this.EnsureGranularLocationPropertyExists();

                this.AdvertisementModel.granularLocation.city = city;
            }
            return this;
        }

        public AdvertisementContentBuilder WithGranularLocationPostCode(object postCode)
        {
            if (postCode == null)
            {
                if (PropertyExists(this.AdvertisementModel, "granularLocation"))
                {
                    TryRemoveProperty(this.AdvertisementModel.granularLocation, "postCode");
                }
            }
            else
            {
                this.EnsureGranularLocationPropertyExists();

                this.AdvertisementModel.granularLocation.postCode = postCode;
            }
            return this;
        }

        public AdvertisementContentBuilder WithGranularLocationOptions(params object[] options)
        {
            if (options == null)
            {
                if (PropertyExists(this.AdvertisementModel, "granularLocation"))
                {
                    TryRemoveProperty(this.AdvertisementModel.granularLocation, "options");
                }
            }
            else
            {
                this.EnsureGranularLocationPropertyExists();

                this.AdvertisementModel.granularLocation.options = options?.Clone<object[]>();
            }
            return this;
        }

        public AdvertisementContentBuilder WithRecruiterFullName(object recruiterFullName)
        {
            this.EnsureRecruiterPropertyExists();
            this.AdvertisementModel.recruiter.fullName = recruiterFullName;
            return this;
        }

        public AdvertisementContentBuilder WithRecruiterEmail(object recruiterEmail)
        {
            this.EnsureRecruiterPropertyExists();
            this.AdvertisementModel.recruiter.email = recruiterEmail;
            return this;
        }

        public AdvertisementContentBuilder WithRecruiterTeamName(object recruiterTeamName)
        {
            this.EnsureRecruiterPropertyExists();
            this.AdvertisementModel.recruiter.teamName = recruiterTeamName;
            return this;
        }

        public AdvertisementContentBuilder WithRequestCreationId(object creationId)
        {
            this.AdvertisementModel.creationId = creationId;
            return this;
        }

        public AdvertisementContentBuilder WithSalaryDetails(object details)
        {
            this.EnsureSalaryPropertyExists();
            this.AdvertisementModel.salary.details = details;
            return this;
        }

        public AdvertisementContentBuilder WithSalaryMaximum(object maximum)
        {
            this.EnsureSalaryPropertyExists();
            this.AdvertisementModel.salary.maximum = maximum;
            return this;
        }

        public AdvertisementContentBuilder WithSalaryMinimum(object minimum)
        {
            this.EnsureSalaryPropertyExists();
            this.AdvertisementModel.salary.minimum = minimum;
            return this;
        }

        public AdvertisementContentBuilder WithSalaryType(object salaryType)
        {
            this.EnsureSalaryPropertyExists();
            this.AdvertisementModel.salary.type = salaryType;
            return this;
        }

        public AdvertisementContentBuilder WithQuestionnaireId(object questionnaireId)
        {
            if (questionnaireId == null)
            {
                TryRemoveProperty(this.AdvertisementModel, "questionnaireId");
            }
            else
            {
                this.AdvertisementModel.questionnaireId = questionnaireId;
            }
            return this;
        }

        public AdvertisementContentBuilder WithScreenId(object screenId)
        {
            if (screenId == null)
            {
                TryRemoveProperty(this.AdvertisementModel, "screenId");
            }
            else
            {
                this.AdvertisementModel.screenId = screenId;
            }
            return this;
        }

        public AdvertisementContentBuilder WithSearchJobTitle(object searchJobTitle)
        {
            this.AdvertisementModel.searchJobTitle = searchJobTitle;
            return this;
        }

        public AdvertisementContentBuilder WithStandoutBullets(params object[] bullets)
        {
            this.EnsureStandoutPropertyExists();

            this.AdvertisementModel.standout.bullets = bullets?.Clone();
            return this;
        }

        public AdvertisementContentBuilder WithStandoutLogoId(object logoId)
        {
            this.EnsureStandoutPropertyExists();

            this.AdvertisementModel.standout.logoId = logoId;
            return this;
        }

        public AdvertisementContentBuilder WithSubclassificationId(object subclassificationId)
        {
            this.AdvertisementModel.subclassificationId = subclassificationId;
            return this;
        }

        public AdvertisementContentBuilder WithTemplateId(object id)
        {
            this.EnsureTemplatePropertyExists();

            this.AdvertisementModel.template.id = id;
            return this;
        }

        public AdvertisementContentBuilder WithTemplateItems(params KeyValuePair<object, object>[] templateItems)
        {
            this.EnsureTemplatePropertyExists();

            this.AdvertisementModel.template.items =
                templateItems?.Select(t => new { name = t.Key, value = t.Value }).ToArray();
            return this;
        }

        public AdvertisementContentBuilder WithVideoPosition(object videoPosition)
        {
            this.EnsureVideoPropertyExists();
            this.AdvertisementModel.video.position = videoPosition;
            return this;
        }

        public AdvertisementContentBuilder WithVideoUrl(object url)
        {
            this.EnsureVideoPropertyExists();
            this.AdvertisementModel.video.url = url;
            return this;
        }

        public AdvertisementContentBuilder WithWorkType(object workType)
        {
            this.AdvertisementModel.workType = workType;
            return this;
        }

        private void CreateOrRemoveThirdParties(object advertiserId, object agentId)
        {
            if (advertiserId == null && agentId == null)
            {
                TryRemoveProperty(this.AdvertisementModel, "thirdParties");
                return;
            }

            if (!PropertyExists(this.AdvertisementModel, "thirdParties"))
            {
                this.AdvertisementModel.thirdParties = new ExpandoObject();
            }

            if (advertiserId == null)
            {
                TryRemoveProperty(this.AdvertisementModel.thirdParties, "advertiserId");
            }
            else
            {
                this.AdvertisementModel.thirdParties.advertiserId = advertiserId;
            }

            if (agentId == null)
            {
                TryRemoveProperty(this.AdvertisementModel.thirdParties, "agentId");
            }
            else
            {
                this.AdvertisementModel.thirdParties.agentId = agentId;
            }
        }

        private void EnsureContactPropertyExists()
        {
            if (!((IDictionary<string, object>)this.AdvertisementModel).ContainsKey("contact"))
            {
                this.AdvertisementModel.contact = new ExpandoObject();
            }
        }

        private void EnsureLocationPropertyExists()
        {
            if (!((IDictionary<string, object>)this.AdvertisementModel).ContainsKey("location"))
            {
                this.AdvertisementModel.location = new ExpandoObject();
            }
        }

        private void EnsureGranularLocationPropertyExists()
        {
            if (!((IDictionary<string, object>)this.AdvertisementModel).ContainsKey("granularLocation"))
            {
                this.AdvertisementModel.granularLocation = new ExpandoObject();
            }
        }

        private void EnsureRecruiterPropertyExists()
        {
            if (!((IDictionary<string, object>)this.AdvertisementModel).ContainsKey("recruiter"))
            {
                this.AdvertisementModel.recruiter = new ExpandoObject();
            }
        }

        private void EnsureSalaryPropertyExists()
        {
            if (!((IDictionary<string, object>)this.AdvertisementModel).ContainsKey("salary"))
            {
                this.AdvertisementModel.salary = new ExpandoObject();
            }
        }

        private void EnsureStandoutPropertyExists()
        {
            if (!((IDictionary<string, object>)this.AdvertisementModel).ContainsKey("standout"))
            {
                this.AdvertisementModel.standout = new ExpandoObject();
            }
        }

        private void EnsureTemplatePropertyExists()
        {
            if (!((IDictionary<string, object>)this.AdvertisementModel).ContainsKey("template"))
            {
                this.AdvertisementModel.template = new ExpandoObject();
            }
        }

        private void EnsureVideoPropertyExists()
        {
            if (!((IDictionary<string, object>)this.AdvertisementModel).ContainsKey("video"))
            {
                this.AdvertisementModel.video = new ExpandoObject();
            }
        }

        private bool PropertyExists(dynamic model, string propertyName)
        {
            return ((IDictionary<string, object>)model).ContainsKey(propertyName);
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
    }
}