using System.Collections.Generic;

namespace mRemoteNG.Credential
{
    public interface ICredentialRepositoryList : IEnumerable<ICredentialRepository>
    {
        IEnumerable<ICredentialRepository> CredentialProviders { get; }

        void AddProvider(ICredentialRepository credentialProvider);

        void RemoveProvider(ICredentialRepository credentialProvider);
    }
}