namespace SEEK.AdPostingApi.Client.Models
{
    public class Location
    {
        public LocationCountry Country { get; set; }

        public string Suburb { get; set; }

        public string PostCode { get; set; }

        public LocationOptions[] Options { get; set; }
    }
}