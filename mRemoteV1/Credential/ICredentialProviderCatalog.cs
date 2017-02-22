using System.Collections.Generic;

namespace mRemoteNG.Credential
{
    public interface ICredentialProviderCatalog : IEnumerable<ICredentialProvider>
    {
        IEnumerable<ICredentialProvider> CredentialProviders { get; }

        void AddProvider(ICredentialProvider credentialProvider);

        void RemoveProvider(ICredentialProvider credentialProvider);
    }
}