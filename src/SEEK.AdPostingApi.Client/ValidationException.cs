using System;
using System.Net.Http;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    public class ValidationException : RequestException
    {
        public ValidationException(string requestId, HttpMethod method, IErrorResponse errorResponse)
            : base(requestId, 422, $"{method:G} failed.{errorResponse?.Message.PadLeft(errorResponse.Message.Length + 1)}")
        {
            this.Errors = errorResponse?.Errors ?? new Error[0];
        }

        public Error[] Errors { get; }
    }
}