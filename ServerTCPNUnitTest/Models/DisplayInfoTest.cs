using NUnit.Framework;
using ServerTCP;
using System.IO;
using System;
using ServerTCP.Models.Interfaces;
using ServerTCP.Models;

namespace ServerTCPNUnitTest.Models
{
    [TestFixture]
    class DisplayInfoTest
    {
        private IDisplayInfo displayInfo;
        private ClientInfo clientInfo;

        [SetUp]
        public void Setup()
        {
            clientInfo = new ClientInfo();
            clientInfo.IP = "127.0.0.1";
            clientInfo.Port = 8080;
            clientInfo.FileName = "test.txt";
            displayInfo = new DisplayInfo(clientInfo);
        }

        [Test]
        public void TestDisplayClientIP()
        {
            string ip = displayInfo.GetClientIP();
            Assert.AreEqual(clientInfo.IP, ip);
        }

        [Test]
        public void TestDisplayClientPort()
        {
            int port = displayInfo.GetClientPort();
            Assert.AreEqual(clientInfo.Port, port);
        }

        [Test]
        public void TestDisplayClientFile()
        {
            string fileName = displayInfo.GetClientFileName();
            Assert.AreEqual(clientInfo.FileName, fileName);
        }

        [Test]
        public void TestSendFile()
        {
            var file = displayInfo.SendFile();
            Assert.NotNull(file);

        }
    }
}
