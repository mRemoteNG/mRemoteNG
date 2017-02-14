namespace mRemoteNG.Config.Serializers
{
    public interface IDeserializer<out TOut>
    {
        TOut Deserialize();
    }
}