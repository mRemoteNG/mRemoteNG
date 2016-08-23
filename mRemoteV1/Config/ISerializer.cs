using mRemoteNG.Tree;

namespace mRemoteNG.Config
{
    public interface ISerializer
    {
        string Serialize(ConnectionTreeModel connectionTreeModel);
    }
}