using System;
using mRemoteNG.Connection;
using mRemoteNG.Config.Putty;
using mRemoteNG.Tree.Root;
using mRemoteNG.Tree.ClickHandlers;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Controls.ConnectionTree
{
    /// <summary>
    /// Tracks mouse clicks on tree nodes and triggers an inline rename
    /// when the same already-selected node is clicked a second time
    /// after a short delay (Explorer-style slow-click rename).
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed class SlowClickRenameHandler : ISlowClickRenameHandler
    {
        private readonly ISlowClickRenameTimer _timer;
        private readonly Action _triggerRename;
        private readonly Func<ConnectionInfo> _getSelectedNode;
        private ConnectionInfo _pendingNode;

        public SlowClickRenameHandler(
            ISlowClickRenameTimer timer,
            Action triggerRename,
            Func<ConnectionInfo> getSelectedNode)
        {
            _timer = timer ?? throw new ArgumentNullException(nameof(timer));
            _triggerRename = triggerRename ?? throw new ArgumentNullException(nameof(triggerRename));
            _getSelectedNode = getSelectedNode ?? throw new ArgumentNullException(nameof(getSelectedNode));
            _timer.Tick += OnTimerTick;
        }

        /// <inheritdoc />
        public void Execute(ConnectionInfo clickedNode)
        {
            if (!IsRenameEligible(clickedNode))
            {
                Cancel();
                return;
            }

            if (_pendingNode == null)
            {
                // First click on this node — record it, wait to see if a slow second click follows
                _pendingNode = clickedNode;
                return;
            }

            if (_pendingNode == clickedNode)
            {
                // Slow second click on the same node — start rename timer
                _timer.Stop();
                _timer.Start();
            }
            else
            {
                // Clicked a different node — cancel and track the new one
                Cancel();
                _pendingNode = clickedNode;
            }
        }

        public void Cancel()
        {
            _timer.Stop();
            _pendingNode = null;
        }

        /// <summary>
        /// Cancels the pending rename only if the selection has moved to a different node.
        /// This avoids interfering with clicks on the already-selected node, where
        /// SelectionChanged fires after NodeMouseClick due to Application.Idle timing.
        /// </summary>
        public void CancelIfDifferentNode(ConnectionInfo currentlySelected)
        {
            if (_pendingNode != null && _pendingNode != currentlySelected)
                Cancel();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            _timer.Stop();
            if (_pendingNode != null && _pendingNode == _getSelectedNode())
                _triggerRename();
            _pendingNode = null;
        }

        private static bool IsRenameEligible(ConnectionInfo node) =>
            node is not null
            && node is not RootNodeInfo
            && node is not PuttySessionInfo
            && node is not RootPuttySessionsNodeInfo;

        public void Dispose()
        {
            _timer.Tick -= OnTimerTick;
            _timer.Dispose();
        }
    }
}