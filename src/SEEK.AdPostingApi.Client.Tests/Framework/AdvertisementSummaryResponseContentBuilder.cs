using System;
using System.Collections.Generic;
using System.Dynamic;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class AdvertisementSummaryResponseContentBuilder
    {
        private readonly dynamic _advertisementModel = new ExpandoObject();

        public AdvertisementSummaryResponseContentBuilder()
        {
            this
                .WithId(Guid.NewGuid().ToString())
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

        public AdvertisementSummaryResponseContentBuilder WithAdvertiserId(object advertiserId)
        {
            if (advertiserId == null)
            {
                TryRemoveProperty(this._advertisementModel, "advertiserId");
            }
            else
            {
                this._advertisementModel.advertiserId = advertiserId;
            }

            return this;
        }

        public AdvertisementSummaryResponseContentBuilder WithId(object id)
        {
            this._advertisementModel.id = id;

            return this;
        }

        public AdvertisementSummaryResponseContentBuilder WithJobTitle(object jobTitle)
        {
            this._advertisementModel.jobTitle = jobTitle;
            return this;
        }

        public AdvertisementSummaryResponseContentBuilder WithJobReference(object jobReference)
        {
            this._advertisementModel.jobReference = jobReference;
            return this;
        }

        public AdvertisementSummaryResponseContentBuilder WithResponseLink(string linkName, object linkRef)
        {
            if (!((IDictionary<string, object>)this._advertisementModel).ContainsKey("_links"))
            {
                this._advertisementModel._links = new ExpandoObject();
            }

            dynamic href = new ExpandoObject();
            href.href = linkRef;
            ((IDictionary<string, object>)this._advertisementModel._links).Add(linkName, href);

            return this;
        }

        public dynamic Build()
        {
            return ((IDictionary<string, object>)this._advertisementModel).Clone();
        }
    }
}