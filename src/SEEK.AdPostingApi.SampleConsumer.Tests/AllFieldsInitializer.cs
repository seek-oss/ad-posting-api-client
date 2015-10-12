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
                .WithContactDetails(GetDefaultContactDetails())
                .WithVideo(GetDefaultVideoUrl(), GetDefaultVideoPosition().ToString())
                .WithApplicationEmail(GetDefaultApplicationEmail())
                .WithApplicationFormUrl(GetDefaultApplicationFormUrl())
                .WithScreenId(GetDefaultScreenId())
                .WithJobReference(GetDefaultJobReference())
                .WithTemplate(GetDefaultTemplateId())
                .WithTemplateItem(GetDefaultTemplateItemName(1), GetDefaultTemplateItemValue(1))
                .WithTemplateItem(GetDefaultTemplateItemName(2), GetDefaultTemplateItemValue(2))
                .WithStandoutLogoId(GetDefaultLogoId())
                .WithStandoutBullets(GetDefaultStandoutBullet(1), GetDefaultStandoutBullet(2), GetDefaultStandoutBullet(3))
                .WithSeekCodes(GetDefaultSeekCodesAsObjects());
        }

        public void Initialize(AdvertisementModelBuilder builder)
        {
            _minimumFieldsInitializer.Initialize(builder);

            builder
                .WithAgentId(GetDefaultAgentId())
                .WithAdvertisementType(AdvertisementType.StandOut)
                .WithSalaryDetails(GetDefaultSalaryDetails())
                .WithContactDetails(GetDefaultContactDetails())
                .WithVideo(GetDefaultVideoUrl(), GetDefaultVideoPosition())
                .WithApplicationEmail(GetDefaultApplicationEmail())
                .WithApplicationFormUrl(GetDefaultApplicationFormUrl())
                .WithScreenId(GetDefaultScreenId())
                .WithJobReference(GetDefaultJobReference())
                .WithTemplate(GetDefaultTemplateId())
                .WithTemplateItem(GetDefaultTemplateItemName(1), GetDefaultTemplateItemValue(1))
                .WithTemplateItem(GetDefaultTemplateItemName(2), GetDefaultTemplateItemValue(2))
                .WithStandoutLogoId(GetDefaultLogoId())
                .WithStandoutBullets(GetDefaultStandoutBullet(1), GetDefaultStandoutBullet(2), GetDefaultStandoutBullet(3))
                .WithSeekCodes(GetDefaultSeekCodes());
        }

        private string GetDefaultAgentId()
        {
            return "agentA";
        }

        private string GetDefaultSalaryDetails()
        {
            return "Huge bonus";
        }

        private string GetDefaultContactDetails()
        {
            return "0412345678";
        }

        private string GetDefaultVideoUrl()
        {
            return "http://www.youtube.com/v/abc";
        }

        private VideoPosition GetDefaultVideoPosition()
        {
            return VideoPosition.Above;
        }

        private string GetDefaultApplicationEmail()
        {
            return "me@contactme.com.au";
        }

        private string GetDefaultApplicationFormUrl()
        {
            return "http://FakeATS.com.au";
        }

        private int GetDefaultScreenId()
        {
            return 100;
        }

        private string GetDefaultJobReference()
        {
            return "REF1234";
        }

        private int GetDefaultTemplateId()
        {
            return 43496;
        }

        private string GetDefaultTemplateItemName(int itemNumber)
        {
            return $"template{itemNumber}";
        }

        private string GetDefaultTemplateItemValue(int itemNumber)
        {
            return $"value{itemNumber}";
        }

        private int GetDefaultLogoId()
        {
            return 39;
        }

        private string GetDefaultStandoutBullet(int itemNumber)
        {
            return $"standout bullet {itemNumber}";
        }

        private string[] GetDefaultSeekCodes()
        {
            return new[] { "SK840239A", "SK4232A", "SK23894023A", "SK23432A", "SK238429A" };
        }

        private object[] GetDefaultSeekCodesAsObjects()
        {
            return GetDefaultSeekCodes().ToArray<object>();
        }
    }
}
