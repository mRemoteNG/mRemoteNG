using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.Forms;
using mRemoteNG.App.Runtime;

namespace mRemoteNG.App
{
	public class Export
	{
		public static void ExportToFile(TreeNode rootTreeNode, TreeNode selectedTreeNode)
		{
			try {
				TreeNode exportTreeNode = null;
				Security.Save saveSecurity = new Security.Save();

				using (ExportForm exportForm = new ExportForm()) {
					var _with1 = exportForm;
					switch (mRemoteNG.Tree.Node.GetNodeType(selectedTreeNode)) {
						case mRemoteNG.Tree.Node.Type.Container:
							_with1.SelectedFolder = selectedTreeNode;
							break;
						case mRemoteNG.Tree.Node.Type.Connection:
							if (mRemoteNG.Tree.Node.GetNodeType(selectedTreeNode.Parent) == mRemoteNG.Tree.Node.Type.Container) {
								_with1.SelectedFolder = selectedTreeNode.Parent;
							}
							_with1.SelectedConnection = selectedTreeNode;
							break;
					}

					if (!(exportForm.ShowDialog(frmMain) == DialogResult.OK))
						return;

					switch (_with1.Scope) {
						case exportForm.ExportScope.SelectedFolder:
							exportTreeNode = _with1.SelectedFolder;
							break;
						case exportForm.ExportScope.SelectedConnection:
							exportTreeNode = _with1.SelectedConnection;
							break;
						default:
							exportTreeNode = rootTreeNode;
							break;
					}

					saveSecurity.Username = _with1.IncludeUsername;
					saveSecurity.Password = _with1.IncludePassword;
					saveSecurity.Domain = _with1.IncludeDomain;
					saveSecurity.Inheritance = _with1.IncludeInheritance;

					SaveExportFile(exportForm.FileName, exportForm.SaveFormat, exportTreeNode, saveSecurity);
				}
			} catch (Exception ex) {
				Runtime.MessageCollector.AddExceptionMessage("App.Export.ExportToFile() failed.", ex, , true);
			}
		}

		private static void SaveExportFile(string fileName, Config.Connections.Save.Format saveFormat, TreeNode rootNode, Security.Save saveSecurity)
		{
			bool previousTimerEnabled = false;

			try {
				if (Runtime.TimerSqlWatcher != null) {
					previousTimerEnabled = Runtime.TimerSqlWatcher.Enabled;
					Runtime.TimerSqlWatcher.Enabled = false;
				}

				Config.Connections.Save connectionsSave = new Config.Connections.Save();
				var _with2 = connectionsSave;
				_with2.Export = true;
				_with2.ConnectionFileName = fileName;
				_with2.SaveFormat = saveFormat;

				_with2.ConnectionList = Runtime.ConnectionList;
				_with2.ContainerList = Runtime.ContainerList;
				_with2.RootTreeNode = rootNode;

				_with2.SaveSecurity = saveSecurity;

				connectionsSave.Save();
			} catch (Exception ex) {
				Runtime.MessageCollector.AddExceptionMessage(string.Format("Export.SaveExportFile(\"{0}\") failed.", fileName), ex);
			} finally {
				if (Runtime.TimerSqlWatcher != null) {
					Runtime.TimerSqlWatcher.Enabled = previousTimerEnabled;
				}
			}
		}
	}
}
