namespace mRemoteNG.Connection.Protocol.Rlogin
{
	public class ProtocolRlogin : PuttyBase
	{
		public ProtocolRlogin(ConnectionInfo connectionInfo)
		    : base(connectionInfo)
        {
			PuttyProtocol = Putty_Protocol.rlogin;
		}
				
		public enum Defaults
		{
			Port = 513
		}
	}
}