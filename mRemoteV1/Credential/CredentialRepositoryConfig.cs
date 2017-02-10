using System;
using System.Security;
using mRemoteNG.Credential.Repositories;

namespace mRemoteNG.Credential
{
    public class CredentialRepositoryConfig : ICredentialRepositoryConfig
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; } = "";
        public string Source { get; set; } = "";
        public SecureString Key { get; set; } = new SecureString();
    }
}