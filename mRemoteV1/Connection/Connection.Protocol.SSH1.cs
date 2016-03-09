

namespace mRemoteNG.Connection.Protocol
{
	public class SSH1 : PuttyBase
	{
				
		public SSH1()
		{
			this.PuttyProtocol = Putty_Protocol.ssh;
			this.PuttySSHVersion = Putty_SSHVersion.ssh1;
		}
				
		public enum Defaults
		{
			Port = 22
		}
	}
}
