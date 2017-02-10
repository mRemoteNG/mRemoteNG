using System.Collections.Generic;
using System.Security;
using mRemoteNG.Credential.Repositories;


namespace mRemoteNG.Credential
{
    public interface ICredentialRepository
    {
        ICredentialRepositoryConfig Config { get; }
        IEnumerable<ICredentialRecord> LoadCredentials(SecureString key);
    }
}