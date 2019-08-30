namespace mRemoteNG.Connection.Protocol.Http
{
    public class ProtocolHTTPS : HttpBase
    {
        public ProtocolHTTPS(RenderingEngine RenderingEngine) : base(RenderingEngine)
        {
            HttpOrHttps = "https";
            DefaultPort = (int)Defaults.Port;
        }

        public enum Defaults
        {
            Port = 443
        }
    }
}