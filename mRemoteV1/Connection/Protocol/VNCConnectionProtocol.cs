using System;
using System.Collections.Generic;
using System.Text;

namespace mRemoteNG.Connection.Protocol
{
    public class VNCConnectionProtocol : ConnectionProtocol
    {
        Protocols _name;
        
        public Protocols Name
        {
            get { return _name; }
        }

        public VNCConnectionProtocol()
        {
            _name = Protocols.VNC;
        }

        public void Connect()
        { }

        public void Disconnect()
        { }

        public void Reconnect()
        { }
    }
}