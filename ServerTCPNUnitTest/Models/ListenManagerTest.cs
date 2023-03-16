using Moq;
using Moq.Language.Flow;
using NUnit.Framework;
using ServerTCP;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Linq;
using ServerTCP.Models.Interfaces;
using ServerTCP.Models;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace ServerTCPNUnitTest.Models
{
    [TestFixture]
    class ListenManagerTest
    {
        private IListenManager listenManager;
        private const string RemoteIpAddress = "127.0.0.1";
        private const int RemotePort = 8080;
        private ClientInfo localClientInfo;

        [SetUp]
        public void Setup()
        {
            localClientInfo = new ClientInfo
            {
                IP = "0,0,0,0",
                Port = 0,
                FileName = "test.txt"
            };
            listenManager = new ListenManager();
            listenManager.Start();
        }

        [Test]
        public void TestGetClientInfo()
        {
            using (TcpClient client = new TcpClient())
            {
                client.Connect(IPAddress.Parse(RemoteIpAddress), RemotePort);
                string message = $"Download the file:{localClientInfo.FileName}";
                byte[] bytes = Encoding.UTF8.GetBytes(message);

                NetworkStream stream = client.GetStream();
                stream.Write(bytes, 0, bytes.Length);

                // Get the local site clientInfo
                IPEndPoint localEndPoint = (IPEndPoint)client.Client.LocalEndPoint;
                IPAddress ipAddress = localEndPoint.Address;
                if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    ipAddress = ipAddress.MapToIPv4();
                }
                localClientInfo.IP = ipAddress.ToString();
                localClientInfo.Port = localEndPoint.Port;

                byte[] buffer = new byte[1024];
                int read = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, read);
                //Assert.AreEqual("Hi, Client!", response);

                ClientInfo RemoteTCP = listenManager.GetClientInfo();

                Assert.AreEqual(localClientInfo.IP, RemoteTCP.IP);
                Assert.AreEqual(localClientInfo.Port, RemoteTCP.Port);
                Assert.AreEqual(localClientInfo.FileName, RemoteTCP.FileName);
            }
        }

        [Test]
        public void TestSendFile()
        {
            using (TcpClient client = new TcpClient())
            {
                client.Connect(IPAddress.Parse(RemoteIpAddress), RemotePort);
                string message = $"Download the file:{localClientInfo.FileName}";
                byte[] bytes = Encoding.UTF8.GetBytes(message);

                NetworkStream stream = client.GetStream();
                string response = ReadMessage(ref stream);

                stream.Write(bytes, 0, bytes.Length);

                // Get the download file
                byte[] buffer = new byte[1024];
                response = ReadMessage(ref stream);
                //File.WriteAllBytes(localClientInfo.FileName, buffer.Take(read).ToArray());
                string checkFile = Encoding.UTF8.GetString(listenManager.SendFile(localClientInfo.FileName));
                Assert.AreEqual(checkFile, response);
            }
        }

        [TearDown]
        public void TearDown()
        {
            listenManager.Close();
        }

        [Test]
        public void TestSocketService()
        {
            using (TcpClient client = new TcpClient())
            {
                client.Connect(IPAddress.Parse(RemoteIpAddress), RemotePort);
                string message = "Hello, Server";
                byte[] bytes = Encoding.UTF8.GetBytes(message);

                NetworkStream stream = client.GetStream();                
                string response = ReadMessage(ref stream);

                stream.Write(bytes, 0, bytes.Length);
                response = ReadMessage(ref stream);
                Assert.AreEqual("Hi, Client!", response);
            }
        }

        private string ReadMessage(ref NetworkStream stream) 
        {
            byte[] buffer = new byte[1024];
            int read = stream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, read);
        }
    }
}
