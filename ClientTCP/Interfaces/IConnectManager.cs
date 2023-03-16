using static ClientTCP.ConnectManager;
using System;
using System.Threading.Tasks;

namespace ClientTCP.Interfaces
{
    public interface IConnectManager
    {
        event EventHandler<ServerConnectedEventArgs> ServerConnected;
        event EventHandler<ServerConnectedEventArgs> ServerDisconnected;

        Task ConnectAsync();
        void Disconnect();
        string GetServerIP();
        string GetServerMessage();
        int GetServerPort();
        void SendMessage(string message);
        void SyncServerInfo(ServerInfo serverInfo);
    }
}