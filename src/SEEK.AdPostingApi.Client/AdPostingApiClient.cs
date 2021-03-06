﻿using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.Client
{
    public class AdPostingApiClient : IAdPostingApiClient
    {
        private readonly IOAuth2TokenClient _tokenClient;
        private IndexResource _indexResource;
        private readonly Lazy<Task> _ensureIndexResourceInitialised;
        private readonly Hal.Client _client;

        public AdPostingApiClient(string id, string secret, Environment env = Environment.Production, DelegatingHandler customMessageHandler = null)
            : this(env.GetAttribute<EnvironmentUrlAttribute>().Uri, new OAuth2TokenClient(id, secret), customMessageHandler)
        {
        }

        public AdPostingApiClient(string id, string secret, Uri adPostingUri, DelegatingHandler customMessageHandler = null)
            : this(adPostingUri, new OAuth2TokenClient(id, secret), customMessageHandler)
        {
        }

        public AdPostingApiClient(IOAuth2TokenClient tokenClient, Uri adPostingUri, DelegatingHandler customMessageHandler = null)
            : this(adPostingUri, tokenClient, customMessageHandler)
        {
        }

        internal AdPostingApiClient(Uri adPostingUri, IOAuth2TokenClient tokenClient, DelegatingHandler customMessageHandler = null)
        {
            this._ensureIndexResourceInitialised = new Lazy<Task>(() => this.InitialiseIndexResource(adPostingUri), LazyThreadSafetyMode.ExecutionAndPublication);
            this._tokenClient = tokenClient;

            HttpMessageHandler pipeline = new HttpClientHandler()
                // Add the custom handler as the "last" handler only if it was given
                .DecorateWith(customMessageHandler, customMessageHandler != null)
                .DecorateWith(new OAuthMessageHandler(tokenClient))
                .DecorateWith(new AdPostingApiMessageHandler());

            this._client = new Hal.Client(new HttpClient(pipeline));
        }

        private Task EnsureIndexResourceInitialised()
        {
            return this._ensureIndexResourceInitialised.Value;
        }

        internal async Task InitialiseIndexResource(Uri adPostingUri)
        {
            this._indexResource = await this.GetIndexResourceAsync(adPostingUri, this._client);
        }

        protected internal virtual async Task<IndexResource> GetIndexResourceAsync(Uri adPostingUri, Hal.Client halClient)
        {
            return await halClient.GetResourceAsync<IndexResource, AdvertisementErrorResponse>(adPostingUri);
        }

        public async Task<AdvertisementResource> CreateAdvertisementAsync(Advertisement advertisement)
        {
            if (advertisement == null)
            {
                throw new ArgumentNullException(nameof(advertisement));
            }

            await this.EnsureIndexResourceInitialised();

            return await this._indexResource.CreateAdvertisementAsync(advertisement);
        }

        public async Task<AdvertisementResource> ExpireAdvertisementAsync(Guid advertisementId)
        {
            await this.EnsureIndexResourceInitialised();

            return await this.ExpireAdvertisementAsync(this._indexResource.GenerateAdvertisementUri(advertisementId));
        }

        public async Task<AdvertisementResource> ExpireAdvertisementAsync(Uri uri)
        {
            return await this._client.PatchResourceAsync<AdvertisementResource, ExpireAdvertisementJsonPatch>(uri, new ExpireAdvertisementJsonPatch());
        }

        public async Task<AdvertisementResource> GetAdvertisementAsync(Guid advertisementId)
        {
            await this.EnsureIndexResourceInitialised();

            return await this.GetAdvertisementAsync(this._indexResource.GenerateAdvertisementUri(advertisementId));
        }

        public async Task<AdvertisementResource> GetAdvertisementAsync(Uri uri)
        {
            return await this._client.GetResourceAsync<AdvertisementResource, AdvertisementErrorResponse>(uri);
        }

        [Obsolete("The returned status will always be completed. All validation is done upfront and the advertisement will not fail once successfully submitted.")]
        public async Task<ProcessingStatus> GetAdvertisementStatusAsync(Guid advertisementId)
        {
            await this.EnsureIndexResourceInitialised();

            return await this.GetAdvertisementStatusAsync(this._indexResource.GenerateAdvertisementUri(advertisementId));
        }

        [Obsolete("The returned status will always be completed. All validation is done upfront and the advertisement will not fail once successfully submitted.")]
        public async Task<ProcessingStatus> GetAdvertisementStatusAsync(Uri uri)
        {
            HttpResponseHeaders httpResponseHeaders = await this._client.HeadResourceAsync<AdvertisementResource>(uri);

            return (ProcessingStatus)Enum.Parse(typeof(ProcessingStatus), httpResponseHeaders.GetValues("Processing-Status").Single());
        }

        public async Task<AdvertisementSummaryPageResource> GetAllAdvertisementsAsync(string advertiserId = null)
        {
            await this.EnsureIndexResourceInitialised();

            return await this._indexResource.GetAllAdvertisements(advertiserId);
        }

        public async Task<AdvertisementResource> UpdateAdvertisementAsync(Guid advertisementId, Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException(nameof(advertisement));

            await this.EnsureIndexResourceInitialised();

            return await this.UpdateAdvertisementAsync(this._indexResource.GenerateAdvertisementUri(advertisementId), advertisement);
        }

        public async Task<AdvertisementResource> UpdateAdvertisementAsync(Uri uri, Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException(nameof(advertisement));

            return await this._client.PutResourceAsync<AdvertisementResource, Advertisement>(uri, advertisement);
        }

        public async Task<TemplateSummaryListResource> GetAllTemplatesAsync(Uri uri)
        {
            return await this._client.GetResourceAsync<TemplateSummaryListResource, AdvertisementErrorResponse>(uri);
        }

        public async Task<TemplateSummaryListResource> GetAllTemplatesAsync(string advertiserId = null, int? after = null)
        {
            await this.EnsureIndexResourceInitialised();

            return await this._indexResource.GetAllTemplates(advertiserId, after);
        }

        public async Task<LogoSummaryListResource> GetAllLogosAsync(Uri uri)
        {
            return await this._client.GetResourceAsync<LogoSummaryListResource, AdvertisementErrorResponse>(uri);
        }

        public async Task<LogoSummaryListResource> GetAllLogosAsync(string advertiserId = null)
        {
            await this.EnsureIndexResourceInitialised();

            return await this._indexResource.GetAllLogos(advertiserId);
        }

        public void Dispose()
        {
            this._tokenClient.Dispose();
            this._client.Dispose();
        }
    }
}