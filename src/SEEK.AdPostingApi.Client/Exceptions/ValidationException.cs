using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Exceptions
{
    public class ValidationException : Exception
    {
        public Dictionary<string, ValidationData[]> ValidationDataDictionary { get; private set; }

        public ValidationException(HttpMethod method, Dictionary<string, ValidationData[]> validationDataDictionary)
            : this(method, validationDataDictionary, null)
        {
        }

        public ValidationException(HttpMethod method, Dictionary<string, ValidationData[]> validationDataDictionary, Exception innerException)
            : base($"{method:G} failed due to validation errors.", innerException)
        {
            ValidationDataDictionary = validationDataDictionary;
        }

        protected ValidationException(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ValidationDataDictionary), ValidationDataDictionary);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            ValidationDataDictionary =
                (Dictionary<string, ValidationData[]>) info.GetValue(nameof(ValidationDataDictionary), typeof (Dictionary<string, ValidationData[]>));
        }
    }
}