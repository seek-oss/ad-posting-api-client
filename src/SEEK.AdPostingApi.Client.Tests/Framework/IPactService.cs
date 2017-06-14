using System;
using PactNet.Mocks.MockHttpService;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public interface IPactService
    {
        IMockProviderService MockProviderService { get; }

        Uri MockProviderServiceBaseUri { get; }
    }
}