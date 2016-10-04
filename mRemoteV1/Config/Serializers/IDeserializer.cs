using mRemoteNG.Tree;

namespace mRemoteNG.Config.Serializers
{
    public interface IDeserializer
    {
        ConnectionTreeModel Deserialize();
    }
}