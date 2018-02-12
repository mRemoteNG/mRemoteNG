namespace mRemoteNG.Connection.Protocol.Telnet
{
	public class ProtocolTelnet : PuttyBase
	{
		public ProtocolTelnet(ConnectionInfo connectionInfo)
		    : base(connectionInfo)
        {
			PuttyProtocol = Putty_Protocol.telnet;
		}
				
		public enum Defaults
		{
			Port = 23
		}
	}
}