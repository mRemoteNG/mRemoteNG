using System.Collections.Generic;

namespace mRemoteNG.Credential
{
    public interface ICredentialProviderCatalog
    {
        void AddProvider(ICredentialProvider credentialProvider);

        void RemoveProvider(ICredentialProvider credentialProvider);
    }
}