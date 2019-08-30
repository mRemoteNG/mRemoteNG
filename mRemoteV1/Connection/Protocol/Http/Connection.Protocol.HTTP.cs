namespace mRemoteNG.Connection.Protocol.Http
{
    public class ProtocolHTTP : HttpBase
    {
        public ProtocolHTTP(RenderingEngine RenderingEngine) : base(RenderingEngine)
        {
            HttpOrHttps = "http";
            DefaultPort = (int)Defaults.Port;
        }

        public enum Defaults
        {
            Port = 80
        }
    }
}