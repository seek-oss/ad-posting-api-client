using System;
using System.Net.Http;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    public class ValidationException : RequestException
    {
        public ValidationException(string requestId, HttpMethod method, AdvertisementErrorResponse errorResponse)
            : base(requestId, 422, $"{method:G} failed.{errorResponse?.Message.PadLeft(errorResponse.Message.Length + 1)}")
        {
            this.Errors = errorResponse?.Errors ?? new AdvertisementError[0];
        }

        public AdvertisementError[] Errors { get; }
    }
}