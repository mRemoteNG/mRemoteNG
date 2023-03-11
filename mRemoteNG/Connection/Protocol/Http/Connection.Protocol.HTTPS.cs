using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol.Http
{
    public class ProtocolHTTPS : HTTPBase
    {
        [SupportedOSPlatform("windows")]
        public ProtocolHTTPS(RenderingEngine RenderingEngine) : base(RenderingEngine)
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