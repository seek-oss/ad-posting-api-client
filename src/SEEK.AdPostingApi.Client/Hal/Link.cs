using System;
using Tavis.UriTemplates;

namespace SEEK.AdPostingApi.Client.Hal
{
    public class Link
    {
        public string Href { get; set; }

        public bool Templated { get; set; }

        private readonly Lazy<UriTemplate> uriTemplate;

        public Link()
        {
            this.uriTemplate = new Lazy<UriTemplate>(() => new UriTemplate(this.Href));
        }

        public string Resolve(object parameters)
        {
            if (!Templated)
                return this.Href;

            return uriTemplate.Value
                .AddParameters(parameters)
                .Resolve();
        }
    }
}