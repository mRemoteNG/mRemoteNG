using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Container;


namespace mRemoteNG.Config.Import
{
	public class RemoteDesktopConnectionManagerImporter : IConnectionImporter<string>
	{
	    public void Import(string filePath, ContainerInfo destinationContainer)
		{
            var dataProvider = new FileDataProvider(filePath);
            var fileContent = dataProvider.Load();

            var deserializer = new RemoteDesktopConnectionManagerDeserializer();
            var serializationResult = deserializer.Deserialize(fileContent);

            destinationContainer.AddChildRange(serializationResult.ConnectionRecords);
        }
	}
}