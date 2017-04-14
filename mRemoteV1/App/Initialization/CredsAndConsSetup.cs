using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Config;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Tools;

namespace mRemoteNG.App.Initialization
{
    public class CredsAndConsSetup
    {
        private readonly CredentialManager _credentialManager;
        private readonly string _credentialFilePath;

        public CredsAndConsSetup(CredentialManager credentialManager, string credentialFilePath)
        {
            if (credentialManager == null)
                throw new ArgumentNullException(nameof(credentialManager));

            _credentialManager = credentialManager;
            _credentialFilePath = credentialFilePath;
        }

        public void LoadCredsAndCons()
        {
            if (Settings.Default.FirstStart && !Settings.Default.LoadConsFromCustomLocation && !File.Exists(Runtime.GetStartupConnectionFileName()))
                Runtime.NewConnections(Runtime.GetStartupConnectionFileName());

            var upgradeMap = UpgradeUserFilesForCredentialManager();

            LoadCredentials(_credentialManager);
            _credentialManager.CredentialsChanged += (o, args) => SaveCredentialList(_credentialManager.GetCredentialRecords());
            LoadDefaultConnectionCredentials();
            Runtime.LoadConnections();

            if (upgradeMap != null)
                ApplyCredentialMapping(upgradeMap);
        }

        private Dictionary<Guid, ICredentialRecord> UpgradeUserFilesForCredentialManager()
        {
            var connectionFileProvider = new FileDataProvider(Runtime.GetStartupConnectionFileName());
            var xdoc = XDocument.Parse(connectionFileProvider.Load());

            if (double.Parse(xdoc.Root?.Attribute("ConfVersion")?.Value) >= 2.7) return null;
            EnsureConnectionXmlElementsHaveIds(xdoc);
            connectionFileProvider.Save($"{xdoc.Declaration}\n {xdoc}");

            var cryptoProvider = CryptographyProviderFactory.BuildFromXml(xdoc.Root);
            var encryptedValue = xdoc.Root?.Attribute("Protected")?.Value;
            var auth = new PasswordAuthenticator(cryptoProvider, encryptedValue)
            {
                AuthenticationRequestor = () => MiscTools.PasswordDialog("", false)
            };
            if (!auth.Authenticate(Runtime.EncryptionKey))
                throw new Exception("Could not authenticate");

            var credentialHarvester = new CredentialHarvester();
            SaveCredentialList(credentialHarvester.Harvest(xdoc, auth.LastAuthenticatedPassword));
            return credentialHarvester.ConnectionToCredentialMap;
        }

        private void EnsureConnectionXmlElementsHaveIds(XDocument xdoc)
        {
            var adapter = new ConfConsEnsureConnectionsHaveIds();
            adapter.EnsureElementsHaveIds(xdoc);
        }

        private void LoadCredentials(CredentialManager credentialManager)
        {
            var credentialLoader = new CredentialRecordLoader(new FileDataProvider(_credentialFilePath), new XmlCredentialDeserializer());
            credentialManager.AddRange(credentialLoader.Load("tempEncryptionKey".ConvertToSecureString()).Cast<INotifyingCredentialRecord>());
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Loaded credentials from file: {_credentialFilePath}", true);
        }

        private void SaveCredentialList(IEnumerable<ICredentialRecord> records)
        {
            var engineFromSettings = Settings.Default.EncryptionEngine;
            var modeFromSettings = Settings.Default.EncryptionBlockCipherMode;
            var cryptoProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(engineFromSettings, modeFromSettings);
            cryptoProvider.KeyDerivationIterations = Settings.Default.EncryptionKeyDerivationIterations;

            var serializer = new XmlCredentialRecordSerializer(cryptoProvider);
            var dataProvider = new FileDataProviderWithRollingBackup(_credentialFilePath);
            var credentialSaver = new CredentialRecordSaver(dataProvider, serializer);
            credentialSaver.Save(records, "tempEncryptionKey".ConvertToSecureString());
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"Saved credentials to file: {_credentialFilePath}");
        }

        private void LoadDefaultConnectionCredentials()
        {
            var defaultCredId = Settings.Default.ConDefaultCredentialRecord;
            var matchedCredentials = Runtime.CredentialManager.GetCredentialRecords().Where(record => record.Id.Equals(defaultCredId)).ToArray();
            DefaultConnectionInfo.Instance.CredentialRecord = matchedCredentials.Any() ? matchedCredentials.First() : null;
        }

        private void ApplyCredentialMapping(IDictionary<Guid, ICredentialRecord> map)
        {
            foreach (var connectionInfo in Runtime.ConnectionTreeModel.GetRecursiveChildList())
            {
                Guid id;
                Guid.TryParse(connectionInfo.ConstantID, out id);
                if (map.ContainsKey(id))
                    connectionInfo.CredentialRecord = map[id];
            }
        }
    }
}