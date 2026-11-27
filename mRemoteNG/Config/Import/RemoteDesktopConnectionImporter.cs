using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Container;


namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    public class RemoteDesktopConnectionImporter : IConnectionImporter<string>
    {
        public void Import(string fileName, ContainerInfo destinationContainer)
        {
            string content = ImportFileReader.ReadText(fileName);

            RemoteDesktopConnectionDeserializer deserializer = new();
            Tree.ConnectionTreeModel connectionTreeModel = deserializer.Deserialize(content);
            ImportBudget.ValidateTree(connectionTreeModel);

            Connection.ConnectionInfo importedConnection = connectionTreeModel.RootNodes.First().Children.First();

            if (importedConnection == null) return;
            importedConnection.Name = Path.GetFileNameWithoutExtension(fileName);
            destinationContainer.AddChild(importedConnection);
        }
    }
}
