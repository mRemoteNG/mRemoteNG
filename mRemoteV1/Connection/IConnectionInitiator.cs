using mRemoteNG.Container;
using System.Collections.Generic;

namespace mRemoteNG.Connection
{
    public interface IConnectionInitiator
    {
        IEnumerable<string> ActiveConnections { get; }

        void OpenConnection(ConnectionInfo connectionInfo);

        void OpenConnection(ContainerInfo containerInfo, ConnectionInfo.Force force = ConnectionInfo.Force.None);

        void OpenConnection(ConnectionInfo connectionInfo, ConnectionInfo.Force force);

        bool SwitchToOpenConnection(ConnectionInfo connectionInfo);
    }
}