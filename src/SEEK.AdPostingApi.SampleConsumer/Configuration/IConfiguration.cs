using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Configuration
{
    public interface IConfiguration
    {
        string ClientKey { get; }

        string ClientSecret { get; }

        string AdPostingApiBaseUrl { get; }

        EnvironmentType Environment { get;  }
    }
}
