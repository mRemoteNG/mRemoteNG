

namespace mRemoteNG.Connection.Protocol.SSH
{
	public class ProtocolSSH1 : PuttyBase
	{
		public ProtocolSSH1(ConnectionInfo connectionInfo)
            : base(connectionInfo)
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
