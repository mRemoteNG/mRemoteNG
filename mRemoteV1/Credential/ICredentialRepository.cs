using System.Collections.Generic;
using System.ComponentModel;
using mRemoteNG.Credential.Repositories;


namespace mRemoteNG.Credential
{
    public interface ICredentialRepository : INotifyPropertyChanged
    {
        ICredentialRepositoryConfig Config { get; }
        IList<ICredentialRecord> CredentialRecords { get; }
        void LoadCredentials();
        void SaveCredentials();
    }
}