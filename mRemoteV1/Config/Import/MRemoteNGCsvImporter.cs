using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.Csv;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using System.IO;

namespace mRemoteNG.Config.Import
{
    public class MRemoteNGCsvImporter : IConnectionImporter<string>
    {
        public void Import(string filePath, ContainerInfo destinationContainer)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Unable to import file. File path is null.");
                return;
            }

            if (!File.Exists(filePath))
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Unable to import file. File does not exist. Path: {filePath}");

            var dataProvider = new FileDataProvider(filePath);
            var xmlString = dataProvider.Load();
            var xmlConnectionsDeserializer = new CsvConnectionsDeserializerMremotengFormat();
            var serializationResult = xmlConnectionsDeserializer.Deserialize(xmlString);

            var rootImportContainer = new ContainerInfo { Name = Path.GetFileNameWithoutExtension(filePath) };
            rootImportContainer.AddChildRange(serializationResult.ConnectionRecords);
            destinationContainer.AddChild(rootImportContainer);
        }
    }
}
