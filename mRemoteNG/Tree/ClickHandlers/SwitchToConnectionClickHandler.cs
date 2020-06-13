using System;
using mRemoteNG.Connection;

namespace mRemoteNG.Tree.ClickHandlers
{
    public class SwitchToConnectionClickHandler : ITreeNodeClickHandler<ConnectionInfo>
    {
        private readonly IConnectionInitiator _connectionInitiator;

        public SwitchToConnectionClickHandler(IConnectionInitiator connectionInitiator)
        {
            if (connectionInitiator == null)
                throw new ArgumentNullException(nameof(connectionInitiator));
            _connectionInitiator = connectionInitiator;
        }

        public void Execute(ConnectionInfo clickedNode)
        {
            if (clickedNode == null)
                throw new ArgumentNullException(nameof(clickedNode));
            if (clickedNode.GetTreeNodeType() != TreeNodeType.Connection &&
                clickedNode.GetTreeNodeType() != TreeNodeType.PuttySession) return;
            _connectionInitiator.SwitchToOpenConnection(clickedNode);
        }
    }
}