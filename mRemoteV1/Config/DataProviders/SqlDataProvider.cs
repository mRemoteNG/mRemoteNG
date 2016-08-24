using System;


namespace mRemoteNG.Config.DataProviders
{
    public class SqlDataProvider : IDataProvider
    {
        private IDatabaseConnector _sqlConnector;

        public SqlDataProvider(IDatabaseConnector sqlConnector)
        {
            _sqlConnector = sqlConnector;
        }

        public string Load()
        {
            throw new NotImplementedException();
        }

        public void Save(string contents)
        {
            throw new NotImplementedException();
        }
    }
}