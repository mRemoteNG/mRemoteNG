namespace mRemoteNG.Connection.Protocol.Http
{
	public class ProtocolHTTP : HTTPBase
	{
				
		public ProtocolHTTP(RenderingEngine RenderingEngine) : base(RenderingEngine)
		{
		}
				
		public override void NewExtended()
		{
			base.NewExtended();
					
			httpOrS = "http";
			defaultPort = (int)Defaults.Port;
		}
				
		public enum Defaults
		{
			Port = 80
		}
	}
}
