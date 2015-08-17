namespace SEEK.AdPostingApi.Client
{
    public enum Environment
    {
        [EnvironmentUrl("https://adposting-integration.cloud.seek.com.au")]
        Integration,

        [EnvironmentUrl("https://adposting.cloud.seek.com.au")]
        Production
    }
}