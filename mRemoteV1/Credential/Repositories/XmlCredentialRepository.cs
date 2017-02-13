using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Security;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Tools.CustomCollections;

namespace mRemoteNG.Credential.Repositories
{
    public class XmlCredentialRepository : ICredentialRepository
    {
        private readonly IDataProvider<string> _dataProvider;
        private readonly XmlCredentialDeserializer _deserializer;
        private readonly XmlCredentialRecordSerializer _serializer;

        public ICredentialRepositoryConfig Config { get; }
        public IList<ICredentialRecord> CredentialRecords { get; }
        public IAuthenticator Authenticator { get; set; }

        public XmlCredentialRepository(ICredentialRepositoryConfig config, IDataProvider<string> dataProvider, ICryptographyProvider cryptographyProvider)
        {
            if (dataProvider == null)
                throw new ArgumentNullException(nameof(dataProvider));
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            Config = config;
            CredentialRecords = new FullyObservableCollection<ICredentialRecord>();
            ((FullyObservableCollection<ICredentialRecord>) CredentialRecords).CollectionUpdated += RaiseCredentialsUpdatedEvent;
            Config.PropertyChanged += (sender, args) => RaiseRepositoryConfigUpdatedEvent(args);
            _dataProvider = dataProvider;
            _deserializer = new XmlCredentialDeserializer();
            _serializer = new XmlCredentialRecordSerializer(cryptographyProvider);
        }

        public void LoadCredentials()
        {
            var serializedCredentials = _dataProvider.Load();
            var newCredentials = _deserializer.Deserialize(serializedCredentials, Config.Key);
            foreach (var newCredential in newCredentials)
            {
                if (CredentialRecords.Any(cred => cred.Id.Equals(newCredential.Id))) continue;
                CredentialRecords.Add(newCredential);
            }
        }

        public void SaveCredentials()
        {
            var data = _serializer.Serialize(CredentialRecords, Config.Key);
            _dataProvider.Save(data);
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