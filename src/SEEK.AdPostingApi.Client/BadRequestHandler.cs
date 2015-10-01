using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SEEK.AdPostingApi.Client.Exceptions;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    public class BadRequestHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            Dictionary<string, ValidationData[]> validationDataDictionary;
            try
            {
                response = await base.SendAsync(request, cancellationToken);
                if (response.StatusCode != HttpStatusCode.BadRequest) return response;

                var responseContent = await response.Content.ReadAsStringAsync();
                TryDeserialize(responseContent, out validationDataDictionary);
            }
            catch (ResourceActionException ex)
            {
                if (!TryDeserialize(ex.ResponseContent, out validationDataDictionary)) throw;
            }

            if (validationDataDictionary == null) return response;
            throw new ValidationException(request.Method, validationDataDictionary);
        }

        private bool TryDeserialize(string responseContent, out Dictionary<string, ValidationData[]> validationDataDictionary)
        {
            if (!string.IsNullOrWhiteSpace(responseContent))
            {
                try
                {
                    validationDataDictionary = JsonConvert.DeserializeObject<Dictionary<string, ValidationData[]>>(responseContent);
                    return validationDataDictionary != null;
                }
                catch
                {
                    // Ignore failed deserialization.
                }
            }

            validationDataDictionary = null;
            return false;
        }
    }
}
