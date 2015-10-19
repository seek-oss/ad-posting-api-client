using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Models
{
    [FromHeader("Processing-Status")]
    public enum ProcessingStatus
    {
        Failed,
        Pending,
        Completed
    }
}