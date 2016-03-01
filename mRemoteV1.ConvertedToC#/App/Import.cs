using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using mRemoteNG.My;
using mRemoteNG.App.Runtime;
using PSTaskDialog;

namespace mRemoteNG.App
{
	public class Import
	{
		#region "Public Methods"
		public static void ImportFromFile(TreeNode rootTreeNode, TreeNode selectedTreeNode, bool alwaysUseSelectedTreeNode = false)
		{
			try {
				using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
					var _with1 = openFileDialog;
					_with1.CheckFileExists = true;
					_with1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					_with1.Multiselect = true;

					List<string> fileTypes = new List<string>();
					fileTypes.AddRange({
						Language.strFilterAllImportable,
						"*.xml;*.rdp;*.rdg;*.dat"
					});
					fileTypes.AddRange({
						Language.strFiltermRemoteXML,
						"*.xml"
					});
					fileTypes.AddRange({
						Language.strFilterRDP,
						"*.rdp"
					});
					fileTypes.AddRange({
						Language.strFilterRdgFiles,
						"*.rdg"
					});
					fileTypes.AddRange({
						Language.strFilterPuttyConnectionManager,
						"*.dat"
					});
					fileTypes.AddRange({
						Language.strFilterAll,
						"*.*"
					});

					_with1.Filter = string.Join("|", fileTypes.ToArray());

					if (!(openFileDialog.ShowDialog() == DialogResult.OK))
						return;

					TreeNode parentTreeNode = GetParentTreeNode(rootTreeNode, selectedTreeNode, alwaysUseSelectedTreeNode);
					if (parentTreeNode == null)
						return;

					foreach (string fileName in openFileDialog.FileNames) {
						try {
							switch (DetermineFileType(fileName)) {
								case FileType.mRemoteXml:
									mRemoteNG.Config.Import.mRemoteNG.Import(fileName, parentTreeNode);
									break;
								case FileType.RemoteDesktopConnection:
									mRemoteNG.Config.Import.RemoteDesktopConnection.Import(fileName, parentTreeNode);
									break;
								case FileType.RemoteDesktopConnectionManager:
									mRemoteNG.Config.Import.RemoteDesktopConnectionManager.Import(fileName, parentTreeNode);
									break;
								case FileType.PuttyConnectionManager:
									mRemoteNG.Config.Import.PuttyConnectionManager.Import(fileName, parentTreeNode);
									break;
								default:
									throw new FileFormatException("Unrecognized file format.");
							}
						} catch (Exception ex) {
							cTaskDialog.ShowTaskDialogBox(Application.ProductName, Language.strImportFileFailedMainInstruction, string.Format(Language.strImportFileFailedContent, fileName), mRemoteNG.Tools.Misc.GetExceptionMessageRecursive(ex), "", "", "", "", eTaskDialogButtons.OK, eSysIcons.Error,
							null);
						}
					}

					parentTreeNode.Expand();
					Container.Info parentContainer = parentTreeNode.Tag as Container.Info;
					if (parentContainer != null)
						parentContainer.IsExpanded = true;

					Runtime.SaveConnectionsBG();
				}
			} catch (Exception ex) {
				Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromFile() failed.", ex, , true);
			}
		}

		public static void ImportFromActiveDirectory(string ldapPath)
		{
			try {
				TreeNode rootTreeNode = mRemoteNG.Tree.Node.TreeView.Nodes[0];
				TreeNode selectedTreeNode = mRemoteNG.Tree.Node.TreeView.SelectedNode;

				TreeNode parentTreeNode = GetParentTreeNode(rootTreeNode, selectedTreeNode);
				if (parentTreeNode == null)
					return;

				mRemoteNG.Config.Import.ActiveDirectory.Import(ldapPath, parentTreeNode);

				parentTreeNode.Expand();
				Container.Info parentContainer = parentTreeNode.Tag as Container.Info;
				if (parentContainer != null)
					parentContainer.IsExpanded = true;

				Runtime.SaveConnectionsBG();
			} catch (Exception ex) {
				Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromActiveDirectory() failed.", ex, , true);
			}
		}

		public static void ImportFromPortScan(IEnumerable hosts, Connection.Protocol.Protocols protocol)
		{
			try {
				TreeNode rootTreeNode = mRemoteNG.Tree.Node.TreeView.Nodes[0];
				TreeNode selectedTreeNode = mRemoteNG.Tree.Node.TreeView.SelectedNode;

				TreeNode parentTreeNode = GetParentTreeNode(rootTreeNode, selectedTreeNode);
				if (parentTreeNode == null)
					return;

				mRemoteNG.Config.Import.PortScan.Import(hosts, protocol, parentTreeNode);

				parentTreeNode.Expand();
				Container.Info parentContainer = parentTreeNode.Tag as Container.Info;
				if (parentContainer != null)
					parentContainer.IsExpanded = true;

				Runtime.SaveConnectionsBG();
			} catch (Exception ex) {
				Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromPortScan() failed.", ex, , true);
			}
		}
		#endregion

		#region "Private Methods"
		private static TreeNode GetParentTreeNode(TreeNode rootTreeNode, TreeNode selectedTreeNode, bool alwaysUseSelectedTreeNode = false)
		{
			TreeNode parentTreeNode = null;

			selectedTreeNode = GetContainerTreeNode(selectedTreeNode);
			if (selectedTreeNode == null || object.ReferenceEquals(selectedTreeNode, rootTreeNode)) {
				parentTreeNode = rootTreeNode;
			} else {
				if (alwaysUseSelectedTreeNode) {
					parentTreeNode = GetContainerTreeNode(selectedTreeNode);
				} else {
					cTaskDialog.ShowCommandBox(Application.ProductName, Language.strImportLocationMainInstruction, Language.strImportLocationContent, "", "", "", string.Format(Language.strImportLocationCommandButtons, Constants.vbLf, rootTreeNode.Text, selectedTreeNode.Text), true, eSysIcons.Question, 0);
					switch (cTaskDialog.CommandButtonResult) {
						case 0:
							// Root
							parentTreeNode = rootTreeNode;
							break;
						case 1:
							// Selected Folder
							parentTreeNode = GetContainerTreeNode(selectedTreeNode);
							break;
						default:
							// Cancel
							parentTreeNode = null;
							break;
					}
				}
			}

			return parentTreeNode;
		}

		private static TreeNode GetContainerTreeNode(TreeNode treeNode)
		{
			switch (mRemoteNG.Tree.Node.GetNodeType(treeNode)) {
				case mRemoteNG.Tree.Node.Type.Root:
				case mRemoteNG.Tree.Node.Type.Container:
					return treeNode;
				case mRemoteNG.Tree.Node.Type.Connection:
					return treeNode.Parent;
				default:
					return null;
			}
		}

		private static FileType DetermineFileType(string fileName)
		{
			// TODO: Use the file contents to determine the file type instead of trusting the extension
			string fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
			switch (fileExtension) {
				case ".xml":
					return FileType.mRemoteXml;
				case ".rdp":
					return FileType.RemoteDesktopConnection;
				case ".rdg":
					return FileType.RemoteDesktopConnectionManager;
				case ".dat":
					return FileType.PuttyConnectionManager;
				default:
					return FileType.Unknown;
			}
		}
		#endregion

		#region "Private Enumerations"
		private enum FileType : int
		{
			Unknown = 0,
			// ReSharper disable once InconsistentNaming
			mRemoteXml,
			RemoteDesktopConnection,
			RemoteDesktopConnectionManager,
			PuttyConnectionManager
		}
		#endregion
	}
}
