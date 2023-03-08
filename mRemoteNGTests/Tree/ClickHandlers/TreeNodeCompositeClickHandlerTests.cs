using System;
using mRemoteNG.Connection;
using mRemoteNG.Tree.ClickHandlers;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Tree.ClickHandlers
{
    public class TreeNodeCompositeClickHandlerTests
    {
        private TreeNodeCompositeClickHandler _clickHandler;
        private ConnectionInfo _connectionInfo;

        [SetUp]
        public void Setup()
        {
            _clickHandler = new TreeNodeCompositeClickHandler();
            _connectionInfo = new ConnectionInfo();
        }

        [Test]
        public void ExecutesAllItsHandlers()
        {
            var handler1 = Substitute.For<ITreeNodeClickHandler<ConnectionInfo>>();
            var handler2 = Substitute.For<ITreeNodeClickHandler<ConnectionInfo>>();
            _clickHandler.ClickHandlers = new[] { handler1, handler2 };
            _clickHandler.Execute(_connectionInfo);
            handler1.Received().Execute(_connectionInfo);
            handler2.Received().Execute(_connectionInfo);
        }

        [Test]
        public void ThrowWhenExecuteGivenNullArg()
        {
            Assert.Throws<ArgumentNullException>(() => _clickHandler.Execute(null));
        }
    }
}