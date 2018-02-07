using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class DelegatingHandlerFixture : IDisposable
    {
        private const string HostUrl = "http://localhost:5001";
        private readonly IWebHost _webHost;
        private readonly HashSet<string> _requestedPaths = new HashSet<string>();

        public DelegatingHandlerFixture()
        {
            IWebHostBuilder hostBuilder = new WebHostBuilder()
                .UseUrls(HostUrl)
                .UseKestrel()
                .Configure(app => app
                    .Use(async (context, next) =>
                    {
                        string path = context.Request.Path.ToString();
                        _requestedPaths.Add(path);
                        context.Response.StatusCode = 500;
                        await next();
                    }));

            this._webHost = hostBuilder.Build();
            this._webHost.Start();
        }

        public AdPostingApiClient CreateClient(DelegatingHandler customHandler)
        {
            var oAuthClient = Mock.Of<IOAuth2TokenClient>(c => c.GetOAuth2TokenAsync() == Task.FromResult(new OAuth2Token
            {
                AccessToken = "access_token",
                ExpiresIn = 300
            }));
            return new AdPostingApiClient(this.BaseUri, oAuthClient, customHandler);
        }

        public Uri BaseUri => new Uri(HostUrl);

        public ISet<string> RequestedPaths => _requestedPaths;

        public void Dispose()
        {
            this._webHost.Dispose();
        }
    }
}