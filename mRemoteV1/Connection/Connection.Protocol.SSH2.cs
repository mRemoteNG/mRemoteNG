

namespace mRemoteNG.Connection.Protocol
{
	public class SSH2 : PuttyBase
	{
				
		public SSH2()
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
