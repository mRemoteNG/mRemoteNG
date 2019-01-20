using mRemoteNG.Config.Serializers;

namespace mRemoteNG.Config.Connections
{
    public interface IConnectionsLoader
    {
        SerializationResult Load();
    }
}