using System.Linq;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Container;


namespace mRemoteNG.Config.Import
{
    public class PuttyConnectionManagerImporter : IConnectionImporter<string>
    {
        public void Import(string filePath, ContainerInfo destinationContainer)
        {
            var dataProvider = new FileDataProvider(filePath);
            var xmlContent = dataProvider.Load();

            var deserializer = new PuttyConnectionManagerDeserializer();
            var connectionTreeModel = deserializer.Deserialize(xmlContent);

            var importedRootNode = connectionTreeModel.RootNodes.First();
            if (importedRootNode == null) return;
            var childrenToAdd = importedRootNode.Children.ToArray();
            destinationContainer.AddChildRange(childrenToAdd);
        }
    }
}