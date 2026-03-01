using mRemoteNG.App;
using mRemoteNG.Security.SymmetricEncryption;
using System;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.DatabaseConnectors
{
    [SupportedOSPlatform("windows")]
    public static class DatabaseConnectorFactory
    {
        public const string MsSqlType = "mssql";
        public const string MySqlType = "mysql";
        public const string OdbcType = "odbc";
        public const string WindowsAuthentication = "Windows Authentication";
        public const string MicrosoftEntraIntegratedAuthentication = "Microsoft Entra Integrated";

        public static IDatabaseConnector DatabaseConnectorFromSettings()
        {
            // TODO: add custom port handling?
            string sqlType = Properties.OptionsDBsPage.Default.SQLServerType;
            string sqlHost = Properties.OptionsDBsPage.Default.SQLHost;
            string sqlCatalog = Properties.OptionsDBsPage.Default.SQLDatabaseName;
            string sqlUsername = Properties.OptionsDBsPage.Default.SQLUser;
            string sqlAuthType = Properties.OptionsDBsPage.Default.SQLAuthType;
            LegacyRijndaelCryptographyProvider cryptographyProvider = new();
            string sqlPassword = cryptographyProvider.Decrypt(Properties.OptionsDBsPage.Default.SQLPass, Runtime.EncryptionKey);

            return DatabaseConnector(sqlType, sqlHost, sqlCatalog, sqlUsername, sqlPassword, sqlAuthType);
        }

        public static IDatabaseConnector DatabaseConnector(string? type, string server, string database, string username, string password, string? authType = null)
        {
            string normalizedType = NormalizeType(type);

            if (ShouldUseIntegratedSecurity(normalizedType, authType))
            {
                username = string.Empty;
                password = string.Empty;
            }

            switch (normalizedType)
            {
                case MySqlType:
                    return new MySqlDatabaseConnector(server, database, username, password);
                case OdbcType:
                    return new OdbcDatabaseConnector(server, database, username, password);
                case MsSqlType:
                default:
                    return new MSSqlDatabaseConnector(server, database, username, password);
            }
        }

        public static bool ShouldUseIntegratedSecurity(string? type, string? authType)
        {
            if (!string.Equals(NormalizeType(type), MsSqlType, StringComparison.OrdinalIgnoreCase))
                return false;

            return string.Equals(authType, WindowsAuthentication, StringComparison.OrdinalIgnoreCase)
                || string.Equals(authType, MicrosoftEntraIntegratedAuthentication, StringComparison.OrdinalIgnoreCase);
        }

        public static string NormalizeType(string? type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return MsSqlType;

            string normalizedType = type.Trim();

            if (string.Equals(normalizedType, "MSSQL - developed by Microsoft", StringComparison.OrdinalIgnoreCase))
                return MsSqlType;
            if (string.Equals(normalizedType, "MySQL - developed by Oracle", StringComparison.OrdinalIgnoreCase))
                return MySqlType;
            if (string.Equals(normalizedType, "ODBC - Open Database Connectivity", StringComparison.OrdinalIgnoreCase))
                return OdbcType;

            return normalizedType.ToLowerInvariant() switch
            {
                MySqlType => MySqlType,
                OdbcType => OdbcType,
                _ => MsSqlType
            };
        }
    }
}