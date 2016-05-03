using System.Collections.Generic;
using System;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using mRemoteNG.My;
using PSTaskDialog;
using mRemoteNG.Tree;


namespace mRemoteNG.App
{
	public class Import
	{
        #region Public Methods
		public static void ImportFromFile(TreeNode rootTreeNode, TreeNode selectedTreeNode, bool alwaysUseSelectedTreeNode = false)
		{
			try
			{
				using (OpenFileDialog openFileDialog = new OpenFileDialog())
				{
					openFileDialog.CheckFileExists = true;
					openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					openFileDialog.Multiselect = true;
						
					List<string> fileTypes = new List<string>();
					fileTypes.AddRange(new[] {Language.strFilterAllImportable, "*.xml;*.rdp;*.rdg;*.dat"});
					fileTypes.AddRange(new[] {Language.strFiltermRemoteXML, "*.xml"});
					fileTypes.AddRange(new[] {Language.strFilterRDP, "*.rdp"});
					fileTypes.AddRange(new[] {Language.strFilterRdgFiles, "*.rdg"});
					fileTypes.AddRange(new[] {Language.strFilterPuttyConnectionManager, "*.dat"});
					fileTypes.AddRange(new[] {Language.strFilterAll, "*.*"});
						
					openFileDialog.Filter = string.Join("|", fileTypes.ToArray());
						
					if (!(openFileDialog.ShowDialog() == DialogResult.OK))
					{
						return ;
					}
						
					TreeNode parentTreeNode = GetParentTreeNode(rootTreeNode, selectedTreeNode, alwaysUseSelectedTreeNode);
					if (parentTreeNode == null)
					{
						return ;
					}
						
					foreach (string fileName in openFileDialog.FileNames)
					{
						try
						{
							switch (DetermineFileType(fileName))
							{
								case FileType.mRemoteXml:
									Config.Import.mRemoteNG.Import(fileName, parentTreeNode);
									break;
								case FileType.RemoteDesktopConnection:
                                    Config.Import.RemoteDesktopConnection.Import(fileName, parentTreeNode);
									break;
								case FileType.RemoteDesktopConnectionManager:
                                    Config.Import.RemoteDesktopConnectionManager.Import(fileName, parentTreeNode);
									break;
								case FileType.PuttyConnectionManager:
                                    Config.Import.PuttyConnectionManager.Import(fileName, parentTreeNode);
									break;
								default:
									throw new FileFormatException("Unrecognized file format.");
							}
						}
						catch (Exception ex)
						{
							cTaskDialog.ShowTaskDialogBox(System.Windows.Forms.Application.ProductName, Language.strImportFileFailedMainInstruction, string.Format(Language.strImportFileFailedContent, fileName), Tools.MiscTools.GetExceptionMessageRecursive(ex), "", "", "", "", eTaskDialogButtons.OK, eSysIcons.Error, eSysIcons.Error);
						}
					}
						
					parentTreeNode.Expand();
                    Container.ContainerInfo parentContainer = (Container.ContainerInfo)parentTreeNode.Tag;
					if (parentContainer != null)
					{
						parentContainer.IsExpanded = true;
					}
						
					Runtime.SaveConnectionsBG();
				}
					
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: "App.Import.ImportFromFile() failed.", ex: ex, logOnly: true);
			}
		}
			
		public static void ImportFromActiveDirectory(string ldapPath)
		{
			try
			{
                TreeNode rootTreeNode = ConnectionTree.TreeView.Nodes[0];
                TreeNode selectedTreeNode = ConnectionTree.TreeView.SelectedNode;
					
				TreeNode parentTreeNode = GetParentTreeNode(rootTreeNode, selectedTreeNode);
				if (parentTreeNode == null)
				{
					return ;
				}
					
				Config.Import.ActiveDirectory.Import(ldapPath, parentTreeNode);
					
				parentTreeNode.Expand();
                Container.ContainerInfo parentContainer = (Container.ContainerInfo)parentTreeNode.Tag;
				if (parentContainer != null)
				{
					parentContainer.IsExpanded = true;
				}
					
				Runtime.SaveConnectionsBG();
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionMessage(message: "App.Import.ImportFromActiveDirectory() failed.", ex: ex, logOnly: true);
			}
		}
			
		public static void ImportFromPortScan(IEnumerable hosts, Connection.Protocol.ProtocolType protocol)
		{
			try
			{
                TreeNode rootTreeNode = ConnectionTree.TreeView.Nodes[0];
                TreeNode selectedTreeNode = ConnectionTree.TreeView.SelectedNode;
					
				TreeNode parentTreeNode = GetParentTreeNode(rootTreeNode, selectedTreeNode);
				if (parentTreeNode == null)
				{
					return ;
				}
					
				Config.Import.PortScan.Import(hosts, protocol, parentTreeNode);
					
				parentTreeNode.Expand();
                Container.ContainerInfo parentContainer = (Container.ContainerInfo)parentTreeNode.Tag;
				if (parentContainer != null)
				{
					parentContainer.IsExpanded = true;
				}
					
				Runtime.SaveConnectionsBG();
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionMessage(message: "App.Import.ImportFromPortScan() failed.", ex: ex, logOnly: true);
			}
		}
        #endregion
			
        #region Private Methods
		private static TreeNode GetParentTreeNode(TreeNode rootTreeNode, TreeNode selectedTreeNode, bool alwaysUseSelectedTreeNode = false)
		{
			TreeNode parentTreeNode = default(TreeNode);
				
			selectedTreeNode = GetContainerTreeNode(selectedTreeNode);
			if (selectedTreeNode == null || selectedTreeNode == rootTreeNode)
			{
				parentTreeNode = rootTreeNode;
			}
			else
			{
				if (alwaysUseSelectedTreeNode)
				{
					parentTreeNode = GetContainerTreeNode(selectedTreeNode);
				}
				else
				{
					cTaskDialog.ShowCommandBox(System.Windows.Forms.Application.ProductName, Language.strImportLocationMainInstruction, Language.strImportLocationContent, "", "", "", string.Format(Language.strImportLocationCommandButtons, Constants.vbLf, rootTreeNode.Text, selectedTreeNode.Text), true, eSysIcons.Question, (PSTaskDialog.eSysIcons) 0);
					switch (cTaskDialog.CommandButtonResult)
					{
						case 0: // Root
							parentTreeNode = rootTreeNode;
							break;
						case 1: // Selected Folder
							parentTreeNode = GetContainerTreeNode(selectedTreeNode);
							break;
						default: // Cancel
							parentTreeNode = null;
							break;
					}
				}
			}
				
			return parentTreeNode;
		}
			
		private static TreeNode GetContainerTreeNode(TreeNode treeNode)
		{
			if ((Tree.Node.GetNodeType(treeNode) == Tree.TreeNodeType.Root) || (Tree.Node.GetNodeType(treeNode) == Tree.TreeNodeType.Container))
			{
				return treeNode;
			}
			else if (Tree.Node.GetNodeType(treeNode) == Tree.TreeNodeType.Connection)
			{
				return treeNode.Parent;
			}
			else
			{
				return null;
			}
		}
			
		private static FileType DetermineFileType(string fileName)
		{
			// TODO: Use the file contents to determine the file type instead of trusting the extension
			string fileExtension = System.Convert.ToString(Path.GetExtension(fileName).ToLowerInvariant());
			switch (fileExtension)
			{
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
			
        #region Private Enumerations
		private enum FileType
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