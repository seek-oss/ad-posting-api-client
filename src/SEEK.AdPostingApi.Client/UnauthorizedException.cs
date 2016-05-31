using System;
using System.Runtime.Serialization;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class UnauthorizedException : RequestException
    {
        public ForbiddenMessageData[] Errors { get; set; }

        public UnauthorizedException(string requestId, string message) : base(requestId, message)
        {
        }

        public UnauthorizedException(string requestId, ForbiddenMessage forbiddenMessage) : base(requestId, forbiddenMessage?.Message)
        {
            this.Errors = forbiddenMessage?.Errors;
        }

        protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Errors = (ForbiddenMessageData[])info.GetValue(nameof(this.Errors), typeof(ForbiddenMessageData[]));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.Errors), this.Errors);

            base.GetObjectData(info, context);
        }
    }
}