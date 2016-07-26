using mRemoteNG.App;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Connection.Protocol.RDP;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Security;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.App.Info;
using mRemoteNG.Tree;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.UI.Forms;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.TaskDialog;

namespace mRemoteNG.Config.Connections
{
	public class ConnectionsLoader
	{
		private XmlDocument _xmlDocument;
		private double _confVersion;
		
        public bool UseSql { get; set; }
	    public string SqlHost { get; set; }
	    public string SqlDatabaseName { get; set; }
	    public string SqlUsername { get; set; }
	    public string SqlPassword { get; set; }
	    public bool SqlUpdate { get; set; }
	    public string PreviousSelected { get; set; }
	    public string ConnectionFileName { get; set; }
	    public TreeNode RootTreeNode { get; set; }
		public ConnectionList ConnectionList { get; set; }
        public ContainerList ContainerList { get; set; }
	    public ConnectionList PreviousConnectionList { get; set; }
	    public ContainerList PreviousContainerList { get; set; }
		

		public void LoadConnections(bool import)
		{
			if (UseSql)
			{
			    var sqlConnectionsLoader = new SqlConnectionsLoader()
			    {
			        ConnectionList = ConnectionList,
			        ContainerList = ContainerList,
			        PreviousConnectionList = PreviousConnectionList,
			        PreviousContainerList = PreviousContainerList,
			        PreviousSelected = PreviousSelected,
			        RootTreeNode = RootTreeNode,
			        SqlDatabaseName = SqlDatabaseName,
			        SqlHost = SqlHost,
			        SqlPassword = SqlPassword,
			        SqlUpdate = SqlUpdate,
			        SqlUsername = SqlUsername
			    };
                sqlConnectionsLoader.LoadFromSql();
			}
			else
			{
                var xmlConnectionsLoader = new XmlConnectionsLoader()
                {
                    ConnectionFileName = ConnectionFileName,
                    ConnectionList = ConnectionList,
                    ContainerList = ContainerList,
                    RootTreeNode = RootTreeNode,
                    UseSql = UseSql
                };
				xmlConnectionsLoader.LoadFromXml(import);
			}
			
			frmMain.Default.AreWeUsingSqlServerForSavingConnections = UseSql;
			frmMain.Default.ConnectionsFileName = ConnectionFileName;
			
			if (!import)
				Putty.Sessions.AddSessionsToTree();
		}
    }
}