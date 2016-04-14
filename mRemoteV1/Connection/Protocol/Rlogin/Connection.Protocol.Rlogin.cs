namespace mRemoteNG.Connection.Protocol.Rlogin
{
	public class ProtocolRlogin : PuttyBase
	{
				
		public ProtocolRlogin()
		{
			this.PuttyProtocol = Putty_Protocol.rlogin;
		}
				
		public enum Defaults
		{
			Port = 513
		}
	}
}