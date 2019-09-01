namespace mRemoteNG.UI.Window
{
    public partial class ConnectionTreeWindow : BaseWindow
	{
        #region  Windows Form Designer generated code
		internal System.Windows.Forms.MenuStrip msMain;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewExpandAllFolders;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewCollapseAllFolders;
		internal System.Windows.Forms.ToolStripMenuItem mMenSort;
		internal System.Windows.Forms.ToolStripMenuItem mMenAddConnection;
		internal System.Windows.Forms.ToolStripMenuItem mMenAddFolder;
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
            this.mMenSort = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFavorites = new System.Windows.Forms.ToolStripMenuItem();
            this.vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.pbSearch = new mRemoteNG.UI.Controls.Base.NGPictureBox(this.components);
            this.txtSearch = new mRemoteNG.UI.Controls.Base.NGSearchBox();
            this.searchBoxLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.olvConnections)).BeginInit();
            this.msMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSearch)).BeginInit();
            this.searchBoxLayoutPanel.SuspendLayout();
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
            this.mMenSort,
            this.mMenFavorites});
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
            this.mMenSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenSort.Image = global::mRemoteNG.Resources.Sort_AZ;
            this.mMenSort.Name = "mMenSort";
            this.mMenSort.Size = new System.Drawing.Size(28, 20);
            // 
            // mMenFavorites
            // 
            this.mMenFavorites.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenFavorites.Image = global::mRemoteNG.Resources.star;
            this.mMenFavorites.Name = "mMenFavorites";
            this.mMenFavorites.Size = new System.Drawing.Size(28, 20);
            this.mMenFavorites.Text = "Favorites";
            // 
            // vsToolStripExtender
            // 
            this.vsToolStripExtender.DefaultRenderer = null;
            // 
            // pbSearch
            // 
            this.pbSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbSearch.Image = global::mRemoteNG.Resources.Search;
            this.pbSearch.Location = new System.Drawing.Point(0, 0);
            this.pbSearch.Margin = new System.Windows.Forms.Padding(0);
            this.pbSearch.Name = "pbSearch";
            this.pbSearch.Size = new System.Drawing.Size(26, 21);
            this.pbSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbSearch.TabIndex = 1;
            this.pbSearch.TabStop = false;
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
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // tableLayoutPanel1
            // 
            this.searchBoxLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.searchBoxLayoutPanel.ColumnCount = 2;
            this.searchBoxLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.searchBoxLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.searchBoxLayoutPanel.Controls.Add(this.pbSearch, 0, 0);
            this.searchBoxLayoutPanel.Controls.Add(this.txtSearch);
            this.searchBoxLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.searchBoxLayoutPanel.Location = new System.Drawing.Point(0, 390);
            this.searchBoxLayoutPanel.Name = "searchBoxLayoutPanel";
            this.searchBoxLayoutPanel.RowCount = 1;
            this.searchBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.searchBoxLayoutPanel.Size = new System.Drawing.Size(204, 21);
            this.searchBoxLayoutPanel.TabIndex = 32;
            // 
            // ConnectionTreeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(204, 411);
            this.Controls.Add(this.olvConnections);
            this.Controls.Add(this.searchBoxLayoutPanel);
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
            ((System.ComponentModel.ISupportInitialize)(this.pbSearch)).EndInit();
            this.searchBoxLayoutPanel.ResumeLayout(false);
            this.searchBoxLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion

        private System.ComponentModel.IContainer components;
        private Controls.ConnectionTree olvConnections;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
        internal Controls.Base.NGPictureBox pbSearch;
        internal Controls.Base.NGSearchBox txtSearch;
        public System.Windows.Forms.TableLayoutPanel searchBoxLayoutPanel;
        internal System.Windows.Forms.ToolStripMenuItem mMenFavorites;
    }
}
