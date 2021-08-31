using System;
using System.Collections.Generic;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools;
using mRemoteNG.Tools.CustomCollections;

namespace mRemoteNG.Credential
{
    public interface ICredentialRepositoryList : IEnumerable<ICredentialRepository>
    {
        IEnumerable<ICredentialRepository> CredentialProviders { get; }

        void AddProvider(ICredentialRepository credentialProvider);

        void RemoveProvider(ICredentialRepository credentialProvider);

        Optional<ICredentialRepository> GetProvider(Guid id);

        bool Contains(ICredentialRepositoryConfig repositoryConfig);

        IEnumerable<ICredentialRecord> GetCredentialRecords();

        ICredentialRecord GetCredentialRecord(Guid id);

        event EventHandler<CollectionUpdatedEventArgs<ICredentialRepository>> RepositoriesUpdated;
        event EventHandler<CollectionUpdatedEventArgs<ICredentialRecord>> CredentialsUpdated;
    }
}