using System;
using System.Windows.Forms;
using mRemoteNG.Config.Connections;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.App
{
	public static class Export
	{
		public static void ExportToFile(TreeNode rootTreeNode, TreeNode selectedTreeNode, ConnectionTreeModel connectionTreeModel)
		{
			try
			{
			    var saveSecurity = new Save();
					
				using (var exportForm = new ExportForm())
				{
					if (ConnectionTreeNode.GetNodeType(selectedTreeNode) == TreeNodeType.Container)
						exportForm.SelectedFolder = selectedTreeNode;
					else if (ConnectionTreeNode.GetNodeType(selectedTreeNode) == TreeNodeType.Connection)
					{
						if (ConnectionTreeNode.GetNodeType(selectedTreeNode.Parent) == TreeNodeType.Container)
							exportForm.SelectedFolder = selectedTreeNode.Parent;
						exportForm.SelectedConnection = selectedTreeNode;
					}
						
					if (exportForm.ShowDialog(frmMain.Default) != DialogResult.OK)
						return ;

				    TreeNode exportTreeNode;
				    switch (exportForm.Scope)
					{
						case ExportForm.ExportScope.SelectedFolder:
							exportTreeNode = exportForm.SelectedFolder;
							break;
                        case ExportForm.ExportScope.SelectedConnection:
							exportTreeNode = exportForm.SelectedConnection;
							break;
						default:
							exportTreeNode = rootTreeNode;
							break;
					}
						
					saveSecurity.Username = exportForm.IncludeUsername;
					saveSecurity.Password = exportForm.IncludePassword;
					saveSecurity.Domain = exportForm.IncludeDomain;
					saveSecurity.Inheritance = exportForm.IncludeInheritance;
						
					SaveExportFile(exportForm.FileName, exportForm.SaveFormat, exportTreeNode, saveSecurity, connectionTreeModel);
				}
					
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionMessage("App.Export.ExportToFile() failed.", ex, logOnly: true);
			}
		}
			
		private static void SaveExportFile(string fileName, ConnectionsSaver.Format saveFormat, TreeNode rootNode, Save saveSecurity, ConnectionTreeModel connectionTreeModel)
		{
			try
			{
                if (Runtime.SQLConnProvider != null)
                    Runtime.SQLConnProvider.Disable();

			    var connectionsSave = new ConnectionsSaver
			    {
			        Export = true,
			        ConnectionFileName = fileName,
			        SaveFormat = saveFormat,
			        ConnectionList = Runtime.ConnectionList,
			        ContainerList = Runtime.ContainerList,
			        RootTreeNode = rootNode,
			        SaveSecurity = saveSecurity,
                    ConnectionTreeModel = connectionTreeModel
			    };
			    connectionsSave.SaveConnections();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace($"Export.SaveExportFile(\"{fileName}\") failed.", ex);
			}
			finally
			{
                if (Runtime.SQLConnProvider != null)
                    Runtime.SQLConnProvider.Enable();
			}
		}
	}
}