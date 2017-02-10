using System.Collections;
using System.Collections.Generic;


namespace mRemoteNG.Credential
{
    public class CredentialProviderCatalog : ICredentialProviderCatalog
    {
        private readonly List<ICredentialRepository> _credentialProviders = new List<ICredentialRepository>();

        public IEnumerable<ICredentialRepository> CredentialProviders => _credentialProviders;


        public void AddProvider(ICredentialRepository credentialProvider)
        {
            _credentialProviders.Add(credentialProvider);
        }

        public void RemoveProvider(ICredentialRepository credentialProvider)
        {
            _credentialProviders.Remove(credentialProvider);
        }

        public IEnumerator<ICredentialRepository> GetEnumerator()
        {
            return _credentialProviders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}