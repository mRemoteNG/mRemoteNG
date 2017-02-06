using mRemoteNG.Connection;


namespace mRemoteNG.Config.Serializers
{
    public interface IConnectionSerializer<out TFormat>
    {
        TFormat Serialize(ConnectionInfo serializationTarget);
    }
}