using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Security;
using mRemoteNG.Security.Authentication;

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
            CredentialRecords = new ObservableCollection<ICredentialRecord>();
            ((ObservableCollection<ICredentialRecord>) CredentialRecords).CollectionChanged += RaiseCollectionChangedEvent;
            Config.PropertyChanged += (sender, args) => RaisePropertyChangedEvent(args);
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

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        protected virtual void RaisePropertyChangedEvent(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        protected virtual void RaiseCollectionChangedEvent(object sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }
    }
}