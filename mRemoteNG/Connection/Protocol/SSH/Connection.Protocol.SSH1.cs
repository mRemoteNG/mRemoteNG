using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol.SSH
{
    [SupportedOSPlatform("windows")]
    public class ProtocolSSH1 : SshTerminalBase
    {
        // SSH1 is deprecated — SSH.NET connects as SSH2
        public enum Defaults
        {
            Port = 22
        }
    }
}
