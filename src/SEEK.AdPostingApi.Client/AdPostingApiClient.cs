using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SEEK.AdPostingApi.Client
{
    public class AdPostingApiClient : IAdPostingApiClient
    {
        private readonly Uri _adpostingUri;
        private string _id;
        private string _secret;
        private Oauth2Token _token = null;
        private readonly HttpClient _httpClient;
        private const string AdvertisementLinkKey = "advertisements";
        private const string AdPostingUriProd = "https://adposting.cloud.seek.com.au";
        private const string AdPostingUriIntegration = "https://adposting-integration.cloud.seek.com.au";
        private ISEEKOauth2TokenClient _tokenClient;

        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public AdPostingApiClient(string id, string secret)
            :this(id, secret, new SEEKOauth2TokenClient())
        {
        }

        internal AdPostingApiClient(string id, string secret, ISEEKOauth2TokenClient tokenClient)
        {
            _id = id;
            _secret = secret;
            _adpostingUri = new Uri(AdPostingUriProd);
            _httpClient = new HttpClient();
            _tokenClient = tokenClient;
        }

        public AdPostingApiClient(string id, string secret, EnvironmentType env) : this(id, secret)
        {
            if (env == EnvironmentType.Prod)
            {
                _adpostingUri = new Uri(AdPostingUriProd);
            }
            else if (env == EnvironmentType.Integration)
            {
                _adpostingUri = new Uri(AdPostingUriIntegration);
            }
        }

        internal AdPostingApiClient(string id, string secret, String adPostingUri, ISEEKOauth2TokenClient tokenClient)
            : this(id, secret, tokenClient)
        {
            _adpostingUri = new Uri(adPostingUri);
        }

        public AdPostingApiClient(string id, string secret, String adPostingUri) : this(id, secret)
        {
            _adpostingUri = new Uri(adPostingUri);
        }

        private Uri GenerateFullRequestUri(string path)
        {
            return new Uri(_adpostingUri, path);
        }

        private async Task<AvailableActions> GetAvailableApiActions()
        {
            var availableActionsRequest = new HttpRequestMessage(HttpMethod.Get, GenerateFullRequestUri(""));

            availableActionsRequest.Headers.Add("Accept", "application/json");
            availableActionsRequest.Headers.Connection.Add("Keep-Alive");

            HttpResponseMessage availableActionsResponse = null;

            try
            {
                availableActionsResponse = (await _httpClient.SendAsync(availableActionsRequest)).EnsureSuccessStatusCode();

                var availableActions = JsonConvert.DeserializeObject<AvailableActions>(await availableActionsResponse.Content.ReadAsStringAsync(), _jsonSettings);
                return availableActions;
            }
            finally
            {
                Dispose(availableActionsRequest, availableActionsResponse);
            }
        }

        private async Task<Uri> CreateJobAd(string postjobUri, string accessToken, Advertisement advertisement)
        {
            var createJobRequest = new HttpRequestMessage(HttpMethod.Post, GenerateFullRequestUri(postjobUri));
            createJobRequest.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken));
            var jobJson = JsonConvert.SerializeObject(advertisement, _jsonSettings);
            createJobRequest.Content = new StringContent(jobJson, Encoding.UTF8);
            createJobRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage createJobResponse = null;

            try
            {
                createJobResponse = (await _httpClient.SendAsync(createJobRequest)).EnsureSuccessStatusCode();
                var createdJobLink = createJobResponse.Headers.Location;
                return createdJobLink;
            }
            finally
            {
                Dispose(createJobRequest, createJobResponse);
            }
        }

        public async Task<Uri> CreateAdvertisementAsync(Advertisement advertisement)
        {
            if (advertisement == null)
            {
                throw new ArgumentNullException("advertisement");
            }

            if (_token == null)
            {
                _token = await _tokenClient.GetOauth2Token(_id, _secret);
            }          

            var availableActions = await GetAvailableApiActions();

            if (!availableActions.IsSupported(AdvertisementLinkKey))
            {
                throw new NotSupportedException(String.Format("'{0}' is not a supported API action.", AdvertisementLinkKey));
            }

            var postAdUri = availableActions.Links[AdvertisementLinkKey].Href;

            var adUri = await CreateJobAd(postAdUri, _token.AccessToken, advertisement);

            return adUri;
        }

        public void Dispose()
        {
            Dispose(_httpClient);
        }

        private void Dispose(params IDisposable[] disposables)
        {
            if (disposables == null)
            {
                return;
            }

            foreach (var disposable in disposables.Where(d => d != null))
            {
                disposable.Dispose();
            }
        }
    }
}
