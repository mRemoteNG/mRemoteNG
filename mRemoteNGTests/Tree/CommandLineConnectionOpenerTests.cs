using mRemoteNG.Connection;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls.ConnectionTree;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Tree
{
    public class CommandLineConnectionOpenerTests
    {
        private IConnectionInitiator _connectionInitiator = null!;
        private IConnectionTree _connectionTree = null!;
        private RootNodeInfo _rootConnectionNode = null!;
        private ConnectionInfo _connection = null!;

        [SetUp]
        public void Setup()
        {
            _connectionInitiator = Substitute.For<IConnectionInitiator>();
            _connection = new ConnectionInfo { Name = "ConnA", Hostname = "hostA" };

            _rootConnectionNode = new RootNodeInfo(RootNodeType.Connection);
            _rootConnectionNode.AddChild(_connection);

            _connectionTree = Substitute.For<IConnectionTree>();
            _connectionTree.GetRootConnectionNode().Returns(_rootConnectionNode);
        }

        [Test]
        public void Execute_OpensMatchingConnection_WhenTargetIsProvided()
        {
            var commandLineConnectionOpener = new CommandLineConnectionOpener(_connectionInitiator, "ConnA", "--startup");

            commandLineConnectionOpener.Execute(_connectionTree);

            _connectionInitiator.Received().OpenConnection(_connection);
        }

        [Test]
        public void Execute_OpensMatchingConnection_WhenRootNodeIsProvided()
        {
            var commandLineConnectionOpener = new CommandLineConnectionOpener(_connectionInitiator, "ConnA", "--startup");

            commandLineConnectionOpener.Execute(_rootConnectionNode);

            _connectionInitiator.Received().OpenConnection(_connection);
        }

        [Test]
        public void Execute_DoesNotOpenAnyConnection_WhenTargetIsEmpty()
        {
            var commandLineConnectionOpener = new CommandLineConnectionOpener(_connectionInitiator, string.Empty, "--startup");

            commandLineConnectionOpener.Execute(_connectionTree);

            _connectionInitiator.DidNotReceiveWithAnyArgs().OpenConnection(new ConnectionInfo());
        }
    }
}
