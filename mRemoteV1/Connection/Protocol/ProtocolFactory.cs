using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RAW;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Rlogin;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Serial;
using mRemoteNG.Connection.Protocol.Telnet;
using mRemoteNG.Connection.Protocol.VNC;
using System;
using mRemoteNG.Connection.Protocol.PowerShell;

namespace mRemoteNG.Connection.Protocol
{
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
                    rdp.LoadBalanceInfoUseUtf8 = Settings.Default.RdpLoadBalanceInfoUseUtf8;
                    return rdp;
                case ProtocolType.VNC:
                    return new ProtocolVNC();
                case ProtocolType.SSH1:
                    return new ProtocolSSH1();
                case ProtocolType.SSH2:
                    return new ProtocolSSH2();
                case ProtocolType.Serial:
					return new ProtocolSerial();
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
                case ProtocolType.ICA:
                    var icaProtocol = new IcaProtocol();
                    icaProtocol.tmrReconnect.Elapsed += icaProtocol.tmrReconnect_Elapsed;
                    return icaProtocol;
                case ProtocolType.PowerShell:
                    return new ProtocolPowerShell(connectionInfo);
                case ProtocolType.IntApp:
                    if (connectionInfo.ExtApp == "")
                    {
                        throw (new Exception(Language.strNoExtAppDefined));
                    }
                    return new IntegratedProgram();
            }

            return default;
        }
    }
}
