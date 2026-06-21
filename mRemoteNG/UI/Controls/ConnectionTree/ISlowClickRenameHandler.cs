using System;
using mRemoteNG.Connection;
using mRemoteNG.Tree.ClickHandlers;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Controls.ConnectionTree
{
    /// <summary>
    /// A click handler that implements Explorer-style slow-click rename behaviour,
    /// with additional lifecycle controls to cancel or scope the pending rename.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public interface ISlowClickRenameHandler : ITreeNodeClickHandler<ConnectionInfo>, IDisposable
    {
        /// <summary>Cancels any pending rename unconditionally.</summary>
        void Cancel();

        /// <summary>
        /// Cancels the pending rename only when the selection has moved
        /// away from the node that was originally clicked.
        /// </summary>
        void CancelIfDifferentNode(ConnectionInfo currentlySelected);
    }
}