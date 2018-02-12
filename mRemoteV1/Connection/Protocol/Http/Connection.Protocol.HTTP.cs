namespace mRemoteNG.Connection.Protocol.Http
{
	public class ProtocolHTTP : HTTPBase
	{
				
		public ProtocolHTTP(ConnectionInfo connectionInfo, RenderingEngine renderingEngine) 
            : base(connectionInfo, renderingEngine)
		{
            httpOrS = "http";
            defaultPort = (int)Defaults.Port;
        }
				
		public enum Defaults
		{
			Port = 80
		}
	}
}
