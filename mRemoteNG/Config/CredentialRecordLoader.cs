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
        private readonly ISecureDeserializer<string, IEnumerable<ICredentialRecord>> _deserializer;

        public CredentialRecordLoader(IDataProvider<string> dataProvider,
                                      ISecureDeserializer<string, IEnumerable<ICredentialRecord>> deserializer)
        {
            ArgumentNullException.ThrowIfNull(dataProvider);
            ArgumentNullException.ThrowIfNull(deserializer);
            _dataProvider = dataProvider;
            _deserializer = deserializer;
        }

        public IEnumerable<ICredentialRecord> Load(SecureString key)
        {
            string serializedCredentials = _dataProvider.Load();
            return _deserializer.Deserialize(serializedCredentials, key);
        }
    }
}