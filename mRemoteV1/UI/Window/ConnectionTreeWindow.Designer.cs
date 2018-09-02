

using mRemoteNG.Connection;
using mRemoteNG.Tree;

namespace mRemoteNG.UI.Window
{
	public partial class ConnectionTreeWindow : BaseWindow
	{
        #region  Windows Form Designer generated code
		internal System.Windows.Forms.MenuStrip msMain;
		internal System.Windows.Forms.ToolStripMenuItem mMenView;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewExpandAllFolders;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewCollapseAllFolders;
		internal System.Windows.Forms.ToolStripMenuItem mMenSortAscending;
		internal System.Windows.Forms.ToolStripMenuItem mMenAddConnection;
		internal System.Windows.Forms.ToolStripMenuItem mMenAddFolder;
		public System.Windows.Forms.TreeView tvConnections;
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            mRemoteNG.Tree.ConnectionTreeModel connectionTreeModel1 = new mRemoteNG.Tree.ConnectionTreeModel();
            mRemoteNG.Tree.TreeNodeCompositeClickHandler treeNodeCompositeClickHandler1 = new mRemoteNG.Tree.TreeNodeCompositeClickHandler();
            mRemoteNG.Tree.AlwaysConfirmYes alwaysConfirmYes1 = new mRemoteNG.Tree.AlwaysConfirmYes();
            mRemoteNG.Tree.TreeNodeCompositeClickHandler treeNodeCompositeClickHandler2 = new mRemoteNG.Tree.TreeNodeCompositeClickHandler();
            this.olvConnections = new mRemoteNG.UI.Controls.ConnectionTree();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.mMenAddConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenView = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewExpandAllFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewCollapseAllFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenSortAscending = new System.Windows.Forms.ToolStripMenuItem();
            this.vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtSearch = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.olvConnections)).BeginInit();
            this.msMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // olvConnections
            // 
            this.olvConnections.AllowDrop = true;
            this.olvConnections.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.olvConnections.CellEditUseWholeCell = false;
            this.tableLayoutPanel1.SetColumnSpan(this.olvConnections, 2);
            this.olvConnections.ConnectionTreeModel = connectionTreeModel1;
            this.olvConnections.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            treeNodeCompositeClickHandler1.ClickHandlers = new mRemoteNG.Tree.ITreeNodeClickHandler<mRemoteNG.Connection.ConnectionInfo>[0];
            this.olvConnections.DoubleClickHandler = treeNodeCompositeClickHandler1;
            this.olvConnections.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.olvConnections.HideSelection = false;
            this.olvConnections.IsSimpleDragSource = true;
            this.olvConnections.IsSimpleDropSink = true;
            this.olvConnections.LabelEdit = true;
            this.olvConnections.Location = new System.Drawing.Point(3, 3);
            this.olvConnections.MultiSelect = false;
            this.olvConnections.Name = "olvConnections";
            this.olvConnections.NodeDeletionConfirmer = alwaysConfirmYes1;
            this.olvConnections.PostSetupActions = new mRemoteNG.UI.Controls.IConnectionTreeDelegate[0];
            this.olvConnections.SelectedBackColor = System.Drawing.SystemColors.Highlight;
            this.olvConnections.SelectedForeColor = System.Drawing.SystemColors.HighlightText;
            this.olvConnections.ShowGroups = false;
            treeNodeCompositeClickHandler2.ClickHandlers = new mRemoteNG.Tree.ITreeNodeClickHandler<mRemoteNG.Connection.ConnectionInfo>[0];
            this.olvConnections.SingleClickHandler = treeNodeCompositeClickHandler2;
            this.olvConnections.Size = new System.Drawing.Size(381, 365);
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
            this.mMenView,
            this.mMenSortAscending});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
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
            this.mMenAddConnection.Size = new System.Drawing.Size(28, 20);
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
            // mMenView
            // 
            this.mMenView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenViewExpandAllFolders,
            this.mMenViewCollapseAllFolders});
            this.mMenView.Image = global::mRemoteNG.Resources.View;
            this.mMenView.Name = "mMenView";
            this.mMenView.Size = new System.Drawing.Size(28, 20);
            this.mMenView.Text = "&View";
            // 
            // mMenViewExpandAllFolders
            // 
            this.mMenViewExpandAllFolders.Image = global::mRemoteNG.Resources.Expand;
            this.mMenViewExpandAllFolders.Name = "mMenViewExpandAllFolders";
            this.mMenViewExpandAllFolders.Size = new System.Drawing.Size(172, 22);
            this.mMenViewExpandAllFolders.Text = "Expand all folders";
            // 
            // mMenViewCollapseAllFolders
            // 
            this.mMenViewCollapseAllFolders.Image = global::mRemoteNG.Resources.Collapse;
            this.mMenViewCollapseAllFolders.Name = "mMenViewCollapseAllFolders";
            this.mMenViewCollapseAllFolders.Size = new System.Drawing.Size(172, 22);
            this.mMenViewCollapseAllFolders.Text = "Collapse all folders";
            // 
            // mMenSortAscending
            // 
            this.mMenSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenSortAscending.Image = global::mRemoteNG.Resources.Sort_AZ;
            this.mMenSortAscending.Name = "mMenSortAscending";
            this.mMenSortAscending.Size = new System.Drawing.Size(28, 20);
            // 
            // vsToolStripExtender
            // 
            this.vsToolStripExtender.DefaultRenderer = null;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.olvConnections, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.PictureBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtSearch, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(204, 387);
            this.tableLayoutPanel1.TabIndex = 31;
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtSearch.Location = new System.Drawing.Point(16, 371);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(371, 15);
            this.txtSearch.TabIndex = 30;
            this.txtSearch.TabStop = false;
            this.txtSearch.Text = "Search";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.GotFocus += new System.EventHandler(this.txtSearch_GotFocus);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtSearch.LostFocus += new System.EventHandler(this.txtSearch_LostFocus);
            // 
            // PictureBox1
            // 
            this.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBox1.Image = global::mRemoteNG.Resources.Search;
            this.PictureBox1.Location = new System.Drawing.Point(0, 371);
            this.PictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(16, 16);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureBox1.TabIndex = 1;
            this.PictureBox1.TabStop = false;
            // 
            // ConnectionTreeWindow
            // 
            this.ClientSize = new System.Drawing.Size(204, 411);
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
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion

        private System.ComponentModel.IContainer components;
        private Controls.ConnectionTree olvConnections;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal System.Windows.Forms.PictureBox PictureBox1;
        internal Controls.Base.NGTextBox txtSearch;
    }
}
