﻿using System.Linq;
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
                .WithContactDetails(GetDefaultContactDetails())
                .WithVideoUrl(GetDefaultVideoUrl())
                .WithVideoPosition(GetDefaultVideoPosition().ToString())
                .WithApplicationEmail(GetDefaultApplicationEmail())
                .WithApplicationFormUrl(GetDefaultApplicationFormUrl())
                .WithScreenId(GetDefaultScreenId())
                .WithJobReference(GetDefaultJobReference())
                .WithTemplateId(GetDefaultTemplateId())
                .WithTemplateItem(GetDefaultTemplateItemName(1), GetDefaultTemplateItemValue(1))
                .WithTemplateItem(GetDefaultTemplateItemName(2), GetDefaultTemplateItemValue(2))
                .WithStandoutLogoId(GetDefaultLogoId())
                .WithStandoutBullets(GetDefaultStandoutBullet(1), GetDefaultStandoutBullet(2), GetDefaultStandoutBullet(3))
                .WithSeekCodes(GetDefaultSeekCodesAsObjects())
                .WithAdditionalProperties(GetDefaultAdditionalPropertiesAsObjects());
        }

        public void Initialize(AdvertisementModelBuilder builder)
        {
            _minimumFieldsInitializer.Initialize(builder);

            builder
                .WithAgentId(GetDefaultAgentId())
                .WithAdvertisementType(AdvertisementType.StandOut)
                .WithSalaryDetails(GetDefaultSalaryDetails())
                .WithContactDetails(GetDefaultContactDetails())
                .WithVideoUrl(GetDefaultVideoUrl())
                .WithVideoPosition(GetDefaultVideoPosition())
                .WithApplicationEmail(GetDefaultApplicationEmail())
                .WithApplicationFormUrl(GetDefaultApplicationFormUrl())
                .WithScreenId(GetDefaultScreenId())
                .WithJobReference(GetDefaultJobReference())
                .WithTemplateId(GetDefaultTemplateId())
                .WithTemplateItem(GetDefaultTemplateItemName(1), GetDefaultTemplateItemValue(1))
                .WithTemplateItem(GetDefaultTemplateItemName(2), GetDefaultTemplateItemValue(2))
                .WithStandoutLogoId(GetDefaultLogoId())
                .WithStandoutBullets(GetDefaultStandoutBullet(1), GetDefaultStandoutBullet(2), GetDefaultStandoutBullet(3))
                .WithSeekCodes(GetDefaultSeekCodes())
                .WithAdditionalProperties(GetDefaultAdditionalProperties());
        }

        private string GetDefaultAgentId()
        {
            return "agentA";
        }

        private string GetDefaultSalaryDetails()
        {
            return "We will pay you";
        }

        private string GetDefaultContactDetails()
        {
            return "Call me";
        }

        private string GetDefaultVideoUrl()
        {
            return "https://www.youtube.com/watch?v=dVDk7PXNXB8";
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
            return "http://applicationform/";
        }

        private int GetDefaultScreenId()
        {
            return 20;
        }

        private string GetDefaultJobReference()
        {
            return "JOB1234";
        }

        private int GetDefaultTemplateId()
        {
            return 99;
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
            return 333;
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

        private string[] GetDefaultSeekCodes()
        {
            return new[] { "SK010001Z", "SK010010z", "SK0101OOZ", "SK910101A" };
        }

        private object[] GetDefaultSeekCodesAsObjects()
        {
            return GetDefaultSeekCodes().ToArray<object>();
        }

        private AdditionalPropertyType[] GetDefaultAdditionalProperties()
        {
            return new[] { AdditionalPropertyType.ResidentsOnly };
        }

        private object[] GetDefaultAdditionalPropertiesAsObjects()
        {
            return GetDefaultAdditionalProperties().Select(a => a.ToString()).ToArray<object>();
        }
    }
}