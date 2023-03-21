using NUnit.Framework;
using ServerTCP.ViewModels;
using System;

namespace ServerTCPNUnitTest.ViewModels
{
    [TestFixture]
    internal class RelayCommandTest
    {
        private bool executeCalled;
        private bool canExecuteResult;

        [SetUp]
        public void SetUp() 
        {
            executeCalled = false;
            canExecuteResult = true;
        }

        /// <summary>
        /// Test Execute() when it canExecuteResult = true, then call Execute(), It should return true.
        /// </summary>
        [Test]
        public void TestExecute_ShouldCallExecuteAction() 
        {
            //Arrange
            var command = new RelayCommand(() => executeCalled = true, () => canExecuteResult);

            //Act
            command.Execute(null);

            //Assert
            Assert.IsTrue(executeCalled);
        }

        /// <summary>
        /// Test CanExecute() when it canExecuteResult = true, then call CanExecute(), It should return true.
        /// </summary>
        [Test]
        public void TestCanExecute_ShouldReturnTrue()
        {
            // Arrange
            var command = new RelayCommand(() => executeCalled = true, () => canExecuteResult);

            // Act
            var canExecute = command.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        /// <summary>
        /// Test CanExecute() when it canExecuteResult = false, then call CanExecute(), It should return false.
        /// </summary>
        [Test]
        public void TestCanExecute_ShouldReturnFalse()
        {
            //Arrange
            var command = new RelayCommand(() => executeCalled = true, () => canExecuteResult);

            //Act
            canExecuteResult = false;
            command.CanExecute(null);

            //Assert
            Assert.IsFalse(canExecuteResult);
        }

        /// <summary>
        /// Test CanExecute() when second argu = null, then call CanExecute(), It should return true.
        /// </summary>
        [Test]
        public void TestCanExecute_WithNullCanExecuteFunc()
        {
            // Arrange
            var command = new RelayCommand(() => executeCalled = true, null);

            // Act
            var canExecute = command.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }
    }
}
