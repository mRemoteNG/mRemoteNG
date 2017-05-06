namespace mRemoteNG.Config.Serializers
{
    public interface IDeserializer<in TIn, out TOut>
    {
        TOut Deserialize(TIn serializedData);
    }
}