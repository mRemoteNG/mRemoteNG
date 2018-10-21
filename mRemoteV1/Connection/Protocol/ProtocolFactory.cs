using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RAW;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Rlogin;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Telnet;
using mRemoteNG.Connection.Protocol.VNC;
using System;

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
					newProtocol = new RdpProtocol
					{
					    LoadBalanceInfoUseUtf8 = Settings.Default.RdpLoadBalanceInfoUseUtf8
                    };
					((RdpProtocol) newProtocol).tmrReconnect.Elapsed += ((RdpProtocol) newProtocol).tmrReconnect_Elapsed;
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
					newProtocol = new RawProtocol();
					break;
				case ProtocolType.HTTP:
					newProtocol = new ProtocolHTTP(connectionInfo.RenderingEngine);
					break;
				case ProtocolType.HTTPS:
					newProtocol = new ProtocolHTTPS(connectionInfo.RenderingEngine);
					break;
				case ProtocolType.ICA:
					newProtocol = new IcaProtocol();
					((IcaProtocol) newProtocol).tmrReconnect.Elapsed += ((IcaProtocol) newProtocol).tmrReconnect_Elapsed;
					break;
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