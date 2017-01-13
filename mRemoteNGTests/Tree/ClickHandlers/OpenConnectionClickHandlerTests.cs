using System;
using mRemoteNG.Connection;
using mRemoteNG.Tree;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Tree
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
        public void ExceptionThrownWhenConstructorGivenNullArg()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new OpenConnectionClickHandler(null));
        }
    }
}
