using System;
using System.Collections.Generic;
using System.Text;

namespace mRemoteNG.Connection.Protocol
{
    interface ProtocolManager
    {
        ConnectionProtocol GetConnectionProtocol(Protocols protocol);
        string ProtocolToString(Protocols protocol);
        Protocols StringToProtocol(string protocol);
    }
}