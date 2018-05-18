using System;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Hal;
using SEEK.AdPostingApi.Client.Resources;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    internal class LogoPactAdPostingApiClient : AdPostingApiClient
    {
        internal LogoPactAdPostingApiClient(Uri adPostingUri, IOAuth2TokenClient tokenClient) : base(adPostingUri, tokenClient)
        {
        }

        protected internal override Task<IndexResource> GetIndexResourceAsync(Uri adPostingUri, Hal.Client halClient)
        {
            IndexResource indexResource = new IndexResource
            {
                Links = new Links(adPostingUri)
                {
                    { "logos", new Link { Templated = true, Href = AdPostingLogoApiFixture.LogoApiLink } }
                }
            };

            indexResource.Initialise(halClient);

            return Task.FromResult(indexResource);
        }
    }
}