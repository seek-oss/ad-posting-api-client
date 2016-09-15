using System.Collections.Generic;
using System.Linq;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class AllFieldsInitializer : IBuilderInitializer
    {
        private readonly LocationType _locationType;
        private readonly IBuilderInitializer _minimumFieldsInitializer = new MinimumFieldsInitializer();

        public AllFieldsInitializer(LocationType locationType = LocationType.UseLocation)
        {
            this._locationType = locationType;
        }

        public void Initialize(AdvertisementContentBuilder builder)
        {
            this._minimumFieldsInitializer.Initialize(builder);

            builder
                .WithSearchJobTitle(this.GetDefaultSearchJobTitle())
                .WithAgentId(this.GetDefaultAgentId())
                .WithAdvertisementType(AdvertisementType.StandOut.ToString())
                .WithSalaryDetails(this.GetDefaultSalaryDetails())
                .WithContactName(this.GetDefaultContactName())
                .WithContactEmail(this.GetDefaultContactEmail())
                .WithContactPhone(this.GetDefaultContactPhone())
                .WithVideoUrl(this.GetDefaultVideoUrl())
                .WithVideoPosition(this.GetDefaultVideoPosition().ToString())
                .WithApplicationEmail(this.GetDefaultApplicationEmail())
                .WithApplicationFormUrl(this.GetDefaultApplicationFormUrl())
                .WithEndApplicationUrl(this.GetDefaultEndApplicationUrl())
                .WithScreenId(this.GetDefaultScreenId())
                .WithJobReference(this.GetDefaultJobReference())
                .WithAgentJobReference(this.GetDefaultAgentJobReference())
                .WithTemplateId(this.GetDefaultTemplateId())
                .WithTemplateItems(
                    new KeyValuePair<object, object>(this.GetDefaultTemplateItemName(1),
                        this.GetDefaultTemplateItemValue(1)),
                    new KeyValuePair<object, object>(this.GetDefaultTemplateItemName(2),
                        this.GetDefaultTemplateItemValue(2)))
                .WithStandoutLogoId(this.GetDefaultLogoId())
                .WithStandoutBullets(this.GetDefaultStandoutBullet(1), this.GetDefaultStandoutBullet(2),
                    this.GetDefaultStandoutBullet(3))
                .WithAdditionalProperties(this.GetDefaultAdditionalPropertiesAsObjects())
                .WithRecruiterTeamName(this.GetDefaultRecruiterTeamName());

            if (_locationType == LocationType.UseGranularLocation)
            {
                builder
                    .WithLocationId(null)
                    .WithLocationAreaId(null)
                    .WithGranularLocationCountry(this.GetDefaultGranularLocationCountry())
                    .WithGranularLocationState(this.GetDefaultGranularLocationState())
                    .WithGranularLocationCity(this.GetDefaultGranularLocationCity())
                    .WithGranularLocationPostCode(this.GetDefaultGranularLocationPostCode());
            }
        }

        public void Initialize<TAdvertisement>(AdvertisementModelBuilder<TAdvertisement> builder)
            where TAdvertisement : Advertisement, new()
        {
            this._minimumFieldsInitializer.Initialize(builder);

            builder
                .WithSearchJobTitle(this.GetDefaultSearchJobTitle())
                .WithAgentId(this.GetDefaultAgentId())
                .WithAdvertisementType(AdvertisementType.StandOut)
                .WithSalaryDetails(this.GetDefaultSalaryDetails())
                .WithContactName(this.GetDefaultContactName())
                .WithContactEmail(this.GetDefaultContactEmail())
                .WithContactPhone(this.GetDefaultContactPhone())
                .WithVideoUrl(this.GetDefaultVideoUrl())
                .WithVideoPosition(this.GetDefaultVideoPosition())
                .WithApplicationEmail(this.GetDefaultApplicationEmail())
                .WithApplicationFormUrl(this.GetDefaultApplicationFormUrl())
                .WithEndApplicationUrl(this.GetDefaultEndApplicationUrl())
                .WithScreenId(this.GetDefaultScreenId())
                .WithJobReference(this.GetDefaultJobReference())
                .WithAgentJobReference(this.GetDefaultAgentJobReference())
                .WithTemplateId(this.GetDefaultTemplateId())
                .WithTemplateItems(
                    new TemplateItem
                    {
                        Name = this.GetDefaultTemplateItemName(1),
                        Value = this.GetDefaultTemplateItemValue(1)
                    },
                    new TemplateItem
                    {
                        Name = this.GetDefaultTemplateItemName(2),
                        Value = this.GetDefaultTemplateItemValue(2)
                    })
                .WithStandoutLogoId(this.GetDefaultLogoId())
                .WithStandoutBullets(this.GetDefaultStandoutBullet(1), this.GetDefaultStandoutBullet(2),
                    this.GetDefaultStandoutBullet(3))
                .WithAdditionalProperties(this.GetDefaultAdditionalProperties())
                .WithRecruiterTeamName(this.GetDefaultRecruiterTeamName());

            if (_locationType == LocationType.UseGranularLocation)
            {
                builder
                    .WithLocationArea(null)
                    .WithGranularLocationCountry(this.GetDefaultGranularLocationCountry())
                    .WithGranularLocationState(this.GetDefaultGranularLocationState())
                    .WithGranularLocationCity(this.GetDefaultGranularLocationCity())
                    .WithGranularLocationPostCode(this.GetDefaultGranularLocationPostCode());
            }
        }

        private string GetDefaultSearchJobTitle()
        {
            return "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies";
        }

        private string GetDefaultAgentId()
        {
            return "385";
        }

        private string GetDefaultSalaryDetails()
        {
            return "We will pay you";
        }

        private string GetDefaultContactName()
        {
            return "Contact name";
        }

        private string GetDefaultContactPhone()
        {
            return "+1 (123) 456 7889";
        }

        private string GetDefaultContactEmail()
        {
            return "qwert@asdf.com";
        }

        private string GetDefaultVideoUrl()
        {
            return "https://www.youtube.com/embed/dVDk7PXNXB8";
        }

        private VideoPosition GetDefaultVideoPosition()
        {
            return VideoPosition.Above;
        }

        private string GetDefaultApplicationEmail()
        {
            return "asdf@asdf.com";
        }

        private string GetDefaultApplicationFormUrl()
        {
            return "http://apply.com/";
        }

        private string GetDefaultEndApplicationUrl()
        {
            return "http://endform.com/";
        }

        private int GetDefaultScreenId()
        {
            return 1;
        }

        private string GetDefaultJobReference()
        {
            return "JOB1234";
        }

        private string GetDefaultAgentJobReference()
        {
            return "AGENTJOB1234";
        }

        private int GetDefaultTemplateId()
        {
            return 1;
        }

        private string GetDefaultTemplateItemName(int itemNumber)
        {
            return $"Template Line {itemNumber}";
        }

        private string GetDefaultTemplateItemValue(int itemNumber)
        {
            return $"Template Value {itemNumber}";
        }

        private int GetDefaultLogoId()
        {
            return 1;
        }

        private string GetDefaultStandoutBullet(int itemNumber)
        {
            switch (itemNumber)
            {
                case 1:
                    return "Uzi";

                case 2:
                    return "Remington Model";

                case 3:
                    return "AK-47";

                default:
                    return $"Standout bullet {itemNumber}";
            }
        }

        private AdditionalPropertyType[] GetDefaultAdditionalProperties()
        {
            return new[] { AdditionalPropertyType.ResidentsOnly, AdditionalPropertyType.Graduate };
        }

        private object[] GetDefaultAdditionalPropertiesAsObjects()
        {
            return this.GetDefaultAdditionalProperties().Select(a => a.ToString()).ToArray<object>();
        }

        private string GetDefaultGranularLocationCountry()
        {
            return "Australia";
        }

        private string GetDefaultGranularLocationState()
        {
            return "Victoria";
        }

        private string GetDefaultGranularLocationCity()
        {
            return "Melbourne";
        }

        private string GetDefaultGranularLocationPostCode()
        {
            return "3000";
        }

        private string GetDefaultRecruiterTeamName()
        {
            return "Recruiter Team Name";
        }
    }
}