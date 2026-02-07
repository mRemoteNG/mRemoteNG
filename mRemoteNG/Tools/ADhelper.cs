using System;
using System.Collections;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using mRemoteNG.Security;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class AdHelper(string domain)
    {
        private DirectoryEntry _dEntry;

        public Hashtable Children { get; } = [];

        private string Domain { get; } = domain;

        public void GetChildEntries(string adPath = "")
        {
            // Sanitize inputs to prevent LDAP injection
            string sanitizedDomain = string.IsNullOrEmpty(Domain) ? string.Empty : LdapPathSanitizer.SanitizeDistinguishedName(Domain);
            string sanitizedAdPath = string.IsNullOrEmpty(adPath) ? string.Empty : LdapPathSanitizer.SanitizeLdapPath(adPath);

            _dEntry = sanitizedAdPath.Length <= 0
                ? sanitizedDomain.Length <= 0 ? new DirectoryEntry() : new DirectoryEntry("LDAP://" + sanitizedDomain)
                : new DirectoryEntry(sanitizedAdPath);
            try
            {
                foreach (DirectoryEntry child in _dEntry.Children)
                    Children.Add(child.Name, child.Path);
            }
            catch (COMException ex)
            {
                if (ex.Message.ToLower().Equals("the server is not operational"))
                    throw new Exception("Could not find AD Server", ex);
            }
        }
    }
}
