using System;
using System.Runtime.Serialization;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnauthorizedException(ForbiddenMessage forbiddenMessage)
        {            
        }

        protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
