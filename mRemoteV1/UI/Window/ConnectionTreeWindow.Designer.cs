

using mRemoteNG.Connection;
using mRemoteNG.Tree;

namespace mRemoteNG.UI.Window
{
	public partial class ConnectionTreeWindow : BaseWindow
	{
        #region  Windows Form Designer generated code
		internal System.Windows.Forms.MenuStrip msMain;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewExpandAllFolders;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewCollapseAllFolders;
		internal System.Windows.Forms.ToolStripMenuItem mMenSortAscending;
		internal System.Windows.Forms.ToolStripMenuItem mMenAddConnection;
		internal System.Windows.Forms.ToolStripMenuItem mMenAddFolder;
        internal System.Windows.Forms.ToolStripMenuItem mMenOptions;
        internal System.Windows.Forms.ToolStripMenuItem mMenOptionsOpenConnFile;
        internal System.Windows.Forms.ToolStripMenuItem mMenOptionsSaveConnFile;
        internal System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        internal System.Windows.Forms.ToolStripMenuItem mMenOptionsShowHideMenu;
        internal System.Windows.Forms.ToolStripMenuItem mMenOptionsOpenOptions;
        internal System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        internal System.Windows.Forms.ToolStripMenuItem mMenOptionsExit;
        public System.Windows.Forms.TreeView tvConnections;
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            mRemoteNG.Tree.ConnectionTreeModel connectionTreeModel2 = new mRemoteNG.Tree.ConnectionTreeModel();
            mRemoteNG.Tree.TreeNodeCompositeClickHandler treeNodeCompositeClickHandler3 = new mRemoteNG.Tree.TreeNodeCompositeClickHandler();
            mRemoteNG.Tree.AlwaysConfirmYes alwaysConfirmYes2 = new mRemoteNG.Tree.AlwaysConfirmYes();
            mRemoteNG.Tree.TreeNodeCompositeClickHandler treeNodeCompositeClickHandler4 = new mRemoteNG.Tree.TreeNodeCompositeClickHandler();
            this.olvConnections = new mRemoteNG.UI.Controls.ConnectionTree();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.mMenAddConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewExpandAllFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewCollapseAllFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenSortAscending = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenOptionsOpenConnFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenOptionsSaveConnFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenOptionsShowHideMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenOptionsOpenOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenOptionsExit = new System.Windows.Forms.ToolStripMenuItem();
            this.vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.PictureBoxSearch = new mRemoteNG.UI.Controls.Base.NGPictureBox(this.components);
            this.txtSearch = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mMenFavorites = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.olvConnections)).BeginInit();
            this.msMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxSearch)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // olvConnections
            // 
            this.olvConnections.AllowDrop = true;
            this.olvConnections.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.olvConnections.CellEditUseWholeCell = false;
            this.olvConnections.ConnectionTreeModel = connectionTreeModel2;
            this.olvConnections.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            treeNodeCompositeClickHandler3.ClickHandlers = new mRemoteNG.Tree.ITreeNodeClickHandler<mRemoteNG.Connection.ConnectionInfo>[0];
            this.olvConnections.DoubleClickHandler = treeNodeCompositeClickHandler3;
            this.olvConnections.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvConnections.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.olvConnections.HideSelection = false;
            this.olvConnections.IsSimpleDragSource = true;
            this.olvConnections.IsSimpleDropSink = true;
            this.olvConnections.LabelEdit = true;
            this.olvConnections.Location = new System.Drawing.Point(0, 24);
            this.olvConnections.MultiSelect = false;
            this.olvConnections.Name = "olvConnections";
            this.olvConnections.NodeDeletionConfirmer = alwaysConfirmYes2;
            this.olvConnections.PostSetupActions = new mRemoteNG.UI.Controls.IConnectionTreeDelegate[0];
            this.olvConnections.SelectedBackColor = System.Drawing.SystemColors.Highlight;
            this.olvConnections.SelectedForeColor = System.Drawing.SystemColors.HighlightText;
            this.olvConnections.ShowGroups = false;
            treeNodeCompositeClickHandler4.ClickHandlers = new mRemoteNG.Tree.ITreeNodeClickHandler<mRemoteNG.Connection.ConnectionInfo>[0];
            this.olvConnections.SingleClickHandler = treeNodeCompositeClickHandler4;
            this.olvConnections.Size = new System.Drawing.Size(204, 366);
            this.olvConnections.TabIndex = 20;
            this.olvConnections.UnfocusedSelectedBackColor = System.Drawing.SystemColors.Highlight;
            this.olvConnections.UnfocusedSelectedForeColor = System.Drawing.SystemColors.HighlightText;
            this.olvConnections.UseCompatibleStateImageBehavior = false;
            this.olvConnections.UseOverlays = false;
            this.olvConnections.View = System.Windows.Forms.View.Details;
            this.olvConnections.VirtualMode = true;
            // 
            // msMain
            // 
            this.msMain.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenAddConnection,
            this.mMenAddFolder,
            this.mMenViewExpandAllFolders,
            this.mMenViewCollapseAllFolders,
            this.mMenSortAscending,
            this.mMenFavorites,
            this.mMenOptions});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.msMain.ShowItemToolTips = true;
            this.msMain.Size = new System.Drawing.Size(204, 24);
            this.msMain.TabIndex = 10;
            this.msMain.Text = "MenuStrip1";
            // 
            // mMenAddConnection
            // 
            this.mMenAddConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenAddConnection.Image = global::mRemoteNG.Resources.Connection_Add;
            this.mMenAddConnection.Name = "mMenAddConnection";
            this.mMenAddConnection.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.mMenAddConnection.Size = new System.Drawing.Size(24, 20);
            this.mMenAddConnection.Click += new System.EventHandler(this.cMenTreeAddConnection_Click);
            // 
            // mMenAddFolder
            // 
            this.mMenAddFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenAddFolder.Image = global::mRemoteNG.Resources.Folder_Add;
            this.mMenAddFolder.Name = "mMenAddFolder";
            this.mMenAddFolder.Size = new System.Drawing.Size(28, 20);
            this.mMenAddFolder.Click += new System.EventHandler(this.cMenTreeAddFolder_Click);
            // 
            // mMenViewExpandAllFolders
            // 
            this.mMenViewExpandAllFolders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenViewExpandAllFolders.Image = global::mRemoteNG.Resources.Expand;
            this.mMenViewExpandAllFolders.Name = "mMenViewExpandAllFolders";
            this.mMenViewExpandAllFolders.Size = new System.Drawing.Size(28, 20);
            this.mMenViewExpandAllFolders.Text = "Expand all folders";
            // 
            // mMenViewCollapseAllFolders
            // 
            this.mMenViewCollapseAllFolders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenViewCollapseAllFolders.Image = global::mRemoteNG.Resources.Collapse;
            this.mMenViewCollapseAllFolders.Name = "mMenViewCollapseAllFolders";
            this.mMenViewCollapseAllFolders.Size = new System.Drawing.Size(28, 20);
            this.mMenViewCollapseAllFolders.Text = "Collapse all folders";
            // 
            // mMenSortAscending
            // 
            this.mMenSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenSortAscending.Image = global::mRemoteNG.Resources.Sort_AZ;
            this.mMenSortAscending.Name = "mMenSortAscending";
            this.mMenSortAscending.Size = new System.Drawing.Size(28, 20);
            //
            // mMenOptions
            // 
            this.mMenOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenOptionsOpenConnFile,
            this.mMenOptionsSaveConnFile,
            this.toolStripSeparator1,
            this.mMenOptionsShowHideMenu,
            this.mMenOptionsOpenOptions,
            this.toolStripSeparator2,
            this.mMenOptionsExit});
            this.mMenOptions.Image = global::mRemoteNG.Resources.cog;
            this.mMenOptions.Name = "mMenOptions";
            this.mMenOptions.Size = new System.Drawing.Size(28, 20);
            // 
            // mMenOptionsOpenConnFile
            // 
            this.mMenOptionsOpenConnFile.Image = global::mRemoteNG.Resources.Connections_Load;
            this.mMenOptionsOpenConnFile.Name = "mMenOptionsOpenConnFile";
            this.mMenOptionsOpenConnFile.Size = new System.Drawing.Size(196, 22);
            this.mMenOptionsOpenConnFile.Text = "Open Connection File...";
            this.mMenOptionsOpenConnFile.Click += new System.EventHandler(this.mMenOptionsOpenConnFile_Click);
            // 
            // mMenOptionsSaveConnFile
            // 
            this.mMenOptionsSaveConnFile.Image = global::mRemoteNG.Resources.Connections_Save;
            this.mMenOptionsSaveConnFile.Name = "mMenOptionsSaveConnFile";
            this.mMenOptionsSaveConnFile.Size = new System.Drawing.Size(196, 22);
            this.mMenOptionsSaveConnFile.Text = "Save Connection File";
            this.mMenOptionsSaveConnFile.Click += new System.EventHandler(this.mMenOptionsSaveConnFile_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // mMenOptionsShowHideMenu
            // 
            this.mMenOptionsShowHideMenu.Image = global::mRemoteNG.Resources.application_side_tree;
            this.mMenOptionsShowHideMenu.Name = "mMenOptionsShowHideMenu";
            this.mMenOptionsShowHideMenu.Size = new System.Drawing.Size(196, 22);
            this.mMenOptionsShowHideMenu.Text = "Show/Hide Main Menu";
            this.mMenOptionsShowHideMenu.Click += new System.EventHandler(this.mMenOptionsShowHideMenu_Click);
            // 
            // mMenOptionsOpenOptions
            // 
            this.mMenOptionsOpenOptions.Image = global::mRemoteNG.Resources.Options;
            this.mMenOptionsOpenOptions.Name = "mMenOptionsOpenOptions";
            this.mMenOptionsOpenOptions.Size = new System.Drawing.Size(196, 22);
            this.mMenOptionsOpenOptions.Text = "Options";
            this.mMenOptionsOpenOptions.Click += new System.EventHandler(this.mMenOptionsOpenOptions_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(193, 6);
            // 
            // mMenOptionsExit
            // 
            this.mMenOptionsExit.Image = global::mRemoteNG.Resources.Quit;
            this.mMenOptionsExit.Name = "mMenOptionsExit";
            this.mMenOptionsExit.Size = new System.Drawing.Size(196, 22);
            this.mMenOptionsExit.Text = "Exit";
            this.mMenOptionsExit.Click += new System.EventHandler(this.mMenOptionsExit_Click);
            // 
            // vsToolStripExtender
            // 
            this.vsToolStripExtender.DefaultRenderer = null;
            // 
            // PictureBoxSearch
            // 
            this.PictureBoxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBoxSearch.Image = global::mRemoteNG.Resources.Search;
            this.PictureBoxSearch.Location = new System.Drawing.Point(0, 0);
            this.PictureBoxSearch.Margin = new System.Windows.Forms.Padding(0);
            this.PictureBoxSearch.Name = "PictureBoxSearch";
            this.PictureBoxSearch.Size = new System.Drawing.Size(26, 21);
            this.PictureBoxSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PictureBoxSearch.TabIndex = 1;
            this.PictureBoxSearch.TabStop = false;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtSearch.Location = new System.Drawing.Point(26, 3);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(178, 15);
            this.txtSearch.TabIndex = 30;
            this.txtSearch.TabStop = false;
            this.txtSearch.Text = "Search";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.GotFocus += new System.EventHandler(this.txtSearch_GotFocus);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtSearch.LostFocus += new System.EventHandler(this.txtSearch_LostFocus);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.PictureBoxSearch, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSearch);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 390);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(204, 21);
            this.tableLayoutPanel1.TabIndex = 32;
            // 
            // mMenFavorites
            // 
            this.mMenFavorites.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenFavorites.Image = global::mRemoteNG.Resources.star;
            this.mMenFavorites.Name = "mMenFavorites";
            this.mMenFavorites.Size = new System.Drawing.Size(28, 20);
            this.mMenFavorites.Text = "Favorites";
            // 
            // ConnectionTreeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(204, 411);
            this.Controls.Add(this.olvConnections);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.msMain);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Icon = global::mRemoteNG.Resources.Root_Icon;
            this.Name = "ConnectionTreeWindow";
            this.TabText = "Connections";
            this.Text = "Connections";
            this.Load += new System.EventHandler(this.Tree_Load);
            ((System.ComponentModel.ISupportInitialize)(this.olvConnections)).EndInit();
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxSearch)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion

        private System.ComponentModel.IContainer components;
        private Controls.ConnectionTree olvConnections;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
        internal Controls.Base.NGPictureBox PictureBoxSearch;
        internal Controls.Base.NGTextBox txtSearch;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal System.Windows.Forms.ToolStripMenuItem mMenFavorites;
    }
}
