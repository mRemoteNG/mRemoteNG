using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Connection;

namespace mRemoteNG.Tree.ClickHandlers
{
    [SupportedOSPlatform("windows")]
    public class OpenConnectionClickHandler : ITreeNodeClickHandler<ConnectionInfo>
    {
        private readonly IConnectionInitiator _connectionInitiator;

        public OpenConnectionClickHandler(IConnectionInitiator connectionInitiator)
        {
            ArgumentNullException.ThrowIfNull(connectionInitiator);
            _connectionInitiator = connectionInitiator;
        }

        public void Execute(ConnectionInfo clickedNode)
        {
            ArgumentNullException.ThrowIfNull(clickedNode);

            var nodeType = clickedNode.GetTreeNodeType();
            bool isConnectable = nodeType == TreeNodeType.Connection ||
                                 nodeType == TreeNodeType.PuttySession ||
                                 (nodeType == TreeNodeType.Container && !string.IsNullOrEmpty(clickedNode.Hostname));

            if (!isConnectable) return;

            // Ctrl+DoubleClick opens a new connection tab even if one is already open (#397)
            var force = Control.ModifierKeys.HasFlag(Keys.Control) || Properties.Settings.Default.DoubleClickOpensNewConnection
                ? ConnectionInfo.Force.DoNotJump
                : ConnectionInfo.Force.None;
            _connectionInitiator.OpenConnection(clickedNode, force);
        }
    }
}