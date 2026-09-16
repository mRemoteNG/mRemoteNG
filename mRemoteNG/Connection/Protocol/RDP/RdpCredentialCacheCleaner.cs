using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Messages;

namespace mRemoteNG.Connection.Protocol.RDP
{
    /// <summary>
    /// Helper to remove a cached RDP credential (TERMSRV/&lt;hostname&gt;) from the Windows
    /// Credential Manager before initiating a connection.
    /// <para>
    /// Windows stores RDP credentials when the user ticks "Remember me" in the mstsc credential
    /// prompt. On subsequent RDP connections to the same host, Windows substitutes the cached
    /// credential for whatever the calling application supplies, which silently breaks
    /// password rotation or credential changes made inside mRemoteNG.
    /// </para>
    /// <para>
    /// Calling this helper before connecting removes the cached entry, so the credentials
    /// configured on the connection are used as-is. Behaviour is opt-in per connection.
    /// </para>
    /// </summary>
    public enum ClearCachedCredentialsResult
    {
        Deleted,
        NotFound,
        Failed,
    }

    [SupportedOSPlatform("windows")]
    internal static class RdpCredentialCacheCleaner
    {
        // CredDeleteW: https://learn.microsoft.com/en-us/windows/win32/api/wincred/nf-wincred-creddeletew
        [DllImport("Advapi32.dll", SetLastError = true, EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode)]
        private static extern bool CredDelete(string target, CredentialType type, int reservedFlag);

        private enum CredentialType : uint
        {
            Generic = 1,
            DomainPassword = 2,
            DomainCertificate = 3,
        }

        private const int ERROR_NOT_FOUND = 1168;

        /// <summary>
        /// Removes the cached TERMSRV/&lt;hostname&gt; entry from the current user's Windows
        /// Credential Manager.
        /// </summary>
        /// <param name="hostname">Hostname or IP address used in the RDP connection.</param>
        /// <returns>Outcome of the deletion attempt.</returns>
        public static ClearCachedCredentialsResult ClearCachedCredentials(string hostname)
        {
            if (string.IsNullOrWhiteSpace(hostname))
                return ClearCachedCredentialsResult.Failed;

            string target = "TERMSRV/" + hostname;
            try
            {
                bool deleted = CredDelete(target, CredentialType.DomainPassword, 0);
                if (deleted)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                        $"Cleared cached RDP credentials for {target}.");
                    return ClearCachedCredentialsResult.Deleted;
                }

                int err = Marshal.GetLastWin32Error();
                if (err == ERROR_NOT_FOUND)
                {
                    return ClearCachedCredentialsResult.NotFound;
                }

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    $"CredDelete failed for {target} (Win32 error {err}).");
                return ClearCachedCredentialsResult.Failed;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                    $"Failed to clear cached RDP credentials for {target}.", ex);
                return ClearCachedCredentialsResult.Failed;
            }
        }
    }
}
