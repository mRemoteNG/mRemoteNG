using System;
using System.Windows.Forms;
using mRemoteNG.Forms;
using mRemoteNG.Config.Connections;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.App
{
	public class Export
	{
		public static void ExportToFile(TreeNode rootTreeNode, TreeNode selectedTreeNode)
		{
			try
			{
				TreeNode exportTreeNode = default(TreeNode);
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
						
					switch (exportForm.Scope)
					{
						case mRemoteNG.Forms.ExportForm.ExportScope.SelectedFolder:
							exportTreeNode = exportForm.SelectedFolder;
							break;
                        case mRemoteNG.Forms.ExportForm.ExportScope.SelectedConnection:
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
						
					SaveExportFile(exportForm.FileName, exportForm.SaveFormat, exportTreeNode, saveSecurity);
				}
					
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionMessage(message: "App.Export.ExportToFile() failed.", ex: ex, logOnly: true);
			}
		}
			
		private static void SaveExportFile(string fileName, ConnectionsSaver.Format saveFormat, TreeNode rootNode, Security.Save saveSecurity)
		{
			try
			{
                if (Runtime.SQLConnProvider != null)
				{
                    Runtime.SQLConnProvider.Disable();
				}
					
				ConnectionsSaver connectionsSave = new ConnectionsSaver();
				connectionsSave.Export = true;
				connectionsSave.ConnectionFileName = fileName;
				connectionsSave.SaveFormat = saveFormat;
                connectionsSave.ConnectionList = Runtime.ConnectionList;
                connectionsSave.ContainerList = Runtime.ContainerList;
				connectionsSave.RootTreeNode = rootNode;
				connectionsSave.SaveSecurity = saveSecurity;
				connectionsSave.SaveConnections();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(string.Format("Export.SaveExportFile(\"{0}\") failed.", fileName), ex);
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