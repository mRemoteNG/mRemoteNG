// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

using mRemoteNG.Forms;
//using mRemoteNG.App.Runtime;


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
						
						if (!(exportForm.ShowDialog(frmMain) == DialogResult.OK))
						{
							return ;
						}
						
						switch (exportForm.Scope)
						{
							case exportForm.ExportScope.SelectedFolder:
								exportTreeNode = exportForm.SelectedFolder;
								break;
							case exportForm.ExportScope.SelectedConnection:
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
					MessageCollector.AddExceptionMessage(message: "App.Export.ExportToFile() failed.", ex: ex, logOnly: true);
				}
			}
			
			private static void SaveExportFile(string fileName, mRemoteNG.Config.Connections.Save.Format saveFormat, TreeNode rootNode, Security.Save saveSecurity)
			{
				bool previousTimerEnabled = false;
				
				try
				{
					if (TimerSqlWatcher != null)
					{
						previousTimerEnabled = TimerSqlWatcher.Enabled;
						TimerSqlWatcher.Enabled = false;
					}
					
					Config.Connections.Save connectionsSave = new Config.Connections.Save();
					connectionsSave.Export = true;
					connectionsSave.ConnectionFileName = fileName;
					connectionsSave.SaveFormat = saveFormat;
					
					connectionsSave.ConnectionList = ConnectionList;
					connectionsSave.ContainerList = ContainerList;
					connectionsSave.RootTreeNode = rootNode;
					
					connectionsSave.SaveSecurity = saveSecurity;
					
					connectionsSave.Save_Renamed();
				}
				catch (Exception ex)
				{
					MessageCollector.AddExceptionMessage(string.Format("Export.SaveExportFile(\"{0}\") failed.", fileName), ex);
				}
				finally
				{
					if (TimerSqlWatcher != null)
					{
						TimerSqlWatcher.Enabled = previousTimerEnabled;
					}
				}
			}
		}
}
