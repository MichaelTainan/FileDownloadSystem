using NUnit.Framework;
using ServerTCP;
using System.IO;
using System;

namespace Tests
{
    [TestFixture]
    public class FileManagerTest
    {
        private FileManager fileManager;
        private string fileName;

        [SetUp]
        public void Setup()
        {
            fileManager = new FileManager();
            fileName = "test.txt";
        }

        [Test]
        public void TestSendFile()
        {
            var file = fileManager.SendFile(fileName);
            Assert.NotNull(file);

        }

        [Test]
        public void TestFindFile()
        {
            Assert.IsTrue(fileManager.FindFile(fileName));
        }

        [Test]
        public void TestFindFilePath()
        {
            var message = fileManager.FindFilePath(fileName);
            Console.WriteLine(message);
            Assert.That(fileName, Is.EqualTo(Path.GetFileName(message)),
                "error: File name is not the same");
        }
    }
}