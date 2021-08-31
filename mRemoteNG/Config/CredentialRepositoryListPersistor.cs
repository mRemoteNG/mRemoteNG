using System.Collections.Generic;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.CredentialProviderSerializer;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools;

namespace mRemoteNG.Config
{
    public class CredentialRepositoryListPersistor : ISaver<IEnumerable<ICredentialRepository>>, ILoader<IEnumerable<ICredentialRepository>>
    {
        private readonly IReadOnlyCollection<ICredentialRepositoryFactory> _repositoryFactories;
        private readonly IDataProvider<string> _dataProvider;
        private readonly CredentialRepositoryListDeserializer _deserializer;
        private readonly CredentialRepositoryListSerializer _serializer;

        public CredentialRepositoryListPersistor(
            IDataProvider<string> dataProvider, 
            IReadOnlyCollection<ICredentialRepositoryFactory> repositoryFactories)
        {
            _repositoryFactories = repositoryFactories.ThrowIfNull(nameof(repositoryFactories));
            _dataProvider = dataProvider.ThrowIfNull(nameof(dataProvider));
            _deserializer = new CredentialRepositoryListDeserializer();
            _serializer = new CredentialRepositoryListSerializer();
        }

        public IEnumerable<ICredentialRepository> Load()
        {
            var data = _dataProvider.Load();
            return _deserializer.Deserialize(data, _repositoryFactories);
        }

        public void Save(IEnumerable<ICredentialRepository> repositories, string propertyNameTrigger = "")
        {
            var data = _serializer.Serialize(repositories);
            _dataProvider.Save(data);
        }
    }
}
