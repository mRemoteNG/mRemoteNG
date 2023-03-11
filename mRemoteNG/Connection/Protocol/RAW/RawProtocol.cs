using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol.RAW
{
    [SupportedOSPlatform("windows")]
    public class RawProtocol : PuttyBase
    {
        public RawProtocol()
        {
            PuttyProtocol = Putty_Protocol.raw;
        }

        public enum Defaults
        {
            Port = 23
        }
    }
}