using mRemoteNG.Connection;


namespace mRemoteNG.Tree
{
    public class OpenConnectionClickHandler : ITreeNodeClickHandler
    {
        public void Execute(ConnectionInfo clickedNode)
        {
            if (clickedNode == null) return;
            if (clickedNode.GetTreeNodeType() != TreeNodeType.Connection && clickedNode.GetTreeNodeType() != TreeNodeType.PuttySession) return;
            ConnectionInitiator.OpenConnection(clickedNode);
        }
    }
}