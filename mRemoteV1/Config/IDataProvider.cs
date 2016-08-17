
namespace mRemoteNG.Config
{
    public interface IDataProvider
    {
        string Load();

        void Save(string contents);
    }
}