using System;

namespace SEEK.AdPostingApi.Client.Models
{
    [Serializable]
    [Obsolete("Use general type SEEK.AdPostingApi.Client.Models.Error")]
    public class AdvertisementError
    {
        public string Field { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        public static implicit operator AdvertisementError(Error error)
        {
            return new AdvertisementError
            {
                Code = error.Code,
                Field = error.Field,
                Message = error.Message
            };
        }
    }
}