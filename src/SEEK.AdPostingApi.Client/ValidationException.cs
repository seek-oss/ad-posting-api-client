using System;
using System.Net.Http;
using System.Runtime.Serialization;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    [Serializable]
    public class ValidationException : RequestException
    {
        public ValidationException(string requestId, HttpMethod method, ValidationMessage validationMessage)
            : base(requestId, $"{method:G} failed.{validationMessage?.Message.PadLeft(validationMessage.Message.Length + 1)}")
        {
            this.ValidationDataItems = validationMessage?.Errors ?? new ValidationData[0];
        }

        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.ValidationDataItems = (ValidationData[])info.GetValue(nameof(this.ValidationDataItems), typeof(ValidationData[]));
        }

        public ValidationData[] ValidationDataItems { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.ValidationDataItems), this.ValidationDataItems);

            base.GetObjectData(info, context);
        }
    }
}