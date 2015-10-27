using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.Client
{
    public class AdPostingApiClient : Hal.Client, IAdPostingApiClient
    {
        private readonly IOAuth2TokenClient _tokenClient;
        private IndexResource _indexResource;
        private readonly Lazy<Task> _ensureInitialised;
        private readonly HttpClient _httpClient;

        public AdPostingApiClient(string id, string secret)
            : this(id, secret, Environment.Production)
        {
        }

        public AdPostingApiClient(string id, string secret, Environment env) : this(id, secret, env.GetAttribute<EnvironmentUrlAttribute>().Uri)
        {
        }

        public AdPostingApiClient(string id, string secret, Uri adPostingUri) : this(adPostingUri, new OAuth2TokenClient(id, secret))
        {
        }

        internal AdPostingApiClient(Uri adPostingUri, IOAuth2TokenClient tokenClient)
        {
            this._ensureInitialised = new Lazy<Task>(() => this.Initialise(adPostingUri), LazyThreadSafetyMode.ExecutionAndPublication);
            _tokenClient = tokenClient;
            this.Initialise(
                _httpClient = new HttpClient(new OAuthMessageHandler(tokenClient)),
                adPostingUri);
        }

        private Task EnsureInitialised()
        {
            return this._ensureInitialised.Value;
        }

        private async Task Initialise(Uri adPostingUri)
        {
            _indexResource = await this.GetResourceAsync<IndexResource>(adPostingUri);
        }

        public async Task<AdvertisementResource> CreateAdvertisementAsync(Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException(nameof(advertisement));

            await this.EnsureInitialised();
            return await this._indexResource.CreateAdvertisementAsync(advertisement);
        }

        public async Task<AdvertisementResource> ExpireAdvertisementAsync(Uri uri, AdvertisementPatch advertisementPatch)
        {
            return await this.PatchResourceAsync<AdvertisementResource, AdvertisementPatch>(uri, advertisementPatch);
        }

        public Task<AdvertisementResource> GetAdvertisementAsync(Uri uri)
        {
            return this.GetResourceAsync<AdvertisementResource>(uri);
        }

        public Task<ProcessingStatus> GetAdvertisementStatusAsync(Uri uri)
        {
            return this.HeadResourceAsync<ProcessingStatus, AdvertisementResource>(uri);
        }

        public async Task<AdvertisementListResource> GetAllAdvertisementsAsync()
        {
            await this.EnsureInitialised();
            return await _indexResource.GetAllAdvertisements();
        }

        public Task<AdvertisementResource> UpdateAdvertisementAsync(Uri uri, Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException(nameof(advertisement));

            return this.PutResourceAsync<AdvertisementResource, Advertisement>(uri, advertisement);
        }

        public void Dispose()
        {
            _tokenClient.Dispose();
            _httpClient.Dispose();
        }
    }
}