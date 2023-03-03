using System;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree.ClickHandlers;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Tree.ClickHandlers
{
    public class OpenConnectionClickHandlerTests
    {
        private OpenConnectionClickHandler _clickHandler;
        private IConnectionInitiator _connectionInitiator;

        [SetUp]
        public void Setup()
        {
            _connectionInitiator = Substitute.For<IConnectionInitiator>();
            _clickHandler = new OpenConnectionClickHandler(_connectionInitiator);
        }

        [Test]
        public void ConnectionOpened()
        {
            var connectionInfo = new ConnectionInfo();
            _clickHandler.Execute(connectionInfo);
            _connectionInitiator.Received().OpenConnection(connectionInfo);
        }

        [Test]
        public void DoesNothingWhenGivenContainerInfo()
        {
            _clickHandler.Execute(new ContainerInfo());
            _connectionInitiator.DidNotReceiveWithAnyArgs().OpenConnection(new ConnectionInfo());
        }

        [Test]
        public void ExceptionThrownWhenConstructorGivenNullArg()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new OpenConnectionClickHandler(null));
        }

        [Test]
        public void ThrowWhenExecuteGivenNullArg()
        {
            Assert.Throws<ArgumentNullException>(() => _clickHandler.Execute(null));
        }
    }
}
