using mRemoteNG.Connection;
using mRemoteNG.Tools;
using System.Collections.Generic;

namespace mRemoteNG.Config.Serializers
{
    /// <summary>
    /// Represents the connections and credentials found during a deserialization.
    /// </summary>
    public class SerializationResult
    {
        public List<ConnectionInfo> ConnectionRecords { get; }
        public ConnectionToCredentialMap ConnectionToCredentialMap { get; }

        public SerializationResult(
            List<ConnectionInfo> connectionRecords, 
            ConnectionToCredentialMap connectionToCredentialMap)
        {
            ConnectionRecords = connectionRecords.ThrowIfNull(nameof(connectionRecords));
            ConnectionToCredentialMap = connectionToCredentialMap.ThrowIfNull(nameof(connectionToCredentialMap));
        }
    }
}
