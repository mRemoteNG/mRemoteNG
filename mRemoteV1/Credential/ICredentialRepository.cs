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
        void LoadCredentials();
        void SaveCredentials();
        event EventHandler RepositoryConfigUpdated;
        event EventHandler<CollectionUpdatedEventArgs<ICredentialRecord>> CredentialsUpdated;
    }
}