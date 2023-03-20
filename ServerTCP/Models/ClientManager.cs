using ServerTCP.Models.Interfaces;
using System;
using System.Net.Sockets;
using System.Text;

namespace ServerTCP.Models
{
    public class ClientManager : IClientManager
    {
        private ClientInfo clientInfo;
        public event EventHandler<ClientInfo> AddClientInfo;
        public event EventHandler<ClientInfo> updateClientInfo;
        public event EventHandler<ClientInfo> RemoveClientInfo;
        public ClientManager()
        {
            clientInfo = new ClientInfo();
        }

        public void CallToAddClientInfo()
        {
            AddClientInfo?.Invoke(this, clientInfo);
        }

        public void CallToUpdateClientInfo()
        {
            updateClientInfo?.Invoke(this, clientInfo);
        }

        public void CallToRemoveClientInfo()
        {
            RemoveClientInfo?.Invoke(this, clientInfo);
        }
        
        /// <summary>
        /// Send message to write to reference NwtworkStream
        /// </summary>
        /// <param name="stream">vall by Reference networkstrem</param>
        /// <param name="message">message want to send to client</param>
        public void SendMessageToClient(ref NetworkStream stream, string message)
        {
            if (stream.CanWrite)
            {
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
