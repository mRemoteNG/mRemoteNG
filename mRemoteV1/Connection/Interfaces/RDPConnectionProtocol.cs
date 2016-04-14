using System;
using System.Collections.Generic;
using System.Text;

namespace mRemoteNG.Connection.Protocol
{
    interface RDPConnectionProtocol : ConnectionProtocol
    {
        bool SmartSize { get; set; }
        bool Fullscreen { get; set; }
        bool RedirectKeys { get; set; }

    }
}