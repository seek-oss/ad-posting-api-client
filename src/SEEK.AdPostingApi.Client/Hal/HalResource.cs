using System;
using System.Collections.Generic;
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
        public Dictionary<string, Link> Links { get; private set; }

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
                    var embeddedResources = content["_embedded"].ToObject(property.PropertyType, JsonSerializer.Create());

                    property.SetValue(this, embeddedResources);
                }
            }
        }

        protected Task<TResource> PatchResourceAsync<TResource, T>(string relation, object parameters, T resource) where TResource : HalResource, new()
        {
            return this.PatchResourceAsync<TResource, T>(new Uri(this.BaseUri, this.Links[relation].Resolve(parameters)), resource);
        }

        protected Task<TResource> PostResourceAsync<TResource, T>(string relation, object parameters, T resource) where TResource : HalResource, new()
        {
            return this.PostResourceAsync<TResource, T>(new Uri(this.BaseUri, this.Links[relation].Resolve(parameters)), resource);
        }

        protected Task<TResource> PostResourceAsync<TResource, T>(string relation, T resource) where TResource : HalResource, new()
        {
            return this.PostResourceAsync<TResource, T>(relation, null, resource);
        }

        protected Task<TResource> PutResourceAsync<TResource, T>(string relation, object parameters, T resource) where TResource : HalResource, new()
        {
            return this.PutResourceAsync<TResource, T>(new Uri(this.BaseUri, this.Links[relation].Resolve(parameters)), resource);
        }

        protected Task<TResource> PutResourceAsync<TResource>(string relation, TResource resource) where TResource : HalResource, new()
        {
            return this.PutResourceAsync<TResource, TResource>(relation, null, resource);
        }

        protected Task<TResource> GetResourceAsync<TResource>(string relation, object parameters) where TResource : HalResource, new()
        {
            return this.GetResourceAsync<TResource>(new Uri(this.BaseUri, this.Links[relation].Resolve(parameters)));
        }

        protected Task<TResource> GetResourceAsync<TResource>(string relation) where TResource : HalResource, new()
        {
            return this.GetResourceAsync<TResource>(relation, null);
        }

        protected Task<T> HeadResourceAsync<T, TResource>(string relation, object parameters)
        {
            return this.HeadResourceAsync<T, TResource>(new Uri(this.BaseUri, this.Links[relation].Resolve(parameters)));
        }
    }
}