using System;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.UI.Controls;


namespace mRemoteNG.Tree
{
    public class ExpandNodeClickHandler : ITreeNodeClickHandler
    {
        private readonly ConnectionTree _connectionTree;

        public ExpandNodeClickHandler(ConnectionTree connectionTree)
        {
            if (connectionTree == null)
                throw new ArgumentNullException(nameof(connectionTree));

            _connectionTree = connectionTree;
        }

        public void Execute(ConnectionInfo clickedNode)
        {
            var clickedNodeAsContainer = clickedNode as ContainerInfo;
            if (clickedNodeAsContainer == null) return;
            _connectionTree.ToggleExpansion(clickedNodeAsContainer);
        }
    }
}