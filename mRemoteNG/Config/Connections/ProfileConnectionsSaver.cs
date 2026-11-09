using System;
using System.Runtime.Versioning;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Config.UserProfiles;
using mRemoteNG.Security;
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

            // Refuse to overwrite the profile store with an empty document. The profile
            // store has no rolling backup, so an empty write destroys the connections
            // outright. An empty result indicates a serialization failure.
            if (string.IsNullOrWhiteSpace(xml))
                throw new InvalidOperationException("Connection serialization produced no output; refusing to overwrite the profile connection store");

            dataProvider.Save(xml);
        }
    }
}
