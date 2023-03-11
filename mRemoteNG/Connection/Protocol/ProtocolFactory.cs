using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RAW;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Rlogin;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Telnet;
using mRemoteNG.Connection.Protocol.VNC;
using System;
using mRemoteNG.Connection.Protocol.PowerShell;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol
{
    [SupportedOSPlatform("windows")]
    public class ProtocolFactory
    {
        private readonly RdpProtocolFactory _rdpProtocolFactory = new RdpProtocolFactory();

        public ProtocolBase CreateProtocol(ConnectionInfo connectionInfo)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (connectionInfo.Protocol)
            {
                case ProtocolType.RDP:
                    var rdp = _rdpProtocolFactory.Build(connectionInfo.RdpVersion);
                    rdp.LoadBalanceInfoUseUtf8 = Properties.OptionsAdvancedPage.Default.RdpLoadBalanceInfoUseUtf8;
                    return rdp;
                case ProtocolType.VNC:
                    return new ProtocolVNC();
                case ProtocolType.SSH1:
                    return new ProtocolSSH1();
                case ProtocolType.SSH2:
                    return new ProtocolSSH2();
                case ProtocolType.Telnet:
                    return new ProtocolTelnet();
                case ProtocolType.Rlogin:
                    return new ProtocolRlogin();
                case ProtocolType.RAW:
                    return new RawProtocol();
                case ProtocolType.HTTP:
                    return new ProtocolHTTP(connectionInfo.RenderingEngine);
                case ProtocolType.HTTPS:
                    return new ProtocolHTTPS(connectionInfo.RenderingEngine);
                case ProtocolType.PowerShell:
                    return new ProtocolPowerShell(connectionInfo);
                case ProtocolType.IntApp:
                    if (connectionInfo.ExtApp == "")
                    {
                        throw (new Exception(Language.NoExtAppDefined));
                    }
                    return new IntegratedProgram();
            }

            return default(ProtocolBase);
        }
    }
}