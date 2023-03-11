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
                ldapPath.ThrowIfNullOrEmpty(nameof(ldapPath));
                var deserializer = new ActiveDirectoryDeserializer(ldapPath, importSubOu);
                var connectionTreeModel = deserializer.Deserialize();
                var importedRootNode = connectionTreeModel.RootNodes.First();
                if (importedRootNode == null) return;
                var childrenToAdd = importedRootNode.Children.ToArray();
                destinationContainer.AddChildRange(childrenToAdd);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Config.Import.ActiveDirectory.Import() failed.", ex);
            }
        }
    }
}