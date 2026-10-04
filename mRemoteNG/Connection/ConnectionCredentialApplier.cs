using System;
using mRemoteNG.Credential;
using mRemoteNG.Security;

namespace mRemoteNG.Connection
{
    /// <summary>
    /// Produces a copy of a <see cref="ConnectionInfo"/> whose credential
    /// fields (username, password and domain) are overridden with the values
    /// from a selected <see cref="ICredentialRecord"/>. This allows a single
    /// connection to be opened using any of the saved credentials without
    /// permanently altering the stored connection.
    /// </summary>
    public static class ConnectionCredentialApplier
    {
        /// <summary>
        /// Returns a clone of <paramref name="connectionInfo"/> with its
        /// username, password and domain replaced by those of
        /// <paramref name="credentialRecord"/>. The original connection is not
        /// modified so the override only applies to the current session.
        /// </summary>
        public static ConnectionInfo ApplyCredential(ConnectionInfo connectionInfo, ICredentialRecord credentialRecord)
        {
            if (connectionInfo == null)
                throw new ArgumentNullException(nameof(connectionInfo));
            if (credentialRecord == null)
                throw new ArgumentNullException(nameof(credentialRecord));

            ConnectionInfo clone = connectionInfo.Clone();
            clone.Parent = connectionInfo.Parent;
            clone.Username = credentialRecord.Username ?? "";
            clone.Domain = credentialRecord.Domain ?? "";
            clone.Password = credentialRecord.Password?.ConvertToUnsecureString() ?? "";
            return clone;
        }
    }
}
