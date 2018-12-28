using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using mRemoteNG.Config;
using mRemoteNG.Tools;
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

        /// <summary>
        /// Creates a new <see cref="XmlCredentialRepository"/> instance,
        /// providing access to load and save credentials stored in an XML
        /// format.
        /// </summary>
        /// <param name="config">
        /// The config representing this repository
        /// </param>
        /// <param name="credentialRecordSaver"></param>
        /// <param name="credentialRecordLoader"></param>
        /// <param name="isLoaded">
        /// Does this instance represent a repository that is already loaded?
        /// </param>
        public XmlCredentialRepository(
            ICredentialRepositoryConfig config, 
            CredentialRecordSaver credentialRecordSaver, 
            CredentialRecordLoader credentialRecordLoader,
            bool isLoaded = false)
        {
            Config = config.ThrowIfNull(nameof(config));
            _credentialRecordSaver = credentialRecordSaver.ThrowIfNull(nameof(credentialRecordSaver));
            _credentialRecordLoader = credentialRecordLoader.ThrowIfNull(nameof(credentialRecordLoader));
            IsLoaded = isLoaded;

            CredentialRecords = new FullyObservableCollection<ICredentialRecord>();
            ((FullyObservableCollection<ICredentialRecord>) CredentialRecords).CollectionUpdated += RaiseCredentialsUpdatedEvent;
            Config.PropertyChanged += (sender, args) => RaiseRepositoryConfigUpdatedEvent(args);
        }

        public void LoadCredentials(SecureString key)
        {
            var credentials = _credentialRecordLoader.Load(key);
            foreach (var newCredential in credentials)
            {
                if (ThisIsADuplicateCredentialRecord(newCredential))
                    continue;

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
            if (!IsLoaded)
                return;

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