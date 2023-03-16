using NUnit.Framework;
using ServerTCP;
using System.IO;
using System;
using ServerTCP.Models.Interfaces;

namespace ServerTCPNUnitTest.Models
{
    [TestFixture]
    public class FileManagerTest
    {
        private IFileManager fileManager;
        private string fileName;

        [SetUp]
        public void Setup()
        {
            fileManager = new FileManager();
            fileName = "test.txt";
        }

        [Test]
        public void TestChangeFileBeByteType()
        {
            var file = fileManager.ChangeFileBeByteType(fileName);
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
    }
}