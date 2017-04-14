using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using mRemoteNG.Config;
using mRemoteNG.Tools.CustomCollections;

namespace mRemoteNG.Credential.Repositories
{
    public class XmlCredentialRepository : ICredentialRepository
    {
        private readonly CredentialRecordSaver _credentialRecordSaver;
        private readonly CredentialRecordLoader _credentialRecordLoader;

        public ICredentialRepositoryConfig Config { get; }
        public IList<ICredentialRecord> CredentialRecords { get; }
        public bool IsLoaded { get; private set; }

        public XmlCredentialRepository(ICredentialRepositoryConfig config, CredentialRecordSaver credentialRecordSaver, CredentialRecordLoader credentialRecordLoader)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (credentialRecordSaver == null)
                throw new ArgumentNullException(nameof(credentialRecordSaver));
            if (credentialRecordLoader == null)
                throw new ArgumentNullException(nameof(credentialRecordLoader));

            Config = config;
            CredentialRecords = new FullyObservableCollection<ICredentialRecord>();
            ((FullyObservableCollection<ICredentialRecord>) CredentialRecords).CollectionUpdated += RaiseCredentialsUpdatedEvent;
            Config.PropertyChanged += (sender, args) => RaiseRepositoryConfigUpdatedEvent(args);
            _credentialRecordSaver = credentialRecordSaver;
            _credentialRecordLoader = credentialRecordLoader;
        }

        public void LoadCredentials(SecureString key)
        {
            var credentials = _credentialRecordLoader.Load(key);
            foreach (var newCredential in credentials)
            {
                if (ThisIsADuplicateCredentialRecord(newCredential)) continue;
                CredentialRecords.Add(newCredential);
            }
            IsLoaded = true;
            Config.Key = key;
        }

        private bool ThisIsADuplicateCredentialRecord(ICredentialRecord newCredential)
        {
            return CredentialRecords.Any(cred => cred.Id.Equals(newCredential.Id));
        }

        public void UnloadCredentials()
        {
            IsLoaded = false;
            CredentialRecords.Clear();
        }

        public void SaveCredentials(SecureString key)
        {
            if (!IsLoaded) return;
            _credentialRecordSaver.Save(CredentialRecords, key);
        }

        public event EventHandler RepositoryConfigUpdated;
        public event EventHandler<CollectionUpdatedEventArgs<ICredentialRecord>> CredentialsUpdated;

        protected virtual void RaiseRepositoryConfigUpdatedEvent(PropertyChangedEventArgs args)
        {
            RepositoryConfigUpdated?.Invoke(this, args);
        }

        protected virtual void RaiseCredentialsUpdatedEvent(object sender, CollectionUpdatedEventArgs<ICredentialRecord> args)
        {
            CredentialsUpdated?.Invoke(this, args);
        }
    }
}