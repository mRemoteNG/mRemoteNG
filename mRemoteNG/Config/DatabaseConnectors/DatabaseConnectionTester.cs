using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace mRemoteNG.Config.DatabaseConnectors
{
    /// <summary>
    /// A helper class for testing database connectivity.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class DatabaseConnectionTester
    {
        public async Task<ConnectionTestResult> TestConnectivity(string type, string server, string database, string username, string password)
        {
            try
            {
                using IDatabaseConnector dbConnector = DatabaseConnectorFactory.DatabaseConnector(type, server, database, username, password);
                await dbConnector.ConnectAsync();
                return ConnectionTestResult.ConnectionSucceded;
            }
            catch (Exception ex)
            {
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
    }
}
