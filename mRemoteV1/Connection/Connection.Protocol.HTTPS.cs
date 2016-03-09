

namespace mRemoteNG.Connection.Protocol
{
	public class HTTPS : HTTPBase
	{
				
		public HTTPS(RenderingEngine RenderingEngine) : base(RenderingEngine)
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
