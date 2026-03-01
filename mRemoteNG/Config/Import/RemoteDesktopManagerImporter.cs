#region

using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using Castle.Core.Internal;
using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Csv.RemoteDesktopManager;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Credential;
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

            FileDataProvider dataProvider = new(filePath);
            string csvString = dataProvider.Load();

            if (!string.IsNullOrEmpty(csvString))
            {
                CsvConnectionsDeserializerRdmFormat csvDeserializer = new();
                Tree.ConnectionTreeModel connectionTreeModel = csvDeserializer.Deserialize(csvString);

                if (Runtime.CredentialProviderCatalog.CredentialProviders.Any())
                {
                    ICredentialRepository repository = Runtime.CredentialProviderCatalog.CredentialProviders.First();
                    foreach (ConnectionInfo child in connectionTreeModel.RootNodes)
                    {
                        CredentialImportHelper.ExtractCredentials(child, repository);
                    }
                }

                ContainerInfo rootContainer = new() { Name = Path.GetFileNameWithoutExtension(filePath) };
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
