using System.Collections.Generic;

namespace mRemoteNG.Credential
{
    public interface ICredentialCatalog
    {
        void AddProvider(ICredentialProvider credentialProvider);

        void RemoveProvider(ICredentialProvider credentialProvider);
    }
}