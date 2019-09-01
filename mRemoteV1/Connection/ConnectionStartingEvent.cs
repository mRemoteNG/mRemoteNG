using System;
using mRemoteNG.Connection.Protocol;

namespace mRemoteNG.Connection
{
    [Serializable]
    public class ConnectionStartingEvent : EventArgs
    {
        public ConnectionInfo ConnectionInfo { get; }
        public ProtocolBase Protocol { get; }

        public ConnectionStartingEvent(ConnectionInfo connectionInfo, ProtocolBase protocol)
        {
            ConnectionInfo = connectionInfo;
            Protocol = protocol;
        }
    }
}
