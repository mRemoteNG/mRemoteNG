using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Container;
using mRemoteNG.Credential;


namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    public class RemoteDesktopConnectionImporter : IConnectionImporter<string>
    {
        public void Import(string fileName, ContainerInfo destinationContainer)
        {
            FileDataProvider dataProvider = new(fileName);
            string content = dataProvider.Load();

            RemoteDesktopConnectionDeserializer deserializer = new();
            Tree.ConnectionTreeModel connectionTreeModel = deserializer.Deserialize(content);

            Connection.ConnectionInfo importedConnection = connectionTreeModel.RootNodes.First().Children.First();

            if (importedConnection == null) return;
            importedConnection.Name = Path.GetFileNameWithoutExtension(fileName);

            if (Runtime.CredentialProviderCatalog.CredentialProviders.Any())
            {
                ICredentialRepository repository = Runtime.CredentialProviderCatalog.CredentialProviders.First();
                CredentialImportHelper.ExtractCredentials(importedConnection, repository);
            }

            destinationContainer.AddChild(importedConnection);
        }
    }
}
