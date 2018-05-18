using System;

namespace SEEK.AdPostingApi.Client.Models
{
    public class LogoSummary
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public LogoStatus State { get; set; }

        public string AdvertiserId { get; set; }

        public DateTimeOffset UpdatedDateTime { get; set; }
    }
}