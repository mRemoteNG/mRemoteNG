namespace mRemoteNG.Connection.Protocol.SSH
{
    public class ProtocolSSH1 : PuttyBase
    {
        public enum Defaults
        {
            Port = 22
        }

        public ProtocolSSH1()
        {
            PuttyProtocol = Putty_Protocol.ssh;
            PuttySSHVersion = Putty_SSHVersion.ssh1;
        }
    }
}