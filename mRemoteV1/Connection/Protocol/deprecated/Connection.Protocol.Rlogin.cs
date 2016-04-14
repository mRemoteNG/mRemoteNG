

namespace mRemoteNG.Connection.Protocol
{
	public class Rlogin : PuttyBase
	{
				
		public Rlogin()
		{
			this.PuttyProtocol = Putty_Protocol.rlogin;
		}
				
		public enum Defaults
		{
			Port = 513
		}
	}
}
