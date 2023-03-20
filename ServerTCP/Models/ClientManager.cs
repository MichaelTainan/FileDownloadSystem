using ServerTCP.Models.Interfaces;
using System;
using System.Net.Sockets;
using System.Text;

namespace ServerTCP.Models
{
    public class ClientManager : IClientManager
    {
        private ClientInfo clientInfo;
        public event EventHandler<ClientConnectedEventArgs> AddClientInfo;
        public event EventHandler<ClientConnectedEventArgs> updateClientInfo;
        public event EventHandler<ClientConnectedEventArgs> RemoveClientInfo;
        public ClientManager()
        {
            clientInfo = new ClientInfo();
        }

        public void CallToAddClientInfo()
        {
            AddClientInfo?.Invoke(this, new ClientConnectedEventArgs(clientInfo));
        }

        public void CallToUpdateClientInfo()
        {
            updateClientInfo?.Invoke(this, new ClientConnectedEventArgs(clientInfo));
        }

        public void CallToRemoveClientInfo()
        {
            RemoveClientInfo?.Invoke(this, new ClientConnectedEventArgs(clientInfo));
        }

        public void SendMessageToClient(ref NetworkStream stream, string message)
        {
            if (stream.CanWrite)
            {
                //string message = "Hi, Client!";
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                stream.Write(bytes, 0, bytes.Length);
                Console.WriteLine($"Send the message to Client: {message}");
            }
        }

        public string IP
        {
            get { return clientInfo.IP; }
            set { clientInfo.IP = value; }
        }

        public int Port
        {
            get { return clientInfo.Port; }
            set { clientInfo.Port = value; }
        }

        public string FileName
        {
            get { return clientInfo.FileName; }
            set { clientInfo.FileName = value; }
        }
    }
}
