using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SEEK.AdPostingApi.Client.Exceptions
{
    [Serializable]
    public class ResourceActionException : Exception
    {
        public HttpMethod Method { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        public HttpResponseHeaders ResponseHeaders { get; private set; }

        public string ResponseContent { get; private set; }

        public ResourceActionException(HttpMethod method, HttpStatusCode statusCode)
            : this(method, statusCode, null, null, null)
        {
        }

        public ResourceActionException(HttpMethod method, HttpStatusCode statusCode, Exception innerException)
            : this(method, statusCode, null, null, innerException)
        {
        }

        public ResourceActionException(HttpMethod method, HttpStatusCode statusCode, string responseContent)
            : this(method, statusCode, null, responseContent, null)
        {
        }

        public ResourceActionException(HttpMethod method, HttpStatusCode statusCode, string responseContent, Exception innerException)
            : this(method, statusCode, null, responseContent, innerException)
        {
        }

        public ResourceActionException(HttpMethod method, HttpStatusCode statusCode, HttpResponseHeaders responseHeaders)
            : this(method, statusCode, responseHeaders, null, null)
        {
        }

        public ResourceActionException(HttpMethod method, HttpStatusCode statusCode, HttpResponseHeaders responseHeaders, Exception innerException)
            : this(method, statusCode, responseHeaders, null, innerException)
        {
        }

        public ResourceActionException(HttpMethod method, HttpStatusCode statusCode, HttpResponseHeaders responseHeaders, string responseContent)
            : this(method, statusCode, responseHeaders, responseContent, null)
        {
        }

        public ResourceActionException(HttpMethod method, HttpStatusCode statusCode, HttpResponseHeaders responseHeaders, string responseContent, Exception innerException)
            : base($"{method:G} failed with HTTP status code {statusCode:D} {statusCode:G}", innerException)
        {
            Method = method;
            StatusCode = statusCode;
            ResponseHeaders = responseHeaders;
            ResponseContent = responseContent;
        }

        protected ResourceActionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            info.AddValue("Method", Method);
            info.AddValue("StatusCode", StatusCode);
            info.AddValue("ResponseHeaders", JsonConvert.SerializeObject(ResponseHeaders));
            info.AddValue("ResponseContent", ResponseContent);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            Method = (HttpMethod) info.GetValue("Method", typeof (HttpMethod));
            StatusCode = (HttpStatusCode) info.GetValue("StatusCode", typeof (HttpStatusCode));
            ResponseHeaders = (HttpResponseHeaders) JsonConvert.DeserializeObject(info.GetString("ResponseHeaders"), typeof(HttpResponseHeaders));
            ResponseContent = info.GetString("ResponseContent");
        }
    }
}
