using mRemoteNG.Container;

namespace mRemoteNG.Connection
{
    public interface IConnectionInitiator
    {
        void OpenConnection(ConnectionInfo connectionInfo);

        void OpenConnection(ContainerInfo containerInfo, ConnectionInfo.Force force = ConnectionInfo.Force.None);

        void OpenConnection(ConnectionInfo connectionInfo, ConnectionInfo.Force force);

        bool SwitchToOpenConnection(ConnectionInfo connectionInfo);
    }
}