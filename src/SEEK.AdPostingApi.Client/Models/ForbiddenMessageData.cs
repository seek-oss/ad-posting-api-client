using System;

namespace SEEK.AdPostingApi.Client.Models
{
    [Serializable]
    public class ForbiddenMessageData
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
}
