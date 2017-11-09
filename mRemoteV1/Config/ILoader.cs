namespace mRemoteNG.Config
{
    public interface ILoader<out T>
    {
        T Load();
    }
}