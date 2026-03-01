using System;
using System.Runtime.Versioning;
using mRemoteNG.Connection;

namespace mRemoteNG.Tree.ClickHandlers
{
    [SupportedOSPlatform("windows")]
    public class SwitchToConnectionClickHandler : ITreeNodeClickHandler<ConnectionInfo>
    {
        private readonly IConnectionInitiator _connectionInitiator;

        public SwitchToConnectionClickHandler(IConnectionInitiator connectionInitiator)
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
            _connectionInitiator.SwitchToOpenConnection(clickedNode);
        }
    }
}