using System;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Connections;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Connection
{
    public class ConnectionsService
    {
        public bool IsConnectionsFileLoaded { get; set; }

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
                Runtime.ConnectionTreeModel = connectionsLoader.LoadConnections(Runtime.CredentialProviderCatalog.GetCredentialRecords(), false);
                Windows.TreeForm.ConnectionTree.ConnectionTreeModel = Runtime.ConnectionTreeModel;
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
    }
}