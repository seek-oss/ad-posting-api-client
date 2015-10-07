using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client.Exceptions;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    /// <summary>
    /// HTTP Response Code 422 - Unprocessable Entity.
    /// WebDAV (Web Distributed Authoring and Versioning) extension to HTTP 1.1
    /// (https://tools.ietf.org/html/rfc4918#section-11.2)
    /// </summary>
    public class UnprocessableEntityHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            ValidationMessage validationMessage;

            try
            {
                response = await base.SendAsync(request, cancellationToken);

                if ((int)response.StatusCode != 422)
                {
                    return response;
                }

                string responseContent = await response.Content.ReadAsStringAsync();

                this.TryDeserialize(responseContent, out validationMessage);
            }
            catch (ResourceActionException ex)
            {
                if (!this.TryDeserialize(ex.ResponseContent, out validationMessage))
                {
                    throw;
                }
            }

            if (validationMessage == null)
            {
                return response;
            }

            throw new ValidationException(request.Method, validationMessage);
        }

        private bool TryDeserialize(string responseContent, out ValidationMessage validationMessage)
        {
            if (!string.IsNullOrWhiteSpace(responseContent))
            {
                try
                {
                    validationMessage = JsonConvert.DeserializeObject<ValidationMessage>(responseContent);

                    return validationMessage != null;
                }
                catch
                {
                    // Ignore failed deserialization.
                }
            }

            validationMessage = null;

            return false;
        }
    }
}