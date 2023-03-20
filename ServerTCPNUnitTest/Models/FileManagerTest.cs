using NUnit.Framework;
using ServerTCP;
using System.IO;
using System;
using ServerTCP.Models.Interfaces;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Moq;

namespace ServerTCPNUnitTest.Models
{
    [TestFixture]
    public class FileManagerTest
    {
        private IFileManager fileManager;
        private string fileName;
        private Mock<Stream> stremMock;

        [SetUp]
        public void Setup()
        {
            fileManager = new FileManager();
            fileName = "test.txt";
        }

        [Test]
        public void TestChangeFileBeByteArray()
        {
            var file = fileManager.ChangeFileBeByteArray(fileName);
            Assert.NotNull(file);

        }

        [Test]
        public void TestFindFile()
        {
            Assert.IsTrue(fileManager.FindFile(fileName));
        }

        [Test]
        public void TestCombineFilePath()
        {
            var message = fileManager.CombineFilePath(fileName);
            Console.WriteLine(message);
            Assert.That(fileName, Is.EqualTo(Path.GetFileName(message)),
                "error: File name is not the same");
        }
        
        [Test]
        public void TestCombineFileContentAndName() 
        {
            //Arrange
            byte[] fileContent = Encoding.UTF8.GetBytes("This is a test file.");
            //CombineFileContentAndName method actually behavior
            byte[] expectBuffer = new byte[fileContent.Length + fileName.Length + 1];
            Array.Copy(Encoding.UTF8.GetBytes(fileName), expectBuffer, fileName.Length);
            expectBuffer[fileName.Length] = 0; // Add 0 after the filName to be a separator
            Array.Copy(fileContent, 0, expectBuffer, fileName.Length + 1, fileContent.Length);

            //Act
            byte[] actualFileBuffer = fileManager.CombineFileContentAndName(fileName);

            //Assert
            Assert.AreEqual(expectBuffer, actualFileBuffer);
        }
    }
}