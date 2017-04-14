using mRemoteNG.Tree;

namespace mRemoteNG.Config.Serializers
{
    public interface ISerializer<out TFormat> : IConnectionSerializer<TFormat>
    {
        TFormat Serialize(ConnectionTreeModel connectionTreeModel);
    }
}