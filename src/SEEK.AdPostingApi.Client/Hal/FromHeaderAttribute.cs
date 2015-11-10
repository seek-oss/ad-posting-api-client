using System;

namespace SEEK.AdPostingApi.Client.Hal
{
    public class FromHeaderAttribute : Attribute
    {
        public FromHeaderAttribute(string header)
        {
            this.Header = header;
        }

        public string Header { get; set; }
    }
}