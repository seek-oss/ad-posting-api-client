using System;
using System.Collections.Generic;
using System.Dynamic;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class LogoSummaryResponseContentBuilder
    {
        private readonly dynamic _logoSummaryModel = new ExpandoObject();

        public LogoSummaryResponseContentBuilder WithId(string id)
        {
            this._logoSummaryModel.logoId = id;
            return this;
        }

        public LogoSummaryResponseContentBuilder WithName(string name)
        {
            this._logoSummaryModel.name = name;
            return this;
        }

        public LogoSummaryResponseContentBuilder WithLogoState(string state)
        {
            this._logoSummaryModel.state = state;
            return this;
        }

        public LogoSummaryResponseContentBuilder WithAdvertiserId(string advertiserId)
        {
            this._logoSummaryModel.advertiserId = advertiserId;
            return this;
        }

        public LogoSummaryResponseContentBuilder WithViewLink(string linkName, string linkRef)
        {
            if (!((IDictionary<string, object>)this._logoSummaryModel).ContainsKey("_links"))
            {
                this._logoSummaryModel._links = new ExpandoObject();
            }

            dynamic href = new ExpandoObject();
            href.href = linkRef;
            ((IDictionary<string, object>)this._logoSummaryModel._links).Add(linkName, href);

            return this;
        }

        public LogoSummaryResponseContentBuilder WithUpdatedDateTime(DateTime updatedDateTime)
        {
            this._logoSummaryModel.updatedDateTime = updatedDateTime;
            return this;
        }

        public dynamic Build()
        {
            return ((IDictionary<string, object>)this._logoSummaryModel).Clone();
        }
    }
}