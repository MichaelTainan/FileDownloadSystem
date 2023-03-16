using ClientTCP;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace ClientTCPNUnitTest
{
    [TestFixture]
    public class ConnectManagerTest
    {
        private ConnectManager connectManager;
        private ServerInfo serverInfo;
        private string expectMessage;
        private TcpListener server;
        private int testPort;

        [SetUp]
        public void Setup()
        {
            testPort = GetRandomUnusedPort();
            serverInfo = new ServerInfo
            {
                IP = "127.0.0.1",
                Port = testPort,
                FileName = "test.txt",
                SaveAs = @"C:\Download",
                Message = ""
            };
            connectManager = new ConnectManager(serverInfo);
            expectMessage = "You had connected...";
            server = new TcpListener(IPAddress.Parse(serverInfo.IP), serverInfo.Port);
            server.Start();
        }

        [Test]
        public async Task TestConnectAsync()
        {
            // Arrange
            server.BeginAcceptTcpClient(async ar =>
            {
                TcpClient serverClient = server.EndAcceptTcpClient(ar);
                // Assert
                Assert.IsTrue(serverClient.Connected);

                // Send a message from the server to the client
                byte[] message = System.Text.Encoding.UTF8.GetBytes(expectMessage);
                serverClient.GetStream().Write(message, 0, message.Length);

                // Clean up the server resources
                serverClient.Close();
                server.Stop();
            }, null);
            // Act
            await connectManager.ConnectAsync();

            // Assert
            Assert.AreEqual(connectManager.GetServerMessage(), expectMessage);
        }

        [Test]
        public void TestGetServerIP()
        {
            //Arrange
            string checkIP = "127.0.0.1";
            //Act
            var ip = connectManager.GetServerIP();
            //Assert
            Assert.IsNotNull(ip);
            Assert.That(ip, Is.EqualTo(checkIP));
        }

        [Test]
        public void TestGetServerPort()
        {
            //Arrange
            int checkPort = testPort;
            //Act
            var port = connectManager.GetServerPort();
            //Assert
            Assert.IsNotNull(port);
            Assert.That(port, Is.EqualTo(checkPort));
        }

        /// <summary>
        /// To avoid using the same TCP port across tests, use random TCP ports in unit tests 
        /// instead of running the server on a fixed TCP port.
        /// </summary>
        /// <returns></returns>
        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        [Test]
        public async Task TestSendMessage()
        {
            // Arrange
            byte[] expectMessageByte = System.Text.Encoding.UTF8.GetBytes(expectMessage);
            server.BeginAcceptTcpClient(async ar =>
            {
                TcpClient serverClient = server.EndAcceptTcpClient(ar);
                // Assert
                Assert.IsTrue(serverClient.Connected);

                // Receive a message from the client
                byte[] messageByte = new byte[1024];
                int bytesRead = await serverClient.GetStream().ReadAsync(messageByte, 0, messageByte.Length);

                // Assert
                Assert.AreEqual(messageByte, expectMessageByte);

                // Clean up the server resources
                serverClient.Close();
                server.Stop();
            }, null);

            // Act
            connectManager.ConnectAsync();
            connectManager.SendMessage(expectMessage);
        }
    }
}