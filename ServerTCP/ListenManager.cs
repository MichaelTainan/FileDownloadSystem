using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace ServerTCP
{
    public class ListenManager
    {
        private const int PORT = 8080;
        private TcpListener tcpListener;
        private TcpClient client;
        private ClientInfo clientInfo;
        private FileManager fileManager;
        public ListenManager()
        {
            clientInfo = new ClientInfo();
            fileManager = new FileManager();
        }

        public void Start()
        {
            tcpListener = new TcpListener(IPAddress.Any, PORT);
            tcpListener.Start(); ;
            Console.WriteLine($"ServerTCP start to listen on port {PORT}");
            tcpListener.BeginAcceptTcpClient(AsyncCallback, tcpListener);
        }

        private void AsyncCallback(IAsyncResult asyncResult)
        {
            var listener = asyncResult.AsyncState as TcpListener;
            client = listener.EndAcceptTcpClient(asyncResult);
            Console.WriteLine("Client had cnnected...");

            IPEndPoint remoteIPEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            clientInfo.IP = remoteIPEndPoint.Address.ToString();
            clientInfo.Port = remoteIPEndPoint.Port;
            NetworkStream stream = client.GetStream();

            // Start receive the message of client
            byte[] buffer = new byte[1024];
            var read = stream.Read(buffer, 0, buffer.Length);
            var receive = Encoding.UTF8.GetString(buffer, 0, read);
            Console.WriteLine($"ReceiveClentMessage：{receive}");
            if (string.Equals("Hello, Server", receive)) 
            {
                //Send the message to client
                SendMessageToClient(ref stream);
            }
            if (string.Equals("Download the file", receive.Split(':')[0]))
            {
                // Send the file to client
                clientInfo.FileName = receive.Split(':')[1];
                var file = SendFile(clientInfo.FileName);
                stream.Write(file, 0, file.Length);
                Console.WriteLine($"Send the {clientInfo.FileName} to Client.");
            }
              
            // close the connection
            stream.Close();
            client.Close();

            // listen the next message
            listener.BeginAcceptTcpClient(AsyncCallback, tcpListener);
        }

        public void SendMessageToClient(ref NetworkStream stream) 
        {
            string message = "Hi, Client!";
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            stream.Write(bytes, 0, bytes.Length);
            Console.WriteLine($"Send the message to Client: {message}");
        } 
        public void Close()
        {
            tcpListener?.Stop();
        }

        public ClientInfo GetClientInfo()
        {
            return clientInfo;
        }

        public byte[] SendFile(string fileName)
        {
            return fileManager.SendFile(fileName);
            
        }
    }
}
