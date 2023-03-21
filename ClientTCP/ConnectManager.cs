using ClientTCP.Interfaces;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientTCP
{
    public class ConnectManager : IConnectManager
    {
        private TcpClient client;
        private NetworkStream stream;
        private ServerInfo serverInfo;
        public event EventHandler<ServerConnectedEventArgs> ServerConnected;
        public event EventHandler<ServerConnectedEventArgs> ServerDisconnected;

        public ConnectManager(ServerInfo serverInfo)
        {
            this.serverInfo = serverInfo;
        }

        public async Task ConnectAsync()
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(serverInfo.IP, serverInfo.Port);
                stream = client.GetStream();
                ServerConnected?.Invoke(this, new ServerConnectedEventArgs(serverInfo));
                await ReceiveMessageAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                serverInfo.Message = "Can't connect to server! Please check the Server site had started.";
                //ServerConnected?.Invoke(this, new ServerConnectedEventArgs(serverInfo));
                ServerDisconnected?.Invoke(this, new ServerConnectedEventArgs(serverInfo));
            }
        }

        public void SendMessage(string message)
        {
            if (stream != null)
            {
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(message);
                    stream.Write(bytes, 0, bytes.Length);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    serverInfo.Message = "The connection is disconnected!";
                    //ServerConnected?.Invoke(this, new ServerConnectedEventArgs(serverInfo));
                    ServerDisconnected?.Invoke(this, new ServerConnectedEventArgs(serverInfo));
                }
            }
            else 
            {
                serverInfo.Message = "No connection! Please check the connection had succeeded.";
                //ServerConnected?.Invoke(this, new ServerConnectedEventArgs(serverInfo));
                ServerDisconnected?.Invoke(this, new ServerConnectedEventArgs(serverInfo));
            }
         }
        private async Task ReceiveMessageAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = 0;

            while (true)
            {
                try
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0) break;

                    string fileName = "";
                    int fileNameEndIndex = Array.IndexOf<byte>(buffer, 0, 0, bytesRead);
                    if (fileNameEndIndex != -1)
                    {
                        fileName = Encoding.UTF8.GetString(buffer, 0, fileNameEndIndex);
                    }

                    if (string.Equals(fileName, serverInfo.FileName))
                    {
                        byte[] fileData = new byte[bytesRead - fileNameEndIndex - 1];
                        Array.Copy(buffer, fileNameEndIndex + 1, fileData, 0, fileData.Length);
                        string filePath = Path.Combine(serverInfo.SaveAs, fileName);
                        File.WriteAllBytes(filePath, fileData);
                        serverInfo.Message = $"{fileName} had saved in {serverInfo.SaveAs}";

                        SendMessage($"{fileName} had succeeded to download!");
                    }
                    else
                    {
                        serverInfo.Message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    }
                    ServerConnected?.Invoke(this, new ServerConnectedEventArgs(serverInfo));
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    serverInfo.Message = "The connection is disconnected! And the Server site had closed!";
                    ServerDisconnected?.Invoke(this, new ServerConnectedEventArgs(serverInfo));
                    break;
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e.Message);
                    serverInfo.Message = "The connection is disconnected!";
                    //ServerConnected?.Invoke(this, new ServerConnectedEventArgs(serverInfo));
                    ServerDisconnected?.Invoke(this, new ServerConnectedEventArgs(serverInfo));
                    break;
                }
                catch (Exception e)
                { 
                    Console.WriteLine(e.Message);
                    serverInfo.Message = $"{e.Message}";
                    ServerConnected?.Invoke(this, new ServerConnectedEventArgs(serverInfo));
                    //break;
                }
            }
        }

        public void Disconnect()
        {
            stream?.Close();
            client?.Close();
        }

        public string GetServerIP()
        {
            return serverInfo.IP;
        }

        public string GetServerMessage()
        {
            return serverInfo.Message;
        }

        public int GetServerPort()
        {
            return (int)serverInfo.Port;
        }

        public void SyncServerInfo(ServerInfo serverInfo)
        {
            this.serverInfo = serverInfo;
        }
    }

    public class ServerConnectedEventArgs : EventArgs
    {
        public ServerInfo ServerInfo { get; set; }

        public ServerConnectedEventArgs(ServerInfo serverInfo)
        {
            this.ServerInfo = serverInfo;
        }
    }
}
