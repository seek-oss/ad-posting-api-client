using System;

namespace SEEK.AdPostingApi.Client.Models
{
    public enum ProcessingOptionsType
    {
        [Obsolete("Advertisement Details are always cleansed, this flag is no longer required.")]
        CleanseAdvertisementDetails = 1
    }
}