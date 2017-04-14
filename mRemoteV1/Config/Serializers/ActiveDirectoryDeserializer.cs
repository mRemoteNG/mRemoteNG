using System;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using mRemoteNG.App;
using mRemoteNG.Config.Import;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Config.Serializers
{
    public class ActiveDirectoryDeserializer : IDeserializer
    {
        private readonly string _ldapPath;
        private readonly bool _importSubOU;

        public ActiveDirectoryDeserializer(string ldapPath, bool importSubOU)
        {
            _ldapPath = ldapPath;
            _importSubOU = importSubOU;
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
            var name = match.Success ? match.Groups[1].Captures[0].Value : Language.strActiveDirectory;

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
                    ldapSearcher.PropertiesToLoad.AddRange(new[] { "securityEquals", "cn", "objectClass" });

                    var ldapResults = ldapSearcher.FindAll();
                    foreach (SearchResult ldapResult in ldapResults)
                    {
                        using (var directoryEntry = ldapResult.GetDirectoryEntry())
                        {
                            if (directoryEntry.Properties["objectClass"].Contains("organizationalUnit"))
                            {
                                // check/continue here so we don't create empty connection objects
                                if(!_importSubOU) continue;

                                ActiveDirectoryImporter.Import(ldapResult.Path, parentContainer, _importSubOU);
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
                Description = description
            };
            newConnectionInfo.Inheritance.TurnOnInheritanceCompletely();
            newConnectionInfo.Inheritance.Description = false;

            parentContainer.AddChild(newConnectionInfo);
        }
    }
}