using mRemoteNG.Tree;

namespace mRemoteNG.Config
{
    public interface IDeserializer
    {
        ConnectionTreeModel Deserialize();
    }
}