using mRemoteNG.Connection;


namespace mRemoteNG.Tree
{
    public class OpenConnectionClickHandler : ITreeNodeClickHandler
    {
        private readonly IConnectionInitiator _connectionInitiator;

        public OpenConnectionClickHandler(IConnectionInitiator connectionInitiator)
        {
            _connectionInitiator = connectionInitiator;
        }

        public void Execute(ConnectionInfo clickedNode)
        {
            if (clickedNode == null) return;
            if (clickedNode.GetTreeNodeType() != TreeNodeType.Connection && clickedNode.GetTreeNodeType() != TreeNodeType.PuttySession) return;
            _connectionInitiator.OpenConnection(clickedNode);
        }
    }
}