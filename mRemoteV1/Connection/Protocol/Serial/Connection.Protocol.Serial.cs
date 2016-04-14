namespace mRemoteNG.Connection.Protocol.Serial
{
	public class ProtocolSerial : PuttyBase
	{
				
		public ProtocolSerial()
		{
			this.PuttyProtocol = Putty_Protocol.serial;
		}
				
		public enum Defaults
		{
			Port = 9600
		}
	}
}