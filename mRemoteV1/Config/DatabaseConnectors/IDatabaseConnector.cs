using System;
using System.Data.SqlClient;

namespace mRemoteNG.Config.DatabaseConnectors
{
    public interface IDatabaseConnector : IDisposable
    {
        bool IsConnected { get; }
        void Connect();
        void Disconnect();
        void AssociateItemToThisConnector(SqlCommand sqlCommand);
    }
}