using System;
using System.Collections.Generic;
using System.Xml.Linq;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using System.Linq;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class XmlCredentialManagerUpgrader
    {
        private readonly IDeserializer<string, ConnectionTreeModel> _deserializer;


        public XmlCredentialManagerUpgrader(IDeserializer<string, ConnectionTreeModel> decoratedDeserializer)
        {
            _deserializer = decoratedDeserializer.ThrowIfNull(nameof(decoratedDeserializer));
        }

        public ConnectionTreeModel Deserialize(string serializedData, ConnectionToCredentialMap upgradeMap)
        {
            var serializedDataAsXDoc = EnsureConnectionXmlElementsHaveIds(serializedData);
            var serializedDataWithIds = $"{serializedDataAsXDoc.Declaration}{serializedDataAsXDoc}";

            var connectionTreeModel = _deserializer.Deserialize(serializedDataWithIds);

            if (upgradeMap != null)
                ApplyCredentialMapping(upgradeMap, connectionTreeModel.GetRecursiveChildList());

            return connectionTreeModel;
        }

        private XDocument EnsureConnectionXmlElementsHaveIds(string serializedData)
        {
            var xdoc = XDocument.Parse(serializedData);
            xdoc.Declaration = new XDeclaration("1.0", "utf-8", null);
            var adapter = new ConfConsEnsureConnectionsHaveIds();
            adapter.EnsureElementsHaveIds(xdoc);
            return xdoc;
        }

        public ConnectionToCredentialMap UpgradeUserFilesForCredentialManager(XDocument xdoc)
        {
            if (!CredentialManagerUpgradeNeeded(xdoc))
            {
                return null;
            }

            var cryptoProvider = new CryptoProviderFactoryFromXml(xdoc.Root).Build();
            var encryptedValue = xdoc.Root?.Attribute("Protected")?.Value;
            var auth = new PasswordAuthenticator(cryptoProvider, encryptedValue, () => MiscTools.PasswordDialog("", false));
            if (!auth.Authenticate(Runtime.EncryptionKey))
                throw new Exception("Could not authenticate");

            var keyForOldConnectionFile = auth.LastAuthenticatedPassword;

            var credentialHarvester = new CredentialHarvester();
            var harvestedCredentials = credentialHarvester.Harvest(xdoc, keyForOldConnectionFile);

            return harvestedCredentials;
        }

        /// <summary>
        /// If any connections in the xml contain a Username/Domain/Password field, we need to upgrade
        /// it to be compatible with the credential manager.
        /// </summary>
        /// <param name="xdoc"></param>
        public static bool CredentialManagerUpgradeNeeded(XContainer xdoc)
        {
            return xdoc
                .Descendants("Node")
                .Any(n => 
                    n.Attribute("Username") != null ||
                    n.Attribute("Domain") != null ||
                    n.Attribute("Password") != null);
        }

        private void ApplyCredentialMapping(IDictionary<Guid, ICredentialRecord> map, IEnumerable<AbstractConnectionRecord> connectionRecords)
        {
            foreach (var connectionInfo in connectionRecords)
            {
                Guid.TryParse(connectionInfo.ConstantID, out var id);
                if (map.ContainsKey(id))
                    connectionInfo.CredentialRecordId = map[id].Id.ToOptional();
            }
        }
    }
}