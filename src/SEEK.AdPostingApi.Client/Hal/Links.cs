using System;
using System.Collections.Generic;

namespace SEEK.AdPostingApi.Client.Hal
{
    public class Links : Dictionary<string, Link>
    {
        public Links()
        {
        }

        public Links(Uri baseUri)
        {
            this.BaseUri = baseUri;
        }

        public Uri BaseUri { get; set; }

        public Uri GenerateLink(string relation, object parameters = null)
        {
            return new Uri(this.BaseUri, this[relation].Resolve(parameters));
        }
    }
}