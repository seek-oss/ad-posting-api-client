using System;

namespace SEEK.AdPostingApi.Client.Models
{
    [Serializable]
    public class ValidationData
    {
        public string Field { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }
    }
}
