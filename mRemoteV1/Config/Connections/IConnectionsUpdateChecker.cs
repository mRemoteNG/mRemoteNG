using System;


namespace mRemoteNG.Config.Connections
{
    public interface IConnectionsUpdateChecker : IDisposable
    {
        bool IsUpdateAvailable();

        void IsUpdateAvailableAsync();
    }
}