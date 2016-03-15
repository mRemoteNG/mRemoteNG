using System;
using System.Collections.Generic;
using System.Text;

namespace mRemoteNG.Connection.Protocol
{
    public class RDPConnectionProtocol : ConnectionProtocol
    {
        private string _name = "RDP";

        public Protocols Name
        {
            get { return _name; }
        }

        public RDPConnectionProtocol()
        {
        }

        public void Connect()
        { }

        public void Disconnect()
        { }

        public void Reconnect()
        { }
    }
}