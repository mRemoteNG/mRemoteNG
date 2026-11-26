using System;
using System.Collections.Generic;
using System.Drawing;
using mRemoteNG.UI.Panels;
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
                () => Panels(new Dictionary<string, string> { [inProd.ConstantID] = "Prod" }));

            opener.Execute(_connectionTree);

            _connectionInitiator.Received(1).OpenConnectionInPanel(inProd, "Prod", null);
            _connectionInitiator.DidNotReceive().OpenConnection(inProd);
            _connectionInitiator.Received(1).OpenConnection(withoutPanel);
        }

        [Test]
        public void FloatingPanelsGetTheirPlacement()
        {
            RootNodeInfo root = new(RootNodeType.Connection);
            ConnectionInfo server = new() { PleaseConnect = true };
            root.AddChild(server);
            _connectionTree.GetRootConnectionNode().Returns(root);
            FloatingPanelPlacement placement = new(new Rectangle(10, 20, 800, 600), true);
            PreviousSessionOpener opener = new(_connectionInitiator,
                () => Panels(new Dictionary<string, string> { [server.ConstantID] = "Floating" },
                             new Dictionary<string, FloatingPanelPlacement> { ["Floating"] = placement }));

            opener.Execute(_connectionTree);

            _connectionInitiator.Received(1).OpenConnectionInPanel(server, "Floating", placement);
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
                return SessionPanels.Empty;
            });

            opener.Execute(_connectionTree);

            Assert.That(panelsRead, Is.False);
            _connectionInitiator.DidNotReceiveWithAnyArgs().OpenConnectionInPanel(default, default, default);
        }

        private static SessionPanels Panels(Dictionary<string, string> connectionPanels,
                                            Dictionary<string, FloatingPanelPlacement> floatingPanels = null)
        {
            return new SessionPanels(connectionPanels, floatingPanels ?? new Dictionary<string, FloatingPanelPlacement>());
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