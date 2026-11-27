using System;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Container;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    public class ActiveDirectoryImporter : IConnectionImporter<string>
    {
        public void Import(string ldapPath, ContainerInfo destinationContainer)
        {
            Import(ldapPath, destinationContainer, false);
        }

        public static void Import(string ldapPath, ContainerInfo destinationContainer, bool importSubOu)
        {
            try
            {
                Import(ldapPath, destinationContainer, importSubOu, new ImportBudget());
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Config.Import.ActiveDirectory.Import() failed.", ex);
            }
        }

        internal static void Import(string ldapPath, ContainerInfo destinationContainer, bool importSubOu, ImportBudget budget)
        {
            ldapPath.ThrowIfNullOrEmpty(nameof(ldapPath));
            ActiveDirectoryDeserializer deserializer = new(ldapPath, importSubOu, budget);
            Tree.ConnectionTreeModel connectionTreeModel = deserializer.Deserialize();
            ImportBudget.ValidateTree(connectionTreeModel);
            ContainerInfo importedRootNode = connectionTreeModel.RootNodes.First();
            if (importedRootNode == null) return;
            Connection.ConnectionInfo[] childrenToAdd = importedRootNode.Children.ToArray();
            destinationContainer.AddChildrenAtomically(childrenToAdd);
        }
    }
}
