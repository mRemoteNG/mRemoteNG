using System.IO;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.CredentialProviderSerializer;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tools;

namespace mRemoteNG.Credential
{
    public class CredentialServiceFactory
    {
        private ICredentialRepositoryList _credentialRepositoryList;

        public CredentialServiceFactory(ICredentialRepositoryList credentialRepositoryList)
        {
            _credentialRepositoryList = credentialRepositoryList.ThrowIfNull(nameof(credentialRepositoryList));
        }

        // When we get a true CompositionRoot we can move this to that class. We should only require 1 instance of this service at a time
        public CredentialServiceFacade Build()
        {
            var cryptoFromSettings = new CryptoProviderFactoryFromSettings();
            var credRepoSerializer = new XmlCredentialPasswordEncryptorDecorator(
                cryptoFromSettings.Build(),
                new XmlCredentialRecordSerializer());
            var credRepoDeserializer = new XmlCredentialPasswordDecryptorDecorator(new XmlCredentialRecordDeserializer());

            var credentialRepoListPath = Path.Combine(SettingsFileInfo.SettingsPath, "credentialRepositories.xml");
            var repoListDataProvider = new FileDataProvider(credentialRepoListPath);
            var repoListLoader = new CredentialRepositoryListLoader(
                repoListDataProvider,
                new CredentialRepositoryListDeserializer(credRepoSerializer, credRepoDeserializer));
            var repoListSaver = new CredentialRepositoryListSaver(repoListDataProvider);

            return new CredentialServiceFacade(_credentialRepositoryList, repoListLoader, repoListSaver);
        }
    }
}