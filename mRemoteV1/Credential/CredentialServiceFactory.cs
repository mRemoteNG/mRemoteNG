using System.Collections.Generic;
using System.IO;
using mRemoteNG.App.Info;
using mRemoteNG.Config;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Credential.Repositories;

namespace mRemoteNG.Credential
{
    public class CredentialServiceFactory
    {
        // When we get a true CompositionRoot we can move this to that class. We should only require 1 instance of this service at a time
        public CredentialService Build()
        {
            var repositoryList = new CredentialRepositoryList();
            var credentialRepoListPath = Path.Combine(SettingsFileInfo.SettingsPath, "credentialRepositories.xml");
            var repoListDataProvider = new FileDataProvider(credentialRepoListPath);
            var repositoryFactories = new List<ICredentialRepositoryFactory>();
            var persistor = new CredentialRepositoryListPersistor(repoListDataProvider, repositoryFactories);

            return new CredentialService(repositoryList, repositoryFactories, persistor, persistor);
        }
    }
}