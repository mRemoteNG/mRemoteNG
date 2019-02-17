﻿using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace mRemoteNG.Config.DatabaseConnectors
{
    /// <summary>
    /// A helper class for testing database connectivity
    /// </summary>
    public class SqlDatabaseConnectionTester
    {
        public async Task<ConnectionTestResult> TestConnectivity(string server,
                                                                 string database,
                                                                 string username,
                                                                 string password)
        {
            using (var sqlConnector = new SqlDatabaseConnector(server, database, username, password))
            {
                try
                {
                    await sqlConnector.ConnectAsync();
                    return ConnectionTestResult.ConnectionSucceded;
                }
                catch (SqlException sqlException)
                {
                    if (sqlException.Message.Contains("The server was not found"))
                        return ConnectionTestResult.ServerNotAccessible;
                    if (sqlException.Message.Contains("Cannot open database"))
                        return ConnectionTestResult.UnknownDatabase;
                    if (sqlException.Message.Contains("Login failed for user"))
                        return ConnectionTestResult.CredentialsRejected;
                    return ConnectionTestResult.UnknownError;
                }
                catch (Exception)
                {
                    return ConnectionTestResult.UnknownError;
                }
            }
        }
    }
}