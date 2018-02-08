using System;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    public interface IOAuth2TokenClient : IDisposable
    {
        Task<OAuth2Token> GetOAuth2TokenAsync();
    }
}