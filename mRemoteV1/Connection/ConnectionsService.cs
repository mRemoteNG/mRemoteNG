using System;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Connection
{
    public class ConnectionsService
    {
        public bool IsConnectionsFileLoaded { get; set; }

        public ConnectionTreeModel ConnectionTreeModel
        {
            get { return Windows.TreeForm.ConnectionTree.ConnectionTreeModel; }
            set { Windows.TreeForm.ConnectionTree.ConnectionTreeModel = value; }
        }

        public void NewConnections(string filename)
        {
            try
            {
                UpdateCustomConsPathSetting(filename);

                var newConnectionsModel = new ConnectionTreeModel();
                newConnectionsModel.AddRootNode(new RootNodeInfo(RootNodeType.Connection));
                var connectionSaver = new ConnectionsSaver
                {
                    ConnectionTreeModel = newConnectionsModel,
                    ConnectionFileName = filename
                };
                connectionSaver.SaveConnections();

                // Load config
                var connectionsLoader = new ConnectionsLoader { ConnectionFileName = filename };
                ConnectionTreeModel = connectionsLoader.LoadConnections(false);
                Windows.TreeForm.ConnectionTree.ConnectionTreeModel = ConnectionTreeModel;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.strCouldNotCreateNewConnectionsFile, ex);
            }
        }

        private void UpdateCustomConsPathSetting(string filename)
        {
            if (filename == GetDefaultStartupConnectionFileName())
            {
                Settings.Default.LoadConsFromCustomLocation = false;
            }
            else
            {
                Settings.Default.LoadConsFromCustomLocation = true;
                Settings.Default.CustomConsPath = filename;
            }
        }

        public string GetStartupConnectionFileName()
        {
            return Settings.Default.LoadConsFromCustomLocation == false ? GetDefaultStartupConnectionFileName() : Settings.Default.CustomConsPath;
        }

        public string GetDefaultStartupConnectionFileName()
        {
            return Runtime.IsPortableEdition ? GetDefaultStartupConnectionFileNamePortableEdition() : GetDefaultStartupConnectionFileNameNormalEdition();
        }

        private string GetDefaultStartupConnectionFileNameNormalEdition()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + Application.ProductName + "\\" + ConnectionsFileInfo.DefaultConnectionsFile;
            return File.Exists(appDataPath) ? appDataPath : GetDefaultStartupConnectionFileNamePortableEdition();
        }

        private string GetDefaultStartupConnectionFileNamePortableEdition()
        {
            return ConnectionsFileInfo.DefaultConnectionsPath + "\\" + ConnectionsFileInfo.DefaultConnectionsFile;
        }

        public ConnectionInfo CreateQuickConnect(string connectionString, ProtocolType protocol)
        {
            try
            {
                var uri = new Uri("dummyscheme" + Uri.SchemeDelimiter + connectionString);
                if (string.IsNullOrEmpty(uri.Host)) return null;

                var newConnectionInfo = new ConnectionInfo();
                newConnectionInfo.CopyFrom(DefaultConnectionInfo.Instance);

                newConnectionInfo.Name = Settings.Default.IdentifyQuickConnectTabs ? string.Format(Language.strQuick, uri.Host) : uri.Host;

                newConnectionInfo.Protocol = protocol;
                newConnectionInfo.Hostname = uri.Host;
                if (uri.Port == -1)
                {
                    newConnectionInfo.SetDefaultPort();
                }
                else
                {
                    newConnectionInfo.Port = uri.Port;
                }
                newConnectionInfo.IsQuickConnect = true;

                return newConnectionInfo;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.strQuickConnectFailed, ex);
                return null;
            }
        }
    }
}