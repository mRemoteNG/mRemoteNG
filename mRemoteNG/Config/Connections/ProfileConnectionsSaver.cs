using System.Runtime.Versioning;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Config.UserProfiles;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Connections
{
    [SupportedOSPlatform("windows")]
    internal sealed class ProfileConnectionsSaver(
        IConnectionProfileDataProvider dataProvider,
        SaveFilter saveFilter) : ISaver<ConnectionTreeModel>
    {
        public void Save(ConnectionTreeModel connectionTreeModel, string propertyNameTrigger = "")
        {
            XmlConnectionSerializerFactory serializerFactory = new();
            string xml = serializerFactory.Build(
                new CryptoProviderFactoryFromSettings().Build(),
                connectionTreeModel,
                saveFilter,
                Properties.OptionsSecurityPage.Default.EncryptCompleteConnectionsFile)
                .Serialize(connectionTreeModel.RootNodes[0]);
            dataProvider.Save(xml);
        }
    }
}
