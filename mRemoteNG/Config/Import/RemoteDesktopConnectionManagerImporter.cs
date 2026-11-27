using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Container;


namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    public class RemoteDesktopConnectionManagerImporter : IConnectionImporter<string>
    {
        public void Import(string filePath, ContainerInfo destinationContainer)
        {
            string fileContent = ImportFileReader.ReadText(filePath);

            RemoteDesktopConnectionManagerDeserializer deserializer = new();
            Tree.ConnectionTreeModel connectionTreeModel = deserializer.Deserialize(fileContent);
            ImportBudget.ValidateTree(connectionTreeModel);

            ContainerInfo importedRootNode = connectionTreeModel.RootNodes.First();
            if (importedRootNode == null) return;
            Connection.ConnectionInfo[] childrenToAdd = importedRootNode.Children.ToArray();
            destinationContainer.AddChildrenAtomically(childrenToAdd);
        }
    }
}
