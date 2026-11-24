using System;
using System.Collections.Generic;
using mRemoteNG.Connection;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls.ConnectionTree;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Tree
{
    public class PreviousSessionOpenerTests
    {
        private PreviousSessionOpener _previousSessionOpener;
        private IConnectionInitiator _connectionInitiator;
        private IConnectionTree _connectionTree;

        [SetUp]
        public void Setup()
        {
            _connectionInitiator = Substitute.For<IConnectionInitiator>();
            _previousSessionOpener = new PreviousSessionOpener(_connectionInitiator);
            _connectionTree = Substitute.For<IConnectionTree>();
            _connectionTree.GetRootConnectionNode().Returns(BuildTree());
        }

        [Test]
        public void AllRequestedSessionsAreReopened()
        {
            _previousSessionOpener.Execute(_connectionTree);
            _connectionInitiator.ReceivedWithAnyArgs(2).OpenConnection(new ConnectionInfo());
        }

        [Test]
        public void ExceptionThrownWhenConstructorGivenNullArg()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new PreviousSessionOpener(null));
        }

        [Test]
        public void ConnectionsReopenInThePanelTheyWereIn()
        {
            RootNodeInfo root = new(RootNodeType.Connection);
            ConnectionInfo inProd = new() { PleaseConnect = true };
            ConnectionInfo withoutPanel = new() { PleaseConnect = true };
            root.AddChild(inProd);
            root.AddChild(withoutPanel);
            _connectionTree.GetRootConnectionNode().Returns(root);
            PreviousSessionOpener opener = new(_connectionInitiator,
                () => new Dictionary<string, string> { [inProd.ConstantID] = "Prod" });

            opener.Execute(_connectionTree);

            _connectionInitiator.Received(1).OpenConnectionInPanel(inProd, "Prod");
            _connectionInitiator.DidNotReceive().OpenConnection(inProd);
            _connectionInitiator.Received(1).OpenConnection(withoutPanel);
        }

        [Test]
        public void SessionPanelsAreNotReadWhenThereIsNothingToReopen()
        {
            RootNodeInfo root = new(RootNodeType.Connection);
            root.AddChild(new ConnectionInfo());
            _connectionTree.GetRootConnectionNode().Returns(root);
            bool panelsRead = false;
            PreviousSessionOpener opener = new(_connectionInitiator, () =>
            {
                panelsRead = true;
                return new Dictionary<string, string>();
            });

            opener.Execute(_connectionTree);

            Assert.That(panelsRead, Is.False);
            _connectionInitiator.DidNotReceiveWithAnyArgs().OpenConnectionInPanel(default, default);
        }

        private RootNodeInfo BuildTree()
        {
            var root = new RootNodeInfo(RootNodeType.Connection);
            root.AddChild(new ConnectionInfo { PleaseConnect = true });
            root.AddChild(new ConnectionInfo());
            root.AddChild(new ConnectionInfo { PleaseConnect = true });
            return root;
        }
    }
}