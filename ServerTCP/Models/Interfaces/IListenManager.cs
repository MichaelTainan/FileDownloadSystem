using System;
using System.Net.Sockets;

namespace ServerTCP.Models.Interfaces
{
    public interface IListenManager
    {
        event EventHandler<ClientConnectedEventArgs> ClientConnected;
        event EventHandler<ClientConnectedEventArgs> ClientDisconnected;

        void Close();
        ClientInfo GetClientInfo();
        byte[] SendFile(string fileName);
        void SendMessageToClient(ref NetworkStream stream, string message);
        void Start();
    }
}