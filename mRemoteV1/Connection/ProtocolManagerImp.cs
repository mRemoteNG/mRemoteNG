using System;
using mRemoteNG.Tools;
using System.Collections.Generic;
using mRemoteNG.Connection.Protocol;

namespace mRemoteNG.Connection
{
	public class ProtocolManagerImp : ProtocolManager
	{
        private static ProtocolManagerImp _instance;
        private IDictionary<Protocols, ConnectionProtocol> _enumInterfaceProtocolPairs;

        private ProtocolManagerImp()
        {
            this.BuildProtocolList();
        }

        private void BuildProtocolList()
        {
            _enumInterfaceProtocolPairs = new Dictionary<Protocols, ConnectionProtocol>();
            _enumInterfaceProtocolPairs.Add(Protocols.HTTP, new HttpConnectionProtocol());
            _enumInterfaceProtocolPairs.Add(Protocols.HTTPS, new HttpsConnectionProtocol());
            _enumInterfaceProtocolPairs.Add(Protocols.ICA, new ICAConnectionProtocol());
            _enumInterfaceProtocolPairs.Add(Protocols.IntApp, new OtherConnectionProtocol());
            _enumInterfaceProtocolPairs.Add(Protocols.RAW, new RAWConnectionProtocol());
            _enumInterfaceProtocolPairs.Add(Protocols.RDP, new RDPConnectionProtocolImp());
            _enumInterfaceProtocolPairs.Add(Protocols.Rlogin, new RloginConnectionProtocol());
            _enumInterfaceProtocolPairs.Add(Protocols.SSH1, new SSH1ConnectionProtocol());
            _enumInterfaceProtocolPairs.Add(Protocols.SSH2, new SSH2ConnectionProtocol());
            _enumInterfaceProtocolPairs.Add(Protocols.Telnet, new TelnetConnectionProtocol());
            _enumInterfaceProtocolPairs.Add(Protocols.VNC, new VNCConnectionProtocol());
        }

        public static ConnectionProtocol GetConnectionProtocol(Protocols protocol)
        {
            return getSingletonInstance()._enumInterfaceProtocolPairs[protocol];
        }

        private static ProtocolManagerImp getSingletonInstance()
        {
            if (_instance == null)
                _instance = new ProtocolManagerImp();
            return _instance;
        }

		public static string ProtocolToString(Protocols protocol)
		{
			return protocol.ToString();
		}
				
		public static Protocols StringToProtocol(string protocol)
		{
			try
			{
				return (Protocols)Enum.Parse(typeof(Protocols), protocol, true);
			}
			catch (Exception)
			{
				return Protocols.RDP;
			}
		}
	}
}