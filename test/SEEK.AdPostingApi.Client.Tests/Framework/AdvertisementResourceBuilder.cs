using System;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class AdvertisementResourceBuilder : AdvertisementResourceBuilder<AdvertisementResource>
    {
        public AdvertisementResourceBuilder(IBuilderInitializer initializer = null) : base(initializer)
        {
        }
    }

    public class AdvertisementResourceBuilder<TAdvertisementResource> :
        AdvertisementModelBuilder<TAdvertisementResource> where TAdvertisementResource : AdvertisementResource, new()
    {
        private AdvertisementError[] _errors;
        private DateTime _expiryDate;
        private Guid _id;
        private Links _links;
        private ProcessingStatus _processingStatus;
        private AdvertisementState _state;
        private AdvertisementError[] _warnings;

        protected AdvertisementResourceBuilder(IBuilderInitializer initializer = null) : base(initializer)
        {
            this.WithId(Guid.NewGuid());
            this.WithState(AdvertisementState.Open);
            this.WithExpiryDate(new DateTime(2015, 11, 7, 12, 59, 59, DateTimeKind.Utc));
        }

        public override TAdvertisementResource Build()
        {
            TAdvertisementResource advertisementResource = base.Build();

            advertisementResource.Id = this._id;
            advertisementResource.Errors = this._errors;
            advertisementResource.ExpiryDate = this._expiryDate;
            advertisementResource.Links = this._links;
            advertisementResource.ProcessingStatus = this._processingStatus;
            advertisementResource.State = this._state;
            advertisementResource.Warnings = this._warnings;
            advertisementResource.RequestId = "PactRequestId";

            return advertisementResource;
        }

        public AdvertisementResourceBuilder<TAdvertisementResource> WithErrors(params AdvertisementError[] errors)
        {
            this._errors = errors;

            return this;
        }

        public AdvertisementResourceBuilder<TAdvertisementResource> WithExpiryDate(DateTime expiryDate)
        {
            this._expiryDate = expiryDate;

            return this;
        }

        public AdvertisementResourceBuilder<TAdvertisementResource> WithId(Guid id)
        {
            this._id = id;

            return this;
        }

        public AdvertisementResourceBuilder<TAdvertisementResource> WithLinks(string advertisementId)
        {
            this._links = new Links(new Uri("http://localhost:8893/"))
            {
                { "self", new Link { Href = $"/advertisement/{advertisementId}" } },
                { "view", new Link { Href = $"/advertisement/{advertisementId}/view" } }
            };

            return this;
        }

        public AdvertisementResourceBuilder<TAdvertisementResource> WithProcessingStatus(ProcessingStatus processingStatus)
        {
            this._processingStatus = processingStatus;

            return this;
        }

        public AdvertisementResourceBuilder<TAdvertisementResource> WithState(AdvertisementState state)
        {
            this._state = state;

            return this;
        }

        public AdvertisementResourceBuilder<TAdvertisementResource> WithWarnings(params AdvertisementError[] warnings)
        {
            this._warnings = warnings;

            return this;
        }
    }
}