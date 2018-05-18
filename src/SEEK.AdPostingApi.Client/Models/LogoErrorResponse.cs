using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Models
{
    [MediaType("application/vnd.seek.logo-error+json;version=1")]
    public class LogoErrorResponse : IErrorResponse
    {
        public string Message { get; set; }

        public Error[] Errors { get; set; }
    }
}