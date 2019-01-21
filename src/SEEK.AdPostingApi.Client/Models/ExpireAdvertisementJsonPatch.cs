using System.Collections.Generic;
using SEEK.AdPostingApi.Client.Hal;

namespace SEEK.AdPostingApi.Client.Models
{
    [MediaType("application/vnd.seek.advertisement-patch+json;version=1")]
    public class ExpireAdvertisementJsonPatch : List<JsonPatchOperation>
    {
        public ExpireAdvertisementJsonPatch()
        {
            Add(new JsonPatchOperation
            {
                Operation = "replace",
                Path = nameof(AdvertisementPatch.State).ToLower(),
                Value = AdvertisementState.Expired.ToString()
            });
        }
    }
}