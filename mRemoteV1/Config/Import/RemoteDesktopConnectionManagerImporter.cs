using System.IO;
using System.Linq;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Container;


namespace mRemoteNG.Config.Import
{
	public class RemoteDesktopConnectionManagerImporter : IConnectionImporter
	{
        public void Import(object filePath, ContainerInfo destinationContainer)
        {
            var fileNameAsString = filePath as string;
            if (fileNameAsString == null)
                return;
            if (File.Exists(fileNameAsString))
                Import(fileNameAsString, destinationContainer);
        }

        public void Import(string filePath, ContainerInfo destinationContainer)
		{
            var dataProvider = new FileDataProvider(filePath);
            var fileContent = dataProvider.Load();

            var deserializer = new RemoteDesktopConnectionManagerDeserializer(fileContent);
            var connectionTreeModel = deserializer.Deserialize();

            var importedRootNode = connectionTreeModel.RootNodes.First();
            if (importedRootNode == null) return;
            var childrenToAdd = importedRootNode.Children.ToArray();
            destinationContainer.AddChildRange(childrenToAdd);
        }
	}
}