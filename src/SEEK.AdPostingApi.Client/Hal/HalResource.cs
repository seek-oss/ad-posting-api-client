using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SEEK.AdPostingApi.Client.Hal
{
    public class HalResource<TProperties> : HalResource where TProperties : new()
    {
        public TProperties Properties { get; internal set; }

        internal override void PopulateResource(JToken content)
        {
            base.PopulateResource(content);

            this.Properties = new TProperties();
            this.Properties = content.ToObject<TProperties>();
        }
    }

    public class HalResource : Client
    {
        public Dictionary<string, Link> Links { get; internal set; }

        public HttpResponseHeaders ResponseHeaders { get; internal set; }

        internal virtual void PopulateResource(JToken content)
        {
            if (content["_links"] != null)
                this.Links = content["_links"].ToObject<Dictionary<string, Link>>();

            if (content["_embedded"] != null)
            {
                var property = this.GetType().GetProperty("Embedded", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance);

                if (property != null)
                {
                    var embeddedResources = content["_embedded"].ToObject(property.PropertyType, JsonSerializer.Create());

                    property.SetValue(this, embeddedResources);
                }
            }
        }

        protected Uri GenerateLink(string relation, object parameters = null)
        {
            return new Uri(this.BaseUri, this.Links[relation].Resolve(parameters));
        }

        public Uri GetUri()
        {
            return this.GenerateLink("self");
        }
    }
}