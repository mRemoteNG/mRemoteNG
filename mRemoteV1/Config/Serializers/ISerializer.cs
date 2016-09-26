using mRemoteNG.Tree;

namespace mRemoteNG.Config.Serializers
{
    public interface ISerializer<TFormat>
    {
        TFormat Serialize(ConnectionTreeModel connectionTreeModel);
    }
}