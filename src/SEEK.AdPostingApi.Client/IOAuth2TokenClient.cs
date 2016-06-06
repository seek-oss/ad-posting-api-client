using System;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    internal interface IOAuth2TokenClient : IDisposable
    {
        Task<OAuth2Token> GetOAuth2TokenAsync();
    }
}