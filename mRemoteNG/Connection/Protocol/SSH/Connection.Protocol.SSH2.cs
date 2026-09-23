using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol.SSH
{
    [SupportedOSPlatform("windows")]
    public class ProtocolSSH2 : SshTerminalBase
    {
        public enum Defaults
        {
            Port = 22
        }
    }
}
