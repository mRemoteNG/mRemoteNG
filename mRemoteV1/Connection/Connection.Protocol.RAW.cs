

namespace mRemoteNG.Connection.Protocol
{
	public class RAW : Connection.Protocol.PuttyBase
	{
		public RAW()
		{
			this.PuttyProtocol = Putty_Protocol.raw;
		}
				
		public enum Defaults
		{
			Port = 23
		}
	}
}
