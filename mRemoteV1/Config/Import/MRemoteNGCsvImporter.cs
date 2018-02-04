using System;
using System.IO;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Container;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Import
{
    public class MRemoteNGCsvImporter : IConnectionImporter
    {
        public void Import(object filePath, ContainerInfo destinationContainer)
        {
            var filePathAsString = filePath as string;
            if (filePathAsString == null)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Unable to import file. File path is null.");
                return;
            }

            if (File.Exists(filePathAsString))
                Import(filePathAsString, destinationContainer);
            else
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Unable to import file. File does not exist. Path: {filePathAsString}");
        }

        public void Import(string fileName, ContainerInfo destinationContainer)
        {
            var dataProvider = new FileDataProvider(fileName);
            var xmlString = dataProvider.Load();
            var xmlConnectionsDeserializer = new CsvConnectionsDeserializerMremotengFormat();
            var connectionTreeModel = xmlConnectionsDeserializer.Deserialize(xmlString);

            var rootImportContainer = new ContainerInfo { Name = Path.GetFileNameWithoutExtension(fileName) };
            rootImportContainer.AddChildRange(connectionTreeModel.RootNodes.First().Children.ToArray());
            destinationContainer.AddChild(rootImportContainer);
        }
    }
}
