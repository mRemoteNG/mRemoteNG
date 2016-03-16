using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace mRemoteNG.Connection
{
    public interface ConnectionProtocol : Connectable
    {
        Protocols Name { get; }
        PropertyInfo[] SupportedSettings { get; }
        Version ProtocolVersion { get; }

        void Initialize();
    }
}