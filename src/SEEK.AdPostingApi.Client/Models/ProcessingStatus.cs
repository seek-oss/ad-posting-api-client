using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Models
{
    [FromHeader("Processing-Status")]
    public enum ProcessingStatus
    {
        Unknown,
        Failed,
        Pending,
        Completed
    }
}