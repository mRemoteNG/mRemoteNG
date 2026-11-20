using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.Versioning;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Config.Serializers.Versioning
{
    /// <summary>
    /// Moves the passwords stored in the database from a key that was the MD5 of the password to
    /// one derived once for the database with PBKDF2-HMAC-SHA256 (CVE-2026-87868). It needs the
    /// password the connection list was opened with, and moves everything in one transaction, so
    /// if anything fails the database stays at version 3.3. Clients older than 3.4 cannot read
    /// the passwords afterwards.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class SqlVersion33To34Upgrader : IVersionUpgrader
    {
        private static readonly string[] PasswordColumns = ["Password", "RDGatewayPassword", "VNCProxyPassword"];

        private readonly Version _version = new(3, 4);
        private readonly IDatabaseConnector _databaseConnector;
        private readonly SecureString _password;

        public SqlVersion33To34Upgrader(IDatabaseConnector databaseConnector, SecureString password)
        {
            _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
            _password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion < _version;
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Upgrading database to version {_version}.");

            // Outside the transaction: MySQL commits on ALTER TABLE anyway. Each column is added
            // only when missing, so a migration that failed after this step can run again.
            AddColumnIfMissing("KdfSalt", "varchar(64)");
            AddColumnIfMissing("KdfIterations", "int");

            LegacyRijndaelCryptographyProvider oldProvider = new();
            DerivedKeyCryptographyProvider newProvider = DerivedKeyCryptographyProvider.WithNewSalt();

            using DbTransaction transaction = _databaseConnector.DbConnection().BeginTransaction(IsolationLevel.Serializable);
            ReencryptConnections(transaction, oldProvider, newProvider);
            UpdateRoot(transaction, oldProvider, newProvider);
            transaction.Commit();

            return _version;
        }

        /// <summary>
        /// A stored value in the new form. A value the old key does not open is taken as it is,
        /// the way the loader has always read such a value.
        /// </summary>
        internal static string Reencrypt(string stored, ICryptographyProvider oldProvider, ICryptographyProvider newProvider, SecureString password)
        {
            if (string.IsNullOrEmpty(stored)) return stored;

            string plainText;
            try
            {
                plainText = oldProvider.Decrypt(stored, password);
            }
            catch (EncryptionException)
            {
                plainText = stored;
            }

            return newProvider.Encrypt(plainText, password);
        }

        private bool IsMsSql => _databaseConnector is MSSqlDatabaseConnector;

        private void AddColumnIfMissing(string column, string type)
        {
            try
            {
                using DbCommand probe = _databaseConnector.DbCommand($"SELECT {column} FROM tblRoot WHERE 1 = 0");
                probe.ExecuteNonQuery();
                return;
            }
            catch (DbException)
            {
                // The column does not exist yet.
            }

            string alter = IsMsSql
                ? $"ALTER TABLE tblRoot ADD {column} {type} NULL"
                : $"ALTER TABLE tblRoot ADD COLUMN `{column}` {type} DEFAULT NULL";
            using DbCommand command = _databaseConnector.DbCommand(alter);
            command.ExecuteNonQuery();
        }

        private void ReencryptConnections(DbTransaction transaction, ICryptographyProvider oldProvider, ICryptographyProvider newProvider)
        {
            List<(string Id, string[] Values)> rows = [];
            using (DbCommand select = Command($"SELECT ConstantID, {string.Join(", ", PasswordColumns)} FROM tblCons", transaction))
            using (DbDataReader reader = select.ExecuteReader())
            {
                while (reader.Read())
                {
                    string[] values = new string[PasswordColumns.Length];
                    for (int i = 0; i < values.Length; i++)
                        values[i] = reader.IsDBNull(i + 1) ? null : reader.GetString(i + 1);
                    rows.Add((reader.GetString(0), values));
                }
            }

            foreach ((string id, string[] values) in rows)
            {
                if (Array.TrueForAll(values, string.IsNullOrEmpty)) continue;

                using DbCommand update = Command(
                    "UPDATE tblCons SET Password = @Password, RDGatewayPassword = @RDGatewayPassword, VNCProxyPassword = @VNCProxyPassword WHERE ConstantID = @ConstantID",
                    transaction);
                for (int i = 0; i < PasswordColumns.Length; i++)
                    AddParameter(update, "@" + PasswordColumns[i], (object)Reencrypt(values[i], oldProvider, newProvider, _password) ?? DBNull.Value);
                AddParameter(update, "@ConstantID", id);
                update.ExecuteNonQuery();
            }
        }

        private void UpdateRoot(DbTransaction transaction, ICryptographyProvider oldProvider, DerivedKeyCryptographyProvider newProvider)
        {
            string protectedValue;
            using (DbCommand select = Command("SELECT Protected FROM tblRoot", transaction))
                protectedValue = select.ExecuteScalar() as string ?? "";

            const string update = "UPDATE tblRoot SET Protected = @Protected, KdfSalt = @KdfSalt, KdfIterations = @KdfIterations, ConfVersion = @ConfVersion;";
            using DbCommand command = Command(IsMsSql ? update : $"SET SQL_SAFE_UPDATES=0; {update} SET SQL_SAFE_UPDATES=1;", transaction);
            AddParameter(command, "@Protected", Reencrypt(protectedValue, oldProvider, newProvider, _password));
            AddParameter(command, "@KdfSalt", Convert.ToBase64String(newProvider.Salt));
            AddParameter(command, "@KdfIterations", newProvider.KeyDerivationIterations);
            AddParameter(command, "@ConfVersion", _version.ToString());
            command.ExecuteNonQuery();
        }

        private DbCommand Command(string sql, DbTransaction transaction)
        {
            DbCommand command = _databaseConnector.DbCommand(sql);
            command.Transaction = transaction;
            return command;
        }

        private static void AddParameter(DbCommand command, string name, object value)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
    }
}
