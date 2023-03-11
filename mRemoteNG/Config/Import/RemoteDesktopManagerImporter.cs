#region

using System.IO;
using System.Runtime.Versioning;
using Castle.Core.Internal;
using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
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

            var dataProvider = new FileDataProvider(filePath);
            var csvString = dataProvider.Load();

            if (!string.IsNullOrEmpty(csvString))
            {
                var csvDeserializer = new CsvConnectionsDeserializerRdmFormat();
                var connectionTreeModel = csvDeserializer.Deserialize(csvString);

                var rootContainer = new ContainerInfo { Name = Path.GetFileNameWithoutExtension(filePath) };
                rootContainer.AddChildRange(connectionTreeModel.RootNodes);
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