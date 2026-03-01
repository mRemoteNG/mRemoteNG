using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol
{
    [SupportedOSPlatform("windows")]
    public interface IProtocolFactory
    {
        ProtocolBase CreateProtocol(ConnectionInfo connectionInfo);
    }
}
