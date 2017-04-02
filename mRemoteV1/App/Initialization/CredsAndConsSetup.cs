using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using mRemoteNG.App.Info;
using mRemoteNG.Config;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.CredentialProviderSerializer;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Security;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Tools;

namespace mRemoteNG.App.Initialization
{
    public class CredsAndConsSetup
    {
        private readonly string _credentialRepoListPath = Path.Combine(SettingsFileInfo.SettingsPath, "credentialRepositories.xml");
        private readonly ICredentialRepositoryList _credentialRepositoryList;
        private readonly string _credentialFilePath;

        public CredsAndConsSetup(ICredentialRepositoryList credentialRepositoryList, string credentialFilePath)
        {
            if (credentialRepositoryList == null)
                throw new ArgumentNullException(nameof(credentialRepositoryList));

            _credentialRepositoryList = credentialRepositoryList;
            _credentialFilePath = credentialFilePath;
        }

        public void LoadCredsAndCons()
        {
            if (Settings.Default.FirstStart && !Settings.Default.LoadConsFromCustomLocation && !File.Exists(Runtime.GetStartupConnectionFileName()))
                Runtime.NewConnections(Runtime.GetStartupConnectionFileName());

            var upgradeMap = UpgradeUserFilesForCredentialManager();

            LoadCredentials();
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
            var harvestedCredentials = credentialHarvester.Harvest(xdoc, auth.LastAuthenticatedPassword);

            var newCredentialRepository = new XmlCredentialRepository(
                new CredentialRepositoryConfig(),
                new FileDataProvider(_credentialFilePath)
            );

            foreach (var credential in harvestedCredentials)
                newCredentialRepository.CredentialRecords.Add(credential);

            _credentialRepositoryList.AddProvider(newCredentialRepository);
            return credentialHarvester.ConnectionToCredentialMap;
        }

        private void EnsureConnectionXmlElementsHaveIds(XDocument xdoc)
        {
            var adapter = new ConfConsEnsureConnectionsHaveIds();
            adapter.EnsureElementsHaveIds(xdoc);
        }

        private void LoadCredentials()
        {
            var credRepoListDataProvider = new FileDataProvider(_credentialRepoListPath);
            var credRepoListLoader = new CredentialRepositoryListLoader(credRepoListDataProvider, new CredentialRepositoryListDeserializer());
            var credRepoListSaver = new CredentialRepositoryListSaver(credRepoListDataProvider);
            foreach (var repository in credRepoListLoader.Load())
            {
                Runtime.CredentialProviderCatalog.AddProvider(repository);
                //repository.LoadCredentials();
            }
            Runtime.CredentialProviderCatalog.RepositoriesUpdated += (sender, args) => credRepoListSaver.Save(Runtime.CredentialProviderCatalog.CredentialProviders);
            Runtime.CredentialProviderCatalog.CredentialsUpdated += (sender, args) => (sender as ICredentialRepository)?.SaveCredentials();
        }

        private void LoadDefaultConnectionCredentials()
        {
            var defaultCredId = Settings.Default.ConDefaultCredentialRecord;
            var matchedCredentials = _credentialRepositoryList.GetCredentialRecords().Where(record => record.Id.Equals(defaultCredId)).ToArray();
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