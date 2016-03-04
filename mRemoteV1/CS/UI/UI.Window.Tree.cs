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
using mRemoteNG.App;
using mRemoteNG.My;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;


namespace mRemoteNG.UI.Window
{
	public partial class Tree : Base
	{
        #region Form Stuff
		public void Tree_Load(object sender, EventArgs e)
		{
			ApplyLanguage();
					
			Themes.ThemeManager.ThemeChanged += ApplyTheme;
			ApplyTheme();
					
			txtSearch.Multiline = true;
			txtSearch.MinimumSize = new Size(0, 14);
			txtSearch.Size = new Size(txtSearch.Size.Width, 14);
			txtSearch.Multiline = false;
		}
				
		private void ApplyLanguage()
		{
			Text = Language.strConnections;
			TabText = Language.strConnections;
					
			mMenAddConnection.ToolTipText = Language.strAddConnection;
			mMenAddFolder.ToolTipText = Language.strAddFolder;
			mMenView.ToolTipText = Language.strMenuView.Replace("&", "");
			mMenViewExpandAllFolders.Text = Language.strExpandAllFolders;
			mMenViewCollapseAllFolders.Text = Language.strCollapseAllFolders;
			mMenSortAscending.ToolTipText = Language.strSortAsc;
					
			cMenTreeConnect.Text = Language.strConnect;
			cMenTreeConnectWithOptions.Text = Language.strConnectWithOptions;
			cMenTreeConnectWithOptionsConnectToConsoleSession.Text = Language.strConnectToConsoleSession;
			cMenTreeConnectWithOptionsDontConnectToConsoleSession.Text = Language.strDontConnectToConsoleSessionMenuItem;
			cMenTreeConnectWithOptionsConnectInFullscreen.Text = Language.strConnectInFullscreen;
			cMenTreeConnectWithOptionsNoCredentials.Text = Language.strConnectNoCredentials;
			cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Text = Language.strChoosePanelBeforeConnecting;
			cMenTreeDisconnect.Text = Language.strMenuDisconnect;
					
			cMenTreeToolsExternalApps.Text = Language.strMenuExternalTools;
			cMenTreeToolsTransferFile.Text = Language.strMenuTransferFile;
					
			cMenTreeDuplicate.Text = Language.strDuplicate;
			cMenTreeRename.Text = Language.strRename;
			cMenTreeDelete.Text = Language.strMenuDelete;
					
			cMenTreeImport.Text = Language.strImportMenuItem;
			cMenTreeImportFile.Text = Language.strImportFromFileMenuItem;
			cMenTreeImportActiveDirectory.Text = Language.strImportAD;
			cMenTreeImportPortScan.Text = Language.strImportPortScan;
			cMenTreeExportFile.Text = Language.strExportToFileMenuItem;
					
			cMenTreeAddConnection.Text = Language.strAddConnection;
			cMenTreeAddFolder.Text = Language.strAddFolder;
					
			cMenTreeToolsSort.Text = Language.strSort;
			cMenTreeToolsSortAscending.Text = Language.strSortAsc;
			cMenTreeToolsSortDescending.Text = Language.strSortDesc;
			cMenTreeMoveUp.Text = Language.strMoveUp;
			cMenTreeMoveDown.Text = Language.strMoveDown;
					
			txtSearch.Text = Language.strSearchPrompt;
		}
				
		public void ApplyTheme()
		{
			msMain.BackColor = Themes.ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			msMain.ForeColor = Themes.ThemeManager.ActiveTheme.ToolbarTextColor;
			tvConnections.BackColor = Themes.ThemeManager.ActiveTheme.ConnectionsPanelBackgroundColor;
			tvConnections.ForeColor = Themes.ThemeManager.ActiveTheme.ConnectionsPanelTextColor;
			tvConnections.LineColor = Themes.ThemeManager.ActiveTheme.ConnectionsPanelTreeLineColor;
			BackColor = Themes.ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			txtSearch.BackColor = Themes.ThemeManager.ActiveTheme.SearchBoxBackgroundColor;
			txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextPromptColor;
		}
        #endregion
				
