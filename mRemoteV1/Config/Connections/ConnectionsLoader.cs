using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Config.Connections
{
	public class ConnectionsLoader
	{		
        public bool UseDatabase { get; set; }
	    public string DatabaseHost { get; set; }
	    public string DatabaseName { get; set; }
	    public string DatabaseUsername { get; set; }
	    public string DatabasePassword { get; set; }
	    public bool DatabaseUpdate { get; set; }
	    public string PreviousSelected { get; set; }
	    public string ConnectionFileName { get; set; }
	    public TreeNode RootTreeNode { get; set; }
		public ConnectionList ConnectionList { get; set; }
        public ContainerList ContainerList { get; set; }
	    public ConnectionList PreviousConnectionList { get; set; }
	    public ContainerList PreviousContainerList { get; set; }
		

		public void LoadConnections(bool import)
		{
			if (UseDatabase)
			{
			    var sqlConnectionsLoader = new SqlConnectionsLoader()
			    {
			        ConnectionList = ConnectionList,
			        ContainerList = ContainerList,
			        PreviousConnectionList = PreviousConnectionList,
			        PreviousContainerList = PreviousContainerList,
			        PreviousSelected = PreviousSelected,
			        RootTreeNode = RootTreeNode,
			        DatabaseName = DatabaseName,
			        DatabaseHost = DatabaseHost,
			        DatabasePassword = DatabasePassword,
			        DatabaseUpdate = DatabaseUpdate,
			        DatabaseUsername = DatabaseUsername
			    };
                sqlConnectionsLoader.LoadFromSql();
			}
			else
			{
			    var dataProvider = new FileDataProvider(ConnectionFileName);
			    var xmlString = dataProvider.Load();
                var xmlConnectionsDeserializer = new XmlConnectionsDeserializer(xmlString)
                {
                    ConnectionList = ConnectionList,
                    ContainerList = ContainerList,
                    RootTreeNode = RootTreeNode,
                };
				xmlConnectionsDeserializer.Deserialize(import);
			}
			
			frmMain.Default.AreWeUsingSqlServerForSavingConnections = UseDatabase;
			frmMain.Default.ConnectionsFileName = ConnectionFileName;
			
			if (!import)
				Putty.Sessions.AddSessionsToTree();
		}
    }
}