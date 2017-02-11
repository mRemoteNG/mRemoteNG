using System;
using System.Security;

namespace mRemoteNG.Credential.Repositories
{
    public interface ICredentialRepositoryConfig
    {
        Guid Id { get; }
        string TypeName { get; }
        string Source { get; set; }
        SecureString Key { get; set; }
    }
}