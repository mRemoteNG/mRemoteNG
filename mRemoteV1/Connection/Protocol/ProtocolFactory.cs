using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RAW;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Rlogin;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Telnet;
using mRemoteNG.Connection.Protocol.VNC;
using System;
using System.Collections.Generic;
using System.Text;
using mRemoteNG.My;

namespace mRemoteNG.Connection.Protocol
{
    public class ProtocolFactory
    {
        public ProtocolFactory()
        {

        }

        public ProtocolBase CreateProtocol(ConnectionInfo connectionInfo)
        {
            ProtocolBase newProtocol = default(ProtocolBase);
			switch (connectionInfo.Protocol)
			{
				case ProtocolType.RDP:
					newProtocol = new ProtocolRDP();
					((ProtocolRDP) newProtocol).tmrReconnect.Elapsed += ((ProtocolRDP) newProtocol).tmrReconnect_Elapsed;
					break;
				case ProtocolType.VNC:
					newProtocol = new ProtocolVNC();
					break;
				case ProtocolType.SSH1:
					newProtocol = new ProtocolSSH1();
					break;
				case ProtocolType.SSH2:
					newProtocol = new ProtocolSSH2();
					break;
				case ProtocolType.Telnet:
					newProtocol = new ProtocolTelnet();
					break;
				case ProtocolType.Rlogin:
					newProtocol = new ProtocolRlogin();
					break;
				case ProtocolType.RAW:
					newProtocol = new ProtocolRAW();
					break;
				case ProtocolType.HTTP:
					newProtocol = new ProtocolHTTP(connectionInfo.RenderingEngine);
					break;
				case ProtocolType.HTTPS:
					newProtocol = new ProtocolHTTPS(connectionInfo.RenderingEngine);
					break;
#if ICA
                case ProtocolType.ICA:
					newProtocol = new mRemoteNG.Connection.Protocol.ICA.ProtocolICA();
					((mRemoteNG.Connection.Protocol.ICA.ProtocolICA) newProtocol).tmrReconnect.Elapsed += ((mRemoteNG.Connection.Protocol.ICA.ProtocolICA) newProtocol).tmrReconnect_Elapsed;
					break;
#endif
				case ProtocolType.IntApp:
					newProtocol = new IntegratedProgram();
					if (connectionInfo.ExtApp == "")
					{
						throw (new Exception(Language.strNoExtAppDefined));
					}
					break;
			}
            return newProtocol;
        }
    }
}
