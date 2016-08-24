using System;
using System.Data.SqlClient;
namespace mRemoteNG.Config
{
    public interface ISqlConnector : IDisposable
    {
        void Connect();
        void Disconnect();
        void AssociateItemToThisConnector(SqlCommand sqlCommand);
    }
}