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
            var serializationResult = deserializer.Deserialize(xmlContent);

            destinationContainer.AddChildRange(serializationResult.ConnectionRecords);
        }
	}
}