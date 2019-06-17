using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace mRemoteNG.Config.DatabaseConnectors
{
    public interface IDatabaseConnector : IDisposable
    {
        DbConnection DbConnection();
        DbCommand DbCommand(string dbCommand);
        bool IsConnected { get; }
        void Connect();
        Task ConnectAsync();
        void Disconnect();
        void AssociateItemToThisConnector(DbCommand dbCommand);
    }
}