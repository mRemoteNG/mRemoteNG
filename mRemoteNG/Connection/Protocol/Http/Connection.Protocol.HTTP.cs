using Mysqlx.Notice;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol.Http
{
    [SupportedOSPlatform("windows")]
    public class ProtocolHTTP : HTTPBase
    {
        public ProtocolHTTP(RenderingEngine RenderingEngine) : base(RenderingEngine)
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