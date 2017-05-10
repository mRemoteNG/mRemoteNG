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

        private static string GetDefaultStartupConnectionFileName()
        {
            var newPath = ConnectionsFileInfo.DefaultConnectionsPath + "\\" + ConnectionsFileInfo.DefaultConnectionsFile;
#if !PORTABLE
            var oldPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + Application.ProductName + "\\" + ConnectionsFileInfo.DefaultConnectionsFile;
            // ReSharper disable once ConvertIfStatementToReturnStatement
			if (File.Exists(oldPath)) return oldPath;
#endif
            return newPath;
        }

    }
}