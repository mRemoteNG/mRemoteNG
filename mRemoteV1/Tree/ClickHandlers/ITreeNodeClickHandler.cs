using mRemoteNG.Connection;


namespace mRemoteNG.Tree
{
    public interface ITreeNodeClickHandler
    {
        void Execute(ConnectionInfo clickedNode);
    }
}