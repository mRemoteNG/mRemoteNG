using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.Csv;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Connections
{
    public class CsvConnectionsSaver : ISaver<ConnectionTreeModel>
    {
        private readonly string _connectionFileName;
        private readonly SaveFilter _saveFilter;
        private readonly ICredentialRepositoryList _credentialRepositoryList;

        public CsvConnectionsSaver(string connectionFileName, SaveFilter saveFilter, ICredentialRepositoryList credentialRepositoryList)
        {
            _connectionFileName = connectionFileName.ThrowIfNullOrEmpty(nameof(connectionFileName));
            _saveFilter = saveFilter.ThrowIfNull(nameof(saveFilter));
            _credentialRepositoryList = credentialRepositoryList.ThrowIfNull(nameof(credentialRepositoryList));
        }

        public void Save(ConnectionTreeModel connectionTreeModel)
        {
            var csvConnectionsSerializer = new CsvConnectionsSerializerMremotengFormat(_saveFilter, _credentialRepositoryList);
            var dataProvider = new FileDataProvider(_connectionFileName);
            var csvContent = csvConnectionsSerializer.Serialize(connectionTreeModel);
            dataProvider.Save(csvContent);
        }
    }
}
