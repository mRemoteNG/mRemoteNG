

namespace mRemoteNG.Connection.Protocol
{
	public class Serial : PuttyBase
	{
				
		public Serial()
		{
			this.PuttyProtocol = Putty_Protocol.serial;
		}
				
		public enum Defaults
		{
			Port = 9600
		}
	}
}
