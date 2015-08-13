﻿using SEEK.AdPostingApi.Client.Models;
using System;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client
{
    public interface ISeekOAuth2TokenClient : IDisposable
    {
        Task<OAuth2Token> GetOAuth2TokenAsync(string clientId, string clientSecret);
    }
}