using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    public class UnauthorizedException : RequestException
    {
        public UnauthorizedException(string requestId, int httpStatusCode, string message) : base(requestId, httpStatusCode, message)
        {
            this.Errors = new AdvertisementError[0];
        }

        public UnauthorizedException(string requestId, int httpStatusCode, AdvertisementErrorResponse errorResponse) : base(requestId, httpStatusCode, errorResponse?.Message)
        {
            this.Errors = errorResponse?.Errors ?? new AdvertisementError[0];
        }

        public AdvertisementError[] Errors { get; }
    }
}