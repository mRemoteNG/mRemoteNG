using System;
using System.Collections.Generic;
using System.Security;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Credential;


namespace mRemoteNG.Config
{
    public class CredentialRecordLoader
    {
        private readonly IDataProvider<string> _dataProvider;
        private readonly XmlCredentialDeserializer _deserializer;

        public CredentialRecordLoader(IDataProvider<string> dataProvider, XmlCredentialDeserializer deserializer)
        {
            if (dataProvider == null)
                throw new ArgumentNullException(nameof(dataProvider));
            if (deserializer == null)
                throw new ArgumentNullException(nameof(deserializer));

            _dataProvider = dataProvider;
            _deserializer = deserializer;
        }

        public IEnumerable<ICredentialRecord> Load(SecureString decryptionKey)
        {
            var serializedCredentials = _dataProvider.Load();
            return _deserializer.Deserialize(serializedCredentials, decryptionKey);
        }
    }
}