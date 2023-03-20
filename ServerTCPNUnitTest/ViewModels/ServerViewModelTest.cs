using Moq;
using NUnit.Framework;
using ServerTCP;
using ServerTCP.Models.Interfaces;
using ServerTCP.ViewModels;

namespace ServerTCPNUnitTest.ViewModels
{
    [TestFixture]
    class ServerViewModelTest
    {
        private Mock<IClientManager> mockClientManager;
        private Mock<IListenManager> mockListenManager;
        private ServerViewModel serverViewModel;

        [SetUp]
        public void SetUp() 
        {
            mockListenManager = new Mock<IListenManager>();
            mockClientManager = new Mock<IClientManager>();
            serverViewModel = new ServerViewModel(mockListenManager.Object, mockClientManager.Object);
        }

        [Test]
        public void TestStartCommand_ShouldCall() 
        {
            //Arrange
            mockListenManager.Setup(x => x.Start());

            //Act
            serverViewModel.StartCommand.Execute(null);
            //Assert
            mockListenManager.Verify(x => x.Start(), Times.Once);
            Assert.AreEqual(serverViewModel.IsRunning, true);
        }

        [Test]
        public void TestStartCommand_ShouldNotCall()
        {
            //Arrange
            mockListenManager.Setup(x => x.Start());
            serverViewModel.IsRunning = true;
            //Act
            serverViewModel.StartCommand.Execute(null);
            //Assert
            mockListenManager.Verify(x => x.Start(), Times.Never);
        }

        [Test]
        public void TestStopCommand_ShouldCall()
        {
            //Arrange
            mockListenManager.Setup(x => x.Close());
            serverViewModel.IsRunning = true;
            //Act
            serverViewModel.StopCommand.Execute(null);
            //Assert
            mockListenManager.Verify(x => x.Close(), Times.Once);
            Assert.AreEqual(serverViewModel.IsRunning, false);
        }

        [Test]
        public void TestStopCommand_ShouldNotCall()
        {
            //Arrange
            mockListenManager.Setup(x => x.Close());
            //Act
            serverViewModel.StopCommand.Execute(null);
            //Assert
            mockListenManager.Verify(x => x.Close(), Times.Never);
        }

        [Test]
        public void TestCanStart_WhenNotRun()
        {
            //Arrange
            serverViewModel.IsRunning = false;

            //Act
            var result = serverViewModel.StartCommand.CanExecute(null);
            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestCanStop_WhenRun()
        {
            //Arrange
            serverViewModel.IsRunning = true;

            //Act
            var result = serverViewModel.StopCommand.CanExecute(null);
            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestPropertyChanged()
        {
            var isPropertyChangedInvoked = false;
            serverViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "IsRunning")
                {
                    isPropertyChangedInvoked = true;
                }
            };

            serverViewModel.IsRunning = !serverViewModel.IsRunning;
            Assert.IsTrue(isPropertyChangedInvoked);
        }
    }
}
