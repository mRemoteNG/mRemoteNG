using System.IO;
using System.Linq;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Container;


namespace mRemoteNG.Config.Import
{
	public class PuttyConnectionManagerImporter : IConnectionImporter
	{
        public void Import(object filePath, ContainerInfo destinationContainer)
        {
            var filePathAsString = filePath as string;
            if (filePathAsString == null)
                return;
            if (File.Exists(filePathAsString))
                Import(filePathAsString, destinationContainer);
        }

        public void Import(string filePath, ContainerInfo destinationContainer)
		{
            var dataProvider = new FileDataProvider(filePath);
            var xmlContent = dataProvider.Load();

            var deserializer = new PuttyConnectionManagerDeserializer(xmlContent);
            var connectionTreeModel = deserializer.Deserialize();

            var importedRootNode = connectionTreeModel.RootNodes.First();
            if (importedRootNode == null) return;
            var childrenToAdd = importedRootNode.Children.ToArray();
            destinationContainer.AddChildRange(childrenToAdd);
        }
	}
}