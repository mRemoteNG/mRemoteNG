using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Security;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Tools.CustomCollections;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.Credential.Repositories
{
    public class XmlCredentialRepository : ICredentialRepository
    {
        private readonly IDataProvider<string> _dataProvider;
        private readonly XmlCredentialRecordDeserializer _deserializer;
        private readonly XmlCredentialRecordSerializer _serializer;

        public ICredentialRepositoryConfig Config { get; }
        public IList<ICredentialRecord> CredentialRecords { get; }
        public IPasswordRequestor PasswordRequestor { get; set; } = new PasswordForm("", false);
        public bool IsLoaded { get; private set; } = true;

        public XmlCredentialRepository(ICredentialRepositoryConfig config, IDataProvider<string> dataProvider)
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
            _deserializer = new XmlCredentialRecordDeserializer();
            _serializer = new XmlCredentialRecordSerializer();
        }

        public void LoadCredentials()
        {
            var fileContents = _dataProvider.Load();
            if (fileContents == "") return;
            var credentials = Deserialize(fileContents);
            foreach (var newCredential in credentials)
            {
                if (CredentialRecords.Any(cred => cred.Id.Equals(newCredential.Id))) continue;
                CredentialRecords.Add(newCredential);
            }
        }

        private IEnumerable<ICredentialRecord> Deserialize(string xml)
        {
            var key = Config.Key;
            var requestAuth = true;
            while(requestAuth)
            { 
                try
                {
                    var credentials = _deserializer.Deserialize(xml).ToArray();
                    Config.Key = key;
                    IsLoaded = true;
                    return credentials;
                }
                catch (Exception)
                {
                    key = PasswordRequestor.RequestPassword();
                    if (key.Length == 0)
                        requestAuth = false;
                }
            }
            return new ICredentialRecord[0];
        }

        public void UnloadCredentials()
        {
            IsLoaded = false;
            CredentialRecords.Clear();
        }

        public void SaveCredentials()
        {
            if (!IsLoaded) return;
            var data = _serializer.Serialize(CredentialRecords);
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