using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SEEK.AdPostingApi.Client.Models;
using SEEK.AdPostingApi.Client.Resources;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client
{
    public class AdPostingApiClient : IAdPostingApiClient
    {
        private const string AdvertisementsLinkKey = "advertisements";

        private const string AdPostingUriProd = "https://adposting.cloud.seek.com.au";
        private const string AdPostingUriIntegration = "https://adposting-integration.cloud.seek.com.au";

        private readonly Uri _adpostingUri;
        private readonly string _id;
        private readonly string _secret;
        private OAuth2Token _token;
        private readonly HttpClient _httpClient;
        private readonly IOAuth2TokenClient _tokenClient;

        public AdPostingApiClient(string id, string secret)
            : this(id, secret, new OAuth2TokenClient())
        {
        }

        public AdPostingApiClient(string id, string secret, Uri adPostingUri) : this(id, secret)
        {
            _adpostingUri = adPostingUri;
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

        internal AdPostingApiClient(string id, string secret, IOAuth2TokenClient tokenClient, Uri adPostingUri = null)
        {
            _id = id;
            _secret = secret;
            _adpostingUri = adPostingUri ?? new Uri(AdPostingUriProd);
            _httpClient = new HttpClient();
            _tokenClient = tokenClient;
        }

        public async Task<Uri> CreateAdvertisementAsync(Advertisement advertisement)
        {
            if (advertisement == null)
            {
                throw new ArgumentNullException("advertisement");
            }

            _token = _token ?? await _tokenClient.GetOAuth2TokenAsync(_id, _secret);

            AvailableActions availableActions = await GetAvailableApiActions();

            if (!availableActions.IsSupported(AdvertisementsLinkKey))
            {
                throw new NotSupportedException(string.Format("'{0}' is not a supported API action.", AdvertisementsLinkKey));
            }

            string postAdUri = availableActions.Links[AdvertisementsLinkKey].Href;

            Uri adUri = await CreateJobAd(postAdUri, _token.AccessToken, advertisement);

            return adUri;
        }

        public async Task<AdvertisementResource> GetAdvertisementAsync(Uri advertisementLocation)
        {
            if (advertisementLocation == null)
            {
                throw new ArgumentNullException("advertisementLocation");
            }

            _token = _token ?? await _tokenClient.GetOAuth2TokenAsync(_id, _secret);

            using (var request = new HttpRequestMessage(HttpMethod.Get, advertisementLocation))
            {
                request.Headers.AddAccessToken(_token.AccessToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.seek.advertisement+json"));

                using (HttpResponseMessage response = (await _httpClient.SendAsync(request)).EnsureSuccessStatusCode())
                {
                    var advertisementResource = JsonConvert.DeserializeObject<AdvertisementResource>(await response.Content.ReadAsStringAsync());

                    advertisementResource.Status = response.Headers.GetValues("status").Single();

                    return await Task.FromResult(advertisementResource);
                }
            }
        }

        public void Dispose()
        {
            _tokenClient.Dispose();
            _httpClient.Dispose();
        }

        private JsonSerializerSettings JsonSettings
        {
            get
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                };

                settings.Converters.Add(new StringEnumConverter());

                return settings;
            }
        }

        private async Task<AvailableActions> GetAvailableApiActions()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, this._adpostingUri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Connection.Add("Keep-Alive");

                using (HttpResponseMessage availableActionsResponse = (await _httpClient.SendAsync(request)).EnsureSuccessStatusCode())
                {
                    var availableActions = JsonConvert.DeserializeObject<AvailableActions>(await availableActionsResponse.Content.ReadAsStringAsync(), this.JsonSettings);

                    return availableActions;
                }
            }
        }

        private async Task<Uri> CreateJobAd(string postjobUri, string accessToken, Advertisement advertisement)
        {
            string jobJson = JsonConvert.SerializeObject(advertisement, this.JsonSettings);

            using (var request = new HttpRequestMessage(HttpMethod.Post, new Uri(_adpostingUri, postjobUri)))
            {
                request.Headers.AddAccessToken(accessToken);
                request.Content = new StringContent(jobJson, Encoding.UTF8);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (HttpResponseMessage createJobResponse = (await _httpClient.SendAsync(request)).EnsureSuccessStatusCode())
                {
                    Uri createdJobLink = createJobResponse.Headers.Location;

                    return createdJobLink;
                }
            }
        }
    }
}