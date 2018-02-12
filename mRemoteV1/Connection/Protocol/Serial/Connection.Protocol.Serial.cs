namespace mRemoteNG.Connection.Protocol.Serial
{
	public class ProtocolSerial : PuttyBase
	{
		public ProtocolSerial(ConnectionInfo connectionInfo)
		    : base(connectionInfo)
        {
			PuttyProtocol = Putty_Protocol.serial;
		}
				
		public enum Defaults
		{
			Port = 9600
		}
	}
}