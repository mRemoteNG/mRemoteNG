using System;
using System.Collections.Generic;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class XmlCredentialManagerUpgrader
    {
        private readonly CredentialServiceFacade _credentialsService;
        private readonly string _credentialFilePath;

        public XmlCredentialManagerUpgrader(CredentialServiceFacade credentialsService, string credentialFilePath)
        {
            if (credentialsService == null)
                throw new ArgumentNullException(nameof(credentialsService));

            _credentialsService = credentialsService;
            _credentialFilePath = credentialFilePath;
        }

        public Dictionary<Guid, ICredentialRecord> UpgradeUserFilesForCredentialManager()
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

            var newCredentialRepository = BuildXmlCredentialRepo(newCredRepoKey);

            AddHarvestedCredentialsToRepo(harvestedCredentials, newCredentialRepository);
            newCredentialRepository.SaveCredentials(newCredRepoKey);

            _credentialsService.AddRepository(newCredentialRepository);
            return credentialHarvester.ConnectionToCredentialMap;
        }

        private ICredentialRepository BuildXmlCredentialRepo(SecureString newCredRepoKey)
        {
            var cryptoFromSettings = new CryptoProviderFactoryFromSettings();
            var credRepoSerializer = new XmlCredentialPasswordEncryptorDecorator(
                cryptoFromSettings.Build(),
                new XmlCredentialRecordSerializer());
            var credRepoDeserializer = new XmlCredentialPasswordDecryptorDecorator(new XmlCredentialRecordDeserializer());

            var xmlRepoFactory = new XmlCredentialRepositoryFactory(credRepoSerializer, credRepoDeserializer);
            var newRepo = xmlRepoFactory.Build(
                new CredentialRepositoryConfig
                {
                    Source = _credentialFilePath,
                    Title = "Converted Credentials",
                    TypeName = "Xml",
                    Key = newCredRepoKey
                }
            );
            newRepo.LoadCredentials(newCredRepoKey);
            return newRepo;
        }

        private void AddHarvestedCredentialsToRepo(IEnumerable<ICredentialRecord> harvestedCredentials, ICredentialRepository repo)
        {
            foreach (var credential in harvestedCredentials)
                repo.CredentialRecords.Add(credential);
        }

        private void EnsureConnectionXmlElementsHaveIds(XDocument xdoc)
        {
            var adapter = new ConfConsEnsureConnectionsHaveIds();
            adapter.EnsureElementsHaveIds(xdoc);
        }

        public void ApplyCredentialMapping(IDictionary<Guid, ICredentialRecord> map, IEnumerable<AbstractConnectionRecord> connectionRecords)
        {
            foreach (var connectionInfo in connectionRecords)
            {
                Guid id;
                Guid.TryParse(connectionInfo.ConstantID, out id);
                if (map.ContainsKey(id))
                    connectionInfo.CredentialRecord = map[id];
            }
        }
    }
}
