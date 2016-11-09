using System.Collections;
using System.Collections.Generic;


namespace mRemoteNG.Credential
{
    public class CredentialProviderCatalog : ICredentialProviderCatalog
    {
        private readonly List<ICredentialProvider> _credentialProviders = new List<ICredentialProvider>();

        public IEnumerable<ICredentialProvider> CredentialProviders => _credentialProviders;


        public void AddProvider(ICredentialProvider credentialProvider)
        {
            _credentialProviders.Add(credentialProvider);
        }

        public void RemoveProvider(ICredentialProvider credentialProvider)
        {
            _credentialProviders.Remove(credentialProvider);
        }

        public IEnumerator<ICredentialProvider> GetEnumerator()
        {
            return _credentialProviders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}