        #region Public Methods
		public Tree(DockContent panel)
		{
			WindowType = Type.Tree;
			DockPnl = panel;
			InitializeComponent();
			FillImageList();
					
			DescriptionTooltip = new ToolTip();
			DescriptionTooltip.InitialDelay = 300;
			DescriptionTooltip.ReshowDelay = 0;
		}
				
		public void InitialRefresh()
		{
			tvConnections_AfterSelect(tvConnections, new TreeViewEventArgs(tvConnections.SelectedNode, TreeViewAction.ByMouse));
		}
        #endregion
				
        #region Public Properties
		public ToolTip DescriptionTooltip {get; set;}
        #endregion
				
        #region Private Methods
		private void FillImageList()
		{
			try
			{
				imgListTree.Images.Add(Resources.Root);
				imgListTree.Images.Add(Resources.Folder);
				imgListTree.Images.Add(Resources.Play);
				imgListTree.Images.Add(Resources.Pause);
				imgListTree.Images.Add(Resources.PuttySessions);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "FillImageList (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		public void tvConnections_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			cMenTreeDelete.ShortcutKeys = Keys.None;
		}
				
		public void tvConnections_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			try
			{
				cMenTreeDelete.ShortcutKeys = Keys.Delete;
						
				mRemoteNG.Tree.Node.FinishRenameSelectedNode(e.Label);
				Windows.configForm.pGrid_SelectedObjectChanged();
				ShowHideTreeContextMenuItems(e.Node);
				SaveConnectionsBG();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_AfterLabelEdit (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		public void tvConnections_AfterSelect(object sender, TreeViewEventArgs e)
		{
			try
			{
				if ((mRemoteNG.Tree.Node.GetNodeType(e.Node) == mRemoteNG.Tree.Node.Type.Connection) || (mRemoteNG.Tree.Node.GetNodeType(e.Node) == mRemoteNG.Tree.Node.Type.PuttySession))
				{
					Windows.configForm.SetPropertyGridObject(e.Node.Tag);
				}
				else if (mRemoteNG.Tree.Node.GetNodeType(e.Node) == mRemoteNG.Tree.Node.Type.Container)
				{
					Windows.configForm.SetPropertyGridObject((e.Node.Tag as Container.Info).ConnectionInfo);
				}
				else if ((mRemoteNG.Tree.Node.GetNodeType(e.Node) == mRemoteNG.Tree.Node.Type.Root) || (mRemoteNG.Tree.Node.GetNodeType(e.Node) == mRemoteNG.Tree.Node.Type.PuttyRoot))
				{
					Windows.configForm.SetPropertyGridObject(e.Node.Tag);
				}
				else
				{
					return;
				}
						
				Windows.configForm.pGrid_SelectedObjectChanged();
				ShowHideTreeContextMenuItems(e.Node);
				Windows.sessionsForm.GetSessions(true);
						
				LastSelected = mRemoteNG.Tree.Node.GetConstantID(e.Node);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_AfterSelect (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		public void tvConnections_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			try
			{
				ShowHideTreeContextMenuItems(tvConnections.SelectedNode);
				tvConnections.SelectedNode = e.Node;
						
				if (e.Button == MouseButtons.Left)
				{
					if (Settings.SingleClickOnConnectionOpensIt && 
						(mRemoteNG.Tree.Node.GetNodeType(e.Node) == mRemoteNG.Tree.Node.Type.Connection | 
						mRemoteNG.Tree.Node.GetNodeType(e.Node) == mRemoteNG.Tree.Node.Type.PuttySession))
					{
						OpenConnection();
					}
							
					if (Settings.SingleClickSwitchesToOpenConnection && mRemoteNG.Tree.Node.GetNodeType(e.Node) == mRemoteNG.Tree.Node.Type.Connection)
					{
						SwitchToOpenConnection(e.Node.Tag);
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_NodeMouseClick (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		static public void tvConnections_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) == mRemoteNG.Tree.Node.Type.Connection | 
				mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) == mRemoteNG.Tree.Node.Type.PuttySession)
			{
				OpenConnection();
			}
		}
				
		public void tvConnections_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				mRemoteNG.Tree.Node.SetNodeToolTip(e, DescriptionTooltip);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_MouseMove (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		private static void EnableMenuItemsRecursive(ToolStripItemCollection items, bool enable = true)
		{
			ToolStripMenuItem menuItem = default(ToolStripMenuItem);
			foreach (ToolStripItem item in items)
			{
				menuItem = item as ToolStripMenuItem;
				if (menuItem == null)
				{
					continue;
				}
				menuItem.Enabled = enable;
				if (menuItem.HasDropDownItems)
				{
					EnableMenuItemsRecursive(menuItem.DropDownItems, enable);
				}
			}
		}
				
		private void ShowHideTreeContextMenuItems(TreeNode selectedNode)
		{
			if (selectedNode == null)
			{
				return ;
			}
					
			try
			{
				cMenTree.Enabled = true;
				EnableMenuItemsRecursive(cMenTree.Items);
						
				if (mRemoteNG.Tree.Node.GetNodeType(selectedNode) == mRemoteNG.Tree.Node.Type.Connection)
				{
					mRemoteNG.Connection.Info connectionInfo = selectedNode.Tag;
							
					if (connectionInfo.OpenConnections.Count == 0)
					{
						cMenTreeDisconnect.Enabled = false;
					}
							
					if (!(connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.SSH1 | 
						connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.SSH2))
					{
						cMenTreeToolsTransferFile.Enabled = false;
					}
							
					if (!(connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP | connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.ICA))
					{
						cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
						cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
					}
							
					if (connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.IntApp)
					{
						cMenTreeConnectWithOptionsNoCredentials.Enabled = false;
					}
				}
				else if (mRemoteNG.Tree.Node.GetNodeType(selectedNode) == mRemoteNG.Tree.Node.Type.PuttySession)
				{
					mRemoteNG.Connection.PuttySession.Info puttySessionInfo = selectedNode.Tag;
							
					cMenTreeAddConnection.Enabled = false;
					cMenTreeAddFolder.Enabled = false;
							
					if (puttySessionInfo.OpenConnections.Count == 0)
					{
						cMenTreeDisconnect.Enabled = false;
					}
							
					if (!(puttySessionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.SSH1 | puttySessionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.SSH2))
					{
						cMenTreeToolsTransferFile.Enabled = false;
					}
							
					cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
					cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
					cMenTreeToolsSort.Enabled = false;
					cMenTreeDuplicate.Enabled = false;
					cMenTreeRename.Enabled = false;
					cMenTreeDelete.Enabled = false;
					cMenTreeMoveUp.Enabled = false;
					cMenTreeMoveDown.Enabled = false;
				}
				else if (mRemoteNG.Tree.Node.GetNodeType(selectedNode) == mRemoteNG.Tree.Node.Type.Container)
				{
					cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
					cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
					cMenTreeDisconnect.Enabled = false;
							
					int openConnections = 0;
					mRemoteNG.Connection.Info connectionInfo = default(mRemoteNG.Connection.Info);
					foreach (TreeNode node in selectedNode.Nodes)
					{
						if (node.Tag is mRemoteNG.Connection.Info)
						{
							connectionInfo = node.Tag;
							openConnections = openConnections + connectionInfo.OpenConnections.Count;
						}
					}
					if (openConnections == 0)
					{
						cMenTreeDisconnect.Enabled = false;
					}
							
					cMenTreeToolsTransferFile.Enabled = false;
					cMenTreeToolsExternalApps.Enabled = false;
				}
				else if (mRemoteNG.Tree.Node.GetNodeType(selectedNode) == mRemoteNG.Tree.Node.Type.Root)
				{
					cMenTreeConnect.Enabled = false;
					cMenTreeConnectWithOptions.Enabled = false;
					cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
					cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
					cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Enabled = false;
					cMenTreeDisconnect.Enabled = false;
					cMenTreeToolsTransferFile.Enabled = false;
					cMenTreeToolsExternalApps.Enabled = false;
					cMenTreeDuplicate.Enabled = false;
					cMenTreeDelete.Enabled = false;
					cMenTreeMoveUp.Enabled = false;
					cMenTreeMoveDown.Enabled = false;
				}
				else if (mRemoteNG.Tree.Node.GetNodeType(selectedNode) == mRemoteNG.Tree.Node.Type.PuttyRoot)
				{
					cMenTreeAddConnection.Enabled = false;
					cMenTreeAddFolder.Enabled = false;
					cMenTreeConnect.Enabled = false;
					cMenTreeConnectWithOptions.Enabled = false;
					cMenTreeDisconnect.Enabled = false;
					cMenTreeToolsTransferFile.Enabled = false;
					cMenTreeConnectWithOptions.Enabled = false;
					cMenTreeToolsSort.Enabled = false;
					cMenTreeToolsExternalApps.Enabled = false;
					cMenTreeDuplicate.Enabled = false;
					cMenTreeRename.Enabled = true;
					cMenTreeDelete.Enabled = false;
					cMenTreeMoveUp.Enabled = false;
					cMenTreeMoveDown.Enabled = false;
				}
				else
				{
					cMenTree.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ShowHideTreeContextMenuItems (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
        #endregion
				
        #region Drag and Drop
		static public void tvConnections_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				//Check that there is a TreeNode being dragged
				if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", true) == false)
				{
					return;
				}
						
				//Get the TreeView raising the event (in case multiple on form)
				TreeView selectedTreeview = (TreeView) sender;
						
				//Get the TreeNode being dragged
				System.Windows.Forms.TreeNode dropNode = (System.Windows.Forms.TreeNode) (e.Data.GetData("System.Windows.Forms.TreeNode"));
						
				//The target node should be selected from the DragOver event
				TreeNode targetNode = selectedTreeview.SelectedNode;
						
				if (dropNode == targetNode)
				{
					return;
				}
						
				if (mRemoteNG.Tree.Node.GetNodeType(dropNode) == mRemoteNG.Tree.Node.Type.Root)
				{
					return;
				}
						
				if (dropNode == targetNode.Parent)
				{
					return;
				}
						
				//Remove the drop node from its current location
				dropNode.Remove();
						
				//If there is no targetNode add dropNode to the bottom of
				//the TreeView root nodes, otherwise add it to the end of
				//the dropNode child nodes
						
				if (mRemoteNG.Tree.Node.GetNodeType(targetNode) == mRemoteNG.Tree.Node.Type.Root | mRemoteNG.Tree.Node.GetNodeType(targetNode) == mRemoteNG.Tree.Node.Type.Container)
				{
					targetNode.Nodes.Insert(0, dropNode);
				}
				else
				{
					targetNode.Parent.Nodes.Insert(targetNode.Index + 1, dropNode);
				}
						
				if (mRemoteNG.Tree.Node.GetNodeType(dropNode) == mRemoteNG.Tree.Node.Type.Connection | mRemoteNG.Tree.Node.GetNodeType(dropNode) == mRemoteNG.Tree.Node.Type.Container)
				{
					if (mRemoteNG.Tree.Node.GetNodeType(dropNode.Parent) == mRemoteNG.Tree.Node.Type.Container)
					{
						dropNode.Tag.Parent = dropNode.Parent.Tag;
					}
					else if (mRemoteNG.Tree.Node.GetNodeType(dropNode.Parent) == mRemoteNG.Tree.Node.Type.Root)
					{
						dropNode.Tag.Parent = null;
						if (mRemoteNG.Tree.Node.GetNodeType(dropNode) == mRemoteNG.Tree.Node.Type.Connection)
						{
							dropNode.Tag.Inherit.TurnOffInheritanceCompletely();
						}
						else if (mRemoteNG.Tree.Node.GetNodeType(dropNode) == mRemoteNG.Tree.Node.Type.Container)
						{
							dropNode.Tag.ConnectionInfo.Inherit.TurnOffInheritanceCompletely();
						}
					}
				}
						
				//Ensure the newly created node is visible to
				//the user and select it
				dropNode.EnsureVisible();
				selectedTreeview.SelectedNode = dropNode;
						
				SaveConnectionsBG();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_DragDrop (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		static public void tvConnections_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				//See if there is a TreeNode being dragged
				if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", true))
				{
					//TreeNode found allow move effect
					e.Effect = DragDropEffects.Move;
				}
				else
				{
					//No TreeNode found, prevent move
					e.Effect = DragDropEffects.None;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_DragEnter (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		static public void tvConnections_DragOver(object sender, DragEventArgs e)
		{
			try
			{
				//Check that there is a TreeNode being dragged
				if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", true) == false)
				{
					return;
				}
						
				//Get the TreeView raising the event (in case multiple on form)
				TreeView selectedTreeview = (TreeView) sender;
						
				//As the mouse moves over nodes, provide feedback to
				//the user by highlighting the node that is the
				//current drop target
				Point pt = ((TreeView) sender).PointToClient(new Point(e.X, e.Y));
				TreeNode targetNode = selectedTreeview.GetNodeAt(pt);
						
				//Select the node currently under the cursor
				selectedTreeview.SelectedNode = targetNode;
						
				//Check that the selected node is not the dropNode and
				//also that it is not a child of the dropNode and
				//therefore an invalid target
				TreeNode dropNode = (TreeNode) (e.Data.GetData("System.Windows.Forms.TreeNode"));
						
				Root.PuttySessions.Info puttyRootInfo = default(Root.PuttySessions.Info);
				while (!(targetNode == null))
				{
					puttyRootInfo = targetNode.Tag as Root.PuttySessions.Info;
					if (puttyRootInfo != null || targetNode == dropNode)
					{
						e.Effect = DragDropEffects.None;
						return ;
					}
					targetNode = targetNode.Parent;
				}
						
				//Currently selected node is a suitable target
				e.Effect = DragDropEffects.Move;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_DragOver (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		public void tvConnections_ItemDrag(object sender, ItemDragEventArgs e)
		{
			try
			{
				TreeNode dragTreeNode = e.Item as TreeNode;
				if (dragTreeNode == null)
				{
					return ;
				}
						
				if (dragTreeNode.Tag == null)
				{
					return ;
				}
				if (dragTreeNode.Tag is mRemoteNG.Connection.PuttySession.Info|| !(dragTreeNode.Tag is mRemoteNG.Connection.Info|| dragTreeNode.Tag is Container.Info))
				{
					tvConnections.SelectedNode = dragTreeNode;
					return ;
				}
						
				//Set the drag node and initiate the DragDrop
				DoDragDrop(e.Item, DragDropEffects.Move);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_ItemDrag (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
        #endregion
				
        #region Tree Context Menu
		public void cMenTreeAddConnection_Click(System.Object sender, EventArgs e)
		{
			AddConnection();
			SaveConnectionsBG();
		}
				
		public void cMenTreeAddFolder_Click(System.Object sender, EventArgs e)
		{
			AddFolder();
			SaveConnectionsBG();
		}
				
		static public void cMenTreeConnect_Click(System.Object sender, EventArgs e)
		{
			OpenConnection(mRemoteNG.Connection.Info.Force.DoNotJump);
		}
				
		static public void cMenTreeConnectWithOptionsConnectToConsoleSession_Click(System.Object sender, EventArgs e)
		{
			OpenConnection(mRemoteNG.Connection.Info.Force.UseConsoleSession | mRemoteNG.Connection.Info.Force.DoNotJump);
		}
				
		static public void cMenTreeConnectWithOptionsNoCredentials_Click(System.Object sender, EventArgs e)
		{
			OpenConnection(mRemoteNG.Connection.Info.Force.NoCredentials);
		}
				
		static public void cMenTreeConnectWithOptionsDontConnectToConsoleSession_Click(System.Object sender, EventArgs e)
		{
			OpenConnection(mRemoteNG.Connection.Info.Force.DontUseConsoleSession | mRemoteNG.Connection.Info.Force.DoNotJump);
		}
				
		static public void cMenTreeConnectWithOptionsConnectInFullscreen_Click(System.Object sender, EventArgs e)
		{
			OpenConnection(mRemoteNG.Connection.Info.Force.Fullscreen | mRemoteNG.Connection.Info.Force.DoNotJump);
		}
				
		static public void cMenTreeConnectWithOptionsChoosePanelBeforeConnecting_Click(System.Object sender, EventArgs e)
		{
			OpenConnection(mRemoteNG.Connection.Info.Force.OverridePanel | mRemoteNG.Connection.Info.Force.DoNotJump);
		}
				
		public void cMenTreeDisconnect_Click(System.Object sender, EventArgs e)
		{
			DisconnectConnection();
		}
				
		static public void cMenTreeToolsTransferFile_Click(System.Object sender, EventArgs e)
		{
			SshTransferFile();
		}
				
		public void mMenSortAscending_Click(System.Object sender, EventArgs e)
		{
			tvConnections.BeginUpdate();
			mRemoteNG.Tree.Node.Sort(tvConnections.Nodes[0], SortOrder.Ascending);
			tvConnections.EndUpdate();
			SaveConnectionsBG();
		}
				
		public void cMenTreeToolsSortAscending_Click(System.Object sender, EventArgs e)
		{
			tvConnections.BeginUpdate();
			mRemoteNG.Tree.Node.Sort(tvConnections.SelectedNode, SortOrder.Ascending);
			tvConnections.EndUpdate();
			SaveConnectionsBG();
		}
				
		public void cMenTreeToolsSortDescending_Click(System.Object sender, EventArgs e)
		{
			tvConnections.BeginUpdate();
			mRemoteNG.Tree.Node.Sort(tvConnections.SelectedNode, SortOrder.Descending);
			tvConnections.EndUpdate();
			SaveConnectionsBG();
		}
				
		public void cMenTree_DropDownOpening(object sender, EventArgs e)
		{
			AddExternalApps();
		}
				
		private static void cMenTreeToolsExternalAppsEntry_Click(object sender, EventArgs e)
		{
			StartExternalApp(sender.Tag);
		}
				
		public void cMenTreeDuplicate_Click(System.Object sender, EventArgs e)
		{
			mRemoteNG.Tree.Node.CloneNode(tvConnections.SelectedNode);
			SaveConnectionsBG();
		}
				
		static public void cMenTreeRename_Click(System.Object sender, EventArgs e)
		{
			mRemoteNG.Tree.Node.StartRenameSelectedNode();
			SaveConnectionsBG();
		}
				
		static public void cMenTreeDelete_Click(System.Object sender, EventArgs e)
		{
			mRemoteNG.Tree.Node.DeleteSelectedNode();
			SaveConnectionsBG();
		}
				
		static public void cMenTreeImportFile_Click(System.Object sender, EventArgs e)
		{
			Import.ImportFromFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode, true);
		}
				
		static public void cMenTreeImportActiveDirectory_Click(System.Object sender, EventArgs e)
		{
			Windows.Show(Type.ActiveDirectoryImport);
		}
				
		static public void cMenTreeImportPortScan_Click(System.Object sender, EventArgs e)
		{
			Windows.Show(UI.Window.Type.PortScan, true);
		}
				
		static public void cMenTreeExportFile_Click(System.Object sender, EventArgs e)
		{
			Export.ExportToFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode);
		}
		static public void cMenTreeMoveUp_Click(System.Object sender, EventArgs e)
		{
			mRemoteNG.Tree.Node.MoveNodeUp();
			SaveConnectionsBG();
		}
				
		static public void cMenTreeMoveDown_Click(System.Object sender, EventArgs e)
		{
			mRemoteNG.Tree.Node.MoveNodeDown();
			SaveConnectionsBG();
		}
        #endregion
				
        #region Context Menu Actions
		public void AddConnection()
		{
			try
			{
				if (tvConnections.SelectedNode == null)
				{
					tvConnections.SelectedNode = tvConnections.Nodes[0];
				}
						
				TreeNode newTreeNode = mRemoteNG.Tree.Node.AddNode(mRemoteNG.Tree.Node.Type.Connection);
				if (newTreeNode == null)
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Tree.AddConnection() failed." + Constants.vbNewLine + "mRemoteNG.Tree.Node.AddNode() returned Nothing.", true);
					return ;
				}
						
				TreeNode containerNode = tvConnections.SelectedNode;
				if (mRemoteNG.Tree.Node.GetNodeType(containerNode) == mRemoteNG.Tree.Node.Type.Connection)
				{
					containerNode = containerNode.Parent;
				}
						
				mRemoteNG.Connection.Info newConnectionInfo = new mRemoteNG.Connection.Info();
				if (mRemoteNG.Tree.Node.GetNodeType(containerNode) == mRemoteNG.Tree.Node.Type.Root)
				{
					newConnectionInfo.Inherit.TurnOffInheritanceCompletely();
				}
				else
				{
					newConnectionInfo.Parent = containerNode.Tag;
				}
						
				newConnectionInfo.TreeNode = newTreeNode;
				newTreeNode.Tag = newConnectionInfo;
				ConnectionList.Add(newConnectionInfo);
						
				containerNode.Nodes.Add(newTreeNode);
						
				tvConnections.SelectedNode = newTreeNode;
				tvConnections.SelectedNode.BeginEdit();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Tree.AddConnection() failed." + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		public void AddFolder()
		{
			try
			{
				TreeNode newNode = mRemoteNG.Tree.Node.AddNode(mRemoteNG.Tree.Node.Type.Container);
				Container.Info newContainerInfo = new Container.Info();
				newNode.Tag = newContainerInfo;
				newContainerInfo.TreeNode = newNode;
						
				TreeNode selectedNode = mRemoteNG.Tree.Node.SelectedNode;
				TreeNode parentNode = default(TreeNode);
				if (selectedNode == null)
				{
					parentNode = tvConnections.Nodes[0];
				}
				else
				{
					if (mRemoteNG.Tree.Node.GetNodeType(selectedNode) == mRemoteNG.Tree.Node.Type.Connection)
					{
						parentNode = selectedNode.Parent;
					}
					else
					{
						parentNode = selectedNode;
					}
				}
						
				newContainerInfo.ConnectionInfo = new mRemoteNG.Connection.Info(newContainerInfo);
				newContainerInfo.ConnectionInfo.Name = newNode.Text;
						
				// We can only inherit from a container node, not the root node or connection nodes
				if (mRemoteNG.Tree.Node.GetNodeType(parentNode) == mRemoteNG.Tree.Node.Type.Container)
				{
					newContainerInfo.Parent = parentNode.Tag;
				}
				else
				{
					newContainerInfo.ConnectionInfo.Inherit.TurnOffInheritanceCompletely();
				}
						
				ContainerList.Add(newContainerInfo);
				parentNode.Nodes.Add(newNode);
						
				tvConnections.SelectedNode = newNode;
				tvConnections.SelectedNode.BeginEdit();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, string.Format(Language.strErrorAddFolderFailed, ex.Message), true);
			}
		}
				
		private void DisconnectConnection()
		{
			try
			{
				if (tvConnections.SelectedNode != null)
				{
					if (tvConnections.SelectedNode.Tag is mRemoteNG.Connection.Info)
					{
						mRemoteNG.Connection.Info conI = tvConnections.SelectedNode.Tag;
						for (int i = 0; i <= conI.OpenConnections.Count - 1; i++)
						{
							conI.OpenConnections[i].Disconnect();
						}
					}
							
					if (tvConnections.SelectedNode.Tag is Container.Info)
					{
						foreach (TreeNode n in tvConnections.SelectedNode.Nodes)
						{
							if (n.Tag is mRemoteNG.Connection.Info)
							{
								mRemoteNG.Connection.Info conI = n.Tag;
								for (int i = 0; i <= conI.OpenConnections.Count - 1; i++)
								{
									conI.OpenConnections[i].Disconnect();
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "DisconnectConnection (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		private static void SshTransferFile()
		{
			try
			{
				Windows.Show(Type.SSHTransfer);
						
				mRemoteNG.Connection.Info conI = mRemoteNG.Tree.Node.SelectedNode.Tag;
						
				Windows.sshtransferForm.Hostname = conI.Hostname;
				Windows.sshtransferForm.Username = conI.Username;
				Windows.sshtransferForm.Password = conI.Password;
				Windows.sshtransferForm.Port = System.Convert.ToString(conI.Port);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SSHTransferFile (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		private void AddExternalApps()
		{
			try
			{
				//clean up
				cMenTreeToolsExternalApps.DropDownItems.Clear();
						
				//add ext apps
				foreach (Tools.ExternalTool extA in Runtime.ExternalTools)
				{
					ToolStripMenuItem nItem = new ToolStripMenuItem();
					nItem.Text = extA.DisplayName;
					nItem.Tag = extA;
							
					nItem.Image = extA.Image;
							
					nItem.Click += cMenTreeToolsExternalAppsEntry_Click;
							
					cMenTreeToolsExternalApps.DropDownItems.Add(nItem);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "cMenTreeTools_DropDownOpening failed (UI.Window.Tree)" + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		private static void StartExternalApp(Tools.ExternalTool externalTool)
		{
			try
			{
				if (mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) == mRemoteNG.Tree.Node.Type.Connection | mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) == mRemoteNG.Tree.Node.Type.PuttySession)
				{
					externalTool.Start(mRemoteNG.Tree.Node.SelectedNode.Tag);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "cMenTreeToolsExternalAppsEntry_Click failed (UI.Window.Tree)" + Constants.vbNewLine + ex.Message, true);
			}
		}
        #endregion
				
        #region Menu
		static public void mMenViewExpandAllFolders_Click(System.Object sender, EventArgs e)
		{
			mRemoteNG.Tree.Node.ExpandAllNodes();
		}
				
		public void mMenViewCollapseAllFolders_Click(System.Object sender, EventArgs e)
		{
			if (tvConnections.SelectedNode != null)
			{
				if (tvConnections.SelectedNode.IsEditing)
				{
					tvConnections.SelectedNode.EndEdit(false);
				}
			}
			mRemoteNG.Tree.Node.CollapseAllNodes();
		}
        #endregion
				
        #region Search
		public void txtSearch_GotFocus(object sender, EventArgs e)
		{
			txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextColor;
			if (txtSearch.Text == Language.strSearchPrompt)
			{
				txtSearch.Text = "";
			}
		}
				
		public void txtSearch_LostFocus(object sender, EventArgs e)
		{
			if (txtSearch.Text == "")
			{
				txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextPromptColor;
				txtSearch.Text = Language.strSearchPrompt;
			}
		}
				
		public void txtSearch_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Escape)
				{
					e.Handled = true;
					tvConnections.Focus();
				}
				else if (e.KeyCode == Keys.Up)
				{
					tvConnections.SelectedNode = tvConnections.SelectedNode.PrevVisibleNode;
				}
				else if (e.KeyCode == Keys.Down)
				{
					tvConnections.SelectedNode = tvConnections.SelectedNode.NextVisibleNode;
				}
				else
				{
					tvConnections_KeyDown(sender, e);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "txtSearch_KeyDown (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		public void txtSearch_TextChanged(System.Object sender, EventArgs e)
		{
			tvConnections.SelectedNode = mRemoteNG.Tree.Node.Find(tvConnections.Nodes[0], txtSearch.Text);
		}
				
		public void tvConnections_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				if (char.IsLetterOrDigit(e.KeyChar))
				{
					txtSearch.Text = e.KeyChar.ToString();
							
					txtSearch.Focus();
					txtSearch.SelectionStart = txtSearch.TextLength;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_KeyPress (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		public void tvConnections_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Enter)
				{
					if (tvConnections.SelectedNode.Tag is mRemoteNG.Connection.Info)
					{
						e.Handled = true;
						OpenConnection();
					}
					else
					{
						if (tvConnections.SelectedNode.IsExpanded)
						{
							tvConnections.SelectedNode.Collapse(true);
						}
						else
						{
							tvConnections.SelectedNode.Expand();
						}
					}
				}
				else if (e.KeyCode == Keys.Escape ^ e.KeyCode == Keys.Control | e.KeyCode == Keys.F)
				{
					txtSearch.Focus();
					txtSearch.SelectionStart = txtSearch.TextLength;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_KeyDown (UI.Window.Tree) failed" + Constants.vbNewLine + ex.Message, true);
			}
		}
        #endregion
	}
}
