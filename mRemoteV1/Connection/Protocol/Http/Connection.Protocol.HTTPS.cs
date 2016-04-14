namespace mRemoteNG.Connection.Protocol.Http
{
	public class ProtocolHTTPS : HTTPBase
	{
				
		public ProtocolHTTPS(RenderingEngine RenderingEngine) : base(RenderingEngine)
		{
		}
				
		public override void NewExtended()
		{
			base.NewExtended();
			httpOrS = "https";
			defaultPort = (int)Defaults.Port;
		}
				
		public enum Defaults
		{
			Port = 443
		}
	}
}
