

using mRemoteNG.Connection;
using mRemoteNG.Tree;

namespace mRemoteNG.UI.Window
{
	public partial class ConnectionTreeWindow : BaseWindow
	{
        #region  Windows Form Designer generated code
		internal Controls.Base.NGTextBox txtSearch;
		internal System.Windows.Forms.Panel pnlConnections;
		internal System.Windows.Forms.MenuStrip msMain;
		internal System.Windows.Forms.ToolStripMenuItem mMenView;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewExpandAllFolders;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewCollapseAllFolders;
		internal System.Windows.Forms.PictureBox PictureBox1;
		internal System.Windows.Forms.ToolStripMenuItem mMenSortAscending;
		internal System.Windows.Forms.ToolStripMenuItem mMenAddConnection;
		internal System.Windows.Forms.ToolStripMenuItem mMenAddFolder;
		public System.Windows.Forms.TreeView tvConnections;
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            mRemoteNG.Tree.TreeNodeCompositeClickHandler treeNodeCompositeClickHandler1 = new mRemoteNG.Tree.TreeNodeCompositeClickHandler();
            mRemoteNG.Tree.AlwaysConfirmYes alwaysConfirmYes1 = new mRemoteNG.Tree.AlwaysConfirmYes();
            mRemoteNG.Tree.TreeNodeCompositeClickHandler treeNodeCompositeClickHandler2 = new mRemoteNG.Tree.TreeNodeCompositeClickHandler();
            this.olvConnections = new mRemoteNG.UI.Controls.ConnectionTree();
            this.pnlConnections = new System.Windows.Forms.Panel();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtSearch = new Controls.Base.NGTextBox();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.mMenAddConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenView = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewExpandAllFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewCollapseAllFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenSortAscending = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.olvConnections)).BeginInit();
            this.pnlConnections.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.msMain.SuspendLayout();
            this.SuspendLayout();
            //
            //Theming support
            //
            this.vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            // 
            // olvConnections
            // 
            this.olvConnections.AllowDrop = true;
            this.olvConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvConnections.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.olvConnections.CellEditUseWholeCell = false;
            this.olvConnections.ConnectionTreeModel = new ConnectionTreeModel();
            this.olvConnections.Cursor = System.Windows.Forms.Cursors.Default;
            treeNodeCompositeClickHandler1.ClickHandlers = new ITreeNodeClickHandler<ConnectionInfo>[0];
            this.olvConnections.DoubleClickHandler = treeNodeCompositeClickHandler1;
            this.olvConnections.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.olvConnections.HideSelection = false;
            this.olvConnections.IsSimpleDragSource = true;
            this.olvConnections.LabelEdit = true;
            this.olvConnections.Location = new System.Drawing.Point(0, 0);
            this.olvConnections.MultiSelect = false;
            this.olvConnections.Name = "olvConnections";
            this.olvConnections.NodeDeletionConfirmer = alwaysConfirmYes1;
            this.olvConnections.PostSetupActions = new mRemoteNG.UI.Controls.IConnectionTreeDelegate[0];
            this.olvConnections.SelectedBackColor = System.Drawing.SystemColors.Highlight;
            this.olvConnections.SelectedForeColor = System.Drawing.SystemColors.HighlightText;
            this.olvConnections.ShowGroups = false;
            treeNodeCompositeClickHandler2.ClickHandlers = new mRemoteNG.Tree.ITreeNodeClickHandler<ConnectionInfo>[0];
            this.olvConnections.SingleClickHandler = treeNodeCompositeClickHandler2;
            this.olvConnections.Size = new System.Drawing.Size(192, 410);
            this.olvConnections.TabIndex = 20;
            this.olvConnections.UnfocusedSelectedBackColor = System.Drawing.SystemColors.Highlight;
            this.olvConnections.UnfocusedSelectedForeColor = System.Drawing.SystemColors.HighlightText;
            this.olvConnections.UseCompatibleStateImageBehavior = false;
            this.olvConnections.UseOverlays = false;
            this.olvConnections.View = System.Windows.Forms.View.Details;
            this.olvConnections.VirtualMode = true;
            // 
            // pnlConnections
            // 
            this.pnlConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConnections.Controls.Add(this.PictureBox1);
            this.pnlConnections.Controls.Add(this.txtSearch);
            this.pnlConnections.Controls.Add(this.olvConnections);
            this.pnlConnections.Location = new System.Drawing.Point(0, 25);
            this.pnlConnections.Name = "pnlConnections";
            this.pnlConnections.Size = new System.Drawing.Size(192, 428);
            this.pnlConnections.TabIndex = 9;
            // 
            // PictureBox1
            // 
            this.PictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PictureBox1.Image = global::mRemoteNG.Resources.Search;
            this.PictureBox1.Location = new System.Drawing.Point(2, 412);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(16, 16);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureBox1.TabIndex = 1;
            this.PictureBox1.TabStop = false;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSearch.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtSearch.Location = new System.Drawing.Point(19, 413);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(171, 15);
            this.txtSearch.TabIndex = 30;
            this.txtSearch.TabStop = false;
            this.txtSearch.Text = "Search";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.GotFocus += new System.EventHandler(this.txtSearch_GotFocus);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtSearch.LostFocus += new System.EventHandler(this.txtSearch_LostFocus);
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
            this.msMain.Size = new System.Drawing.Size(192, 24);
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
            // ConnectionTreeWindow
            // 
            this.ClientSize = new System.Drawing.Size(192, 453);
            this.Controls.Add(this.msMain);
            this.Controls.Add(this.pnlConnections);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Icon = global::mRemoteNG.Resources.Root_Icon;
            this.Name = "ConnectionTreeWindow";
            this.TabText = "Connections";
            this.Text = "Connections";
            this.Load += new System.EventHandler(this.Tree_Load);
            ((System.ComponentModel.ISupportInitialize)(this.olvConnections)).EndInit();
            this.pnlConnections.ResumeLayout(false);
            this.pnlConnections.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion

        private System.ComponentModel.IContainer components;
        private Controls.ConnectionTree olvConnections;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
    }
}
