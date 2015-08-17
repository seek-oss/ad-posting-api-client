using System;

namespace SEEK.AdPostingApi.Client
{
    public class EnvironmentUrlAttribute : Attribute
    {
        public Uri Uri { get; }

        public EnvironmentUrlAttribute(string uri)
        {
            this.Uri = new Uri(uri);
        }
    }
}
