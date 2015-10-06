using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Models
{
    [MediaType("application/vnd.seek.advertisement-error+json")]
    public class ValidationMessage
    {
        public string Message { get; set; }

        public ValidationData[] Errors { get; set; }
    }
}
