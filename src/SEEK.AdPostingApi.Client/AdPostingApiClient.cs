using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Exceptions;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.Client
{
    public class AdPostingApiClient : Hal.Client, IAdPostingApiClient
    {
        private readonly IOAuth2TokenClient _tokenClient;
        private IndexResource _indexResource;
        private readonly Lazy<Task> _ensureInitialised;
        private HttpClient _httpClient;

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
            this._ensureInitialised = new Lazy<Task>(()=>this.Initialise(adPostingUri), LazyThreadSafetyMode.ExecutionAndPublication);
            _tokenClient = tokenClient;
            this.Initialise(
                _httpClient =
                    new HttpClient(new OAuthMessageHandler(tokenClient) { InnerHandler = new BadRequestHandler { InnerHandler = new MonoHttpClientWebExceptionHandler { InnerHandler = new HttpClientHandler() } } }),
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

            try
            {
                return await this._indexResource.CreateAdvertisementAsync(advertisement);
            }
            catch (ResourceActionException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new AdvertisementAlreadyExistsException(advertisement.CreationId, ex);
                }
                throw;
            }
        }

        public async Task<AdvertisementResource> GetAdvertisementAsync(Guid id)
        {
            await this.EnsureInitialised();

            return await this._indexResource.GetAdvertisementByIdAsync(id);
        }

        public Task<AdvertisementResource> GetAdvertisementAsync(Uri uri)
        {
            return this.GetResourceAsync<AdvertisementResource>(uri);
        }

        public async Task<Status> GetAdvertisementStatusAsync(Guid id)
        {
            await this.EnsureInitialised();

            return await this._indexResource.GetAdvertisementStatusByIdAsync(id);
        }

        public Task<Status> GetAdvertisementStatusAsync(Uri uri)
        {
            return this.HeadResourceAsync<Status, AdvertisementResource>(uri);
        }

        public async Task<AdvertisementListResource> GetAllAdvertisementsAsync()
        {
            await this.EnsureInitialised();
            return await _indexResource.GetAllAdvertisements(); 
        }

        public async Task UpdateAdvertisementAsync(Guid id, Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException(nameof(advertisement));

            await this.EnsureInitialised();
            await this._indexResource.UpdateAdvertisementByIdAsync(id, advertisement);
        }

        public Task UpdateAdvertisementAsync(Uri uri, Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException(nameof(advertisement));

            return this.PutResourceAsync(uri, advertisement);
        }

        public void Dispose()
        {
            _tokenClient.Dispose();
            _httpClient.Dispose();
        }
    }
}