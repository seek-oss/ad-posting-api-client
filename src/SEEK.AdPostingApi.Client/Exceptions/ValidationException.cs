using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client.Exceptions
{
    public class ValidationException : Exception
    {
        public Dictionary<string, ValidationData[]> ValidationErrorDictionary { get; private set; }

        public ValidationException(ResourceActionException exception) :
            base(exception.Message, exception)
        {
            if (string.IsNullOrWhiteSpace(exception.ResponseContent))
            {
                ValidationErrorDictionary = new Dictionary<string, ValidationData[]>();
            }
            else
            {
                try
                {
                    ValidationErrorDictionary = JsonConvert.DeserializeObject<Dictionary<string, ValidationData[]>>(exception.ResponseContent);
                }
                catch
                {
                    ValidationErrorDictionary = new Dictionary<string, ValidationData[]>
                    {
                        {
                            "responseContentDeserializationError",
                            new[]
                            {
                                new ValidationData
                                {
                                    Severity = ValidationSeverity.Error,
                                    Code = "ResponseContentDeserializationFailure",
                                    Message = "Could not deserialize response content into validation error data. See the inner exception for the response content."
                                }
                            }
                        }
                    };
                }
            }
        }

        protected ValidationException(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ValidationErrorDictionary), ValidationErrorDictionary);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            ValidationErrorDictionary =
                (Dictionary<string, ValidationData[]>) info.GetValue(nameof(ValidationErrorDictionary), typeof (Dictionary<string, ValidationData[]>));
        }
    }
}