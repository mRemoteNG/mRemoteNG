namespace mRemoteNG.Credential
{
    public interface IFactory<out T>
    {
        T Build();
    }
}