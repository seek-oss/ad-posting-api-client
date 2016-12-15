using System;

namespace SEEK.AdPostingApi.Client.Models
{
    [Obsolete("The processing status will always be completed. All validation is done upfront and the advertisement will not fail once successfully submitted.")]
    public enum ProcessingStatus
    {
        Unknown,
        Failed,
        Pending,
        Completed
    }
}