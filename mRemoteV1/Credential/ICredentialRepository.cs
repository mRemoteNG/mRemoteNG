using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using mRemoteNG.Credential.Repositories;


namespace mRemoteNG.Credential
{
    public interface ICredentialRepository : INotifyPropertyChanged
    {
        ICredentialRepositoryConfig Config { get; }
        IEnumerable<ICredentialRecord> LoadCredentials(SecureString key);
    }
}