using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;


namespace mRemoteNG.Credential
{
    public class CredentialRepositoryList : ICredentialRepositoryList
    {
        private readonly List<ICredentialRepository> _credentialProviders = new List<ICredentialRepository>();

        public IEnumerable<ICredentialRepository> CredentialProviders => _credentialProviders;


        public void AddProvider(ICredentialRepository credentialProvider)
        {
            _credentialProviders.Add(credentialProvider);
            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Add, new[] { credentialProvider });
        }

        public void RemoveProvider(ICredentialRepository credentialProvider)
        {
            _credentialProviders.Remove(credentialProvider);
            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, new[] {credentialProvider});
        }

        public IEnumerator<ICredentialRepository> GetEnumerator()
        {
            return _credentialProviders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, IList items)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, items));
        }
    }
}