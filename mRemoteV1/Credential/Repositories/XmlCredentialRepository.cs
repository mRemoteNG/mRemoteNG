using System;
using System.Collections.Generic;
using System.Security;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;

namespace mRemoteNG.Credential.Repositories
{
    public class XmlCredentialRepository : ICredentialRepository
    {
        private readonly IDataProvider<string> _dataProvider;
        private readonly XmlCredentialDeserializer _deserializer;

        public ICredentialRepositoryConfig Config { get; }

        public XmlCredentialRepository(ICredentialRepositoryConfig config, IDataProvider<string> dataProvider, XmlCredentialDeserializer deserializer)
        {
            if (dataProvider == null)
                throw new ArgumentNullException(nameof(dataProvider));
            if (deserializer == null)
                throw new ArgumentNullException(nameof(deserializer));

            _dataProvider = dataProvider;
            _deserializer = deserializer;
        }

        public IEnumerable<ICredentialRecord> LoadCredentials(SecureString decryptionKey)
        {
            var serializedCredentials = _dataProvider.Load();
            return _deserializer.Deserialize(serializedCredentials, decryptionKey);
        }
    }
}