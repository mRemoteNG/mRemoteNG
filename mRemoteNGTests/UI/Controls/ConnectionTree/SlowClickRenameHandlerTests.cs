using System;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls.ConnectionTree;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls.ConnectionTree
{
    [TestFixture]
    public class SlowClickRenameHandlerTests
    {
        private ISlowClickRenameTimer _timer;
        private bool _renameTriggered;
        private ConnectionInfo _selectedNode;
        private SlowClickRenameHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _timer = Substitute.For<ISlowClickRenameTimer>();
            _renameTriggered = false;
            _selectedNode = null;
            _sut = new SlowClickRenameHandler(
                _timer,
                () => _renameTriggered = true,
                () => _selectedNode);
        }

        [TearDown]
        public void TearDown() => _sut.Dispose();

        // ── Constructor guards ──────────────────────────────────────────────

        [Test]
        public void Constructor_NullTimer_Throws() =>
            Assert.Throws<ArgumentNullException>(() =>
                new SlowClickRenameHandler(null, () => { }, () => null));

        [Test]
        public void Constructor_NullTriggerRename_Throws() =>
            Assert.Throws<ArgumentNullException>(() =>
                new SlowClickRenameHandler(_timer, null, () => null));

        [Test]
        public void Constructor_NullGetSelectedNode_Throws() =>
            Assert.Throws<ArgumentNullException>(() =>
                new SlowClickRenameHandler(_timer, () => { }, null));

        // ── OnNodeClick ─────────────────────────────────────────────────────

        [Test]
        public void Execute_FirstClick_DoesNotStartTimer()
        {
            _sut.Execute(new ConnectionInfo());
            _timer.DidNotReceive().Start();
        }

        [Test]
        public void Execute_SlowSecondClickOnSameNode_StartsTimer()
        {
            var node = new ConnectionInfo();
            _sut.Execute(node);
            _sut.Execute(node);
            _timer.Received(1).Start();
        }

        [Test]
        public void Execute_ClickDifferentNodeOnSecondClick_DoesNotStartTimer()
        {
            _sut.Execute(new ConnectionInfo());
            _sut.Execute(new ConnectionInfo());
            _timer.DidNotReceive().Start();
        }

        [Test]
        public void Execute_ClickDifferentNodeOnSecondClick_CancelsCalled()
        {
            _sut.Execute(new ConnectionInfo());
            _timer.ClearReceivedCalls();
            _sut.Execute(new ConnectionInfo());
            _timer.Received().Stop();
        }

        [Test]
        public void OnNodeClick_NullNode_CancelsWithoutStartingTimer()
        {
            _sut.Execute(new ConnectionInfo());
            _sut.Execute(null);
            _timer.DidNotReceive().Start();
        }

        [TestCase(RootNodeType.Connection)]
        [TestCase(RootNodeType.PuttySessions)]
        public void OnNodeClick_RootNodeInfo_IsNotEligible(RootNodeType rootNodeType)
        {
            var root = new RootNodeInfo(rootNodeType);
            _sut.Execute(root);
            _sut.Execute(root);
            _timer.DidNotReceive().Start();
        }

        [Test]
        public void OnNodeClick_PuttySessionInfo_IsNotEligible()
        {
            var puttySession = new PuttySessionInfo();
            _sut.Execute(puttySession);
            _sut.Execute(puttySession);
            _timer.DidNotReceive().Start();
        }

        // ── Cancel ──────────────────────────────────────────────────────────

        [Test]
        public void Cancel_StopsTimer()
        {
            _sut.Execute(new ConnectionInfo());
            _sut.Cancel();
            _timer.Received().Stop();
        }

        [Test]
        public void Cancel_ClearsPendingNode_SoNextClickIsFirstClick()
        {
            var node = new ConnectionInfo();
            _sut.Execute(node);
            _sut.Cancel();
            _timer.ClearReceivedCalls();

            // Next click should be treated as first click — no timer
            _sut.Execute(node);
            _timer.DidNotReceive().Start();
        }

        // ── CancelIfDifferentNode ───────────────────────────────────────────

        [Test]
        public void CancelIfDifferentNode_NoPendingNode_DoesNotCancel()
        {
            _sut.CancelIfDifferentNode(new ConnectionInfo());
            _timer.DidNotReceive().Stop();
        }

        [Test]
        public void CancelIfDifferentNode_SameNode_DoesNotCancel()
        {
            var node = new ConnectionInfo();
            _sut.Execute(node);
            _timer.ClearReceivedCalls();
            _sut.CancelIfDifferentNode(node);
            _timer.DidNotReceive().Stop();
        }

        [Test]
        public void CancelIfDifferentNode_DifferentNode_Cancels()
        {
            _sut.Execute(new ConnectionInfo());
            _timer.ClearReceivedCalls();
            _sut.CancelIfDifferentNode(new ConnectionInfo());
            _timer.Received().Stop();
        }

        // ── Timer tick ──────────────────────────────────────────────────────

        [Test]
        public void TimerTick_NodeStillSelected_TriggersRename()
        {
            var node = new ConnectionInfo();
            _selectedNode = node;
            _sut.Execute(node);
            _sut.Execute(node);

            _timer.Tick += Raise.EventWith(new object(), EventArgs.Empty);

            Assert.That(_renameTriggered, Is.True);
        }

        [Test]
        public void TimerTick_NodeNoLongerSelected_DoesNotTriggerRename()
        {
            var node = new ConnectionInfo();
            _selectedNode = new ConnectionInfo(); // different node now selected
            _sut.Execute(node);
            _sut.Execute(node);

            _timer.Tick += Raise.EventWith(new object(), EventArgs.Empty);

            Assert.That(_renameTriggered, Is.False);
        }

        [Test]
        public void TimerTick_NoPendingNode_DoesNotTriggerRename()
        {
            _timer.Tick += Raise.EventWith(new object(), EventArgs.Empty);
            Assert.That(_renameTriggered, Is.False);
        }

        [Test]
        public void TimerTick_StopsTimerBeforeInvokingRename()
        {
            var node = new ConnectionInfo();
            _selectedNode = node;
            _sut.Execute(node);
            _sut.Execute(node);

            _timer.Tick += Raise.EventWith(new object(), EventArgs.Empty);

            _timer.Received().Stop();
        }

        // ── Dispose ─────────────────────────────────────────────────────────

        [Test]
        public void Dispose_DisposesTimer()
        {
            _sut.Dispose();
            _timer.Received().Dispose();
        }

        [Test]
        public void Dispose_AfterDispose_TickNoLongerTriggersRename()
        {
            var node = new ConnectionInfo();
            _selectedNode = node;
            _sut.Execute(node);
            _sut.Execute(node);
            _sut.Dispose();

            // Event is unsubscribed — raising tick must not invoke OnTimerTick
            _timer.Tick += Raise.EventWith(new object(), EventArgs.Empty);

            Assert.That(_renameTriggered, Is.False);
        }
    }
}