using System;
using System.Net.Sockets;

namespace ServerTCP.Models.Interfaces
{
    public interface IClientManager
    {
        event EventHandler<ClientConnectedEventArgs> AddClientInfo;
        event EventHandler<ClientConnectedEventArgs> updateClientInfo;
        event EventHandler<ClientConnectedEventArgs> RemoveClientInfo;
        string FileName { get; set; }
        string IP { get; set; }
        int Port { get; set; }
        void SendMessageToClient(ref NetworkStream stream, string message);
        void CallToAddClientInfo();
        void CallToUpdateClientInfo();
        void CallToRemoveClientInfo();
    }
}