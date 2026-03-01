using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RAW;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Rlogin;
using mRemoteNG.Connection.Protocol.Serial;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Telnet;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Connection.Protocol.ARD;
using System;
using mRemoteNG.Connection.Protocol.PowerShell;
using mRemoteNG.Connection.Protocol.WSL;
using mRemoteNG.Connection.Protocol.Terminal;
using mRemoteNG.Connection.Protocol.AnyDesk;
using mRemoteNG.Connection.Protocol.MSRA;
using mRemoteNG.Connection.Protocol.Winbox;
using mRemoteNG.Connection.Protocol.VMRC;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol
{
    [SupportedOSPlatform("windows")]
    public class ProtocolFactory : IProtocolFactory
    {
        private readonly RdpProtocolFactory _rdpProtocolFactory = new();

        public ProtocolBase CreateProtocol(ConnectionInfo connectionInfo)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (connectionInfo.Protocol)
            {
                case ProtocolType.RDP:
                    RdpProtocol rdp = _rdpProtocolFactory.Build(connectionInfo.RdpVersion);
                    rdp.LoadBalanceInfoUseUtf8 = Properties.OptionsAdvancedPage.Default.RdpLoadBalanceInfoUseUtf8;
                    return rdp;
                case ProtocolType.VNC:
                    return new ProtocolVNC();
                case ProtocolType.ARD:
                    return new ProtocolARD();
                case ProtocolType.SSH1:
                    return new ProtocolSSH1();
                case ProtocolType.SSH2:
                    return new ProtocolSSH2();
                case ProtocolType.OpenSSH:
                    return new ProtocolOpenSSH(connectionInfo);
                case ProtocolType.Telnet:
                    return new ProtocolTelnet();
                case ProtocolType.Rlogin:
                    return new ProtocolRlogin();
                case ProtocolType.RAW:
                    return new RawProtocol();
                case ProtocolType.Serial:
                    return new ProtocolSerial();
                case ProtocolType.HTTP:
                    return new ProtocolHTTP(connectionInfo.RenderingEngine);
                case ProtocolType.HTTPS:
                    return new ProtocolHTTPS(connectionInfo.RenderingEngine);
                case ProtocolType.PowerShell:
                    return new ProtocolPowerShell(connectionInfo);
                case ProtocolType.WSL:
                    return new ProtocolWSL(connectionInfo);
                case ProtocolType.Terminal:
                    return new ProtocolTerminal(connectionInfo);
                case ProtocolType.AnyDesk:
                    return new ProtocolAnyDesk(connectionInfo);
                case ProtocolType.MSRA:
                    return new ProtocolMSRA(connectionInfo);
                case ProtocolType.VMRC:
                    return new ProtocolVMRC(connectionInfo);
                case ProtocolType.Winbox:
                    return new ProtocolWinbox(connectionInfo);
                case ProtocolType.IntApp:
                    if (connectionInfo.ExtApp == "")
                    {
                        throw new InvalidOperationException(Language.NoExtAppDefined);
                    }
                    return new IntegratedProgram();
            }

            throw new ArgumentOutOfRangeException(nameof(connectionInfo), connectionInfo.Protocol, Language.NoExtAppDefined);
        }
    }
}
