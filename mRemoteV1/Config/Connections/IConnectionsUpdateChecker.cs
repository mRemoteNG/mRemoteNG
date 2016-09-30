

namespace mRemoteNG.Config.Connections
{
    public interface IConnectionsUpdateChecker
    {
        bool IsUpdateAvailable();

        void IsUpdateAvailableAsync();
    }
}