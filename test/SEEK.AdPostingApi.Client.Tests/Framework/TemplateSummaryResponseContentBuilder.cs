﻿using System.Collections.Generic;
using System.Dynamic;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class TemplateSummaryResponseContentBuilder
    {
        private readonly dynamic _templateSummaryModel = new ExpandoObject();

        public TemplateSummaryResponseContentBuilder WithId(object id)
        {
            this._templateSummaryModel.id = id;
            return this;
        }

        public TemplateSummaryResponseContentBuilder WithName(object name)
        {
            this._templateSummaryModel.name = name;
            return this;
        }

        public TemplateSummaryResponseContentBuilder WithTemplateState(object state)
        {
            this._templateSummaryModel.state = state;
            return this;
        }

        public TemplateSummaryResponseContentBuilder WithAdvertiserId(object advertiserId)
        {
            this._templateSummaryModel.advertiserId = advertiserId;
            return this;
        }

        public TemplateSummaryResponseContentBuilder WithUpdatedDateTime(object updatedDateTime)
        {
            this._templateSummaryModel.updatedDateTime = updatedDateTime;
            return this;
        }

        public TemplateSummaryResponseContentBuilder WithThumbnailLink(string linkName, string linkRef)
        {
            if (!((IDictionary<string, object>)this._templateSummaryModel).ContainsKey("_links"))
            {
                this._templateSummaryModel._links = new ExpandoObject();
            }

            dynamic href = new ExpandoObject();
            href.href = linkRef;
            ((IDictionary<string, object>)this._templateSummaryModel._links).Add(linkName, href);

            return this;
        }

        public dynamic Build()
        {
            return ((IDictionary<string, object>)this._templateSummaryModel).Clone();
        }
    }
}