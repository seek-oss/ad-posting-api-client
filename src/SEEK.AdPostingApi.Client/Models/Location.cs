namespace SEEK.AdPostingApi.Client.Models
{
    public class Location
    {
        public LocationCountry Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public LocationOptions[] Options { get; set; }
    }
}