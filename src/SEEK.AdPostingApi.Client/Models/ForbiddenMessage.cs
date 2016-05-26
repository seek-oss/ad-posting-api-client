using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Models
{
    [MediaType("application/vnd.seek.advertisement-error+json;version=1")]
    public class ForbiddenMessage
    {
        public string Message { get; set; }

        public ForbiddenMessageData[] Errors { get; set; }
    }
}