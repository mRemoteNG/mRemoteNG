using System;
using System.Security;
using mRemoteNG.Credential.Repositories;

namespace mRemoteNG.Credential
{
    public class CredentialRepositoryConfig : ICredentialRepositoryConfig
    {
        public Guid Id { get; }
        public string TypeName { get; set; } = "";
        public string Source { get; set; } = "";
        public SecureString Key { get; set; } = new SecureString();

        public CredentialRepositoryConfig() : this(Guid.NewGuid())
        {
        }

        public CredentialRepositoryConfig(Guid id)
        {
            Id = id;
        }
    }
}