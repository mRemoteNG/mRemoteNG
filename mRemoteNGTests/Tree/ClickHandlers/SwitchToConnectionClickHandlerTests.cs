using System;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree.ClickHandlers;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Tree.ClickHandlers
{
    public class SwitchToConnectionClickHandlerTests
    {
        private SwitchToConnectionClickHandler _clickHandler;
        private IConnectionInitiator _connectionInitiator;

        [SetUp]
        public void Setup()
        {
            _connectionInitiator = Substitute.For<IConnectionInitiator>();
            _clickHandler = new SwitchToConnectionClickHandler(_connectionInitiator);
        }

        [Test]
        public void SwitchesToConnection()
        {
            var connectionInfo = new ConnectionInfo();
            _clickHandler.Execute(connectionInfo);
            _connectionInitiator.Received().SwitchToOpenConnection(connectionInfo);
        }

        [Test]
        public void DoesNothingWhenGivenContainerInfo()
        {
            _clickHandler.Execute(new ContainerInfo());
            _connectionInitiator.DidNotReceiveWithAnyArgs().SwitchToOpenConnection(new ConnectionInfo());
        }

        [Test]
        public void ExceptionThrownWhenConstructorGivenNullArg()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new SwitchToConnectionClickHandler(null));
        }

        [Test]
        public void ThrowWhenExecuteGivenNullArg()
        {
            Assert.Throws<ArgumentNullException>(() => _clickHandler.Execute(null));
        }
    }
}