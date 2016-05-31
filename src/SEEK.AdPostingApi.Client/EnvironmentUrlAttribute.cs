using System;

namespace SEEK.AdPostingApi.Client
{
    internal class EnvironmentUrlAttribute : Attribute
    {
        public EnvironmentUrlAttribute(string uri)
        {
            this.Uri = new Uri(uri);
        }

        public Uri Uri { get; }
    }
}