using System;
using System.Net.Sockets;

namespace ServerTCP.Models.Interfaces
{
    public interface IListenManager : IDisposable
    {
        void Close();
        void Start();
    }
}