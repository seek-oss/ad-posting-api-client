using System.Collections.Generic;
using System.Dynamic;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class TemplateDescriptionResponseContentBuilder
    {
        private readonly dynamic _templateDescriptionModel = new ExpandoObject();

        public TemplateDescriptionResponseContentBuilder WithId(object id)
        {
            this._templateDescriptionModel.id = id;
            return this;
        }

        public TemplateDescriptionResponseContentBuilder WithName(object name)
        {
            this._templateDescriptionModel.name = name;
            return this;
        }

        public TemplateDescriptionResponseContentBuilder WithTemplateState(object state)
        {
            this._templateDescriptionModel.state = state;
            return this;
        }

        public TemplateDescriptionResponseContentBuilder WithAdvertiserId(object advertiserId)
        {
            this._templateDescriptionModel.advertiserId = advertiserId;
            return this;
        }

        public TemplateDescriptionResponseContentBuilder WithUpdateDateTime(object updateDateTime)
        {
            this._templateDescriptionModel.updateDateTime = updateDateTime;
            return this;
        }

        public dynamic Build()
        {
            return ((IDictionary<string, object>)this._templateDescriptionModel).Clone();
        }
    }
}