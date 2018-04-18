using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using PactNet.Mocks.MockHttpService;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class AdPostingTemplateApiFixture : IDisposable
    {
        public const string TemplateApiBasePath = "/template";
        public const string TemplateApiLink = TemplateApiBasePath + "{?advertiserId,after}";

        static AdPostingTemplateApiFixture()
        {
            AdPostingApiMessageHandler.SetProductVersion(AdPostingApiFixture.UserAgentProductVersion);

            // See https://github.com/dennisdoomen/fluentassertions/issues/305 - ShouldBeEquivalentTo fails with objects from the System namespace.
            // Due to this, we need to change the IsValueType predicate so that it does not assume System.Exception and derivatives of it in the System namespace are value types.
            AssertionOptions.IsValueType = type => (type.Namespace == typeof(int).Namespace) && !(type == typeof(Exception) || type.IsSubclassOf(typeof(Exception)));
        }

        public AdPostingTemplateApiFixture(AdPostingTemplateApiPactService adPostingTemplateApiPactService)
        {
            this.MockProviderService = adPostingTemplateApiPactService.MockProviderService;
            this.MockProviderService.ClearInteractions();
            this.AdPostingApiServiceBaseUri = AdPostingTemplateApiPactService.MockProviderServiceBaseUri;
        }

        public IMockProviderService MockProviderService { get; }

        public Uri AdPostingApiServiceBaseUri { get; }

        public void Dispose()
        {
            this.MockProviderService.VerifyInteractions();
        }

        public AdPostingApiClient GetClient(OAuth2Token token)
        {
            var oAuthClient = Mock.Of<IOAuth2TokenClient>(c => c.GetOAuth2TokenAsync() == Task.FromResult(token));

            return new TemplatePactAdPostingApiClient(this.AdPostingApiServiceBaseUri, oAuthClient);
        }

        public HttpRequestMessage CreateGetTemplatesRequest(string queryString, string accessToken)
        {
            var requestUri = new Uri(this.AdPostingApiServiceBaseUri, $"{TemplateApiBasePath}?{queryString}");
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Clear();
            request.Headers.Accept.ParseAdd(ResponseContentTypes.TemplateListVersion1);
            request.Headers.Accept.ParseAdd(ResponseContentTypes.TemplateErrorVersion1);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue(AdPostingApiFixture.UserAgentProductName, AdPostingApiFixture.UserAgentProductVersion));

            return request;
        }
    }
}