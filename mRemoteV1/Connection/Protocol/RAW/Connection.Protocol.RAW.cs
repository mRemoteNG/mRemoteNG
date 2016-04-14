namespace mRemoteNG.Connection.Protocol.RAW
{
	public class ProtocolRAW : Connection.Protocol.PuttyBase
	{
		public ProtocolRAW()
		{
			this.PuttyProtocol = Putty_Protocol.raw;
		}
				
		public enum Defaults
		{
			Port = 23
		}
	}
}
