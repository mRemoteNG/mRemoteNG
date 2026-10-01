namespace mRemoteNG.Config.UserProfiles
{
    public interface IConnectionProfileDataProvider
    {
        string Load();
        void Save(string serializedConnections);
    }
}
