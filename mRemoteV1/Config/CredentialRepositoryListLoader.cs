using System;
using System.Collections.Generic;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Credential;

namespace mRemoteNG.Config
{
    public class CredentialRepositoryListLoader
    {
        private readonly IDataProvider<string> _dataProvider;
        private readonly CredentialRepositoryListDeserializer _deserializer;

        public CredentialRepositoryListLoader(IDataProvider<string> dataProvider, CredentialRepositoryListDeserializer deserializer)
        {
            if (dataProvider == null)
                throw new ArgumentNullException(nameof(dataProvider));
            if (deserializer == null)
                throw new ArgumentNullException(nameof(deserializer));

            _dataProvider = dataProvider;
            _deserializer = deserializer;
        }

        public IEnumerable<ICredentialRepository> Load()
        {
            var data = _dataProvider.Load();
            return _deserializer.Deserialize(data);
        }
    }
}