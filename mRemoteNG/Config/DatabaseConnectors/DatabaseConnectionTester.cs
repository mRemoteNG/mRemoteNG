using System;
using System.Data.Odbc;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace mRemoteNG.Config.DatabaseConnectors
{
    /// <summary>
    /// A helper class for testing database connectivity.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class DatabaseConnectionTester
    {
        public static async Task<ConnectionTestResult> TestConnectivity(string type, string server, string database, string username, string password, string? authType = null)
        {
            try
            {
                using IDatabaseConnector dbConnector = DatabaseConnectorFactory.DatabaseConnector(type, server, database, username, password, authType);
                await dbConnector.ConnectAsync();
                return ConnectionTestResult.ConnectionSucceded;
            }
            catch (SqlException ex)
            {
                return HandleSqlException(ex);
            }
            catch (MySqlException ex)
            {
                return HandleMySqlException(ex);
            }
            catch (OdbcException ex)
            {
                return HandleOdbcException(ex);
            }
            catch (Exception ex)
            {
                // Generic fallback using string matching (supports all connector types)
                string message = ex.Message;
                if (message.Contains("server was not found", StringComparison.OrdinalIgnoreCase)
                    || message.Contains("network-related", StringComparison.OrdinalIgnoreCase)
                    || message.Contains("instance-specific", StringComparison.OrdinalIgnoreCase))
                    return ConnectionTestResult.ServerNotAccessible;
                if (message.Contains("Cannot open database", StringComparison.OrdinalIgnoreCase)
                    || message.Contains("Unknown database", StringComparison.OrdinalIgnoreCase))
                    return ConnectionTestResult.UnknownDatabase;
                if (message.Contains("Login failed", StringComparison.OrdinalIgnoreCase)
                    || message.Contains("Access denied", StringComparison.OrdinalIgnoreCase))
                    return ConnectionTestResult.CredentialsRejected;
                return ConnectionTestResult.UnknownError;
            }
        }

        private static ConnectionTestResult HandleSqlException(SqlException sqlException)
        {
            return sqlException.Number switch
            {
                4060 => ConnectionTestResult.UnknownDatabase,
                18456 => ConnectionTestResult.CredentialsRejected,
                -1 => ConnectionTestResult.ServerNotAccessible,
                _ => ConnectionTestResult.UnknownError
            };
        }

        private static ConnectionTestResult HandleMySqlException(MySqlException mySqlException)
        {
            return mySqlException.Number switch
            {
                1049 => ConnectionTestResult.UnknownDatabase,
                1045 => ConnectionTestResult.CredentialsRejected,
                2002 => ConnectionTestResult.ServerNotAccessible,
                2003 => ConnectionTestResult.ServerNotAccessible,
                2005 => ConnectionTestResult.ServerNotAccessible,
                _ => ConnectionTestResult.UnknownError
            };
        }

        private static ConnectionTestResult HandleOdbcException(OdbcException odbcException)
        {
            foreach (OdbcError error in odbcException.Errors)
            {
                switch (error.SQLState)
                {
                    case "28000":
                        return ConnectionTestResult.CredentialsRejected;
                    case "3D000":
                        return ConnectionTestResult.UnknownDatabase;
                    case "08001":
                    case "08004":
                    case "HYT00":
                    case "HYT01":
                        return ConnectionTestResult.ServerNotAccessible;
                }
            }

            string message = odbcException.Message;

            if (message.Contains("login failed", StringComparison.OrdinalIgnoreCase))
                return ConnectionTestResult.CredentialsRejected;
            if (message.Contains("cannot open database", StringComparison.OrdinalIgnoreCase))
                return ConnectionTestResult.UnknownDatabase;
            if (message.Contains("server was not found", StringComparison.OrdinalIgnoreCase)
                || message.Contains("data source name not found", StringComparison.OrdinalIgnoreCase)
                || message.Contains("network-related", StringComparison.OrdinalIgnoreCase))
                return ConnectionTestResult.ServerNotAccessible;

            return ConnectionTestResult.UnknownError;
        }
    }
}
