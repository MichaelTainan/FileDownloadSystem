using ServerTCP.Models;
using ServerTCP.Models.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Media.TextFormatting;

namespace ServerTCP
{
    public class ListenManager : IListenManager, IDisposable
    {
        private int port;
        private TcpListener tcpListener;
        private TcpClient client;
        private IFileManager fileManager;
        private IClientManager clientManager;
        private bool isRunning = false;

        public ListenManager(IFileManager fileManager, IClientManager clientManager, int port)
        {
            this.fileManager = fileManager;
            this.clientManager = clientManager;
            this.port = port;
        }

        public void Start()
        {
            /// Because sometimes it will meet the duplicate restart, but tcpListener had listening,
            /// like Unit Test task, have to skip the process.
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
                Console.WriteLine($"ServerTCP start to listen on port {port}");
                isRunning = true;
                tcpListener.BeginAcceptTcpClient(OnClientConnected, tcpListener);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// When the ClientConnected have to do something.
        /// </summary>
        /// <param name="asyncResult">input asynchronous task result</param>
        private void OnClientConnected(IAsyncResult asyncResult)
        {
            if (!isRunning)
            {
                return;
            }
            var listener = asyncResult.AsyncState as TcpListener;
            client = listener.EndAcceptTcpClient(asyncResult);
            Console.WriteLine("Client had cnnected...");

            SetUpClientManager();
            NetworkStream stream = client.GetStream();
            clientManager.SendMessageToClient(ref stream, "You had connected...");
            try
            {
                while (true)
                {
                    // Start receive the message of client
                    byte[] buffer = new byte[1024];
                    var read = stream.Read(buffer, 0, buffer.Length);
                    var receive = Encoding.UTF8.GetString(buffer, 0, read);
                    Console.WriteLine($"ReceiveClentMessage：{receive}");

                    if (string.Equals("Hello, Server", receive))
                    {
                        //Send the message to client
                        clientManager.SendMessageToClient(ref stream, "Hi, Client!");
                    }

                    if (string.Equals($"Download the file", receive.Split(':')[0]))
                    {
                        clientManager.FileName = receive.Split(':')[1];
                        clientManager.CallToUpdateClientInfo();
                        // Check and Send the file to client
                        SendRequestedFile(ref stream);
                    }

                    if (string.Equals("Exit", receive))
                    {
                        clientManager.CallToRemoveClientInfo();
                        Console.WriteLine("Client disconnected");
                        break;
                    }

                    if (read == 0)
                    {
                        clientManager.CallToRemoveClientInfo();
                        Console.WriteLine("Client disconnected");
                        break;
                    }

                }
            }
            catch (IOException)
            {
                clientManager.CallToRemoveClientInfo();
                Console.WriteLine("Client disconnected");
            }
            // close the connection
            stream.Close();
            client.Close();
            RestartTcpListener();
        }

        /// <summary>
        /// Update clientManager client infomation property and call the AddClientInfo EventHandler
        /// </summary>
        private void SetUpClientManager()
        {
            IPEndPoint remoteIPEndPoint = (IPEndPoint) client.Client.RemoteEndPoint;
            clientManager.IP = remoteIPEndPoint.Address.ToString();
            clientManager.Port = remoteIPEndPoint.Port;
            clientManager.FileName = "";
            clientManager.CallToAddClientInfo();
        }
        private void RestartTcpListener()
        {
            /// Because sometimes it will meet the duplicate restart, but tcpListener had listening,
            /// like Unit Test task, have to skip the process.
            try
            {
                tcpListener.Stop();
                tcpListener.Start();
                isRunning = true;
                tcpListener.BeginAcceptTcpClient(OnClientConnected, tcpListener);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Close()
        {
            isRunning = false;
            tcpListener?.Stop();
            client?.Close();
        }

        /// <summary>
        /// Send the client requested file if find the file in the folder.
        /// </summary>
        /// <param name="stream">NetworkStream when it be created in connection.</param>
        private void SendRequestedFile(ref NetworkStream stream)
        {
            var file = fileManager.CombineFileContentAndName(clientManager.FileName);
            if (file != null)
            {
                stream.Write(file, 0, file.Length);
                Console.WriteLine($"Send the {clientManager.FileName} to Client.");
            }
            else
            {
                //Send Error message to client
                clientManager.SendMessageToClient(ref stream, $"Error, Can't Find the File:{clientManager.FileName}!");
                Console.WriteLine($"Can't Find the File:{clientManager.FileName}");
            }
        }
        public void Dispose()
        {
            Close();
        }
    }
}
