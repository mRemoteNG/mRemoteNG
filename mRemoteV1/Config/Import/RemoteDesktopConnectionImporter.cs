using System;
using System.IO;
using System.Linq;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Container;


namespace mRemoteNG.Config.Import
{
	public class RemoteDesktopConnectionImporter : IConnectionImporter
	{
        public void Import(object fileName, ContainerInfo destinationContainer)
        {
            var fileNameAsString = fileName as string;
            if(fileNameAsString == null)
                return;
            if (File.Exists(fileNameAsString))
                Import(fileNameAsString, destinationContainer);
        }

        public void Import(string fileName, ContainerInfo destinationContainer)
		{
            var dataProvider = new FileDataProvider(fileName);
            var content = dataProvider.Load();

            var deserializer = new RemoteDesktopConnectionDeserializer();
            var connectionTreeModel = deserializer.Deserialize(content);

            var importedConnection = connectionTreeModel.RootNodes.First().Children.First();
			
            if (importedConnection == null) return;
            importedConnection.Name = Path.GetFileNameWithoutExtension(fileName);
            destinationContainer.AddChild(importedConnection);
		}
    }
}