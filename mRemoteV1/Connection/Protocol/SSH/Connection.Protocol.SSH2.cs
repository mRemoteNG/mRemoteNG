namespace mRemoteNG.Connection.Protocol.SSH
{
	public class ProtocolSSH2 : PuttyBase
	{
				
		public ProtocolSSH2()
		{
			this.PuttyProtocol = Putty_Protocol.ssh;
			this.PuttySSHVersion = Putty_SSHVersion.ssh2;
		}
				
		public enum Defaults
		{
			Port = 22
		}
	}
}
