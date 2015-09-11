using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Models
{
    [FromHeader("Status")]
    public enum Status
    {
        Error,
        Pending,
        Completed,
    }
}
