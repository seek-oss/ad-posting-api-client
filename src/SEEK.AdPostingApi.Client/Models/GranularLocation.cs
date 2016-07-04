namespace SEEK.AdPostingApi.Client.Models
{
    public class GranularLocation
    {
        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public GranularLocationOptions[] Options { get; set; }
    }
}
