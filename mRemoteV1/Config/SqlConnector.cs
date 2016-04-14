using System;
using System.Data.SqlClient;
namespace mRemoteNG.Config
{
    public interface SqlConnector : IDisposable
    {
        void Connect();
        void Disconnect();
        void AssociateItemToThisConnector(SqlCommand sqlCommand);
    }
}