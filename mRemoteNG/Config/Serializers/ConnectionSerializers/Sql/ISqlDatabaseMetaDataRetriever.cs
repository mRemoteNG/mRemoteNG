using System.Data.Common;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Sql
{
    public interface ISqlDatabaseMetaDataRetriever
    {
        SqlConnectionListMetaData? GetDatabaseMetaData(IDatabaseConnector databaseConnector);
        void WriteDatabaseMetaData(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector);
        void WriteDatabaseMetaData(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector, DbTransaction? transaction);
    }
}
