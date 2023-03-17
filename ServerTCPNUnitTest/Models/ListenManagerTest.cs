using Moq;
using NUnit.Framework;
using ServerTCP;
using ServerTCP.Models;
using ServerTCP.Models.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerTCPNUnitTest.Models
{
    [TestFixture]
    class ListenManagerTest
    {
        private IListenManager listenManager;
        private Mock<IFileManager> fileManagerMock;
        private const string RemoteIpAddress = "127.0.0.1";
        private int RemotePort = 8080;
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
            fileManagerMock = new Mock<IFileManager>();
            listenManager = new ListenManager(fileManagerMock.Object);
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
            //Arrange
            byte[] fileContent = Encoding.UTF8.GetBytes($"Download the file:{localClientInfo.FileName}");
            //SendFile method actually behavior
            byte[] expectBuffer = new byte[fileContent.Length + localClientInfo.FileName.Length + 1];
            Array.Copy(Encoding.UTF8.GetBytes(localClientInfo.FileName), expectBuffer, localClientInfo.FileName.Length);
            expectBuffer[localClientInfo.FileName.Length] = 0; // Add 0 after the filName to be a separator
            Array.Copy(fileContent, 0, expectBuffer, localClientInfo.FileName.Length + 1, fileContent.Length);

            fileManagerMock.Setup(x => x.ChangeFileBeByteArray(localClientInfo.FileName)).Returns(fileContent);

            //Act
            byte[] actualFileBuffer = listenManager.SendFile(localClientInfo.FileName);

            //Assert
            Assert.AreEqual(expectBuffer, actualFileBuffer);
        }

        [TearDown]
        public void TearDown()
        {
            listenManager.Close();
            listenManager = null;
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
