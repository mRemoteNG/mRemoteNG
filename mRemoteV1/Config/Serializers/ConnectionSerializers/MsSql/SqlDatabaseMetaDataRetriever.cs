using System;
using System.Data.SqlClient;
using System.Globalization;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.MsSql
{
	public class SqlDatabaseMetaDataRetriever
    {
        public SqlConnectionListMetaData GetDatabaseMetaData(SqlDatabaseConnector sqlDatabaseConnector)
        {
            SqlConnectionListMetaData metaData;
            SqlDataReader sqlDataReader = null;
            try
            {
                var sqlCommand = new SqlCommand("SELECT * FROM tblRoot", sqlDatabaseConnector.SqlConnection);
                if (!sqlDatabaseConnector.IsConnected)
                    sqlDatabaseConnector.Connect();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (!sqlDataReader.HasRows)
                    return null; // assume new empty database
                else
                    sqlDataReader.Read();

                metaData = new SqlConnectionListMetaData
                {
                    Name = sqlDataReader["Name"] as string ?? "",
                    Protected = sqlDataReader["Protected"] as string ?? "",
                    Export = (bool)sqlDataReader["Export"],
                    ConfVersion = new Version(Convert.ToString(sqlDataReader["confVersion"], CultureInfo.InvariantCulture))
                };
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Retrieving database version failed. {ex}");
                throw;
            }
            finally
            {
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                    sqlDataReader.Close();
            }
            return metaData;
        }

        public void WriteDatabaseMetaData(RootNodeInfo rootTreeNode, SqlDatabaseConnector sqlDatabaseConnector)
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

	        var sqlQuery = new SqlCommand("DELETE FROM tblRoot", sqlDatabaseConnector.SqlConnection);
	        sqlQuery.ExecuteNonQuery();

	        if (rootTreeNode != null)
	        {
		        sqlQuery =
			        new SqlCommand(
				        "INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES(\'" +
				        MiscTools.PrepareValueForDB(rootTreeNode.Name) + "\', 0, \'" + strProtected + "\'," +
				        ConnectionsFileInfo.ConnectionFileVersion.ToString(CultureInfo.InvariantCulture) + ")",
				        sqlDatabaseConnector.SqlConnection);
		        sqlQuery.ExecuteNonQuery();
	        }
	        else
	        {
		        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"UpdateRootNodeTable: rootTreeNode was null. Could not insert!");
	        }
		}
    }
}