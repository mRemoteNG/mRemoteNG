using System;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Config;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools;
using mRemoteNG.Tools.CustomCollections;

namespace mRemoteNG.Credential
{
    public class CredentialService
    {
        private readonly List<ICredentialRepositoryFactory> _repositoryFactories;
        private readonly ILoader<IEnumerable<ICredentialRepository>> _loader;
        private readonly ISaver<IEnumerable<ICredentialRepository>> _saver;

        public ICredentialRepositoryList RepositoryList { get; }

        public IReadOnlyCollection<ICredentialRepositoryFactory> RepositoryFactories => _repositoryFactories;

        public CredentialService(
            ICredentialRepositoryList repositoryList,
            List<ICredentialRepositoryFactory> repositoryFactories,
            ILoader<IEnumerable<ICredentialRepository>> loader,
            ISaver<IEnumerable<ICredentialRepository>> saver)
        {
            RepositoryList = repositoryList.ThrowIfNull(nameof(repositoryList));
            _repositoryFactories = repositoryFactories.ThrowIfNull(nameof(repositoryFactories));
            _loader = loader.ThrowIfNull(nameof(loader));
            _saver = saver.ThrowIfNull(nameof(saver));

            SetupEventHandlers();
        }

        public void SaveRepositoryList()
        {
            _saver.Save(RepositoryList);
        }

        public void LoadRepositoryList()
        {
            var loadedRepositories = _loader.Load();

            foreach (var repository in loadedRepositories)
            {
                RepositoryList.AddProvider(repository);
            }
        }

        public void AddRepository(ICredentialRepository repository)
        {
            RepositoryList.AddProvider(repository);
        }

        public void RemoveRepository(ICredentialRepository repository)
        {
            RepositoryList.RemoveProvider(repository);
        }

        public IEnumerable<ICredentialRecord> GetCredentialRecords()
        {
            return RepositoryList.GetCredentialRecords();
        }

        public ICredentialRecord GetCredentialRecord(Guid id)
        {
            return RepositoryList.GetCredentialRecord(id);
        }

        /// <summary>
        /// Registers an <see cref="ICredentialRepositoryFactory"/> for
        /// use throughout the application.
        /// </summary>
        /// <param name="factory"></param>
        public void RegisterRepositoryFactory(ICredentialRepositoryFactory factory)
        {
            if (_repositoryFactories.Contains(factory))
                return;

            _repositoryFactories.Add(factory);
        }

        public Optional<ICredentialRepositoryFactory> GetRepositoryFactoryForConfig(ICredentialRepositoryConfig repositoryConfig)
        {
            return new Optional<ICredentialRepositoryFactory>(
                RepositoryFactories
                .FirstOrDefault(factory => 
                    string.Equals(factory.SupportsConfigType, repositoryConfig.TypeName)));
        }

        #region Setup
        private void SetupEventHandlers()
        {
            RepositoryList.RepositoriesUpdated += HandleRepositoriesUpdatedEvent;
            RepositoryList.CredentialsUpdated += HandleCredentialsUpdatedEvent;
        }

        private void HandleRepositoriesUpdatedEvent(object sender, CollectionUpdatedEventArgs<ICredentialRepository> collectionUpdatedEventArgs)
        {
            SaveRepositoryList();
        }

        private void HandleCredentialsUpdatedEvent(object sender, CollectionUpdatedEventArgs<ICredentialRecord> collectionUpdatedEventArgs)
        {
            var repo = sender as ICredentialRepository;
            repo?.SaveCredentials(repo.Config.Key);
        }
        #endregion
    }
}