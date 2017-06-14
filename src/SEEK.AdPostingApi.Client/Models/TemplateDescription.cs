using System;

namespace SEEK.AdPostingApi.Client.Models
{
    public class TemplateDescription
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public TemplateStatus State { get; set; }

        public string AdvertiserId { get; set; }

        public DateTimeOffset UpdateDateTime { get; set; }
    }
}