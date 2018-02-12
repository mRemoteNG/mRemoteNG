namespace mRemoteNG.Connection.Protocol.Http
{
	public class ProtocolHTTPS : HTTPBase
	{
		public ProtocolHTTPS(ConnectionInfo connectionInfo, RenderingEngine renderingEngine) 
            : base(connectionInfo, renderingEngine)
		{
            httpOrS = "https";
            defaultPort = (int)Defaults.Port;
        }
				
		public enum Defaults
		{
			Port = 443
		}
	}
}
