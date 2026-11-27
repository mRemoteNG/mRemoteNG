#region

using System.IO;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Csv.RemoteDesktopManager;
using mRemoteNG.Container;
using mRemoteNG.Messages;

#endregion

namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    public class RemoteDesktopManagerImporter : IConnectionImporter<string>
    {
        public void Import(string filePath, ContainerInfo destinationContainer)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Unable to import file. File path is null.");
                return;
            }

            if (!File.Exists(filePath))
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Unable to import file. File does not exist. Path: {filePath}");

            string csvString = ImportFileReader.ReadText(filePath);

            if (!string.IsNullOrEmpty(csvString))
            {
                CsvConnectionsDeserializerRdmFormat csvDeserializer = new();
                Tree.ConnectionTreeModel connectionTreeModel = csvDeserializer.Deserialize(csvString);
                ImportBudget.ValidateTree(connectionTreeModel);

                ContainerInfo rootContainer = new() { Name = Path.GetFileNameWithoutExtension(filePath) };
                rootContainer.AddChildrenAtomically(connectionTreeModel.RootNodes);
                destinationContainer.AddChild(rootContainer);
            }
            else
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Unable to import file. File is empty.");
                return;
            }
        }
    }
}
