using System;
using System.ComponentModel;
using System.Security;

namespace mRemoteNG.Credential.Repositories
{
    public interface ICredentialRepositoryConfig : INotifyPropertyChanged
    {
        Guid Id { get; }
        string Title { get; set; }
        string TypeName { get; }
        string Source { get; set; }
        SecureString Key { get; set; }
    }
}