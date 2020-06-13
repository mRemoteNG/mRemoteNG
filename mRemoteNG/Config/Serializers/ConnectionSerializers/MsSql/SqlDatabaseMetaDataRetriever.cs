using System;
using System.Data.Common;
using System.Globalization;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.MsSql
{
    public class SqlDatabaseMetaDataRetriever
    {
        public SqlConnectionListMetaData GetDatabaseMetaData(IDatabaseConnector databaseConnector)
        {
            SqlConnectionListMetaData metaData;
            DbDataReader dbDataReader = null;

            try
            {
                var dbCommand = databaseConnector.DbCommand("SELECT * FROM tblRoot");
                if (!databaseConnector.IsConnected)
                    databaseConnector.Connect();
                dbDataReader = dbCommand.ExecuteReader();
                if (!dbDataReader.HasRows)
                    return null; // assume new empty database
                else
                    dbDataReader.Read();

                metaData = new SqlConnectionListMetaData
                {
                    Name = dbDataReader["Name"] as string ?? "",
                    Protected = dbDataReader["Protected"] as string ?? "",
                    Export = (bool)dbDataReader["Export"],
                    ConfVersion =
                        new Version(Convert.ToString(dbDataReader["confVersion"], CultureInfo.InvariantCulture))
                };
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Retrieving database version failed. {ex}");
                throw;
            }
            finally
            {
                if (dbDataReader != null && !dbDataReader.IsClosed)
                    dbDataReader.Close();
            }

            return metaData;
        }

        public void WriteDatabaseMetaData(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector)
        {
	        var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
	        string strProtected;
	        if (rootTreeNode != null)
	        {
		        if (rootTreeNode.Password)
		        {
			        var password = rootTreeNode.PasswordString.ConvertToSecureString();
			        strProtected = cryptographyProvider.Encrypt("ThisIsProtected", password);
		        }
		        else
		        {
			        strProtected = cryptographyProvider.Encrypt("ThisIsNotProtected", Runtime.EncryptionKey);
		        }
	        }
	        else
	        {
		        strProtected = cryptographyProvider.Encrypt("ThisIsNotProtected", Runtime.EncryptionKey);
	        }

            var cmd = databaseConnector.DbCommand("DELETE FROM tblRoot");
            cmd.ExecuteNonQuery();

	        if (rootTreeNode != null)
	        {
		        cmd = databaseConnector.DbCommand(
                        "INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES(\'" +
				        MiscTools.PrepareValueForDB(rootTreeNode.Name) + "\', 0, \'" + strProtected + "\'," +
				        ConnectionsFileInfo.ConnectionFileVersion.ToString(CultureInfo.InvariantCulture) + ")");
		        cmd.ExecuteNonQuery();
	        }
	        else
	        {
		        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"UpdateRootNodeTable: rootTreeNode was null. Could not insert!");
	        }
		}
    }
}