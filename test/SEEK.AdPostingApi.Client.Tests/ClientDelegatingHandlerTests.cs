using System;
using System.Threading.Tasks;
using FluentAssertions;
using SEEK.AdPostingApi.Client.Tests.Framework;
using Xunit;

namespace SEEK.AdPostingApi.Client.Tests
{
    [Collection(AdPostingApiCollection.Name)]
    public class ClientDelegatingHandlerTests : IClassFixture<DelegatingHandlerFixture>
    {
        private readonly DelegatingHandlerFixture _fixture;

        public ClientDelegatingHandlerTests(DelegatingHandlerFixture fixture)
        {
            this._fixture = fixture;
        }

        [Fact]
        public async Task RequestsPassThroughCustomHandler()
        {
            var messageHandlerSpy = new MessageHandlerSpy();
            using (var client = this._fixture.CreateClient(messageHandlerSpy))
            {
                // The "index" resource will be requested first
                var expectedRequestUri = this._fixture.BaseUri;
                try
                {
                    await client.GetAdvertisementAsync(Guid.Empty);
                }
                catch (AdvertisementNotFoundException)
                {
                    // There's no real advertisement, so this will always be thrown
                }
                messageHandlerSpy.RequestedUris.Should().Contain(expectedRequestUri);
            }
        }
    }
}