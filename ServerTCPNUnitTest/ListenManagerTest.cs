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

namespace Tests
{
    [TestFixture]
    class ListenManagerTest
    {
        private ListenManager listenManager;
        private const string IpAddress = "127.0.0.1";
        private const int Port = 8080;
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
                client.Connect(IPAddress.Parse(IpAddress), Port);
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
                client.Connect(IPAddress.Parse(IpAddress), Port);
                string message = $"Download the file:{localClientInfo.FileName}";
                byte[] bytes = Encoding.UTF8.GetBytes(message);

                NetworkStream stream = client.GetStream();
                stream.Write(bytes, 0, bytes.Length);

                // Get the download file
                byte[] buffer = new byte[1024];
                int read = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, read);
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
                client.Connect(IPAddress.Parse(IpAddress), Port);
                string message = "Hello, Server";
                byte[] bytes = Encoding.UTF8.GetBytes(message);

                NetworkStream stream = client.GetStream();
                stream.Write(bytes, 0, bytes.Length);

                byte[] buffer = new byte[1024];
                int read = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, read);

                Assert.AreEqual("Hi, Client!", response);
            }
        }

        //[Test]
        //public void TestSocketServiceByMock()
        //{
        //    var mockClient = new Mock<TcpClient>();
        //    var mockStream = new Mock<NetworkStream>();
        //    string message = "Hello, Server";
        //    byte[] bytes = Encoding.UTF8.GetBytes(message);

        //    mockClient.Setup(x => x.Connect(IPAddress.Parse(IpAddress), Port)).Verifiable();
        //    mockClient.Setup(x => x.GetStream()).Returns(mockStream.Object);
        //    mockStream.Setup(x => x.Write(bytes, 0, bytes.Length)).Verifiable();
        //    mockStream.Setup(x => x.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
        //      .Callback<byte[], int, int>((buffer, offset, count) => Encoding.UTF8.GetBytes("Hi, Client!").CopyTo(buffer, 0))
        //      .Returns(Encoding.UTF8.GetBytes("Hi, Client!").Length);

        //    var client = mockClient.Object;

        //    // Assert
        //    using (client)
        //    {
        //        client.Connect(IPAddress.Parse(IpAddress), Port);

        //        var stream = client.GetStream();
        //        stream.Write(bytes, 0, bytes.Length);

        //        var buffer = new byte[1024];
        //        var read = stream.Read(buffer, 0, buffer.Length);
        //        var response = Encoding.UTF8.GetString(buffer, 0, read);

        //        Assert.AreEqual("Hi, Client!", response);
        //    }

        //    // 檢查 Mock 的行為是否被呼叫過
        //    mockStream.Verify();

        //}

    }
}
