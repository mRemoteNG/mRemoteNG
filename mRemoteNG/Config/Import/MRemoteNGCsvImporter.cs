using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Csv;
using mRemoteNG.Container;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
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
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    $"Unable to import file. File does not exist. Path: {filePath}");

            string xmlString = ImportFileReader.ReadText(filePath);
            CsvConnectionsDeserializerMremotengFormat xmlConnectionsDeserializer = new();
            Tree.ConnectionTreeModel connectionTreeModel = xmlConnectionsDeserializer.Deserialize(xmlString);
            ImportBudget.ValidateTree(connectionTreeModel);

            ContainerInfo rootImportContainer = new() { Name = Path.GetFileNameWithoutExtension(filePath)};
            rootImportContainer.AddChildrenAtomically(connectionTreeModel.RootNodes[0].Children.ToArray());
            destinationContainer.AddChild(rootImportContainer);
        }
    }
}
