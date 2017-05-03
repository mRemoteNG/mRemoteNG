using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Tools.CustomCollections;

namespace mRemoteNG.Credential.Repositories
{
    public class CredentialRepositoryList : ICredentialRepositoryList
    {
        private readonly List<ICredentialRepository> _credentialProviders = new List<ICredentialRepository>();

        public IEnumerable<ICredentialRepository> CredentialProviders => _credentialProviders;


        public void AddProvider(ICredentialRepository credentialProvider)
        {
            if (Contains(credentialProvider.Config.Id)) return;
            _credentialProviders.Add(credentialProvider);
            credentialProvider.CredentialsUpdated += RaiseCredentialsUpdatedEvent;
            credentialProvider.RepositoryConfigUpdated += OnRepoConfigChanged;
            RaiseRepositoriesUpdatedEvent(ActionType.Added, new[] { credentialProvider });
        }

        public void RemoveProvider(ICredentialRepository credentialProvider)
        {
            if (!Contains(credentialProvider.Config.Id)) return;
            credentialProvider.CredentialsUpdated -= RaiseCredentialsUpdatedEvent;
            credentialProvider.RepositoryConfigUpdated -= OnRepoConfigChanged;
            _credentialProviders.Remove(credentialProvider);
            RaiseRepositoriesUpdatedEvent(ActionType.Removed, new[] { credentialProvider });
        }

        public bool Contains(Guid repositoryId)
        {
            return _credentialProviders.Any(repo => repo.Config.Id == repositoryId);
        }

        public IEnumerable<ICredentialRecord> GetCredentialRecords()
        {
            var list = new List<ICredentialRecord>();
            foreach (var repository in CredentialProviders)
            {
                list.AddRange(repository.CredentialRecords);
            }
            return list;
        }

        public ICredentialRecord GetCredentialRecord(Guid id)
        {
            return CredentialProviders.SelectMany(repo => repo.CredentialRecords).FirstOrDefault(record => record.Id.Equals(id));
        }

        public IEnumerator<ICredentialRepository> GetEnumerator()
        {
            return _credentialProviders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event EventHandler<CollectionUpdatedEventArgs<ICredentialRepository>> RepositoriesUpdated;
        public event EventHandler<CollectionUpdatedEventArgs<ICredentialRecord>> CredentialsUpdated;

        private void RaiseRepositoriesUpdatedEvent(ActionType action, IEnumerable<ICredentialRepository> changedItems)
        {
            RepositoriesUpdated?.Invoke(this, new CollectionUpdatedEventArgs<ICredentialRepository>(action, changedItems));
        }

        private void RaiseCredentialsUpdatedEvent(object sender, CollectionUpdatedEventArgs<ICredentialRecord> args)
        {
            CredentialsUpdated?.Invoke(sender, args);
        }

        private void OnRepoConfigChanged(object sender, EventArgs args)
        {
            var repo = sender as ICredentialRepository;
            if (repo == null) return;
            RaiseRepositoriesUpdatedEvent(ActionType.Updated, new[] { repo });
        }
    }
}