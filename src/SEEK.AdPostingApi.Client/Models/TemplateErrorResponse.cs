using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Models
{
    [MediaType("application/vnd.seek.template-error+json;version=1")]
    public class TemplateErrorResponse : IErrorResponse
    {
        public string Message { get; set; }

        public Error[] Errors { get; set; }
    }
}