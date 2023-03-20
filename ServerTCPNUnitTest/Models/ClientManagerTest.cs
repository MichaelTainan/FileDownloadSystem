using NUnit.Framework;
using ServerTCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace ServerTCPNUnitTest.Models
{
    [TestFixture]
    internal class ClientManagerTest
    {
        private Mock<NetworkStream> streamMock;
        private ClientManager clientManager;
        [SetUp]
        public void SetUp() 
        {
            clientManager = new ClientManager();
            clientManager.IP = "127.0.0.1";
            clientManager.Port = 0;
            clientManager.FileName = "file";

            streamMock = new Mock<NetworkStream>();
        }

        [Test]
        public void TestIP() 
        {
            var ip = "127.0.0.1";
            Assert.AreEqual(ip, clientManager.IP);
        }

        [Test]
        public void TestPort()
        {
            var port = 0;
            Assert.AreEqual(port, clientManager.Port);
        }

        [Test]
        public void TestFileName()
        {
            var ip = "file";
            Assert.AreEqual(ip, clientManager.FileName);
        }
    }
}
