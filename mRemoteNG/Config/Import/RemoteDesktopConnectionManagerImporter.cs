using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Container;


namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    public class RemoteDesktopConnectionManagerImporter : IConnectionImporter<string>
    {
        public void Import(string filePath, ContainerInfo destinationContainer)
        {
            var dataProvider = new FileDataProvider(filePath);
            var fileContent = dataProvider.Load();

            var deserializer = new RemoteDesktopConnectionManagerDeserializer();
            var connectionTreeModel = deserializer.Deserialize(fileContent);

            var importedRootNode = connectionTreeModel.RootNodes.First();
            if (importedRootNode == null) return;
            var childrenToAdd = importedRootNode.Children.ToArray();
            destinationContainer.AddChildRange(childrenToAdd);
        }
    }
}