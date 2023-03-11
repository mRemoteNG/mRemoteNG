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
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Serializers.MiscSerializers
{
    [SupportedOSPlatform("windows")]
    public class ActiveDirectoryDeserializer
    {
        private readonly string _ldapPath;
        private readonly bool _importSubOu;

        public ActiveDirectoryDeserializer(string ldapPath, bool importSubOu)
        {
            _ldapPath = ldapPath.ThrowIfNullOrEmpty(nameof(ldapPath));
            _importSubOu = importSubOu;
        }

        public ConnectionTreeModel Deserialize()
        {
            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);

            ImportContainers(_ldapPath, root);

            return connectionTreeModel;
        }

        private void ImportContainers(string ldapPath, ContainerInfo parentContainer)
        {
            var match = Regex.Match(ldapPath, "ou=([^,]*)", RegexOptions.IgnoreCase);
            var name = match.Success ? match.Groups[1].Captures[0].Value : Language.ActiveDirectory;

            var newContainer = new ContainerInfo {Name = name};
            parentContainer.AddChild(newContainer);

            ImportComputers(ldapPath, newContainer);
        }

        private void ImportComputers(string ldapPath, ContainerInfo parentContainer)
        {
            try
            {
                const string ldapFilter = "(|(objectClass=computer)(objectClass=organizationalUnit))";
                using (var ldapSearcher = new DirectorySearcher())
                {
                    ldapSearcher.SearchRoot = new DirectoryEntry(ldapPath);
                    ldapSearcher.Filter = ldapFilter;
                    ldapSearcher.SearchScope = SearchScope.OneLevel;
                    ldapSearcher.PropertiesToLoad.AddRange(new[] {"securityEquals", "cn", "objectClass"});

                    var ldapResults = ldapSearcher.FindAll();
                    foreach (SearchResult ldapResult in ldapResults)
                    {
                        using (var directoryEntry = ldapResult.GetDirectoryEntry())
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
            var displayName = Convert.ToString(directoryEntry.Properties["cn"].Value);
            var description = Convert.ToString(directoryEntry.Properties["Description"].Value);
            var hostName = Convert.ToString(directoryEntry.Properties["dNSHostName"].Value);

            var newConnectionInfo = new ConnectionInfo
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