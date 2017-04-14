using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Config;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tools;

namespace mRemoteNG.App.Initialization
{
    public class CredsAndConsSetup
    {
        private readonly ISecureSerializer<IEnumerable<ICredentialRecord>, string> _credRepoSerializer;
        private readonly ISecureDeserializer<string, IEnumerable<ICredentialRecord>> _credRepoDeserializer;
        private readonly CredentialsService _credentialsService;
        private readonly string _credentialFilePath;

        public CredsAndConsSetup(CredentialsService credentialsService, string credentialFilePath)
        {
            if (credentialsService == null)
                throw new ArgumentNullException(nameof(credentialsService));

            _credentialFilePath = credentialFilePath;

            var cryptoFromSettings = new CryptoProviderFactoryFromSettings();
            _credRepoSerializer = new XmlCredentialPasswordEncryptorDecorator(
                cryptoFromSettings.Build(),
                new XmlCredentialRecordSerializer());
            _credRepoDeserializer = new XmlCredentialPasswordDecryptorDecorator(new XmlCredentialRecordDeserializer());

            _credentialsService = credentialsService;
        }

        public void LoadCredsAndCons()
        {
            if (Settings.Default.FirstStart && !Settings.Default.LoadConsFromCustomLocation && !File.Exists(Runtime.GetStartupConnectionFileName()))
                Runtime.NewConnections(Runtime.GetStartupConnectionFileName());

            var upgradeMap = UpgradeUserFilesForCredentialManager();

            LoadCredentialRepositoryList();
            LoadDefaultConnectionCredentials();
            Runtime.LoadConnections();

            if (upgradeMap != null)
                ApplyCredentialMapping(upgradeMap);
        }

        private Dictionary<Guid, ICredentialRecord> UpgradeUserFilesForCredentialManager()
        {
            var connectionFileProvider = new FileDataProvider(Runtime.GetStartupConnectionFileName());
            var xdoc = XDocument.Parse(connectionFileProvider.Load());

            if (double.Parse(xdoc.Root?.Attribute("ConfVersion")?.Value ?? "0") >= 2.7) return null;
            EnsureConnectionXmlElementsHaveIds(xdoc);
            connectionFileProvider.Save($"{xdoc.Declaration}\n {xdoc}");

            var cryptoProvider = new CryptoProviderFactoryFromXml(xdoc.Root).Build();
            var encryptedValue = xdoc.Root?.Attribute("Protected")?.Value;
            var auth = new PasswordAuthenticator(cryptoProvider, encryptedValue)
            {
                AuthenticationRequestor = () => MiscTools.PasswordDialog("", false)
            };
            if (!auth.Authenticate(Runtime.EncryptionKey))
                throw new Exception("Could not authenticate");

            var newCredRepoKey = auth.LastAuthenticatedPassword;

            var credentialHarvester = new CredentialHarvester();
            var harvestedCredentials = credentialHarvester.Harvest(xdoc, newCredRepoKey);

            var xmlRepoFactory = new XmlCredentialRepositoryFactory(_credRepoSerializer, _credRepoDeserializer);
            var newCredentialRepository = xmlRepoFactory.Build(
                new CredentialRepositoryConfig
                {
                    Source = _credentialFilePath,
                    Title = "Converted Credentials",
                    TypeName = "Xml",
                    Key = newCredRepoKey
                }
            );

            newCredentialRepository.LoadCredentials(newCredRepoKey);

            foreach (var credential in harvestedCredentials)
                newCredentialRepository.CredentialRecords.Add(credential);
            newCredentialRepository.SaveCredentials(newCredRepoKey);

            _credentialsService.AddRepository(newCredentialRepository);
            return credentialHarvester.ConnectionToCredentialMap;
        }

        private void EnsureConnectionXmlElementsHaveIds(XDocument xdoc)
        {
            var adapter = new ConfConsEnsureConnectionsHaveIds();
            adapter.EnsureElementsHaveIds(xdoc);
        }

        private void LoadCredentialRepositoryList()
        {
            _credentialsService.LoadRepositoryList();
        }

        private void LoadDefaultConnectionCredentials()
        {
            var defaultCredId = Settings.Default.ConDefaultCredentialRecord;
            var matchedCredentials = _credentialsService.GetCredentialRecords().Where(record => record.Id.Equals(defaultCredId)).ToArray();
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