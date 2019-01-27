using System;
using System.Collections.Generic;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools;

namespace mRemoteNG.Credential
{
    public interface ICredentialService
    {
        ICredentialRepositoryList RepositoryList { get; }
        IReadOnlyCollection<ICredentialRepositoryFactory> RepositoryFactories { get; }
        void SaveRepositoryList();
        void LoadRepositoryList();
        void AddRepository(ICredentialRepository repository);
        void RemoveRepository(ICredentialRepository repository);
        IEnumerable<ICredentialRecord> GetCredentialRecords();
        ICredentialRecord GetCredentialRecord(Guid id);

        /// <summary>
        /// Returns the <see cref="ICredentialRepository"/> object to use, taking into account
        /// any default or replacement credentials that may be used.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="allowDefaultFallback">
        /// If True and the <see cref="ICredentialRecord"/> given by <see cref="id"/> cannot be found,
        /// we will attempt to use a default credential specified in settings. If False, no default
        /// fallback value will be used.
        /// </param>
        Optional<ICredentialRecord> GetEffectiveCredentialRecord(Optional<Guid> id, bool allowDefaultFallback = true);

        /// <summary>
        /// Registers an <see cref="ICredentialRepositoryFactory"/> for
        /// use throughout the application.
        /// </summary>
        /// <param name="factory"></param>
        void RegisterRepositoryFactory(ICredentialRepositoryFactory factory);

        Optional<ICredentialRepositoryFactory> GetRepositoryFactoryForConfig(ICredentialRepositoryConfig repositoryConfig);
    }
}