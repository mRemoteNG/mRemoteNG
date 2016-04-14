

namespace mRemoteNG.Connection.Protocol
{
	public class HTTP : HTTPBase
	{
				
		public HTTP(RenderingEngine RenderingEngine) : base(RenderingEngine)
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
