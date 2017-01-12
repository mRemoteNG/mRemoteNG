using mRemoteNG.Connection;
using mRemoteNG.Tree;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Tree
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
    }
}