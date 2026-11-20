using System;
using System.Linq;
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
        public async Task<ConnectionTestResult> TestConnectivity(string type, string server, string database, string username, string password, bool trustServerCertificate = false)
        {
            try
            {
                using IDatabaseConnector dbConnector = DatabaseConnectorFactory.DatabaseConnector(type, server, database, username, password, trustServerCertificate);
                await dbConnector.ConnectAsync();
                return ConnectionTestResult.ConnectionSucceded;
            }
            catch (Exception ex)
            {
                return ResultFor(ex);
            }
        }

        /// <summary>
        /// What the certificate validation of the server reports, from .NET (SslStream) and from the
        /// managed network layer of Microsoft.Data.SqlClient.
        /// </summary>
        private static readonly string[] CertificateFailures =
        [
            "remote certificate",
            "Certificate failed chain validation",
            "Certificate name mismatch",
            "Certificate not available while validating",
        ];

        internal static ConnectionTestResult ResultFor(Exception ex)
        {
            string message = ex.Message;
            if (IsCertificateFailure(ex))
                return ConnectionTestResult.ServerCertificateNotTrusted;
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

        /// <summary>
        /// Only errors of the TLS layer count: the word certificate alone can be part of a login
        /// or database name in an ordinary failure.
        /// </summary>
        private static bool IsCertificateFailure(Exception ex)
        {
            for (Exception current = ex; current != null; current = current.InnerException)
            {
                string message = current.Message;

                // The native network layer reports TLS failures under the SSL provider, a server name
                // that does not match the certificate as the target principal name. Outside it that
                // phrase is a Kerberos error of Windows authentication, not a certificate one.
                if (message.Contains("provider: SSL Provider", StringComparison.OrdinalIgnoreCase)
                    && (message.Contains("certificate", StringComparison.OrdinalIgnoreCase)
                        || message.Contains("target principal name", StringComparison.OrdinalIgnoreCase)))
                    return true;

                if (CertificateFailures.Any(failure => message.Contains(failure, StringComparison.OrdinalIgnoreCase)))
                    return true;
            }

            return false;
        }
    }
}
