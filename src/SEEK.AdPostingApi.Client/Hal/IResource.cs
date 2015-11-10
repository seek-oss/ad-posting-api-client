using System;

namespace SEEK.AdPostingApi.Client.Hal
{
    public interface IResource
    {
        void Initialise(Client client);

        Uri Uri { get; }

        Links Links { get; set; }
    }
}