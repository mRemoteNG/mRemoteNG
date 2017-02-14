using System;
using System.Collections.Generic;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools.CustomCollections;


namespace mRemoteNG.Credential
{
    public interface ICredentialRepository
    {
        ICredentialRepositoryConfig Config { get; }
        IList<ICredentialRecord> CredentialRecords { get; }
        bool IsLoaded { get; }
        void LoadCredentials();
        void SaveCredentials();
        void UnloadCredentials();
        event EventHandler RepositoryConfigUpdated;
        event EventHandler<CollectionUpdatedEventArgs<ICredentialRecord>> CredentialsUpdated;
    }
}