using mRemoteNG.Tree;

namespace mRemoteNG.Config.Serializers
{
    public interface ISerializer
    {
        string Serialize(ConnectionTreeModel connectionTreeModel);
    }
}