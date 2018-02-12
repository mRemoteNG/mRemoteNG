using System;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RAW;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Rlogin;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Telnet;
using mRemoteNG.Connection.Protocol.VNC;

namespace mRemoteNG.Connection.Protocol
{
	public class ProtocolFactory
    {
        public ProtocolBase CreateProtocol(ConnectionInfo connectionInfo)
        {
            var newProtocol = default(ProtocolBase);
            // ReSharper disable once SwitchStatementMissingSomeCases
			switch (connectionInfo.Protocol)
			{
				case ProtocolType.RDP:
					newProtocol = new RdpProtocolFactory().CreateProtocol(connectionInfo);
					break;
				case ProtocolType.VNC:
					newProtocol = new ProtocolVNC(connectionInfo);
					break;
				case ProtocolType.SSH1:
					newProtocol = new ProtocolSSH1(connectionInfo);
					break;
				case ProtocolType.SSH2:
					newProtocol = new ProtocolSSH2(connectionInfo);
					break;
				case ProtocolType.Telnet:
					newProtocol = new ProtocolTelnet(connectionInfo);
					break;
				case ProtocolType.Rlogin:
					newProtocol = new ProtocolRlogin(connectionInfo);
					break;
				case ProtocolType.RAW:
					newProtocol = new RawProtocol(connectionInfo);
					break;
				case ProtocolType.HTTP:
					newProtocol = new ProtocolHTTP(connectionInfo, connectionInfo.RenderingEngine);
					break;
				case ProtocolType.HTTPS:
					newProtocol = new ProtocolHTTPS(connectionInfo, connectionInfo.RenderingEngine);
					break;
				case ProtocolType.ICA:
					newProtocol = new IcaProtocol(connectionInfo);
					((IcaProtocol) newProtocol).tmrReconnect.Elapsed += ((IcaProtocol) newProtocol).tmrReconnect_Elapsed;
					break;
				case ProtocolType.IntApp:
					newProtocol = new IntegratedProgram(connectionInfo);
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