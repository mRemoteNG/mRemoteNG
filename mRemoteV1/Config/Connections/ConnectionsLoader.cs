using System.Collections.Generic;
using System.Security;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Putty;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Credential;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Config.Connections
{
	public class ConnectionsLoader
	{		
        public bool UseDatabase { get; set; }
	    public string ConnectionFileName { get; set; }
		

		public ConnectionTreeModel LoadConnections(IEnumerable<ICredentialRecord> credentialRecords, bool import)
		{
		    IDeserializer deserializer;
			if (UseDatabase)
			{
			    var connector = new SqlDatabaseConnector();
			    var dataProvider = new SqlDataProvider(connector);
			    var dataTable = dataProvider.Load();
			    deserializer = new DataTableDeserializer(dataTable);
			}
			else
			{
			    var dataProvider = new FileDataProvider(ConnectionFileName);
			    var xmlString = dataProvider.Load();
			    deserializer = new XmlConnectionsDeserializer(xmlString, credentialRecords, PromptForPassword);
			}

            var connectionTreeModel = deserializer.Deserialize();

            if (connectionTreeModel != null)
			    FrmMain.Default.ConnectionsFileName = ConnectionFileName;
            else
                connectionTreeModel = new ConnectionTreeModel();

		    if (import) return connectionTreeModel;
		    PuttySessionsManager.Instance.AddSessions();
            connectionTreeModel.RootNodes.AddRange(PuttySessionsManager.Instance.RootPuttySessionsNodes);

		    return connectionTreeModel;
		}

        private SecureString PromptForPassword()
        {
            var password = MiscTools.PasswordDialog("", false);
            return password;
        }
    }
}