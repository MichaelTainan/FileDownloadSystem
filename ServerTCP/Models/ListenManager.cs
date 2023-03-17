using ServerTCP.Models;
using ServerTCP.Models.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace ServerTCP
{
    public class ListenManager : IListenManager, IDisposable
    {
        private const int PORT = 8080;
        private TcpListener tcpListener;
        private TcpClient client;
        private ClientInfo clientInfo;
        private IFileManager fileManager;
        private bool isRunning = false;
        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        public event EventHandler<ClientConnectedEventArgs> ClientDisconnected;
        public ListenManager(IFileManager fileManager)
        {
            clientInfo = new ClientInfo();
            this.fileManager = fileManager;
        }

        public void Start()
        {
            /// Because sometimes it will meet the duplicate restart, but tcpListener had listening,
            /// like Unit Test task, have to skip the process.
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, PORT);
                tcpListener.Start(); ;
                Console.WriteLine($"ServerTCP start to listen on port {PORT}");
                isRunning = true;
                tcpListener.BeginAcceptTcpClient(AsyncCallback, tcpListener);
            }catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void AsyncCallback(IAsyncResult asyncResult)
        {
            if (!isRunning)
            {
                return;
            }

            var listener = asyncResult.AsyncState as TcpListener;
            client = listener.EndAcceptTcpClient(asyncResult);
            Console.WriteLine("Client had cnnected...");

            IPEndPoint remoteIPEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            clientInfo.IP = remoteIPEndPoint.Address.ToString();
            clientInfo.Port = remoteIPEndPoint.Port;
            ClientConnected?.Invoke(this, new ClientConnectedEventArgs(clientInfo));

            NetworkStream stream = client.GetStream();
            SendMessageToClient(ref stream, "You had connected...");
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
                        SendMessageToClient(ref stream, "Hi, Client!");
                    }

                    if (string.Equals("Download the file", receive.Split(':')[0]))
                    {
                        // Check and Send the file to client
                        clientInfo.FileName = receive.Split(':')[1];
                        CheckTheDownloadFile(ref stream);
                    }

                    if (string.Equals("Exit", receive))
                    {
                        CallClientDisconnectedEventHandler();
                        break;
                    }

                    if (read == 0)
                    {
                        CallClientDisconnectedEventHandler();
                        break;
                    }

                }
            }
            catch (IOException)
            {
                CallClientDisconnectedEventHandler();
            }
            // close the connection
            stream.Close();
            client.Close();
            RestartTcpListener();
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
                tcpListener.BeginAcceptTcpClient(AsyncCallback, tcpListener);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void CallClientDisconnectedEventHandler()
        {
            ClientDisconnected?.Invoke(this, new ClientConnectedEventArgs(clientInfo));
            Console.WriteLine("Client disconnected");
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

        public void Close()
        {
            isRunning = false;
            tcpListener?.Stop();
            client?.Close();
        }

        public ClientInfo GetClientInfo()
        {
            return clientInfo;
        }

        private void CheckTheDownloadFile(ref NetworkStream stream) 
        {
            //clientInfo.FileName = receive.Split(':')[1];
            ClientConnected?.Invoke(this, new ClientConnectedEventArgs(clientInfo));

            var file = SendFile(clientInfo.FileName);
            if (file != null)
            {
                stream.Write(file, 0, file.Length);
                Console.WriteLine($"Send the {clientInfo.FileName} to Client.");
            }
            else
            {
                //Send Error message to client
                SendMessageToClient(ref stream, $"Error, Can't Find the File:{clientInfo.FileName}!");
                Console.WriteLine($"Can't Find the File:{clientInfo.FileName}");
            }
        }

        public byte[] SendFile(string fileName)
        {
            if (fileManager.ChangeFileBeByteArray(fileName) == null)
            {
                return null;
            }
            else 
            {
                try
                {
                    byte[] fileData = fileManager.ChangeFileBeByteArray(fileName);
                    //Combine file content and file name to together
                    byte[] buffer = new byte[fileData.Length+fileName.Length+1];
                    Array.Copy(Encoding.UTF8.GetBytes(fileName), buffer, fileName.Length);
                    buffer[fileName.Length] = 0; // Add 0 after the filName to be a separator
                    Array.Copy(fileData, 0, buffer, fileName.Length + 1, fileData.Length);

                    return buffer;
                }catch (Exception e) 
                {
                    Console.WriteLine($"SendFile failed: {e.Message}");
                    return null;
                }
            }


        }

        public void Dispose()
        {
            Close();
        }
    }
}
