using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
namespace mRemoteNG.UI
{
	namespace Window
	{
		public partial class Tree : Base
		{
			#region " Windows Form Designer generated code "
			private System.ComponentModel.IContainer components;
			private System.Windows.Forms.TextBox withEventsField_txtSearch;
			internal System.Windows.Forms.TextBox txtSearch {
				get { return withEventsField_txtSearch; }
				set {
					if (withEventsField_txtSearch != null) {
						withEventsField_txtSearch.GotFocus -= txtSearch_GotFocus;
						withEventsField_txtSearch.LostFocus -= txtSearch_LostFocus;
						withEventsField_txtSearch.KeyDown -= txtSearch_KeyDown;
						withEventsField_txtSearch.TextChanged -= txtSearch_TextChanged;
					}
					withEventsField_txtSearch = value;
					if (withEventsField_txtSearch != null) {
						withEventsField_txtSearch.GotFocus += txtSearch_GotFocus;
						withEventsField_txtSearch.LostFocus += txtSearch_LostFocus;
						withEventsField_txtSearch.KeyDown += txtSearch_KeyDown;
						withEventsField_txtSearch.TextChanged += txtSearch_TextChanged;
					}
				}
			}
			internal System.Windows.Forms.Panel pnlConnections;
			internal System.Windows.Forms.ImageList imgListTree;
			internal System.Windows.Forms.MenuStrip msMain;
			internal System.Windows.Forms.ToolStripMenuItem mMenView;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewExpandAllFolders;
			internal System.Windows.Forms.ToolStripMenuItem mMenViewExpandAllFolders {
				get { return withEventsField_mMenViewExpandAllFolders; }
				set {
					if (withEventsField_mMenViewExpandAllFolders != null) {
						withEventsField_mMenViewExpandAllFolders.Click -= mMenViewExpandAllFolders_Click;
					}
					withEventsField_mMenViewExpandAllFolders = value;
					if (withEventsField_mMenViewExpandAllFolders != null) {
						withEventsField_mMenViewExpandAllFolders.Click += mMenViewExpandAllFolders_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewCollapseAllFolders;
			internal System.Windows.Forms.ToolStripMenuItem mMenViewCollapseAllFolders {
				get { return withEventsField_mMenViewCollapseAllFolders; }
				set {
					if (withEventsField_mMenViewCollapseAllFolders != null) {
						withEventsField_mMenViewCollapseAllFolders.Click -= mMenViewCollapseAllFolders_Click;
					}
					withEventsField_mMenViewCollapseAllFolders = value;
					if (withEventsField_mMenViewCollapseAllFolders != null) {
						withEventsField_mMenViewCollapseAllFolders.Click += mMenViewCollapseAllFolders_Click;
					}
				}
			}
			private System.Windows.Forms.ContextMenuStrip withEventsField_cMenTree;
			internal System.Windows.Forms.ContextMenuStrip cMenTree {
				get { return withEventsField_cMenTree; }
				set {
					if (withEventsField_cMenTree != null) {
						withEventsField_cMenTree.Opening -= cMenTree_DropDownOpening;
					}
					withEventsField_cMenTree = value;
					if (withEventsField_cMenTree != null) {
						withEventsField_cMenTree.Opening += cMenTree_DropDownOpening;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeAddConnection;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeAddConnection {
				get { return withEventsField_cMenTreeAddConnection; }
				set {
					if (withEventsField_cMenTreeAddConnection != null) {
						withEventsField_cMenTreeAddConnection.Click -= cMenTreeAddConnection_Click;
					}
					withEventsField_cMenTreeAddConnection = value;
					if (withEventsField_cMenTreeAddConnection != null) {
						withEventsField_cMenTreeAddConnection.Click += cMenTreeAddConnection_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeAddFolder;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeAddFolder {
				get { return withEventsField_cMenTreeAddFolder; }
				set {
					if (withEventsField_cMenTreeAddFolder != null) {
						withEventsField_cMenTreeAddFolder.Click -= cMenTreeAddFolder_Click;
					}
					withEventsField_cMenTreeAddFolder = value;
					if (withEventsField_cMenTreeAddFolder != null) {
						withEventsField_cMenTreeAddFolder.Click += cMenTreeAddFolder_Click;
					}
				}
			}
			internal System.Windows.Forms.ToolStripSeparator cMenTreeSep1;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeConnect;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnect {
				get { return withEventsField_cMenTreeConnect; }
				set {
					if (withEventsField_cMenTreeConnect != null) {
						withEventsField_cMenTreeConnect.Click -= cMenTreeConnect_Click;
					}
					withEventsField_cMenTreeConnect = value;
					if (withEventsField_cMenTreeConnect != null) {
						withEventsField_cMenTreeConnect.Click += cMenTreeConnect_Click;
					}
				}
			}
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnectWithOptions;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeConnectWithOptionsConnectToConsoleSession;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnectWithOptionsConnectToConsoleSession {
				get { return withEventsField_cMenTreeConnectWithOptionsConnectToConsoleSession; }
				set {
					if (withEventsField_cMenTreeConnectWithOptionsConnectToConsoleSession != null) {
						withEventsField_cMenTreeConnectWithOptionsConnectToConsoleSession.Click -= cMenTreeConnectWithOptionsConnectToConsoleSession_Click;
					}
					withEventsField_cMenTreeConnectWithOptionsConnectToConsoleSession = value;
					if (withEventsField_cMenTreeConnectWithOptionsConnectToConsoleSession != null) {
						withEventsField_cMenTreeConnectWithOptionsConnectToConsoleSession.Click += cMenTreeConnectWithOptionsConnectToConsoleSession_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeConnectWithOptionsNoCredentials;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnectWithOptionsNoCredentials {
				get { return withEventsField_cMenTreeConnectWithOptionsNoCredentials; }
				set {
					if (withEventsField_cMenTreeConnectWithOptionsNoCredentials != null) {
						withEventsField_cMenTreeConnectWithOptionsNoCredentials.Click -= cMenTreeConnectWithOptionsNoCredentials_Click;
					}
					withEventsField_cMenTreeConnectWithOptionsNoCredentials = value;
					if (withEventsField_cMenTreeConnectWithOptionsNoCredentials != null) {
						withEventsField_cMenTreeConnectWithOptionsNoCredentials.Click += cMenTreeConnectWithOptionsNoCredentials_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeConnectWithOptionsConnectInFullscreen;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnectWithOptionsConnectInFullscreen {
				get { return withEventsField_cMenTreeConnectWithOptionsConnectInFullscreen; }
				set {
					if (withEventsField_cMenTreeConnectWithOptionsConnectInFullscreen != null) {
						withEventsField_cMenTreeConnectWithOptionsConnectInFullscreen.Click -= cMenTreeConnectWithOptionsConnectInFullscreen_Click;
					}
					withEventsField_cMenTreeConnectWithOptionsConnectInFullscreen = value;
					if (withEventsField_cMenTreeConnectWithOptionsConnectInFullscreen != null) {
						withEventsField_cMenTreeConnectWithOptionsConnectInFullscreen.Click += cMenTreeConnectWithOptionsConnectInFullscreen_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeDisconnect;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeDisconnect {
				get { return withEventsField_cMenTreeDisconnect; }
				set {
					if (withEventsField_cMenTreeDisconnect != null) {
						withEventsField_cMenTreeDisconnect.Click -= cMenTreeDisconnect_Click;
					}
					withEventsField_cMenTreeDisconnect = value;
					if (withEventsField_cMenTreeDisconnect != null) {
						withEventsField_cMenTreeDisconnect.Click += cMenTreeDisconnect_Click;
					}
				}
			}
			internal System.Windows.Forms.ToolStripSeparator cMenTreeSep2;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeToolsTransferFile;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeToolsTransferFile {
				get { return withEventsField_cMenTreeToolsTransferFile; }
				set {
					if (withEventsField_cMenTreeToolsTransferFile != null) {
						withEventsField_cMenTreeToolsTransferFile.Click -= cMenTreeToolsTransferFile_Click;
					}
					withEventsField_cMenTreeToolsTransferFile = value;
					if (withEventsField_cMenTreeToolsTransferFile != null) {
						withEventsField_cMenTreeToolsTransferFile.Click += cMenTreeToolsTransferFile_Click;
					}
				}
			}
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeToolsSort;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeToolsSortAscending;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeToolsSortAscending {
				get { return withEventsField_cMenTreeToolsSortAscending; }
				set {
					if (withEventsField_cMenTreeToolsSortAscending != null) {
						withEventsField_cMenTreeToolsSortAscending.Click -= cMenTreeToolsSortAscending_Click;
					}
					withEventsField_cMenTreeToolsSortAscending = value;
					if (withEventsField_cMenTreeToolsSortAscending != null) {
						withEventsField_cMenTreeToolsSortAscending.Click += cMenTreeToolsSortAscending_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeToolsSortDescending;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeToolsSortDescending {
				get { return withEventsField_cMenTreeToolsSortDescending; }
				set {
					if (withEventsField_cMenTreeToolsSortDescending != null) {
						withEventsField_cMenTreeToolsSortDescending.Click -= cMenTreeToolsSortDescending_Click;
					}
					withEventsField_cMenTreeToolsSortDescending = value;
					if (withEventsField_cMenTreeToolsSortDescending != null) {
						withEventsField_cMenTreeToolsSortDescending.Click += cMenTreeToolsSortDescending_Click;
					}
				}
			}
			internal System.Windows.Forms.ToolStripSeparator cMenTreeSep3;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeRename;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeRename {
				get { return withEventsField_cMenTreeRename; }
				set {
					if (withEventsField_cMenTreeRename != null) {
						withEventsField_cMenTreeRename.Click -= cMenTreeRename_Click;
					}
					withEventsField_cMenTreeRename = value;
					if (withEventsField_cMenTreeRename != null) {
						withEventsField_cMenTreeRename.Click += cMenTreeRename_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeDelete;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeDelete {
				get { return withEventsField_cMenTreeDelete; }
				set {
					if (withEventsField_cMenTreeDelete != null) {
						withEventsField_cMenTreeDelete.Click -= cMenTreeDelete_Click;
					}
					withEventsField_cMenTreeDelete = value;
					if (withEventsField_cMenTreeDelete != null) {
						withEventsField_cMenTreeDelete.Click += cMenTreeDelete_Click;
					}
				}
			}
			internal System.Windows.Forms.ToolStripSeparator cMenTreeSep4;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeMoveUp;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeMoveUp {
				get { return withEventsField_cMenTreeMoveUp; }
				set {
					if (withEventsField_cMenTreeMoveUp != null) {
						withEventsField_cMenTreeMoveUp.Click -= cMenTreeMoveUp_Click;
					}
					withEventsField_cMenTreeMoveUp = value;
					if (withEventsField_cMenTreeMoveUp != null) {
						withEventsField_cMenTreeMoveUp.Click += cMenTreeMoveUp_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeMoveDown;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeMoveDown {
				get { return withEventsField_cMenTreeMoveDown; }
				set {
					if (withEventsField_cMenTreeMoveDown != null) {
						withEventsField_cMenTreeMoveDown.Click -= cMenTreeMoveDown_Click;
					}
					withEventsField_cMenTreeMoveDown = value;
					if (withEventsField_cMenTreeMoveDown != null) {
						withEventsField_cMenTreeMoveDown.Click += cMenTreeMoveDown_Click;
					}
				}
			}
			internal System.Windows.Forms.PictureBox PictureBox1;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeToolsExternalApps;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeDuplicate;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeDuplicate {
				get { return withEventsField_cMenTreeDuplicate; }
				set {
					if (withEventsField_cMenTreeDuplicate != null) {
						withEventsField_cMenTreeDuplicate.Click -= cMenTreeDuplicate_Click;
					}
					withEventsField_cMenTreeDuplicate = value;
					if (withEventsField_cMenTreeDuplicate != null) {
						withEventsField_cMenTreeDuplicate.Click += cMenTreeDuplicate_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeConnectWithOptionsChoosePanelBeforeConnecting;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnectWithOptionsChoosePanelBeforeConnecting {
				get { return withEventsField_cMenTreeConnectWithOptionsChoosePanelBeforeConnecting; }
				set {
					if (withEventsField_cMenTreeConnectWithOptionsChoosePanelBeforeConnecting != null) {
						withEventsField_cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Click -= cMenTreeConnectWithOptionsChoosePanelBeforeConnecting_Click;
					}
					withEventsField_cMenTreeConnectWithOptionsChoosePanelBeforeConnecting = value;
					if (withEventsField_cMenTreeConnectWithOptionsChoosePanelBeforeConnecting != null) {
						withEventsField_cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Click += cMenTreeConnectWithOptionsChoosePanelBeforeConnecting_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeConnectWithOptionsDontConnectToConsoleSession;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnectWithOptionsDontConnectToConsoleSession {
				get { return withEventsField_cMenTreeConnectWithOptionsDontConnectToConsoleSession; }
				set {
					if (withEventsField_cMenTreeConnectWithOptionsDontConnectToConsoleSession != null) {
						withEventsField_cMenTreeConnectWithOptionsDontConnectToConsoleSession.Click -= cMenTreeConnectWithOptionsDontConnectToConsoleSession_Click;
					}
					withEventsField_cMenTreeConnectWithOptionsDontConnectToConsoleSession = value;
					if (withEventsField_cMenTreeConnectWithOptionsDontConnectToConsoleSession != null) {
						withEventsField_cMenTreeConnectWithOptionsDontConnectToConsoleSession.Click += cMenTreeConnectWithOptionsDontConnectToConsoleSession_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenSortAscending;
			internal System.Windows.Forms.ToolStripMenuItem mMenSortAscending {
				get { return withEventsField_mMenSortAscending; }
				set {
					if (withEventsField_mMenSortAscending != null) {
						withEventsField_mMenSortAscending.Click -= mMenSortAscending_Click;
					}
					withEventsField_mMenSortAscending = value;
					if (withEventsField_mMenSortAscending != null) {
						withEventsField_mMenSortAscending.Click += mMenSortAscending_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenAddConnection;
			internal System.Windows.Forms.ToolStripMenuItem mMenAddConnection {
				get { return withEventsField_mMenAddConnection; }
				set {
					if (withEventsField_mMenAddConnection != null) {
						withEventsField_mMenAddConnection.Click -= cMenTreeAddConnection_Click;
					}
					withEventsField_mMenAddConnection = value;
					if (withEventsField_mMenAddConnection != null) {
						withEventsField_mMenAddConnection.Click += cMenTreeAddConnection_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenAddFolder;
			internal System.Windows.Forms.ToolStripMenuItem mMenAddFolder {
				get { return withEventsField_mMenAddFolder; }
				set {
					if (withEventsField_mMenAddFolder != null) {
						withEventsField_mMenAddFolder.Click -= cMenTreeAddFolder_Click;
					}
					withEventsField_mMenAddFolder = value;
					if (withEventsField_mMenAddFolder != null) {
						withEventsField_mMenAddFolder.Click += cMenTreeAddFolder_Click;
					}
				}
			}
			private System.Windows.Forms.TreeView withEventsField_tvConnections;
			public System.Windows.Forms.TreeView tvConnections {
				get { return withEventsField_tvConnections; }
				set {
					if (withEventsField_tvConnections != null) {
						withEventsField_tvConnections.BeforeLabelEdit -= tvConnections_BeforeLabelEdit;
						withEventsField_tvConnections.AfterLabelEdit -= tvConnections_AfterLabelEdit;
						withEventsField_tvConnections.AfterSelect -= tvConnections_AfterSelect;
						withEventsField_tvConnections.NodeMouseClick -= tvConnections_NodeMouseClick;
						withEventsField_tvConnections.NodeMouseDoubleClick -= tvConnections_NodeMouseDoubleClick;
						withEventsField_tvConnections.MouseMove -= tvConnections_MouseMove;
						withEventsField_tvConnections.DragDrop -= tvConnections_DragDrop;
						withEventsField_tvConnections.DragEnter -= tvConnections_DragEnter;
						withEventsField_tvConnections.DragOver -= tvConnections_DragOver;
						withEventsField_tvConnections.ItemDrag -= tvConnections_ItemDrag;
						withEventsField_tvConnections.KeyPress -= tvConnections_KeyPress;
						withEventsField_tvConnections.KeyDown -= tvConnections_KeyDown;
					}
					withEventsField_tvConnections = value;
					if (withEventsField_tvConnections != null) {
						withEventsField_tvConnections.BeforeLabelEdit += tvConnections_BeforeLabelEdit;
						withEventsField_tvConnections.AfterLabelEdit += tvConnections_AfterLabelEdit;
						withEventsField_tvConnections.AfterSelect += tvConnections_AfterSelect;
						withEventsField_tvConnections.NodeMouseClick += tvConnections_NodeMouseClick;
						withEventsField_tvConnections.NodeMouseDoubleClick += tvConnections_NodeMouseDoubleClick;
						withEventsField_tvConnections.MouseMove += tvConnections_MouseMove;
						withEventsField_tvConnections.DragDrop += tvConnections_DragDrop;
						withEventsField_tvConnections.DragEnter += tvConnections_DragEnter;
						withEventsField_tvConnections.DragOver += tvConnections_DragOver;
						withEventsField_tvConnections.ItemDrag += tvConnections_ItemDrag;
						withEventsField_tvConnections.KeyPress += tvConnections_KeyPress;
						withEventsField_tvConnections.KeyDown += tvConnections_KeyDown;
					}
				}
			}
			private void InitializeComponent()
			{
				this.components = new System.ComponentModel.Container();
				System.Windows.Forms.TreeNode TreeNode1 = new System.Windows.Forms.TreeNode("Connections");
				this.tvConnections = new System.Windows.Forms.TreeView();
				this.cMenTree = new System.Windows.Forms.ContextMenuStrip(this.components);
				this.cMenTreeConnect = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeConnectWithOptions = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeConnectWithOptionsConnectToConsoleSession = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeConnectWithOptionsDontConnectToConsoleSession = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeConnectWithOptionsConnectInFullscreen = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeConnectWithOptionsNoCredentials = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeDisconnect = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeSep1 = new System.Windows.Forms.ToolStripSeparator();
				this.cMenTreeToolsExternalApps = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeToolsTransferFile = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeSep2 = new System.Windows.Forms.ToolStripSeparator();
				this.cMenTreeDuplicate = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeRename = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeDelete = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeSep3 = new System.Windows.Forms.ToolStripSeparator();
				this.cMenTreeImport = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeImportFile = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeImportActiveDirectory = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeImportPortScan = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeExportFile = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeSep4 = new System.Windows.Forms.ToolStripSeparator();
				this.cMenTreeAddConnection = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeAddFolder = new System.Windows.Forms.ToolStripMenuItem();
				this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
				this.cMenTreeToolsSort = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeToolsSortAscending = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeToolsSortDescending = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeMoveUp = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTreeMoveDown = new System.Windows.Forms.ToolStripMenuItem();
				this.imgListTree = new System.Windows.Forms.ImageList(this.components);
				this.pnlConnections = new System.Windows.Forms.Panel();
				this.PictureBox1 = new System.Windows.Forms.PictureBox();
				this.txtSearch = new System.Windows.Forms.TextBox();
				this.msMain = new System.Windows.Forms.MenuStrip();
				this.mMenAddConnection = new System.Windows.Forms.ToolStripMenuItem();
				this.mMenAddFolder = new System.Windows.Forms.ToolStripMenuItem();
				this.mMenView = new System.Windows.Forms.ToolStripMenuItem();
				this.mMenViewExpandAllFolders = new System.Windows.Forms.ToolStripMenuItem();
				this.mMenViewCollapseAllFolders = new System.Windows.Forms.ToolStripMenuItem();
				this.mMenSortAscending = new System.Windows.Forms.ToolStripMenuItem();
				this.cMenTree.SuspendLayout();
				this.pnlConnections.SuspendLayout();
				((System.ComponentModel.ISupportInitialize)this.PictureBox1).BeginInit();
				this.msMain.SuspendLayout();
				this.SuspendLayout();
				//
				//tvConnections
				//
				this.tvConnections.AllowDrop = true;
				this.tvConnections.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.tvConnections.BorderStyle = System.Windows.Forms.BorderStyle.None;
				this.tvConnections.ContextMenuStrip = this.cMenTree;
				this.tvConnections.HideSelection = false;
				this.tvConnections.ImageIndex = 0;
				this.tvConnections.ImageList = this.imgListTree;
				this.tvConnections.LabelEdit = true;
				this.tvConnections.Location = new System.Drawing.Point(0, 0);
				this.tvConnections.Name = "tvConnections";
				TreeNode1.Name = "nodeRoot";
				TreeNode1.Text = "Connections";
				this.tvConnections.Nodes.AddRange(new System.Windows.Forms.TreeNode[] { TreeNode1 });
				this.tvConnections.SelectedImageIndex = 0;
				this.tvConnections.Size = new System.Drawing.Size(192, 410);
				this.tvConnections.TabIndex = 20;
				//
				//cMenTree
				//
				this.cMenTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
				this.cMenTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
					this.cMenTreeConnect,
					this.cMenTreeConnectWithOptions,
					this.cMenTreeDisconnect,
					this.cMenTreeSep1,
					this.cMenTreeToolsExternalApps,
					this.cMenTreeToolsTransferFile,
					this.cMenTreeSep2,
					this.cMenTreeDuplicate,
					this.cMenTreeRename,
					this.cMenTreeDelete,
					this.cMenTreeSep3,
					this.cMenTreeImport,
					this.cMenTreeExportFile,
					this.cMenTreeSep4,
					this.cMenTreeAddConnection,
					this.cMenTreeAddFolder,
					this.ToolStripSeparator1,
					this.cMenTreeToolsSort,
					this.cMenTreeMoveUp,
					this.cMenTreeMoveDown
				});
				this.cMenTree.Name = "cMenTree";
				this.cMenTree.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
				this.cMenTree.Size = new System.Drawing.Size(187, 386);
				//
				//cMenTreeConnect
				//
				this.cMenTreeConnect.Image = global::mRemoteNG.My.Resources.Resources.Play;
				this.cMenTreeConnect.Name = "cMenTreeConnect";
				this.cMenTreeConnect.ShortcutKeys = (System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) | System.Windows.Forms.Keys.C);
				this.cMenTreeConnect.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeConnect.Text = "Connect";
				//
				//cMenTreeConnectWithOptions
				//
				this.cMenTreeConnectWithOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
					this.cMenTreeConnectWithOptionsConnectToConsoleSession,
					this.cMenTreeConnectWithOptionsDontConnectToConsoleSession,
					this.cMenTreeConnectWithOptionsConnectInFullscreen,
					this.cMenTreeConnectWithOptionsNoCredentials,
					this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting
				});
				this.cMenTreeConnectWithOptions.Name = "cMenTreeConnectWithOptions";
				this.cMenTreeConnectWithOptions.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeConnectWithOptions.Text = "Connect (with options)";
				//
				//cMenTreeConnectWithOptionsConnectToConsoleSession
				//
				this.cMenTreeConnectWithOptionsConnectToConsoleSession.Image = global::mRemoteNG.My.Resources.Resources.monitor_go;
				this.cMenTreeConnectWithOptionsConnectToConsoleSession.Name = "cMenTreeConnectWithOptionsConnectToConsoleSession";
				this.cMenTreeConnectWithOptionsConnectToConsoleSession.Size = new System.Drawing.Size(231, 22);
				this.cMenTreeConnectWithOptionsConnectToConsoleSession.Text = "Connect to console session";
				//
				//cMenTreeConnectWithOptionsDontConnectToConsoleSession
				//
				this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Image = global::mRemoteNG.My.Resources.Resources.monitor_delete;
				this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Name = "cMenTreeConnectWithOptionsDontConnectToConsoleSession";
				this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Size = new System.Drawing.Size(231, 22);
				this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Text = "Don't connect to console session";
				this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Visible = false;
				//
				//cMenTreeConnectWithOptionsConnectInFullscreen
				//
				this.cMenTreeConnectWithOptionsConnectInFullscreen.Image = global::mRemoteNG.My.Resources.Resources.arrow_out;
				this.cMenTreeConnectWithOptionsConnectInFullscreen.Name = "cMenTreeConnectWithOptionsConnectInFullscreen";
				this.cMenTreeConnectWithOptionsConnectInFullscreen.Size = new System.Drawing.Size(231, 22);
				this.cMenTreeConnectWithOptionsConnectInFullscreen.Text = "Connect in fullscreen";
				//
				//cMenTreeConnectWithOptionsNoCredentials
				//
				this.cMenTreeConnectWithOptionsNoCredentials.Image = global::mRemoteNG.My.Resources.Resources.key_delete;
				this.cMenTreeConnectWithOptionsNoCredentials.Name = "cMenTreeConnectWithOptionsNoCredentials";
				this.cMenTreeConnectWithOptionsNoCredentials.Size = new System.Drawing.Size(231, 22);
				this.cMenTreeConnectWithOptionsNoCredentials.Text = "Connect without credentials";
				//
				//cMenTreeConnectWithOptionsChoosePanelBeforeConnecting
				//
				this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Image = global::mRemoteNG.My.Resources.Resources.Panels;
				this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Name = "cMenTreeConnectWithOptionsChoosePanelBeforeConnecting";
				this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Size = new System.Drawing.Size(231, 22);
				this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Text = "Choose panel before connecting";
				//
				//cMenTreeDisconnect
				//
				this.cMenTreeDisconnect.Image = global::mRemoteNG.My.Resources.Resources.Pause;
				this.cMenTreeDisconnect.Name = "cMenTreeDisconnect";
				this.cMenTreeDisconnect.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeDisconnect.Text = "Disconnect";
				//
				//cMenTreeSep1
				//
				this.cMenTreeSep1.Name = "cMenTreeSep1";
				this.cMenTreeSep1.Size = new System.Drawing.Size(183, 6);
				//
				//cMenTreeToolsExternalApps
				//
				this.cMenTreeToolsExternalApps.Image = global::mRemoteNG.My.Resources.Resources.ExtApp;
				this.cMenTreeToolsExternalApps.Name = "cMenTreeToolsExternalApps";
				this.cMenTreeToolsExternalApps.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeToolsExternalApps.Text = "External Applications";
				//
				//cMenTreeToolsTransferFile
				//
				this.cMenTreeToolsTransferFile.Image = global::mRemoteNG.My.Resources.Resources.SSHTransfer;
				this.cMenTreeToolsTransferFile.Name = "cMenTreeToolsTransferFile";
				this.cMenTreeToolsTransferFile.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeToolsTransferFile.Text = "Transfer File (SSH)";
				//
				//cMenTreeSep2
				//
				this.cMenTreeSep2.Name = "cMenTreeSep2";
				this.cMenTreeSep2.Size = new System.Drawing.Size(183, 6);
				//
				//cMenTreeDuplicate
				//
				this.cMenTreeDuplicate.Image = global::mRemoteNG.My.Resources.Resources.page_copy;
				this.cMenTreeDuplicate.Name = "cMenTreeDuplicate";
				this.cMenTreeDuplicate.ShortcutKeys = (System.Windows.Forms.Keys)(System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D);
				this.cMenTreeDuplicate.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeDuplicate.Text = "Duplicate";
				//
				//cMenTreeRename
				//
				this.cMenTreeRename.Image = global::mRemoteNG.My.Resources.Resources.Rename;
				this.cMenTreeRename.Name = "cMenTreeRename";
				this.cMenTreeRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
				this.cMenTreeRename.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeRename.Text = "Rename";
				//
				//cMenTreeDelete
				//
				this.cMenTreeDelete.Image = global::mRemoteNG.My.Resources.Resources.Delete;
				this.cMenTreeDelete.Name = "cMenTreeDelete";
				this.cMenTreeDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
				this.cMenTreeDelete.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeDelete.Text = "Delete";
				//
				//cMenTreeSep3
				//
				this.cMenTreeSep3.Name = "cMenTreeSep3";
				this.cMenTreeSep3.Size = new System.Drawing.Size(183, 6);
				//
				//cMenTreeImport
				//
				this.cMenTreeImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
					this.cMenTreeImportFile,
					this.cMenTreeImportActiveDirectory,
					this.cMenTreeImportPortScan
				});
				this.cMenTreeImport.Name = "cMenTreeImport";
				this.cMenTreeImport.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeImport.Text = "&Import";
				//
				//cMenTreeImportFile
				//
				this.cMenTreeImportFile.Name = "cMenTreeImportFile";
				this.cMenTreeImportFile.Size = new System.Drawing.Size(213, 22);
				this.cMenTreeImportFile.Text = "Import from &File...";
				//
				//cMenTreeImportActiveDirectory
				//
				this.cMenTreeImportActiveDirectory.Name = "cMenTreeImportActiveDirectory";
				this.cMenTreeImportActiveDirectory.Size = new System.Drawing.Size(213, 22);
				this.cMenTreeImportActiveDirectory.Text = "Import from &Active Directory...";
				//
				//cMenTreeImportPortScan
				//
				this.cMenTreeImportPortScan.Name = "cMenTreeImportPortScan";
				this.cMenTreeImportPortScan.Size = new System.Drawing.Size(213, 22);
				this.cMenTreeImportPortScan.Text = "Import from &Port Scan...";
				//
				//cMenTreeExportFile
				//
				this.cMenTreeExportFile.Name = "cMenTreeExportFile";
				this.cMenTreeExportFile.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeExportFile.Text = "&Export to File...";
				//
				//cMenTreeSep4
				//
				this.cMenTreeSep4.Name = "cMenTreeSep4";
				this.cMenTreeSep4.Size = new System.Drawing.Size(183, 6);
				//
				//cMenTreeAddConnection
				//
				this.cMenTreeAddConnection.Image = global::mRemoteNG.My.Resources.Resources.Connection_Add;
				this.cMenTreeAddConnection.Name = "cMenTreeAddConnection";
				this.cMenTreeAddConnection.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeAddConnection.Text = "New Connection";
				//
				//cMenTreeAddFolder
				//
				this.cMenTreeAddFolder.Image = global::mRemoteNG.My.Resources.Resources.Folder_Add;
				this.cMenTreeAddFolder.Name = "cMenTreeAddFolder";
				this.cMenTreeAddFolder.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeAddFolder.Text = "New Folder";
				//
				//ToolStripSeparator1
				//
				this.ToolStripSeparator1.Name = "ToolStripSeparator1";
				this.ToolStripSeparator1.Size = new System.Drawing.Size(183, 6);
				//
				//cMenTreeToolsSort
				//
				this.cMenTreeToolsSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
					this.cMenTreeToolsSortAscending,
					this.cMenTreeToolsSortDescending
				});
				this.cMenTreeToolsSort.Name = "cMenTreeToolsSort";
				this.cMenTreeToolsSort.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeToolsSort.Text = "Sort";
				//
				//cMenTreeToolsSortAscending
				//
				this.cMenTreeToolsSortAscending.Image = global::mRemoteNG.My.Resources.Resources.Sort_AZ;
				this.cMenTreeToolsSortAscending.Name = "cMenTreeToolsSortAscending";
				this.cMenTreeToolsSortAscending.Size = new System.Drawing.Size(157, 22);
				this.cMenTreeToolsSortAscending.Text = "Ascending (A-Z)";
				//
				//cMenTreeToolsSortDescending
				//
				this.cMenTreeToolsSortDescending.Image = global::mRemoteNG.My.Resources.Resources.Sort_ZA;
				this.cMenTreeToolsSortDescending.Name = "cMenTreeToolsSortDescending";
				this.cMenTreeToolsSortDescending.Size = new System.Drawing.Size(157, 22);
				this.cMenTreeToolsSortDescending.Text = "Descending (Z-A)";
				//
				//cMenTreeMoveUp
				//
				this.cMenTreeMoveUp.Image = global::mRemoteNG.My.Resources.Resources.Arrow_Up;
				this.cMenTreeMoveUp.Name = "cMenTreeMoveUp";
				this.cMenTreeMoveUp.ShortcutKeys = (System.Windows.Forms.Keys)(System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up);
				this.cMenTreeMoveUp.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeMoveUp.Text = "Move up";
				//
				//cMenTreeMoveDown
				//
				this.cMenTreeMoveDown.Image = global::mRemoteNG.My.Resources.Resources.Arrow_Down;
				this.cMenTreeMoveDown.Name = "cMenTreeMoveDown";
				this.cMenTreeMoveDown.ShortcutKeys = (System.Windows.Forms.Keys)(System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down);
				this.cMenTreeMoveDown.Size = new System.Drawing.Size(186, 22);
				this.cMenTreeMoveDown.Text = "Move down";
				//
				//imgListTree
				//
				this.imgListTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
				this.imgListTree.ImageSize = new System.Drawing.Size(16, 16);
				this.imgListTree.TransparentColor = System.Drawing.Color.Transparent;
				//
				//pnlConnections
				//
				this.pnlConnections.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.pnlConnections.Controls.Add(this.PictureBox1);
				this.pnlConnections.Controls.Add(this.txtSearch);
				this.pnlConnections.Controls.Add(this.tvConnections);
				this.pnlConnections.Location = new System.Drawing.Point(0, 25);
				this.pnlConnections.Name = "pnlConnections";
				this.pnlConnections.Size = new System.Drawing.Size(192, 428);
				this.pnlConnections.TabIndex = 9;
				//
				//PictureBox1
				//
				this.PictureBox1.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
				this.PictureBox1.Image = global::mRemoteNG.My.Resources.Resources.Search;
				this.PictureBox1.Location = new System.Drawing.Point(2, 412);
				this.PictureBox1.Name = "PictureBox1";
				this.PictureBox1.Size = new System.Drawing.Size(16, 16);
				this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
				this.PictureBox1.TabIndex = 1;
				this.PictureBox1.TabStop = false;
				//
				//txtSearch
				//
				this.txtSearch.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
				this.txtSearch.ForeColor = System.Drawing.SystemColors.GrayText;
				this.txtSearch.Location = new System.Drawing.Point(19, 413);
				this.txtSearch.Name = "txtSearch";
				this.txtSearch.Size = new System.Drawing.Size(171, 13);
				this.txtSearch.TabIndex = 30;
				this.txtSearch.TabStop = false;
				this.txtSearch.Text = "Search";
				//
				//msMain
				//
				this.msMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
				this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
					this.mMenAddConnection,
					this.mMenAddFolder,
					this.mMenView,
					this.mMenSortAscending
				});
				this.msMain.Location = new System.Drawing.Point(0, 0);
				this.msMain.Name = "msMain";
				this.msMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
				this.msMain.ShowItemToolTips = true;
				this.msMain.Size = new System.Drawing.Size(192, 24);
				this.msMain.TabIndex = 10;
				this.msMain.Text = "MenuStrip1";
				//
				//mMenAddConnection
				//
				this.mMenAddConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.mMenAddConnection.Image = global::mRemoteNG.My.Resources.Resources.Connection_Add;
				this.mMenAddConnection.Name = "mMenAddConnection";
				this.mMenAddConnection.Size = new System.Drawing.Size(28, 20);
				//
				//mMenAddFolder
				//
				this.mMenAddFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.mMenAddFolder.Image = global::mRemoteNG.My.Resources.Resources.Folder_Add;
				this.mMenAddFolder.Name = "mMenAddFolder";
				this.mMenAddFolder.Size = new System.Drawing.Size(28, 20);
				//
				//mMenView
				//
				this.mMenView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.mMenView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
					this.mMenViewExpandAllFolders,
					this.mMenViewCollapseAllFolders
				});
				this.mMenView.Image = global::mRemoteNG.My.Resources.Resources.View;
				this.mMenView.Name = "mMenView";
				this.mMenView.Size = new System.Drawing.Size(28, 20);
				this.mMenView.Text = "&View";
				//
				//mMenViewExpandAllFolders
				//
				this.mMenViewExpandAllFolders.Image = global::mRemoteNG.My.Resources.Resources.Expand;
				this.mMenViewExpandAllFolders.Name = "mMenViewExpandAllFolders";
				this.mMenViewExpandAllFolders.Size = new System.Drawing.Size(161, 22);
				this.mMenViewExpandAllFolders.Text = "Expand all folders";
				//
				//mMenViewCollapseAllFolders
				//
				this.mMenViewCollapseAllFolders.Image = global::mRemoteNG.My.Resources.Resources.Collapse;
				this.mMenViewCollapseAllFolders.Name = "mMenViewCollapseAllFolders";
				this.mMenViewCollapseAllFolders.Size = new System.Drawing.Size(161, 22);
				this.mMenViewCollapseAllFolders.Text = "Collapse all folders";
				//
				//mMenSortAscending
				//
				this.mMenSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.mMenSortAscending.Image = global::mRemoteNG.My.Resources.Resources.Sort_AZ;
				this.mMenSortAscending.Name = "mMenSortAscending";
				this.mMenSortAscending.Size = new System.Drawing.Size(28, 20);
				//
				//Tree
				//
				this.ClientSize = new System.Drawing.Size(192, 453);
				this.Controls.Add(this.msMain);
				this.Controls.Add(this.pnlConnections);
				this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
				this.HideOnClose = true;
				this.Icon = global::mRemoteNG.My.Resources.Resources.Root_Icon;
				this.Name = "Tree";
				this.TabText = "Connections";
				this.Text = "Connections";
				this.cMenTree.ResumeLayout(false);
				this.pnlConnections.ResumeLayout(false);
				this.pnlConnections.PerformLayout();
				((System.ComponentModel.ISupportInitialize)this.PictureBox1).EndInit();
				this.msMain.ResumeLayout(false);
				this.msMain.PerformLayout();
				this.ResumeLayout(false);
				this.PerformLayout();

			}
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeImport;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeExportFile;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeExportFile {
				get { return withEventsField_cMenTreeExportFile; }
				set {
					if (withEventsField_cMenTreeExportFile != null) {
						withEventsField_cMenTreeExportFile.Click -= cMenTreeExportFile_Click;
					}
					withEventsField_cMenTreeExportFile = value;
					if (withEventsField_cMenTreeExportFile != null) {
						withEventsField_cMenTreeExportFile.Click += cMenTreeExportFile_Click;
					}
				}
			}
			internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeImportFile;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeImportFile {
				get { return withEventsField_cMenTreeImportFile; }
				set {
					if (withEventsField_cMenTreeImportFile != null) {
						withEventsField_cMenTreeImportFile.Click -= cMenTreeImportFile_Click;
					}
					withEventsField_cMenTreeImportFile = value;
					if (withEventsField_cMenTreeImportFile != null) {
						withEventsField_cMenTreeImportFile.Click += cMenTreeImportFile_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeImportActiveDirectory;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeImportActiveDirectory {
				get { return withEventsField_cMenTreeImportActiveDirectory; }
				set {
					if (withEventsField_cMenTreeImportActiveDirectory != null) {
						withEventsField_cMenTreeImportActiveDirectory.Click -= cMenTreeImportActiveDirectory_Click;
					}
					withEventsField_cMenTreeImportActiveDirectory = value;
					if (withEventsField_cMenTreeImportActiveDirectory != null) {
						withEventsField_cMenTreeImportActiveDirectory.Click += cMenTreeImportActiveDirectory_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenTreeImportPortScan;
			internal System.Windows.Forms.ToolStripMenuItem cMenTreeImportPortScan {
				get { return withEventsField_cMenTreeImportPortScan; }
				set {
					if (withEventsField_cMenTreeImportPortScan != null) {
						withEventsField_cMenTreeImportPortScan.Click -= cMenTreeImportPortScan_Click;
					}
					withEventsField_cMenTreeImportPortScan = value;
					if (withEventsField_cMenTreeImportPortScan != null) {
						withEventsField_cMenTreeImportPortScan.Click += cMenTreeImportPortScan_Click;
					}
				}
				#endregion
			}
		}
	}
}
