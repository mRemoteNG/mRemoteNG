using System;
using System.Collections.Generic;
using mRemoteNG.Config;
using mRemoteNG.Tools.CustomCollections;

namespace mRemoteNG.Credential
{
    public class CredentialServiceFacade
    {
        private readonly ICredentialRepositoryList _repositoryList;
        private readonly ILoader<IEnumerable<ICredentialRepository>> _loader;
        private readonly ISaver<IEnumerable<ICredentialRepository>> _saver;

        public IEnumerable<ICredentialRepository> CredentialRepositories => _repositoryList;

        public CredentialServiceFacade(ICredentialRepositoryList repositoryList, ILoader<IEnumerable<ICredentialRepository>> loader, ISaver<IEnumerable<ICredentialRepository>> saver)
        {
            if (repositoryList == null)
                throw new ArgumentNullException(nameof(repositoryList));
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));
            if (saver == null)
                throw new ArgumentNullException(nameof(saver));

            _repositoryList = repositoryList;
            _loader = loader;
            _saver = saver;
            SetupEventHandlers();
        }

        public void SaveRepositoryList()
        {
            _saver.Save(_repositoryList);
        }

        public void LoadRepositoryList()
        {
            foreach (var repository in _loader.Load())
            {
                _repositoryList.AddProvider(repository);
            }
        }

        public void AddRepository(ICredentialRepository repository)
        {
            _repositoryList.AddProvider(repository);
        }

        public void RemoveRepository(ICredentialRepository repository)
        {
            _repositoryList.RemoveProvider(repository);
        }

        public IEnumerable<ICredentialRecord> GetCredentialRecords()
        {
            return _repositoryList.GetCredentialRecords();
        }

        public ICredentialRecord GetCredentialRecord(Guid id)
        {
            return _repositoryList.GetCredentialRecord(id);
        }

        #region Setup

        private void SetupEventHandlers()
        {
            _repositoryList.RepositoriesUpdated += HandleRepositoriesUpdatedEvent;
            _repositoryList.CredentialsUpdated += HandleCredentialsUpdatedEvent;
        }

        private void HandleRepositoriesUpdatedEvent(object sender,
                                                    CollectionUpdatedEventArgs<ICredentialRepository>
                                                        collectionUpdatedEventArgs)
        {
            SaveRepositoryList();
        }

        private void HandleCredentialsUpdatedEvent(object sender,
                                                   CollectionUpdatedEventArgs<ICredentialRecord>
                                                       collectionUpdatedEventArgs)
        {
            var repo = sender as ICredentialRepository;
            repo?.SaveCredentials(repo.Config.Key);
        }

        #endregion
    }
}