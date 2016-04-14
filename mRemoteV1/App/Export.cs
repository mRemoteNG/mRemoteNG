using System;
using System.Windows.Forms;
using mRemoteNG.Forms;


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
					if (Tree.Node.GetNodeType(selectedTreeNode) == Tree.Node.Type.Container)
					{
						exportForm.SelectedFolder = selectedTreeNode;
					}
					else if (Tree.Node.GetNodeType(selectedTreeNode) == Tree.Node.Type.Connection)
					{
						if (Tree.Node.GetNodeType(selectedTreeNode.Parent) == Tree.Node.Type.Container)
						{
							exportForm.SelectedFolder = selectedTreeNode.Parent;
						}
						exportForm.SelectedConnection = selectedTreeNode;
					}
						
					if (!(exportForm.ShowDialog(frmMain.Default) == DialogResult.OK))
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
			
		private static void SaveExportFile(string fileName, mRemoteNG.Config.Connections.Save.Format saveFormat, TreeNode rootNode, Security.Save saveSecurity)
		{
			bool previousTimerEnabled = false;
				
			try
			{
				if (Runtime.TimerSqlWatcher != null)
				{
                    previousTimerEnabled = Runtime.TimerSqlWatcher.Enabled;
                    Runtime.TimerSqlWatcher.Enabled = false;
				}
					
				Config.Connections.Save connectionsSave = new Config.Connections.Save();
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
                if (Runtime.TimerSqlWatcher != null)
				{
                    Runtime.TimerSqlWatcher.Enabled = previousTimerEnabled;
				}
			}
		}
	}
}
