using System;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.Security;

namespace mRemoteNG.Credential
{
    /// <summary>
    /// Resolves credential sets by their GUID (stored in UserViaAPI) and returns
    /// the username, password, and domain from the matching credential record.
    /// </summary>
    public static class CredentialSetResolver
    {
        /// <summary>
        /// Resolves a credential set identified by <paramref name="credentialIdOrTitle"/> and
        /// outputs the username, password, and domain.
        /// </summary>
        /// <param name="credentialIdOrTitle">The GUID or title of the credential set to resolve.</param>
        /// <param name="username">The resolved username.</param>
        /// <param name="password">The resolved password.</param>
        /// <param name="domain">The resolved domain.</param>
        public static void Resolve(string credentialIdOrTitle, out string username, out string password, out string domain)
        {
            username = "";
            password = "";
            domain = "";

            if (string.IsNullOrEmpty(credentialIdOrTitle))
                return;

            ICredentialRepositoryList? catalog = Runtime.CredentialProviderCatalog;
            if (catalog == null)
                return;

            ICredentialRecord? credential = null;

            // Try to resolve by GUID first
            if (Guid.TryParse(credentialIdOrTitle, out Guid credentialId))
            {
                credential = catalog.GetCredentialRecords()
                    .FirstOrDefault(c => c.Id == credentialId);
            }

            // Fall back to matching by title
            if (credential == null)
            {
                credential = catalog.GetCredentialRecords()
                    .FirstOrDefault(c => string.Equals(c.Title, credentialIdOrTitle, StringComparison.OrdinalIgnoreCase));
            }

            if (credential == null)
                return;

            username = credential.Username ?? "";
            password = credential.Password?.ConvertToUnsecureString() ?? "";
            domain = credential.Domain ?? "";
        }
    }
}
