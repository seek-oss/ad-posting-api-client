using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.Client
{
    public class AdPostingApiClient : IAdPostingApiClient
    {
        private readonly Uri _adpostingUri;
        private readonly HttpClient _httpClient;
        private readonly IOAuth2TokenClient _tokenClient;
        private IndexResource _indexResource;
        private readonly Lazy<Task> _ensureInitialised;

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
            this._ensureInitialised = new Lazy<Task>(()=>this.Initialise(adPostingUri, tokenClient), LazyThreadSafetyMode.ExecutionAndPublication);
            
            _tokenClient = tokenClient;
            _httpClient = new HttpClient();
        }

        private Task EnsureInitialised()
        {
            return this._ensureInitialised.Value;
        }

        private async Task Initialise(Uri adPostingUri, IOAuth2TokenClient tokenClient)
        {
            this._indexResource = new IndexResource();

            await this._indexResource.Initialise(_httpClient, adPostingUri, tokenClient);
        }

        public async Task<Uri> CreateAdvertisementAsync(Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException(nameof(advertisement));

            await this.EnsureInitialised();

            return await this._indexResource.PostAdvertisementAsync(advertisement);
        }

        public async Task<AdvertisementResource> GetAdvertisementAsync(Guid id)
        {
            await this.EnsureInitialised();

            return await this._indexResource.GetAdvertisementByIdAsync(id);
        }

        public async Task<AdvertisementResource> GetAdvertisementAsync(Uri uri)
        {
            var resource = new AdvertisementResource();
            await resource.Initialise(this._httpClient, uri, _tokenClient);
            return resource;
        }

        public void Dispose()
        {
            _tokenClient.Dispose();
            _httpClient.Dispose();
        }
    }
}