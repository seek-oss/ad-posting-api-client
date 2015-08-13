namespace SEEK.AdPostingApi.Client.Models
{
    public class AvailableActions : HypermediaEnabledResponse
    {
        public bool IsSupported(string actionName)
        {
            return Links.ContainsKey(actionName);
        }
    }
}
