using System;
using System.Net.Http;
using System.Runtime.Serialization;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    public class ValidationException : Exception
    {
        public ValidationData[] ValidationDataItems { get; private set; }

        public ValidationException(HttpMethod method, ValidationMessage validationMessage)
            : this(method, validationMessage, null)
        {
        }

        public ValidationException(HttpMethod method, ValidationMessage validationMessage, Exception innerException)
            : base($"{method:G} failed.{validationMessage?.Message.PadLeft(validationMessage.Message.Length + 1)}", innerException)
        {
            ValidationDataItems = validationMessage?.Errors ?? new ValidationData[0];
        }

        protected ValidationException(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ValidationDataItems), ValidationDataItems);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            ValidationDataItems = (ValidationData[])info.GetValue(nameof(ValidationDataItems), typeof(ValidationData[]));
        }
    }
}