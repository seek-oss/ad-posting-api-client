using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client.Tests.Framework
{
    public class MessageHandlerSpy : DelegatingHandler
    {
        private readonly HashSet<Uri> _requestedUris = new HashSet<Uri>();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this._requestedUris.Add(request.RequestUri);
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }

        public ISet<Uri> RequestedUris => _requestedUris;
    }
}