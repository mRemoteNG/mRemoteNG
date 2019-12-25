using System.Collections.Generic;
using mRemoteNG.Container;
using mRemoteNG.UI.Window;

namespace mRemoteNG.Connection
{
    public interface IConnectionInitiator
    {
        IEnumerable<string> ActiveConnections { get; }

        void OpenConnection(
            ContainerInfo containerInfo,
            ConnectionInfo.Force force = ConnectionInfo.Force.None,
            ConnectionWindow conForm = null);

        void OpenConnection(
            ConnectionInfo connectionInfo,
            ConnectionInfo.Force force = ConnectionInfo.Force.None,
            ConnectionWindow conForm = null);

        bool SwitchToOpenConnection(ConnectionInfo connectionInfo);
    }
}