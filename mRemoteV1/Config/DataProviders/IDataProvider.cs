
namespace mRemoteNG.Config.DataProviders
{
    public interface IDataProvider
    {
        string Load();

        void Save(string contents);
    }
}