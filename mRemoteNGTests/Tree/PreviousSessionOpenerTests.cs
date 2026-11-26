using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using mRemoteNG.UI.Panels;
using mRemoteNG.Connection;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls.ConnectionTree;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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
            _connectionInitiator.OpenConnectionAndWaitAsync(default, default, default)
                                .ReturnsForAnyArgs(Task.FromResult(true));
            _previousSessionOpener = new PreviousSessionOpener(_connectionInitiator);
            _connectionTree = Substitute.For<IConnectionTree>();
            _connectionTree.GetRootConnectionNode().Returns(BuildTree());
        }

        [Test]
        public async Task AllRequestedSessionsAreReopened()
        {
            await _previousSessionOpener.ReopenAsync(_connectionTree);

            await _connectionInitiator.ReceivedWithAnyArgs(2).OpenConnectionAndWaitAsync(default, default, default);
        }

        [Test]
        public void ExceptionThrownWhenConstructorGivenNullArg()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new PreviousSessionOpener(null));
        }

        [Test]
        public async Task ConnectionsReopenInThePanelTheyWereIn()
        {
            RootNodeInfo root = new(RootNodeType.Connection);
            ConnectionInfo inProd = new() { PleaseConnect = true };
            ConnectionInfo withoutPanel = new() { PleaseConnect = true };
            root.AddChild(inProd);
            root.AddChild(withoutPanel);
            _connectionTree.GetRootConnectionNode().Returns(root);
            PreviousSessionOpener opener = new(_connectionInitiator,
                () => Panels(new Dictionary<string, string> { [inProd.ConstantID] = "Prod" }));

            await opener.ReopenAsync(_connectionTree);

            await _connectionInitiator.Received(1).OpenConnectionAndWaitAsync(inProd, "Prod", null);
            await _connectionInitiator.Received(1).OpenConnectionAndWaitAsync(withoutPanel, null, null);
        }

        [Test]
        public async Task FloatingPanelsGetTheirPlacement()
        {
            RootNodeInfo root = new(RootNodeType.Connection);
            ConnectionInfo server = new() { PleaseConnect = true };
            root.AddChild(server);
            _connectionTree.GetRootConnectionNode().Returns(root);
            FloatingPanelPlacement placement = new(new Rectangle(10, 20, 800, 600), true);
            PreviousSessionOpener opener = new(_connectionInitiator,
                () => Panels(new Dictionary<string, string> { [server.ConstantID] = "Floating" },
                             new Dictionary<string, FloatingPanelPlacement> { ["Floating"] = placement }));

            await opener.ReopenAsync(_connectionTree);

            await _connectionInitiator.Received(1).OpenConnectionAndWaitAsync(server, "Floating", placement);
        }

        [Test]
        public async Task SessionPanelsAreNotReadWhenThereIsNothingToReopen()
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

            await opener.ReopenAsync(_connectionTree);

            Assert.That(panelsRead, Is.False);
            await _connectionInitiator.DidNotReceiveWithAnyArgs().OpenConnectionAndWaitAsync(default, default, default);
        }

        [Test]
        public async Task EachConnectionWaitsForTheOneBefore()
        {
            RootNodeInfo root = new(RootNodeType.Connection);
            ConnectionInfo first = new() { PleaseConnect = true };
            ConnectionInfo second = new() { PleaseConnect = true };
            root.AddChild(first);
            root.AddChild(second);
            _connectionTree.GetRootConnectionNode().Returns(root);
            TaskCompletionSource<bool> firstOutcome = new();
            _connectionInitiator.OpenConnectionAndWaitAsync(first, Arg.Any<string>(), Arg.Any<FloatingPanelPlacement>())
                                .Returns(firstOutcome.Task);

            Task reopening = _previousSessionOpener.ReopenAsync(_connectionTree);

            await _connectionInitiator.Received(1).OpenConnectionAndWaitAsync(first, null, null);
            await _connectionInitiator.DidNotReceive().OpenConnectionAndWaitAsync(second, Arg.Any<string>(), Arg.Any<FloatingPanelPlacement>());

            // The first connection timed out at its login prompt: the next one still opens
            firstOutcome.SetResult(false);
            await reopening;

            await _connectionInitiator.Received(1).OpenConnectionAndWaitAsync(second, null, null);
        }

        [Test]
        public async Task AConnectionThatThrowsDoesNotStopTheOthers()
        {
            RootNodeInfo root = new(RootNodeType.Connection);
            ConnectionInfo failing = new() { PleaseConnect = true, Name = "failing" };
            ConnectionInfo next = new() { PleaseConnect = true };
            root.AddChild(failing);
            root.AddChild(next);
            _connectionTree.GetRootConnectionNode().Returns(root);
            _connectionInitiator.OpenConnectionAndWaitAsync(failing, Arg.Any<string>(), Arg.Any<FloatingPanelPlacement>())
                                .ThrowsAsync(new InvalidOperationException("test"));

            await _previousSessionOpener.ReopenAsync(_connectionTree);

            await _connectionInitiator.Received(1).OpenConnectionAndWaitAsync(next, null, null);
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
