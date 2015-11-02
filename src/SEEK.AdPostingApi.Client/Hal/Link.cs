using System;
using Tavis.UriTemplates;

namespace SEEK.AdPostingApi.Client.Hal
{
    public class Link
    {
        public string Href { get; set; }

        public bool Templated { get; set; }

        private readonly Lazy<UriTemplate> _uriTemplate;

        public Link()
        {
            this._uriTemplate = new Lazy<UriTemplate>(() => new UriTemplate(this.Href));
        }

        public string Resolve(object parameters)
        {
            if (!Templated)
                return this.Href;

            return _uriTemplate.Value
                .AddParameters(parameters)
                .Resolve();
        }
    }
}