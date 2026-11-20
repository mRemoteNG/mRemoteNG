using System.Runtime.Versioning;
using Microsoft.Win32;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNG.Config.Settings.Registry
{
    [SupportedOSPlatform("windows")]
    public sealed partial class OptRegistrySqlServerPage
    {
        /// <summary>
        /// Specifies whether SQL Server is being used.
        /// </summary>
        public WinRegistryEntry<bool> UseSQLServer { get; private set; }

        /// <summary>
        /// Specifies the type of SQL Server being used.
        /// </summary>
        public WinRegistryEntry<string> SQLServerType { get; private set; }

        /// <summary>
        /// Specifies the host of the SQL Server.
        /// </summary>
        public WinRegistryEntry<string> SQLHost { get; private set; }

        /// <summary>
        /// Specifies the name/instance of the SQL database.
        /// </summary>
        public WinRegistryEntry<string> SQLDatabaseName { get; private set; }

        /// <summary>
        /// Specifies the username for accessing the SQL Server.
        /// </summary>
        public WinRegistryEntry<string> SQLUser { get; private set; }

        /// <summary>
        /// Specifies the password for accessing the SQL Server.
        /// </summary>
        public WinRegistryEntry<string> SQLPassword { get; private set; }

        /// <summary>
        /// Specifies whether the SQL connection is read-only.
        /// </summary>
        public WinRegistryEntry<bool> SQLReadOnly { get; private set; }

        /// <summary>
        /// Specifies whether the SQL Server certificate is accepted without validation.
        /// </summary>
        public WinRegistryEntry<bool> SQLTrustServerCertificate { get; private set; }

        public OptRegistrySqlServerPage() : this(WindowsRegistryInfo.Hive, WindowsRegistryInfo.SQLServerOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance reading from an explicit hive/subkey instead of the production
        /// <see cref="WindowsRegistryInfo.Hive"/>/<see cref="WindowsRegistryInfo.SQLServerOptions"/>
        /// location, so tests can use a throwaway <see cref="RegistryHive.CurrentUser"/> path.
        /// </summary>
        /// <param name="hive">The registry hive to read settings from.</param>
        /// <param name="subKey">The registry subkey to read settings from.</param>
        internal OptRegistrySqlServerPage(RegistryHive hive, string subKey)
        {
            UseSQLServer = new WinRegistryEntry<bool>(hive, subKey, nameof(UseSQLServer)).Read();
            SQLServerType = new WinRegistryEntry<string>(hive, subKey, nameof(SQLServerType)).Read();
            SQLHost = new WinRegistryEntry<string>(hive, subKey, nameof(SQLHost)).Read();
            SQLDatabaseName = new WinRegistryEntry<string>(hive, subKey, nameof(SQLDatabaseName)).Read();
            SQLUser = new WinRegistryEntry<string>(hive, subKey, nameof(SQLUser)).Read();
            SQLPassword = new WinRegistryEntry<string>(hive, subKey, nameof(SQLPassword)).Read();
            SQLReadOnly = new WinRegistryEntry<bool>(hive, subKey, nameof(SQLReadOnly)).Read();
            SQLTrustServerCertificate = new WinRegistryEntry<bool>(hive, subKey, nameof(SQLTrustServerCertificate)).Read();

            SetupValidation();
            Apply();
        }

        /// <summary>
        /// Configures validation settings for various parameters
        /// </summary>
        private void SetupValidation()
        {
            SQLServerType.SetValidation(
                new string[] {
                    "mssql",
                    "mysql"
                });
        }

        /// <summary>
        /// Applies registry settings and overrides various properties.
        /// </summary>
        private void Apply()
        {
            if (!UseSQLServer.IsSet) { return; }

            ApplyUseSQLServer();

            if (!Properties.OptionsDBsPage.Default.UseSQLServer) { return; }

            ApplySQLServerType();
            ApplySQLHost();
            ApplySQLDatabaseName();
            ApplySQLUser();
            ApplySQLPassword();
            ApplySQLReadOnly();
            ApplySQLTrustServerCertificate();
        }

        private void ApplyUseSQLServer()
        {
            Properties.OptionsDBsPage.Default.UseSQLServer = UseSQLServer.Value;
        }

        private void ApplySQLServerType()
        {
            if (SQLServerType.IsValid)
                Properties.OptionsDBsPage.Default.SQLServerType = SQLServerType.Value;
            
        }

        private void ApplySQLHost()
        {
            if (SQLHost.IsSet)
                Properties.OptionsDBsPage.Default.SQLHost = SQLHost.Value;
        }

        private void ApplySQLDatabaseName()
        {
            if (SQLDatabaseName.IsSet)
                Properties.OptionsDBsPage.Default.SQLDatabaseName = SQLDatabaseName.Value;
            
        }

        private void ApplySQLUser()
        {
            if (SQLUser.IsSet)
                Properties.OptionsDBsPage.Default.SQLUser = SQLUser.Value;
        }

        private void ApplySQLPassword()
        {
            if (SQLPassword.IsSet)
            {
                // Prevents potential issues when using SQLPass later.
                try
                {
                    LegacyRijndaelCryptographyProvider cryptographyProvider = new();
                    string decryptedPassword;
                    string sqlPassword = SQLPassword.Value;
                    decryptedPassword = cryptographyProvider.Decrypt(sqlPassword, Runtime.EncryptionKey);

                    Properties.OptionsDBsPage.Default.SQLPass = sqlPassword;
                }
                catch
                {
                    // Fire-and-forget: The password in the registry is not encrypted.
                }
            }
        }

        private void ApplySQLReadOnly()
        {
            if (SQLReadOnly.IsSet)
                Properties.OptionsDBsPage.Default.SQLReadOnly = SQLReadOnly.Value;
        }

        private void ApplySQLTrustServerCertificate()
        {
            if (SQLTrustServerCertificate.IsSet)
                Properties.OptionsDBsPage.Default.SQLTrustServerCertificate = SQLTrustServerCertificate.Value;
        }
    }
}