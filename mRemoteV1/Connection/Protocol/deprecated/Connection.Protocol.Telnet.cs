

namespace mRemoteNG.Connection.Protocol
{
	public class Telnet : PuttyBase
	{
				
		public Telnet()
		{
			this.PuttyProtocol = Putty_Protocol.telnet;
		}
				
		public enum Defaults
		{
			Port = 23
		}
	}
}
