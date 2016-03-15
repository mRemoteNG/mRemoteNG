using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace mRemoteNG.Connection
{
    public interface ConnectionProtocol
    {
        Protocols Name { get; }
        PropertyInfo[] SupportedSettings { get; }
        
        void Connect();
        void Disconnect();
        void Reconnect();
    }
}