using mRemoteNG.Config.DatabaseConnectors;


namespace mRemoteNG.Config.Connections
{
    public delegate void SqlUpdateAvailableEventHandler(object sender, SqlUpdateAvailableEventArgs args);
    public class SqlUpdateAvailableEventArgs
    {
        public SqlDatabaseConnector DatabaseConnector { get; private set; }

        public SqlUpdateAvailableEventArgs(SqlDatabaseConnector databaseConnector)
        {
            DatabaseConnector = databaseConnector;
        }
    }
}