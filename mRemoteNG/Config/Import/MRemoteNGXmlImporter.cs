using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Container;
using mRemoteNG.Messages;


namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    // ReSharper disable once InconsistentNaming
    public class MRemoteNGXmlImporter : IConnectionImporter<string>
    {
        public void Import(string fileName, ContainerInfo destinationContainer)
        {
            if (fileName == null)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Unable to import file. File path is null.");
                return;
            }

            if (!File.Exists(fileName))
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    $"Unable to import file. File does not exist. Path: {fileName}");

            var dataProvider = new FileDataProvider(fileName);
            var xmlString = dataProvider.Load();
            var xmlConnectionsDeserializer = new XmlConnectionsDeserializer();
            var connectionTreeModel = xmlConnectionsDeserializer.Deserialize(xmlString, true);

            var rootImportContainer = new ContainerInfo {Name = Path.GetFileNameWithoutExtension(fileName)};
            rootImportContainer.AddChildRange(connectionTreeModel.RootNodes.First().Children.ToArray());
            destinationContainer.AddChild(rootImportContainer);
        }
    }
}