using System.Collections.Generic;
using mRemoteNG.Config.Putty;
using mRemoteNG.Credential;
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
		    ConnectionTreeModel connectionTreeModel;

            if (UseDatabase)
			{
			    var sqlLoader = new SqlConnectionsLoader();
                connectionTreeModel = sqlLoader.Load();
            }
			else
			{
                var xmlLoader = new XmlConnectionsLoader(ConnectionFileName, credentialRecords);
			    connectionTreeModel = xmlLoader.Load();
            }

            if (connectionTreeModel != null)
			    FrmMain.Default.ConnectionsFileName = ConnectionFileName;
            else
                connectionTreeModel = new ConnectionTreeModel();

		    if (!import)
		        AddPuttySessions(connectionTreeModel);

            return connectionTreeModel;
		}

	    private void AddPuttySessions(ConnectionTreeModel connectionTreeModel)
	    {
            PuttySessionsManager.Instance.AddSessions();
            connectionTreeModel.RootNodes.AddRange(PuttySessionsManager.Instance.RootPuttySessionsNodes);
        }
    }
}