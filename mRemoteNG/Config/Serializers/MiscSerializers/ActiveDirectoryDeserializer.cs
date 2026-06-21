using System;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using mRemoteNG.App;
using mRemoteNG.Config.Import;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.Resources.Language;
using mRemoteNG.Security;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Serializers.MiscSerializers
{
    [SupportedOSPlatform("windows")]
    public class ActiveDirectoryDeserializer(string ldapPath, bool importSubOu)
    {
        private readonly string _ldapPath = SanitizeLdapPath(ldapPath.ThrowIfNullOrEmpty(nameof(ldapPath)));
        private readonly bool _importSubOu = importSubOu;

        private static string SanitizeLdapPath(string ldapPath)
        {
            // Validate the LDAP path format
            if (!LdapPathSanitizer.IsValidDistinguishedNameFormat(ldapPath))
            {
                throw new ArgumentException("Invalid LDAP path format", nameof(ldapPath));
            }

            // For LDAP paths (URIs like LDAP://...), we need to sanitize the DN portion
            // If it starts with LDAP:// or LDAPS://, extract and sanitize the DN part
            if (ldapPath.StartsWith("LDAP://", StringComparison.OrdinalIgnoreCase) ||
                ldapPath.StartsWith("LDAPS://", StringComparison.OrdinalIgnoreCase))
            {
                int schemeEndIndex = ldapPath.IndexOf("://", StringComparison.OrdinalIgnoreCase) + 3;
                if (schemeEndIndex < ldapPath.Length)
                {
                    // Find the server/domain part (before the first /)
                    int pathStartIndex = ldapPath.IndexOf('/', schemeEndIndex);
                    if (pathStartIndex > 0)
                    {
                        string scheme = ldapPath.Substring(0, schemeEndIndex);
                        string serverPart = ldapPath.Substring(schemeEndIndex, pathStartIndex - schemeEndIndex);
                        string dnPart = ldapPath.Substring(pathStartIndex + 1);
                        
                        // Sanitize the DN part
                        string sanitizedDn = LdapPathSanitizer.SanitizeDistinguishedName(dnPart);
                        return scheme + serverPart + "/" + sanitizedDn;
                    }
                }
                // If no DN part found, return the path as-is (just the server)
                return ldapPath;
            }
            else
            {
                // For plain DN strings, sanitize directly
                return LdapPathSanitizer.SanitizeDistinguishedName(ldapPath);
            }
        }

        public ConnectionTreeModel Deserialize()
        {
            ConnectionTreeModel connectionTreeModel = new();
            RootNodeInfo root = new(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);

            ImportContainers(_ldapPath, root);

            return connectionTreeModel;
        }

        private void ImportContainers(string ldapPath, ContainerInfo parentContainer)
        {
            Match match = Regex.Match(ldapPath, "ou=([^,]*)", RegexOptions.IgnoreCase);
            string name = match.Success ? match.Groups[1].Captures[0].Value : Language.ActiveDirectory;

            ContainerInfo newContainer = new() { Name = name};
            parentContainer.AddChild(newContainer);

            ImportComputers(ldapPath, newContainer);
        }

        private void ImportComputers(string ldapPath, ContainerInfo parentContainer)
        {
            try
            {
                const string ldapFilter = "(|(objectClass=computer)(objectClass=organizationalUnit))";
                using (DirectorySearcher ldapSearcher = new())
                {
                    ldapSearcher.SearchRoot = new DirectoryEntry(ldapPath);
                    ldapSearcher.Filter = ldapFilter;
                    ldapSearcher.SearchScope = SearchScope.OneLevel;
                    ldapSearcher.PropertiesToLoad.AddRange(new[] {"securityEquals", "cn", "objectClass"});

                    SearchResultCollection ldapResults = ldapSearcher.FindAll();
                    foreach (SearchResult ldapResult in ldapResults)
                    {
                        using (DirectoryEntry directoryEntry = ldapResult.GetDirectoryEntry())
                        {
                            if (directoryEntry.Properties["objectClass"].Contains("organizationalUnit"))
                            {
                                // check/continue here so we don't create empty connection objects
                                if (!_importSubOu) continue;

                                // TODO - this is a circular call. A deserializer should not call an importer
                                ActiveDirectoryImporter.Import(ldapResult.Path, parentContainer, _importSubOu);
                                continue;
                            }

                            DeserializeConnection(directoryEntry, parentContainer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Config.Import.ActiveDirectory.ImportComputers() failed.", ex);
            }
        }

        private void DeserializeConnection(DirectoryEntry directoryEntry, ContainerInfo parentContainer)
        {
            string displayName = Convert.ToString(directoryEntry.Properties["cn"].Value);
            string description = Convert.ToString(directoryEntry.Properties["Description"].Value);
            string hostName = Convert.ToString(directoryEntry.Properties["dNSHostName"].Value);

            ConnectionInfo newConnectionInfo = new()
            {
                Name = displayName,
                Hostname = hostName,
                Description = description,
                Protocol = ProtocolType.RDP
            };
            newConnectionInfo.Inheritance.TurnOnInheritanceCompletely();
            newConnectionInfo.Inheritance.Description = false;

            parentContainer.AddChild(newConnectionInfo);
        }
    }
}