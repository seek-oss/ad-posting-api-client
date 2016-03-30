using System.Collections.Generic;
using System.Linq;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.SampleConsumer.Tests
{
    public class AllFieldsInitializer : IBuilderInitializer
    {
        private readonly IBuilderInitializer _minimumFieldsInitializer = new MinimumFieldsInitializer();

        public void Initialize(AdvertisementContentBuilder builder)
        {
            _minimumFieldsInitializer.Initialize(builder);

            builder
                .WithAgentId(GetDefaultAgentId())
                .WithAdvertisementType(AdvertisementType.StandOut.ToString())
                .WithSalaryDetails(GetDefaultSalaryDetails())
                .WithContactName(GetDefaultContactName())
                .WithContactEmail(GetDefaultContactEmail())
                .WithContactPhone(GetDefaultContactPhone())
                .WithVideoUrl(GetDefaultVideoUrl())
                .WithVideoPosition(GetDefaultVideoPosition().ToString())
                .WithApplicationEmail(GetDefaultApplicationEmail())
                .WithApplicationFormUrl(GetDefaultApplicationFormUrl())
                .WithEndApplicationUrl(GetDefaultEndApplicationUrl())
                .WithScreenId(GetDefaultScreenId())
                .WithJobReference(GetDefaultJobReference())
                .WithTemplateId(GetDefaultTemplateId())
                .WithTemplateItems(
                    new KeyValuePair<object, object>(GetDefaultTemplateItemName(1), GetDefaultTemplateItemValue(1)),
                    new KeyValuePair<object, object>(GetDefaultTemplateItemName(2), GetDefaultTemplateItemValue(2)))
                .WithStandoutLogoId(GetDefaultLogoId())
                .WithStandoutBullets(GetDefaultStandoutBullet(1), GetDefaultStandoutBullet(2), GetDefaultStandoutBullet(3))
                .WithAdditionalProperties(GetDefaultAdditionalPropertiesAsObjects());
        }

        public void Initialize(AdvertisementModelBuilder builder)
        {
            _minimumFieldsInitializer.Initialize(builder);

            builder
                .WithAgentId(GetDefaultAgentId())
                .WithAdvertisementType(AdvertisementType.StandOut)
                .WithSalaryDetails(GetDefaultSalaryDetails())
                .WithContactName(GetDefaultContactName())
                .WithContactEmail(GetDefaultContactEmail())
                .WithContactPhone(GetDefaultContactPhone())
                .WithVideoUrl(GetDefaultVideoUrl())
                .WithVideoPosition(GetDefaultVideoPosition())
                .WithApplicationEmail(GetDefaultApplicationEmail())
                .WithApplicationFormUrl(GetDefaultApplicationFormUrl())
                .WithEndApplicationUrl(GetDefaultEndApplicationUrl())
                .WithScreenId(GetDefaultScreenId())
                .WithJobReference(GetDefaultJobReference())
                .WithTemplateId(GetDefaultTemplateId())
                .WithTemplateItems(
                    new TemplateItemModel { Name = GetDefaultTemplateItemName(1), Value = GetDefaultTemplateItemValue(1) },
                    new TemplateItemModel { Name = GetDefaultTemplateItemName(2), Value = GetDefaultTemplateItemValue(2) })
                .WithStandoutLogoId(GetDefaultLogoId())
                .WithStandoutBullets(GetDefaultStandoutBullet(1), GetDefaultStandoutBullet(2), GetDefaultStandoutBullet(3))
                .WithAdditionalProperties(GetDefaultAdditionalProperties());
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
            return GetDefaultAdditionalProperties().Select(a => a.ToString()).ToArray<object>();
        }
    }
}