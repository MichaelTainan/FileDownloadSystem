using ClientTCP;
using ClientTCP.Interfaces;

namespace ClientTCPNUnitTest
{
    [TestFixture]
    public class RecordManagerTesT
    {
        private IRecordManager recordManager;
        private ServerInfo serverInfo;
        [SetUp]
        public void Setup()
        {
            serverInfo = new ServerInfo();
            recordManager = new RecordManager(serverInfo);
        }

        [Test]
        public void TestGetIP()
        {
            //Arrange
            serverInfo.IP = "127.0.0.1";
            recordManager.SaveServerInfoFromClient(serverInfo);
            //Act
            var ip = recordManager.GetServerInfo().IP;
            //Assert
            Assert.IsNotNull(ip);
            Assert.That(ip, Is.EqualTo(serverInfo.IP));
        }

        [Test]
        public void TestGetPort()
        {
            //Arrange
            serverInfo.Port = 8080;
            recordManager.SaveServerInfoFromClient(serverInfo);
            //Act
            var port = recordManager.GetServerInfo().Port;
            //Assert
            Assert.IsNotNull(port);
            Assert.That(port, Is.EqualTo(serverInfo.Port));
        }

        [Test]
        public void TestGetFileName()
        {
            //Arrange
            serverInfo.FileName = "test.txt";
            recordManager.SaveServerInfoFromClient(serverInfo);
            //Act
            var fileName = recordManager.GetServerInfo().FileName;
            //Assert
            Assert.IsNotNull(fileName);
            Assert.That(fileName, Is.EqualTo(serverInfo.FileName));
        }

        [Test]
        public void TestGetSaveAsPath()
        {
            //Arrange
            serverInfo.SaveAs = @"C:\Download";
            recordManager.SaveServerInfoFromClient(serverInfo);
            //Act
            var saveAsPath = recordManager.GetServerInfo().SaveAs;
            //Assert
            Assert.IsNotNull(saveAsPath);
            Assert.That(saveAsPath, Is.EqualTo(serverInfo.SaveAs));
        }

        [Test]
        public void TestGetMessage()
        {
            //Arrange
            string message = "This is a Test message";
            recordManager.SaveServerInfoFromServer(message);
            //Act
            var serverInfoMessage = recordManager.GetServerInfo().Message;
            //Assert
            Assert.IsNotNull(serverInfoMessage);
            //Because SaveServerInfoFromServer() will auto add "\n", Is.EqualTo() have to used (message + "\n") to test
            Assert.That(serverInfoMessage, Is.EqualTo(message + "\n"));
        }
    }
}