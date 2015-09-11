using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SEEK.AdPostingApi.Client.Hal
{
    public class HalResource<TProperties> : HalResource where TProperties : new()
    {
        public TProperties Properties { get; private set; }

        internal override void PopulateResource(JToken content)
        {
            base.PopulateResource(content);

            this.Properties = new TProperties();
            this.Properties = content.ToObject<TProperties>();
        }
    }

    public class HalResource : Client
    {
        protected Dictionary<string, Link> Links { get; private set; }

        private JsonSerializerSettings serializerSettings;

        public HttpResponseHeaders ResponseHeaders { get; set; }

        internal virtual void PopulateResource(JToken content)
        {
            if (content["_links"] != null)
                this.Links = content["_links"].ToObject<Dictionary<string, Link>>();

            if (content["_embedded"] != null)
            {
                var property = this.GetType().GetProperty("Embedded", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance);

                if (property != null)
                {
                    var embeddedResources = content["_embedded"].ToObject(property.PropertyType, JsonSerializer.Create(this.serializerSettings));

                    property.SetValue(this, embeddedResources);
                }
            }
        }

        protected Task<Uri> PostResourceAsync<TResource>(string relation, object parameters, TResource resource)
        {
            return base.PostResourceAsync<TResource>(new Uri(this.BaseUri, this.Links[relation].Resolve(parameters)), resource);
        }

        protected Task<Uri> PostResourceAsync<TResource>(string relation, TResource resource)
        {
            return this.PostResourceAsync(relation, null, resource);
        }

        protected Task PutResourceAsync<TResource>(string relation, object parameters, TResource resource)
        {
            return base.PutResourceAsync(new Uri(this.BaseUri, this.Links[relation].Resolve(parameters)), resource);
        }

        protected Task PutResourceAsync<TResource>(string relation, TResource resource)
        {
            return this.PutResourceAsync(relation, null, resource);
        }

        protected Task<TResource> GetResourceAsync<TResource>(string relation, object parameters) where TResource : HalResource, new()
        {
            return base.GetResourceAsync<TResource>(new Uri(this.BaseUri, this.Links[relation].Resolve(parameters)));
        }

        protected Task<TResource> GetResourceAsync<TResource>(string relation) where TResource : HalResource, new()
        {
            return this.GetResourceAsync<TResource>(relation, null);
        }

        protected Task<T> HeadResourceAsync<T, TResource>(string relation, object parameters)
        {
            return base.HeadResourceAsync<T, TResource>(new Uri(this.BaseUri, this.Links[relation].Resolve(parameters)));
        }
    }
}
