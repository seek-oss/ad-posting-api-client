using System.Collections.Generic;
using System.Dynamic;

namespace SEEK.AdPostingApi.Client.Tests
{
    public class AdvertisementSummaryContentBuilder
    {
        private readonly dynamic _advertisementModel = new ExpandoObject();

        public AdvertisementSummaryContentBuilder()
        {
            this
                .WithAdvertiserId("1")
                .WithJobTitle("Exciting Senior Developer role in a great CBD location. Great $$$")
                .WithJobReference("JobReference1");
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

        public AdvertisementSummaryContentBuilder WithAdvertiserId(object advertiserId)
        {
            if (advertiserId == null)
            {
                TryRemoveProperty(_advertisementModel, "advertiserId");
            }
            else
            {
                _advertisementModel.advertiserId = advertiserId;
            }

            return this;
        }

        public AdvertisementSummaryContentBuilder WithJobTitle(object jobTitle)
        {
            _advertisementModel.jobTitle = jobTitle;
            return this;
        }

        public AdvertisementSummaryContentBuilder WithJobReference(object jobReference)
        {
            _advertisementModel.jobReference = jobReference;
            return this;
        }

        public AdvertisementSummaryContentBuilder WithResponseLink(string linkName, object linkRef)
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

        public dynamic Build()
        {
            return ((IDictionary<string, object>)_advertisementModel).Clone();
        }
    }
}