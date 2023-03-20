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
        private IClientManager clientManager;
        private Mock<IFileManager> fileManagerMock;
        private Mock<IClientManager> clientManagerMock;
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
            clientManagerMock = new Mock<IClientManager>();
            listenManager = new ListenManager(fileManagerMock.Object, clientManagerMock.Object, RemotePort);
            listenManager.Start();
        }

        //[Test]
        //public void TestGetClientInfo()
        //{
        //    using (TcpClient client = new TcpClient())
        //    {
        //        client.Connect(IPAddress.Parse(RemoteIpAddress), RemotePort);
        //        string message = $"Download the file:{localClientInfo.FileName}";
        //        byte[] bytes = Encoding.UTF8.GetBytes(message);

        //        NetworkStream stream = client.GetStream();
        //        stream.Write(bytes, 0, bytes.Length);

        //        // Get the local site clientInfo
        //        IPEndPoint localEndPoint = (IPEndPoint)client.Client.LocalEndPoint;
        //        IPAddress ipAddress = localEndPoint.Address;
        //        if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
        //        {
        //            ipAddress = ipAddress.MapToIPv4();
        //        }
        //        localClientInfo.IP = ipAddress.ToString();
        //        localClientInfo.Port = localEndPoint.Port;

        //        byte[] buffer = new byte[1024];
        //        int read = stream.Read(buffer, 0, buffer.Length);
        //        string response = Encoding.UTF8.GetString(buffer, 0, read);
        //        //Assert.AreEqual("Hi, Client!", response);

        //        ClientInfo RemoteTCP = listenManager.GetClientInfo();

        //        Assert.AreEqual(localClientInfo.IP, RemoteTCP.IP);
        //        Assert.AreEqual(localClientInfo.Port, RemoteTCP.Port);
        //        Assert.AreEqual(localClientInfo.FileName, RemoteTCP.FileName);
        //    }
        //}

        [TearDown]
        public void TearDown()
        {
            listenManager.Close();
            listenManager = null;
        }
    }
}
