using System;
using System.Collections.Generic;
using System.Dynamic;

namespace SEEK.AdPostingApi.Client.Tests
{
    public class AdvertisementResponseContentBuilder : AdvertisementContentBuilder
    {
        public AdvertisementResponseContentBuilder(IBuilderInitializer initializer) : base(initializer)
        {
            this.WithExpiryDate(new DateTime(2015, 11, 6, 21, 19, 00, DateTimeKind.Utc));
        }

        public AdvertisementResponseContentBuilder WithExpiryDate(DateTime expiryDate)
        {
            this.AdvertisementModel.expiryDate = expiryDate;

            return this;
        }

        public AdvertisementResponseContentBuilder WithErrors(params object[] errors)
        {
            this.AdvertisementModel.errors = errors?.Clone<object[]>();

            return this;
        }

        public AdvertisementResponseContentBuilder WithLink(string linkName, object linkRef)
        {
            if (!((IDictionary<string, object>)this.AdvertisementModel).ContainsKey("_links"))
            {
                this.AdvertisementModel._links = new ExpandoObject();
            }

            dynamic href = new ExpandoObject();
            href.href = linkRef;
            ((IDictionary<string, object>)this.AdvertisementModel._links).Add(linkName, href);

            return this;
        }

        public AdvertisementResponseContentBuilder WithWarnings(params object[] warnings)
        {
            this.AdvertisementModel.warnings = warnings?.Clone<object[]>();

            return this;
        }

        public AdvertisementResponseContentBuilder WithState(string state)
        {
            this.AdvertisementModel.state = state;

            return this;
        }
    }
}