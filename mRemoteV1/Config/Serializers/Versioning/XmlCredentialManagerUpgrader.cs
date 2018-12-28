using System;
using System.Collections.Generic;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.App;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using System.Linq;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class XmlCredentialManagerUpgrader : IDeserializer<string, ConnectionTreeModel>
    {
        private readonly CredentialService _credentialsService;
        private readonly IDeserializer<string, ConnectionTreeModel> _decoratedDeserializer;
        private readonly SecureString _newRepoPassword;

        public string CredentialFilePath { get; set; }
        

        public XmlCredentialManagerUpgrader(
            CredentialService credentialsService, 
            string credentialFilePath, 
            IDeserializer<string, ConnectionTreeModel> decoratedDeserializer,
            SecureString newRepoPassword)
        {
            _credentialsService = credentialsService.ThrowIfNull(nameof(credentialsService));
            CredentialFilePath = credentialFilePath;
            _newRepoPassword = newRepoPassword;
            _decoratedDeserializer = decoratedDeserializer.ThrowIfNull(nameof(decoratedDeserializer));
        }

        public ConnectionTreeModel Deserialize(string serializedData)
        {
            var serializedDataAsXDoc = EnsureConnectionXmlElementsHaveIds(serializedData);
            var upgradeMap = UpgradeUserFilesForCredentialManager(serializedDataAsXDoc);
            var serializedDataWithIds = $"{serializedDataAsXDoc.Declaration}{serializedDataAsXDoc}";

            var connectionTreeModel = _decoratedDeserializer.Deserialize(serializedDataWithIds);

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

        public Dictionary<Guid, ICredentialRecord> UpgradeUserFilesForCredentialManager(XDocument xdoc)
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

            var newCredentialRepository = BuildXmlCredentialRepo(_newRepoPassword);

            AddHarvestedCredentialsToRepo(harvestedCredentials, newCredentialRepository);
            newCredentialRepository.SaveCredentials(_newRepoPassword);

            _credentialsService.AddRepository(newCredentialRepository);
            return credentialHarvester.ConnectionToCredentialMap;
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

        private ICredentialRepository BuildXmlCredentialRepo(SecureString newCredRepoKey)
        {
            var repositoryConfig = new CredentialRepositoryConfig
            {
                Source = CredentialFilePath,
                Title = "Converted Credentials",
                TypeName = "Xml",
                Key = newCredRepoKey
            };
            
            var xmlRepoFactory = _credentialsService.GetRepositoryFactoryForConfig(repositoryConfig);

            if (!xmlRepoFactory.Any())
                throw new CredentialRepositoryTypeNotSupportedException(repositoryConfig.TypeName);

            var newRepo = xmlRepoFactory.First().Build(repositoryConfig);
            newRepo.LoadCredentials(newCredRepoKey);
            return newRepo;
        }

        private void AddHarvestedCredentialsToRepo(IEnumerable<ICredentialRecord> harvestedCredentials, ICredentialRepository repo)
        {
            foreach (var credential in harvestedCredentials)
                repo.CredentialRecords.Add(credential);
        }

        public void ApplyCredentialMapping(IDictionary<Guid, ICredentialRecord> map, IEnumerable<AbstractConnectionRecord> connectionRecords)
        {
            foreach (var connectionInfo in connectionRecords)
            {
                Guid.TryParse(connectionInfo.ConstantID, out var id);
                if (map.ContainsKey(id))
                    connectionInfo.CredentialRecordId = map[id].Id.Maybe();
            }
        }
    }
}