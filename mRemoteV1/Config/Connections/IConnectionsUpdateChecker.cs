using System;


namespace mRemoteNG.Config.Connections
{
    public interface IConnectionsUpdateChecker : IDisposable
    {
        bool IsUpdateAvailable();

        void IsUpdateAvailableAsync();

        event UpdateCheckFinishedEventHandler UpdateCheckFinished;
        event ConnectionsUpdateAvailableEventHandler ConnectionsUpdateAvailable;
    }
}