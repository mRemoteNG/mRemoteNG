using System;
using System.Collections.Generic;
using System.Security;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools.CustomCollections;


namespace mRemoteNG.Credential
{
    public interface ICredentialRepository
    {
        /// <summary>
        /// The configuration information for this credential repository.
        /// </summary>
        ICredentialRepositoryConfig Config { get; }

        /// <summary>
        /// A list of the <see cref="ICredentialRecord"/>s provided by this repository.
        /// </summary>
        IList<ICredentialRecord> CredentialRecords { get; }

        /// <summary>
        /// A friendly name for this repository.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Whether or not this repository has been unlocked and is able to provide
        /// credentials.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Unlock the repository with the given key and load all available credentials.
        /// </summary>
        /// <param name="key"></param>
        void LoadCredentials(SecureString key);

        /// <summary>
        /// Save all credentials provided by this repository.
        /// </summary>
        /// <param name="key"></param>
        void SaveCredentials(SecureString key);

        /// <summary>
        /// Lock and unload all credentials provided by this repository.
        /// </summary>
        void UnloadCredentials();

        /// <summary>
        /// This event is raised when any changes are made to the assigned <see cref="Config"/>.
        /// </summary>
        event EventHandler RepositoryConfigUpdated;

        /// <summary>
        /// This event is raised when a credential is added or removed from this repository.
        /// </summary>
        event EventHandler<CollectionUpdatedEventArgs<ICredentialRecord>> CredentialsUpdated;
    }
}