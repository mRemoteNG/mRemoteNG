using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using mRemoteNG.Credential.Repositories;


namespace mRemoteNG.Credential
{
    public interface ICredentialRepository : INotifyPropertyChanged, INotifyCollectionChanged
    {
        ICredentialRepositoryConfig Config { get; }
        IList<ICredentialRecord> CredentialRecords { get; }
        void LoadCredentials();
        void SaveCredentials();
    }
}