using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    public class UnauthorizedException : RequestException
    {
        public UnauthorizedException(string requestId, int httpStatusCode, string message) : base(requestId, httpStatusCode, message)
        {
            this.Errors = new Error[0];
        }

        public UnauthorizedException(string requestId, int httpStatusCode, IErrorResponse errorResponse) : base(requestId, httpStatusCode, errorResponse?.Message)
        {
            this.Errors = errorResponse?.Errors ?? new Error[0];
        }

        public Error[] Errors { get; }
    }
}