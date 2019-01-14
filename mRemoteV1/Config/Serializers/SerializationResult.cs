using mRemoteNG.Connection;
using mRemoteNG.Credential;
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
        public List<ICredentialRecord> CredentialRecords { get; }
        public ConnectionToCredentialMap ConnectionToCredentialMap { get; }

        public SerializationResult(
            List<ConnectionInfo> connectionRecords, 
            List<ICredentialRecord> credentialRecords, 
            ConnectionToCredentialMap connectionToCredentialMap)
        {
            ConnectionRecords = connectionRecords.ThrowIfNull(nameof(connectionRecords));
            CredentialRecords = credentialRecords.ThrowIfNull(nameof(credentialRecords));
            ConnectionToCredentialMap = connectionToCredentialMap.ThrowIfNull(nameof(connectionToCredentialMap));
        }
    }
}
