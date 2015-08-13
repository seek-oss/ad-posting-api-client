using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Client
{
    public interface ISEEKOauth2TokenClient : IDisposable
    {
        Task<Oauth2Token> GetOauth2Token(string clientId, string clientSecret);
    }
}
