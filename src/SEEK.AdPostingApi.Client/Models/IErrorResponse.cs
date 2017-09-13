namespace SEEK.AdPostingApi.Client.Models
{
    public interface IErrorResponse
    {
        string Message { get; set; }
        Error[] Errors { get; set; }
    }
}