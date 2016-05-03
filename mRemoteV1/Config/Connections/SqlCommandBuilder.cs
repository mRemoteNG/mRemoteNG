using System.Data.SqlClient;

namespace mRemoteNG.Config.Connections
{
    public interface SqlCommandBuilder
    {
        SqlCommand BuildCommand();
    }
}