using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Models
{
    [MediaType("application/vnd.seek.advertisement-error+json;version=1")]
    public class AdvertisementErrorResponse : IErrorResponse
    {
        public string Message { get; set; }

        public Error[] Errors { get; set; }
    }
}