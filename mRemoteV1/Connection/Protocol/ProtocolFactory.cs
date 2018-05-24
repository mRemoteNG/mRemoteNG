using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RAW;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Rlogin;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Telnet;
using mRemoteNG.Connection.Protocol.VNC;
using System;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.Connection.Protocol
{
    public class ProtocolFactory
    {
        private readonly ExternalToolsService _externalToolsService;
        private readonly FrmMain _frmMain;
        private readonly IConnectionsService _connectionsService;

        public ProtocolFactory(ExternalToolsService externalToolsService, FrmMain frmMain, IConnectionsService connectionsService)
        {
            _externalToolsService = externalToolsService.ThrowIfNull(nameof(externalToolsService));
            _frmMain = frmMain.ThrowIfNull(nameof(frmMain));
            _connectionsService = connectionsService.ThrowIfNull(nameof(connectionsService));
        }

        public ProtocolBase CreateProtocol(ConnectionInfo connectionInfo)
        {
            var newProtocol = default(ProtocolBase);
            // ReSharper disable once SwitchStatementMissingSomeCases
			switch (connectionInfo.Protocol)
			{
				case ProtocolType.RDP:
					newProtocol = new RdpProtocol(_frmMain, _connectionsService)
					{
					    LoadBalanceInfoUseUtf8 = Settings.Default.RdpLoadBalanceInfoUseUtf8
                    };
					((RdpProtocol) newProtocol).tmrReconnect.Elapsed += ((RdpProtocol) newProtocol).tmrReconnect_Elapsed;
					break;
				case ProtocolType.VNC:
					newProtocol = new ProtocolVNC();
					break;
				case ProtocolType.SSH1:
					newProtocol = new ProtocolSSH1(_connectionsService);
					break;
				case ProtocolType.SSH2:
					newProtocol = new ProtocolSSH2(_connectionsService);
					break;
				case ProtocolType.Telnet:
					newProtocol = new ProtocolTelnet(_connectionsService);
					break;
				case ProtocolType.Rlogin:
					newProtocol = new ProtocolRlogin(_connectionsService);
					break;
				case ProtocolType.RAW:
					newProtocol = new RawProtocol(_connectionsService);
					break;
				case ProtocolType.HTTP:
					newProtocol = new ProtocolHTTP(connectionInfo.RenderingEngine);
					break;
				case ProtocolType.HTTPS:
					newProtocol = new ProtocolHTTPS(connectionInfo.RenderingEngine);
					break;
				case ProtocolType.ICA:
					newProtocol = new IcaProtocol(_frmMain, _connectionsService);
					((IcaProtocol) newProtocol).tmrReconnect.Elapsed += ((IcaProtocol) newProtocol).tmrReconnect_Elapsed;
					break;
				case ProtocolType.IntApp:
					newProtocol = new IntegratedProgram(_externalToolsService, _connectionsService);
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