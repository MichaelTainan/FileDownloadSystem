using NUnit.Framework;
using ServerTCP.Models;
using ServerTCP.ViewModels;

namespace ServerTCPNUnitTest.ViewModels
{
    [TestFixture]
    class ClientViewModelTest
    {
        private ClientInfo clientInfo;
        private ClientViewModel clientViewModel;

        [SetUp]
        public void SetUp()
        {

            clientInfo = new ClientInfo
            {
                IP = "127.0.0.1",
                Port = 8080
            };
            clientViewModel = new ClientViewModel(clientInfo);
        }

        [Test]
        public void TestPropertyChanged()
        {
            var eventRaised = false;
            clientViewModel.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "FileName") 
                    {
                        eventRaised = true;
                    }
                };

            clientViewModel.FileName = "test.txt";
            Assert.AreEqual("test.txt", clientViewModel.FileName);
            Assert.IsTrue(eventRaised);
        }
    }
}