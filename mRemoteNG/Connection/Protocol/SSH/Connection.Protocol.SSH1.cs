using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol.SSH
{
    [SupportedOSPlatform("windows")]
    public class ProtocolSSH1 : PuttyBase
    {
        public ProtocolSSH1()
        {
            PuttyProtocol = Putty_Protocol.ssh;
            PuttySSHVersion = Putty_SSHVersion.ssh1;
        }

        public enum Defaults
        {
            Port = 22
        }
    }
}