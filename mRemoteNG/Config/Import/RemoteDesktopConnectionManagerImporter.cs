using System;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Credential;
using mRemoteNG.Security;


namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    public class RemoteDesktopConnectionManagerImporter : IConnectionImporter<string>
    {
        public void Import(string filePath, ContainerInfo destinationContainer)
        {
            FileDataProvider dataProvider = new(filePath);
            string fileContent = dataProvider.Load();

            RemoteDesktopConnectionManagerDeserializer deserializer = new();
            Tree.ConnectionTreeModel connectionTreeModel = deserializer.Deserialize(fileContent);

            ContainerInfo importedRootNode = connectionTreeModel.RootNodes.First();
            if (importedRootNode == null) return;
            Connection.ConnectionInfo[] childrenToAdd = importedRootNode.Children.ToArray();

            if (Runtime.CredentialProviderCatalog.CredentialProviders.Any())
            {
                ICredentialRepository repository = Runtime.CredentialProviderCatalog.CredentialProviders.First();
                foreach (ConnectionInfo child in childrenToAdd)
                {
                    CredentialImportHelper.ExtractCredentials(child, repository);
                }
            }

            destinationContainer.AddChildRange(childrenToAdd);
        }
    }
}