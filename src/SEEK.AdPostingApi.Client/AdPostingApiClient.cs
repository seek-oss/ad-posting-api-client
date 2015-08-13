using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SEEK.AdPostingApi.Client.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client
{
    public class AdPostingApiClient : IAdPostingApiClient
    {
        private const string AdvertisementLinkKey = "advertisements";
        private const string AdPostingUriProd = "https://adposting.cloud.seek.com.au";
        private const string AdPostingUriIntegration = "https://adposting-integration.cloud.seek.com.au";

        private readonly Uri _adpostingUri;
        private readonly string _id;
        private readonly string _secret;
        private Oauth2Token _token;
        private readonly HttpClient _httpClient;
        private readonly ISEEKOauth2TokenClient _tokenClient;

        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public AdPostingApiClient(string id, string secret)
            : this(id, secret, new SEEKOauth2TokenClient())
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
            switch (env)
            {
                case EnvironmentType.Production:
                    _adpostingUri = new Uri(AdPostingUriProd);
                    break;

                case EnvironmentType.Integration:
                    _adpostingUri = new Uri(AdPostingUriIntegration);
                    break;
            }
        }

        internal AdPostingApiClient(string id, string secret, string adPostingUri, ISEEKOauth2TokenClient tokenClient)
            : this(id, secret, tokenClient)
        {
            _adpostingUri = new Uri(adPostingUri);
        }

        public AdPostingApiClient(string id, string secret, string adPostingUri) : this(id, secret)
        {
            _adpostingUri = new Uri(adPostingUri);
        }

        private Uri GenerateFullRequestUri(string path)
        {
            return new Uri(_adpostingUri, path);
        }

        private async Task<AvailableActions> GetAvailableApiActions()
        {
            using (var availableActionsRequest = new HttpRequestMessage(HttpMethod.Get, GenerateFullRequestUri("")))
            {
                availableActionsRequest.Headers.Add("Accept", "application/json");
                availableActionsRequest.Headers.Connection.Add("Keep-Alive");

                using (HttpResponseMessage availableActionsResponse = (await _httpClient.SendAsync(availableActionsRequest)).EnsureSuccessStatusCode())
                {
                    var availableActions = JsonConvert.DeserializeObject<AvailableActions>(await availableActionsResponse.Content.ReadAsStringAsync(), _jsonSettings);

                    return availableActions;
                }
            }
        }

        private async Task<Uri> CreateJobAd(string postjobUri, string accessToken, Advertisement advertisement)
        {
            string jobJson = JsonConvert.SerializeObject(advertisement, _jsonSettings);

            using (var createJobRequest = new HttpRequestMessage(HttpMethod.Post, GenerateFullRequestUri(postjobUri)))
            {
                createJobRequest.Headers.Add("Authorization", string.Format("Bearer {0}", accessToken));
                createJobRequest.Content = new StringContent(jobJson, Encoding.UTF8);
                createJobRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (HttpResponseMessage createJobResponse = (await _httpClient.SendAsync(createJobRequest)).EnsureSuccessStatusCode())
                {
                    Uri createdJobLink = createJobResponse.Headers.Location;

                    return createdJobLink;
                }
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

            AvailableActions availableActions = await GetAvailableApiActions();

            if (!availableActions.IsSupported(AdvertisementLinkKey))
            {
                throw new NotSupportedException(string.Format("'{0}' is not a supported API action.", AdvertisementLinkKey));
            }

            string postAdUri = availableActions.Links[AdvertisementLinkKey].Href;

            Uri adUri = await CreateJobAd(postAdUri, _token.AccessToken, advertisement);

            return adUri;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}