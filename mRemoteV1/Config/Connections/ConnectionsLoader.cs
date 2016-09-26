using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Putty;
using mRemoteNG.Config.Serializers;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Config.Connections
{
	public class ConnectionsLoader
	{		
        public bool UseDatabase { get; set; }
	    public string ConnectionFileName { get; set; }
		

		public void LoadConnections(bool import)
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
			    deserializer = new XmlConnectionsDeserializer(xmlString);
			}

            var connectionTreeModel = deserializer.Deserialize();
            Runtime.ConnectionTreeModel = connectionTreeModel;

            frmMain.Default.AreWeUsingSqlServerForSavingConnections = UseDatabase;
			frmMain.Default.ConnectionsFileName = ConnectionFileName;

		    if (import) return;
		    PuttySessionsManager.Instance.AddSessions();
		    Runtime.ConnectionTreeModel.RootNodes.AddRange(PuttySessionsManager.Instance.RootPuttySessionsNodes);
		}
    }
}