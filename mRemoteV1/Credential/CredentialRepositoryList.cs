using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;


namespace mRemoteNG.Credential
{
    public class CredentialRepositoryList : ICredentialRepositoryList
    {
        private readonly List<ICredentialRepository> _credentialProviders = new List<ICredentialRepository>();

        public IEnumerable<ICredentialRepository> CredentialProviders => _credentialProviders;


        public void AddProvider(ICredentialRepository credentialProvider)
        {
            if (Contains(credentialProvider.Config.Id)) return;
            _credentialProviders.Add(credentialProvider);
            credentialProvider.PropertyChanged += CredentialProviderOnPropertyChanged;
            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Add, new[] { credentialProvider });
        }

        

        public void RemoveProvider(ICredentialRepository credentialProvider)
        {
            if (!Contains(credentialProvider.Config.Id)) return;
            credentialProvider.PropertyChanged -= CredentialProviderOnPropertyChanged;
            _credentialProviders.Remove(credentialProvider);
            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, new[] {credentialProvider});
        }

        public bool Contains(Guid repositoryId)
        {
            return _credentialProviders.Any(repo => repo.Config.Id == repositoryId);
        }

        public IEnumerable<ICredentialRecord> GetCredentialRecords()
        {
            var list = new List<ICredentialRecord>();
            foreach (var repository in CredentialProviders)
            {
                list.AddRange(repository.CredentialRecords);
            }
            return list;
        }

        public IEnumerator<ICredentialRepository> GetEnumerator()
        {
            return _credentialProviders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void CredentialProviderOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Add, new[] { sender });
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, IList items)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, items));
        }
    }
}