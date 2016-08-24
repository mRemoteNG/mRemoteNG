using System;
using System.Windows.Forms;
using mRemoteNG.Config.Connections;
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
			    Security.Save saveSecurity = new Security.Save();
					
				using (ExportForm exportForm = new ExportForm())
				{
					if (Tree.ConnectionTreeNode.GetNodeType(selectedTreeNode) == Tree.TreeNodeType.Container)
					{
						exportForm.SelectedFolder = selectedTreeNode;
					}
					else if (Tree.ConnectionTreeNode.GetNodeType(selectedTreeNode) == Tree.TreeNodeType.Connection)
					{
						if (Tree.ConnectionTreeNode.GetNodeType(selectedTreeNode.Parent) == Tree.TreeNodeType.Container)
						{
							exportForm.SelectedFolder = selectedTreeNode.Parent;
						}
						exportForm.SelectedConnection = selectedTreeNode;
					}
						
					if (exportForm.ShowDialog(frmMain.Default) != DialogResult.OK)
					{
						return ;
					}

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
			
		private static void SaveExportFile(string fileName, ConnectionsSaver.Format saveFormat, TreeNode rootNode, Security.Save saveSecurity, ConnectionTreeModel connectionTreeModel)
		{
			try
			{
                if (Runtime.SQLConnProvider != null)
				{
                    Runtime.SQLConnProvider.Disable();
				}

			    ConnectionsSaver connectionsSave = new ConnectionsSaver
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
				{
                    Runtime.SQLConnProvider.Enable();
				}
			}
		}
	}
}