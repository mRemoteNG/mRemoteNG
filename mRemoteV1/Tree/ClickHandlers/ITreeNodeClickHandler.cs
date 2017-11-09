using mRemoteNG.Connection;

namespace mRemoteNG.Tree
{
    public interface ITreeNodeClickHandler<in T>
    {
        void Execute(T clickedNode);
    }
}