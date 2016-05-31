using System;

namespace SEEK.AdPostingApi.Client.Hal
{
    public interface IResource
    {
        Links Links { get; set; }

        Uri Uri { get; }

        void Initialise(Client client);
    }
}