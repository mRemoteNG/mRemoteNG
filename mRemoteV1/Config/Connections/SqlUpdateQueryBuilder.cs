using System.Data.SqlClient;

namespace mRemoteNG.Config.Connections
{
    public class SqlUpdateQueryBuilder : SqlCommandBuilder
    {
        private string _updateQuery;

        public SqlUpdateQueryBuilder()
        {
            Initialize();
        }

        private void Initialize()
        {
            _updateQuery = "SELECT * FROM tblUpdate";
        }

        public SqlCommand BuildCommand()
        {
            return new SqlCommand(_updateQuery);
        }
    }
}