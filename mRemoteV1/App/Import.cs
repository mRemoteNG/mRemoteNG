using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.Config.Import;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.UI.TaskDialog;

namespace mRemoteNG.App
{
    public class Import
    {
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

        #region Public Methods
        //TODO Fix for TreeListView
        public static void ImportFromFile(TreeNode rootTreeNode, TreeNode selectedTreeNode,
            bool alwaysUseSelectedTreeNode = false)
        {
            try
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    openFileDialog.Multiselect = true;

                    var fileTypes = new List<string>();
                    fileTypes.AddRange(new[] {Language.strFilterAllImportable, "*.xml;*.rdp;*.rdg;*.dat"});
                    fileTypes.AddRange(new[] {Language.strFiltermRemoteXML, "*.xml"});
                    fileTypes.AddRange(new[] {Language.strFilterRDP, "*.rdp"});
                    fileTypes.AddRange(new[] {Language.strFilterRdgFiles, "*.rdg"});
                    fileTypes.AddRange(new[] {Language.strFilterPuttyConnectionManager, "*.dat"});
                    fileTypes.AddRange(new[] {Language.strFilterAll, "*.*"});

                    openFileDialog.Filter = string.Join("|", fileTypes.ToArray());

                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    var parentTreeNode = GetParentTreeNode(rootTreeNode, selectedTreeNode, alwaysUseSelectedTreeNode);
                    if (parentTreeNode == null)
                    {
                        return;
                    }

                    foreach (var fileName in openFileDialog.FileNames)
                    {
                        try
                        {
                            switch (DetermineFileType(fileName))
                            {
                                case FileType.mRemoteXml:
                                    Config.Import.mRemoteNGImporter.Import(fileName, parentTreeNode);
                                    break;
                                case FileType.RemoteDesktopConnection:
                                    RemoteDesktopConnection.Import(fileName, parentTreeNode);
                                    break;
                                case FileType.RemoteDesktopConnectionManager:
                                    RemoteDesktopConnectionManager.Import(fileName, parentTreeNode);
                                    break;
                                case FileType.PuttyConnectionManager:
                                    PuttyConnectionManager.Import(fileName, parentTreeNode);
                                    break;
                                default:
                                    throw new FileFormatException("Unrecognized file format.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(string.Format(Language.strImportFileFailedContent, fileName), Language.strImportFileFailedMainInstruction,
                                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromFile() failed:1", ex, logOnly: true);
                        }
                    }

                    parentTreeNode.Expand();
                    var parentContainer = (ContainerInfo) parentTreeNode.Tag;
                    if (parentContainer != null)
                    {
                        parentContainer.IsExpanded = true;
                    }

                    Runtime.SaveConnectionsBG();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromFile() failed:2", ex, logOnly: true);
            }
        }

        public static void ImportFromActiveDirectory(string ldapPath)
        {
            try
            {
                var rootTreeNode = ConnectionTree.TreeView.Nodes[0];
                var selectedTreeNode = ConnectionTree.TreeView.SelectedNode;

                var parentTreeNode = GetParentTreeNode(rootTreeNode, selectedTreeNode);
                if (parentTreeNode == null)
                {
                    return;
                }

                ActiveDirectory.Import(ldapPath, parentTreeNode);

                parentTreeNode.Expand();
                var parentContainer = (ContainerInfo) parentTreeNode.Tag;
                if (parentContainer != null)
                {
                    parentContainer.IsExpanded = true;
                }

                Runtime.SaveConnectionsBG();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromActiveDirectory() failed.", ex,
                    logOnly: true);
            }
        }

        public static void ImportFromPortScan(IEnumerable hosts, ProtocolType protocol)
        {
            try
            {
                var rootTreeNode = ConnectionTree.TreeView.Nodes[0];
                var selectedTreeNode = ConnectionTree.TreeView.SelectedNode;

                var parentTreeNode = GetParentTreeNode(rootTreeNode, selectedTreeNode);
                if (parentTreeNode == null)
                {
                    return;
                }

                PortScan.Import(hosts, protocol, parentTreeNode);

                parentTreeNode.Expand();
                var parentContainer = (ContainerInfo) parentTreeNode.Tag;
                if (parentContainer != null)
                {
                    parentContainer.IsExpanded = true;
                }

                Runtime.SaveConnectionsBG();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromPortScan() failed.", ex,
                    logOnly: true);
            }
        }

        #endregion

        #region Private Methods

        private static TreeNode GetParentTreeNode(TreeNode rootTreeNode, TreeNode selectedTreeNode,
            bool alwaysUseSelectedTreeNode = false)
        {
            TreeNode parentTreeNode;

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
                    CTaskDialog.ShowCommandBox(Application.ProductName, Language.strImportLocationMainInstruction,
                        Language.strImportLocationContent, "", "", "",
                        string.Format(Language.strImportLocationCommandButtons, Environment.NewLine, rootTreeNode.Text,
                            selectedTreeNode.Text), true, ESysIcons.Question, 0);
                    switch (CTaskDialog.CommandButtonResult)
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
            if ((ConnectionTreeNode.GetNodeType(treeNode) == TreeNodeType.Root) ||
                (ConnectionTreeNode.GetNodeType(treeNode) == TreeNodeType.Container))
            {
                return treeNode;
            }
            if (ConnectionTreeNode.GetNodeType(treeNode) == TreeNodeType.Connection)
            {
                return treeNode.Parent;
            }
            return null;
        }

        private static FileType DetermineFileType(string fileName)
        {
            // TODO: Use the file contents to determine the file type instead of trusting the extension
            var fileExtension = Convert.ToString(Path.GetExtension(fileName).ToLowerInvariant());
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
    }
}