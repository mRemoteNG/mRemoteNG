using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace mRemoteNG.Credential
{
    public interface ICredentialRepositoryList : IEnumerable<ICredentialRepository>, INotifyCollectionChanged
    {
        IEnumerable<ICredentialRepository> CredentialProviders { get; }

        void AddProvider(ICredentialRepository credentialProvider);

        void RemoveProvider(ICredentialRepository credentialProvider);

        bool Contains(Guid repositoryId);

        IEnumerable<ICredentialRecord> GetCredentialRecords();
    }
}