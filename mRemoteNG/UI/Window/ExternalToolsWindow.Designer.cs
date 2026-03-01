
using mRemoteNG.Themes;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Window
{
	public partial class ExternalToolsWindow : BaseWindow
	{
        #region  Windows Form Designer generated code
		internal BrightIdeasSoftware.OLVColumn FilenameColumnHeader;
        internal BrightIdeasSoftware.OLVColumn DisplayNameColumnHeader;
        internal BrightIdeasSoftware.OLVColumn ArgumentsColumnHeader;
        internal BrightIdeasSoftware.OLVColumn WaitForExitColumnHeader;
        internal BrightIdeasSoftware.OLVColumn TryToIntegrateColumnHeader;
	    internal BrightIdeasSoftware.OLVColumn WorkingDirColumnHeader;
	    internal BrightIdeasSoftware.OLVColumn RunElevateHeader;
		internal Controls.MrngTextBox DisplayNameTextBox;
        internal BrightIdeasSoftware.OLVColumn ShowOnToolbarColumnHeader;
        internal BrightIdeasSoftware.OLVColumn CategoryColumnHeader;
        internal BrightIdeasSoftware.OLVColumn HiddenColumnHeader;
		internal Controls.MrngLabel DisplayNameLabel;
		internal Controls.MrngTextBox ArgumentsCheckBox;
		internal Controls.MrngTextBox FilenameTextBox;
		internal Controls.MrngLabel ArgumentsLabel;
		internal Controls.MrngLabel FilenameLabel;
		internal Controls.MrngLabel IconPathLabel;
		internal Controls.MrngTextBox IconPathTextBox;
		internal MrngButton BrowseIconButton; 
		internal MrngButton BrowseButton; 
        internal MrngButton VariablesButton;
		internal System.Windows.Forms.ContextMenuStrip ToolsContextMenuStrip;
		internal System.Windows.Forms.ToolStripMenuItem NewToolMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem DeleteToolMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
		internal System.Windows.Forms.ToolStripMenuItem LaunchToolMenuItem; 
		internal MrngCheckBox WaitForExitCheckBox;
		internal Controls.MrngLabel OptionsLabel;
		internal MrngCheckBox TryToIntegrateCheckBox;
        internal MrngCheckBox ShowOnToolbarCheckBox;
        internal Controls.MrngListView ToolsListObjView;
	    internal Controls.MrngLabel WorkingDirLabel;
	    internal Controls.MrngTextBox WorkingDirTextBox;
	    internal MrngButton BrowseWorkingDir;
	    internal MrngCheckBox RunElevatedCheckBox;
	    internal MrngCheckBox HiddenCheckBox;
	    internal Controls.MrngLabel CategoryLabel;
	    internal Controls.MrngTextBox CategoryTextBox;
	    internal Controls.MrngLabel HotkeyLabel;
	    internal Controls.MrngTextBox HotkeyTextBox;
        internal Controls.MrngLabel AuthenticationTypeLabel;
        internal Controls.MrngTextBox AuthenticationTypeTextBox;
        internal Controls.MrngLabel AuthenticationUsernameLabel;
        internal Controls.MrngTextBox AuthenticationUsernameTextBox;
        internal Controls.MrngLabel AuthenticationPasswordLabel;
        internal Controls.MrngTextBox AuthenticationPasswordTextBox;
        internal Controls.MrngLabel PrivateKeyFileLabel;
        internal Controls.MrngTextBox PrivateKeyFileTextBox;
        internal MrngButton BrowsePrivateKeyButton;
        internal Controls.MrngLabel PassphraseLabel;
        internal Controls.MrngTextBox PassphraseTextBox;

        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExternalToolsWindow));
            this.ToolsListObjView = new mRemoteNG.UI.Controls.MrngListView();
            this.DisplayNameColumnHeader = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.FilenameColumnHeader = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ArgumentsColumnHeader = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.WorkingDirColumnHeader = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.WaitForExitColumnHeader = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.TryToIntegrateColumnHeader = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.RunElevateHeader = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ShowOnToolbarColumnHeader = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.CategoryColumnHeader = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.HiddenColumnHeader = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ToolsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.NewToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.LaunchToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PropertiesGroupBox = new MrngGroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.DisplayNameLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.ShowOnToolbarCheckBox = new MrngCheckBox();
            this.WorkingDirTextBox = new mRemoteNG.UI.Controls.MrngTextBox();
            this.DisplayNameTextBox = new mRemoteNG.UI.Controls.MrngTextBox();
            this.FilenameTextBox = new mRemoteNG.UI.Controls.MrngTextBox();
            this.ArgumentsCheckBox = new mRemoteNG.UI.Controls.MrngTextBox();
            this.FilenameLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.RunElevatedCheckBox = new MrngCheckBox();
            this.HiddenCheckBox = new MrngCheckBox();
            this.ArgumentsLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.TryToIntegrateCheckBox = new MrngCheckBox();
            this.WorkingDirLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.OptionsLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.WaitForExitCheckBox = new MrngCheckBox();
            this.IconPathLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.IconPathTextBox = new mRemoteNG.UI.Controls.MrngTextBox();
            this.BrowseIconButton = new MrngButton();
            this.BrowseButton = new MrngButton();
            this.VariablesButton = new MrngButton();
            this.BrowseWorkingDir = new MrngButton();
            this.HotkeyLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.HotkeyTextBox = new mRemoteNG.UI.Controls.MrngTextBox();
            this.AuthenticationTypeLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.AuthenticationTypeTextBox = new mRemoteNG.UI.Controls.MrngTextBox();
            this.AuthenticationUsernameLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.AuthenticationUsernameTextBox = new mRemoteNG.UI.Controls.MrngTextBox();
            this.AuthenticationPasswordLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.AuthenticationPasswordTextBox = new mRemoteNG.UI.Controls.MrngTextBox();
            this.PrivateKeyFileLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.PrivateKeyFileTextBox = new mRemoteNG.UI.Controls.MrngTextBox();
            this.BrowsePrivateKeyButton = new MrngButton();
            this.PassphraseLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.PassphraseTextBox = new mRemoteNG.UI.Controls.MrngTextBox();
            this.ToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.NewToolToolstripButton = new System.Windows.Forms.ToolStripButton();
            this.DeleteToolToolstripButton = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.LaunchToolToolstripButton = new System.Windows.Forms.ToolStripButton();
            this.vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ToolsListObjView)).BeginInit();
            this.ToolsContextMenuStrip.SuspendLayout();
            this.PropertiesGroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.ToolStripContainer.ContentPanel.SuspendLayout();
            this.ToolStripContainer.TopToolStripPanel.SuspendLayout();
            this.ToolStripContainer.SuspendLayout();
            this.ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolsListObjView
            // 
            this.ToolsListObjView.AllColumns.Add(this.DisplayNameColumnHeader);
            this.ToolsListObjView.AllColumns.Add(this.FilenameColumnHeader);
            this.ToolsListObjView.AllColumns.Add(this.ArgumentsColumnHeader);
            this.ToolsListObjView.AllColumns.Add(this.WorkingDirColumnHeader);
            this.ToolsListObjView.AllColumns.Add(this.WaitForExitColumnHeader);
            this.ToolsListObjView.AllColumns.Add(this.TryToIntegrateColumnHeader);
            this.ToolsListObjView.AllColumns.Add(this.RunElevateHeader);
            this.ToolsListObjView.AllColumns.Add(this.ShowOnToolbarColumnHeader);
            this.ToolsListObjView.AllColumns.Add(this.CategoryColumnHeader);
            this.ToolsListObjView.AllColumns.Add(this.HiddenColumnHeader);
            this.ToolsListObjView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ToolsListObjView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ToolsListObjView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.F2Only;
            this.ToolsListObjView.CellEditUseWholeCell = false;
            this.ToolsListObjView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DisplayNameColumnHeader,
            this.FilenameColumnHeader,
            this.ArgumentsColumnHeader,
            this.WorkingDirColumnHeader,
            this.WaitForExitColumnHeader,
            this.TryToIntegrateColumnHeader,
            this.RunElevateHeader,
            this.ShowOnToolbarColumnHeader,
            this.CategoryColumnHeader,
            this.HiddenColumnHeader});
            this.ToolsListObjView.ContextMenuStrip = this.ToolsContextMenuStrip;
            this.ToolsListObjView.Cursor = System.Windows.Forms.Cursors.Default;
            this.ToolsListObjView.DecorateLines = true;
            this.ToolsListObjView.FullRowSelect = true;
            this.ToolsListObjView.GridLines = true;
            this.ToolsListObjView.HideSelection = false;
            this.ToolsListObjView.Location = new System.Drawing.Point(0, 0);
            this.ToolsListObjView.Name = "ToolsListObjView";
            this.ToolsListObjView.RenderNonEditableCheckboxesAsDisabled = true;
            this.ToolsListObjView.ShowCommandMenuOnRightClick = true;
            this.ToolsListObjView.ShowGroups = false;
            this.ToolsListObjView.Size = new System.Drawing.Size(827, 186);
            this.ToolsListObjView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ToolsListObjView.TabIndex = 0;
            this.ToolsListObjView.UseCompatibleStateImageBehavior = false;
            this.ToolsListObjView.UseNotifyPropertyChanged = true;
            this.ToolsListObjView.View = System.Windows.Forms.View.Details;
            this.ToolsListObjView.CellToolTipShowing += new System.EventHandler<BrightIdeasSoftware.ToolTipShowingEventArgs>(this.ToolsListObjView_CellToolTipShowing);
            this.ToolsListObjView.SelectedIndexChanged += new System.EventHandler(this.ToolsListObjView_SelectedIndexChanged);
            this.ToolsListObjView.DoubleClick += new System.EventHandler(this.ToolsListObjView_DoubleClick);
            // 
            // DisplayNameColumnHeader
            // 
            this.DisplayNameColumnHeader.AspectName = "DisplayName";
            this.DisplayNameColumnHeader.AutoCompleteEditor = false;
            this.DisplayNameColumnHeader.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.DisplayNameColumnHeader.Text = "Display Name";
            this.DisplayNameColumnHeader.UseInitialLetterForGroup = true;
            this.DisplayNameColumnHeader.Width = 100;
            // 
            // FilenameColumnHeader
            // 
            this.FilenameColumnHeader.AspectName = "FileName";
            this.FilenameColumnHeader.AutoCompleteEditor = false;
            this.FilenameColumnHeader.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.FilenameColumnHeader.Groupable = false;
            this.FilenameColumnHeader.Text = "Filename";
            this.FilenameColumnHeader.Width = 100;
            // 
            // ArgumentsColumnHeader
            // 
            this.ArgumentsColumnHeader.AspectName = "Arguments";
            this.ArgumentsColumnHeader.AutoCompleteEditor = false;
            this.ArgumentsColumnHeader.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.ArgumentsColumnHeader.Groupable = false;
            this.ArgumentsColumnHeader.Text = "Arguments";
            this.ArgumentsColumnHeader.Width = 100;
            // 
            // WorkingDirColumnHeader
            // 
            this.WorkingDirColumnHeader.AspectName = "WorkingDir";
            this.WorkingDirColumnHeader.AutoCompleteEditor = false;
            this.WorkingDirColumnHeader.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.WorkingDirColumnHeader.Groupable = false;
            this.WorkingDirColumnHeader.Text = "Working Directory";
            this.WorkingDirColumnHeader.Width = 120;
            // 
            // WaitForExitColumnHeader
            // 
            this.WaitForExitColumnHeader.AspectName = "WaitForExit";
            this.WaitForExitColumnHeader.CheckBoxes = true;
            this.WaitForExitColumnHeader.Groupable = false;
            this.WaitForExitColumnHeader.Text = "Wait for exit";
            this.WaitForExitColumnHeader.Width = 75;
            // 
            // TryToIntegrateColumnHeader
            // 
            this.TryToIntegrateColumnHeader.AspectName = "TryIntegrate";
            this.TryToIntegrateColumnHeader.CheckBoxes = true;
            this.TryToIntegrateColumnHeader.Groupable = false;
            this.TryToIntegrateColumnHeader.Text = "Try To Integrate";
            this.TryToIntegrateColumnHeader.Width = 95;
            // 
            // RunElevateHeader
            // 
            this.RunElevateHeader.AspectName = "RunElevated";
            this.RunElevateHeader.CheckBoxes = true;
            this.RunElevateHeader.Groupable = false;
            this.RunElevateHeader.Text = "Run Elevated";
            this.RunElevateHeader.Width = 95;
            // 
            // ShowOnToolbarColumnHeader
            // 
            this.ShowOnToolbarColumnHeader.AspectName = "ShowOnToolbar";
            this.ShowOnToolbarColumnHeader.CheckBoxes = true;
            this.ShowOnToolbarColumnHeader.Groupable = false;
            this.ShowOnToolbarColumnHeader.Text = "Show On Toolbar";
            this.ShowOnToolbarColumnHeader.Width = 120;
            //
            // CategoryColumnHeader
            //
            this.CategoryColumnHeader.AspectName = "Category";
            this.CategoryColumnHeader.AutoCompleteEditor = false;
            this.CategoryColumnHeader.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.CategoryColumnHeader.Groupable = true;
            this.CategoryColumnHeader.Text = "Category";
            this.CategoryColumnHeader.Width = 100;
            //
            // HiddenColumnHeader
            //
            this.HiddenColumnHeader.AspectName = "Hidden";
            this.HiddenColumnHeader.CheckBoxes = true;
            this.HiddenColumnHeader.Groupable = false;
            this.HiddenColumnHeader.Text = "Hidden";
            this.HiddenColumnHeader.Width = 60;
            //
            // ToolsContextMenuStrip
            // 
            this.ToolsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewToolMenuItem,
            this.DeleteToolMenuItem,
            this.ToolStripSeparator1,
            this.LaunchToolMenuItem});
            this.ToolsContextMenuStrip.Name = "cMenApps";
            this.ToolsContextMenuStrip.Size = new System.Drawing.Size(220, 76);
            // 
            // NewToolMenuItem
            // 
            this.NewToolMenuItem.Image = global::mRemoteNG.Properties.Resources.Add_16x;
            this.NewToolMenuItem.Name = "NewToolMenuItem";
            this.NewToolMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F4)));
            this.NewToolMenuItem.Size = new System.Drawing.Size(219, 22);
            this.NewToolMenuItem.Text = "New External Tool";
            this.NewToolMenuItem.Click += new System.EventHandler(this.NewTool_Click);
            // 
            // DeleteToolMenuItem
            // 
            this.DeleteToolMenuItem.Enabled = false;
            this.DeleteToolMenuItem.Image = global::mRemoteNG.Properties.Resources.Remove_16x;
            this.DeleteToolMenuItem.Name = "DeleteToolMenuItem";
            this.DeleteToolMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.DeleteToolMenuItem.Size = new System.Drawing.Size(219, 22);
            this.DeleteToolMenuItem.Text = "Delete External Tool...";
            this.DeleteToolMenuItem.Click += new System.EventHandler(this.DeleteTool_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(216, 6);
            // 
            // LaunchToolMenuItem
            // 
            this.LaunchToolMenuItem.Enabled = false;
            this.LaunchToolMenuItem.Image = global::mRemoteNG.Properties.Resources.Run_16x;
            this.LaunchToolMenuItem.Name = "LaunchToolMenuItem";
            this.LaunchToolMenuItem.Size = new System.Drawing.Size(219, 22);
            this.LaunchToolMenuItem.Text = "Launch External Tool";
            this.LaunchToolMenuItem.Click += new System.EventHandler(this.LaunchTool_Click);
            // 
            // PropertiesGroupBox
            // 
            this.PropertiesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertiesGroupBox.Controls.Add(this.tableLayoutPanel1);
            this.PropertiesGroupBox.Enabled = false;
            this.PropertiesGroupBox.Location = new System.Drawing.Point(0, 192);
            this.PropertiesGroupBox.Name = "PropertiesGroupBox";
            this.PropertiesGroupBox.Size = new System.Drawing.Size(827, 360);
            this.PropertiesGroupBox.TabIndex = 1;
            this.PropertiesGroupBox.TabStop = false;
            this.PropertiesGroupBox.Text = "External Tool Properties";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.DisplayNameLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ShowOnToolbarCheckBox, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.WorkingDirTextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.DisplayNameTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.FilenameTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ArgumentsCheckBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.FilenameLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.RunElevatedCheckBox, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.ArgumentsLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.TryToIntegrateCheckBox, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.WorkingDirLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.IconPathLabel, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.IconPathTextBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.BrowseIconButton, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.HotkeyLabel, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.HotkeyTextBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.OptionsLabel, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.WaitForExitCheckBox, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.BrowseButton, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.VariablesButton, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.BrowseWorkingDir, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.AuthenticationTypeLabel, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.AuthenticationTypeTextBox, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.AuthenticationUsernameLabel, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.AuthenticationUsernameTextBox, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.AuthenticationPasswordLabel, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.AuthenticationPasswordTextBox, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.PrivateKeyFileLabel, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.PrivateKeyFileTextBox, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.BrowsePrivateKeyButton, 4, 11);
            this.tableLayoutPanel1.Controls.Add(this.PassphraseLabel, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.PassphraseTextBox, 1, 12);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 18);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 14;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(821, 339);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // DisplayNameLabel
            // 
            this.DisplayNameLabel.AutoSize = true;
            this.DisplayNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DisplayNameLabel.Location = new System.Drawing.Point(3, 0);
            this.DisplayNameLabel.Name = "DisplayNameLabel";
            this.DisplayNameLabel.Size = new System.Drawing.Size(104, 26);
            this.DisplayNameLabel.TabIndex = 0;
            this.DisplayNameLabel.Text = "Display Name:";
            this.DisplayNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ShowOnToolbarCheckBox
            // 
            this.ShowOnToolbarCheckBox._mice = MrngCheckBox.MouseState.HOVER;
            this.ShowOnToolbarCheckBox.AutoSize = true;
            this.ShowOnToolbarCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShowOnToolbarCheckBox.Location = new System.Drawing.Point(239, 159);
            this.ShowOnToolbarCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.ShowOnToolbarCheckBox.Name = "ShowOnToolbarCheckBox";
            this.ShowOnToolbarCheckBox.Size = new System.Drawing.Size(113, 20);
            this.ShowOnToolbarCheckBox.TabIndex = 12;
            this.ShowOnToolbarCheckBox.Text = "Show on toolbar";
            this.ShowOnToolbarCheckBox.UseVisualStyleBackColor = true;
            this.ShowOnToolbarCheckBox.Click += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // WorkingDirTextBox
            // 
            this.WorkingDirTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.WorkingDirTextBox, 3);
            this.WorkingDirTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WorkingDirTextBox.Location = new System.Drawing.Point(110, 80);
            this.WorkingDirTextBox.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.WorkingDirTextBox.Name = "WorkingDirTextBox";
            this.WorkingDirTextBox.Size = new System.Drawing.Size(607, 22);
            this.WorkingDirTextBox.TabIndex = 5;
            this.WorkingDirTextBox.Leave += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // IconPathLabel
            // 
            this.IconPathLabel.AutoSize = true;
            this.IconPathLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IconPathLabel.Location = new System.Drawing.Point(3, 104);
            this.IconPathLabel.Name = "IconPathLabel";
            this.IconPathLabel.Size = new System.Drawing.Size(104, 26);
            this.IconPathLabel.TabIndex = 13;
            this.IconPathLabel.Text = "Icon Path:";
            this.IconPathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IconPathTextBox
            // 
            this.IconPathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.IconPathTextBox, 3);
            this.IconPathTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IconPathTextBox.Location = new System.Drawing.Point(110, 106);
            this.IconPathTextBox.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.IconPathTextBox.Name = "IconPathTextBox";
            this.IconPathTextBox.Size = new System.Drawing.Size(607, 22);
            this.IconPathTextBox.TabIndex = 6;
            this.IconPathTextBox.Leave += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // BrowseIconButton
            // 
            this.BrowseIconButton._mice = MrngButton.MouseState.HOVER;
            this.BrowseIconButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowseIconButton.Location = new System.Drawing.Point(723, 107);
            this.BrowseIconButton.Name = "BrowseIconButton";
            this.BrowseIconButton.Size = new System.Drawing.Size(95, 20);
            this.BrowseIconButton.TabIndex = 7;
            this.BrowseIconButton.Text = "Browse...";
            this.BrowseIconButton.UseVisualStyleBackColor = true;
            this.BrowseIconButton.Click += new System.EventHandler(this.BrowseIconButton_Click);

            // 
            // DisplayNameTextBox
            // 
            this.DisplayNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.DisplayNameTextBox, 3);
            this.DisplayNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DisplayNameTextBox.Location = new System.Drawing.Point(110, 2);
            this.DisplayNameTextBox.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.DisplayNameTextBox.Name = "DisplayNameTextBox";
            this.DisplayNameTextBox.Size = new System.Drawing.Size(607, 22);
            this.DisplayNameTextBox.TabIndex = 1;
            this.DisplayNameTextBox.LostFocus += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // FilenameTextBox
            // 
            this.FilenameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.FilenameTextBox, 3);
            this.FilenameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilenameTextBox.Location = new System.Drawing.Point(110, 28);
            this.FilenameTextBox.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.FilenameTextBox.Name = "FilenameTextBox";
            this.FilenameTextBox.Size = new System.Drawing.Size(607, 22);
            this.FilenameTextBox.TabIndex = 2;
            this.FilenameTextBox.LostFocus += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // ArgumentsCheckBox
            // 
            this.ArgumentsCheckBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.ArgumentsCheckBox, 3);
            this.ArgumentsCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ArgumentsCheckBox.Location = new System.Drawing.Point(110, 54);
            this.ArgumentsCheckBox.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.ArgumentsCheckBox.Name = "ArgumentsCheckBox";
            this.ArgumentsCheckBox.Size = new System.Drawing.Size(607, 22);
            this.ArgumentsCheckBox.TabIndex = 4;
            this.ArgumentsCheckBox.LostFocus += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // FilenameLabel
            // 
            this.FilenameLabel.AutoSize = true;
            this.FilenameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilenameLabel.Location = new System.Drawing.Point(3, 26);
            this.FilenameLabel.Name = "FilenameLabel";
            this.FilenameLabel.Size = new System.Drawing.Size(104, 26);
            this.FilenameLabel.TabIndex = 2;
            this.FilenameLabel.Text = "Filename:";
            this.FilenameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RunElevatedCheckBox
            // 
            this.RunElevatedCheckBox._mice = MrngCheckBox.MouseState.HOVER;
            this.RunElevatedCheckBox.AutoSize = true;
            this.RunElevatedCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RunElevatedCheckBox.Location = new System.Drawing.Point(113, 159);
            this.RunElevatedCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.RunElevatedCheckBox.Name = "RunElevatedCheckBox";
            this.RunElevatedCheckBox.Size = new System.Drawing.Size(93, 20);
            this.RunElevatedCheckBox.TabIndex = 11;
            this.RunElevatedCheckBox.Text = "Run Elevated";
            this.RunElevatedCheckBox.UseVisualStyleBackColor = true;
            this.RunElevatedCheckBox.Click += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // ArgumentsLabel
            // 
            this.ArgumentsLabel.AutoSize = true;
            this.ArgumentsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ArgumentsLabel.Location = new System.Drawing.Point(3, 52);
            this.ArgumentsLabel.Name = "ArgumentsLabel";
            this.ArgumentsLabel.Size = new System.Drawing.Size(104, 26);
            this.ArgumentsLabel.TabIndex = 5;
            this.ArgumentsLabel.Text = "Arguments:";
            this.ArgumentsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TryToIntegrateCheckBox
            // 
            this.TryToIntegrateCheckBox._mice = MrngCheckBox.MouseState.HOVER;
            this.TryToIntegrateCheckBox.AutoSize = true;
            this.TryToIntegrateCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TryToIntegrateCheckBox.Location = new System.Drawing.Point(239, 107);
            this.TryToIntegrateCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.TryToIntegrateCheckBox.Name = "TryToIntegrateCheckBox";
            this.TryToIntegrateCheckBox.Size = new System.Drawing.Size(113, 20);
            this.TryToIntegrateCheckBox.TabIndex = 8;
            this.TryToIntegrateCheckBox.Text = "Try to integrate";
            this.TryToIntegrateCheckBox.UseVisualStyleBackColor = true;
            this.TryToIntegrateCheckBox.Click += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // WorkingDirLabel
            // 
            this.WorkingDirLabel.AutoSize = true;
            this.WorkingDirLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WorkingDirLabel.Location = new System.Drawing.Point(3, 78);
            this.WorkingDirLabel.Name = "WorkingDirLabel";
            this.WorkingDirLabel.Size = new System.Drawing.Size(104, 26);
            this.WorkingDirLabel.TabIndex = 11;
            this.WorkingDirLabel.Text = "Working Directory:";
            this.WorkingDirLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OptionsLabel
            // 
            this.OptionsLabel.AutoSize = true;
            this.OptionsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OptionsLabel.Location = new System.Drawing.Point(3, 104);
            this.OptionsLabel.Name = "OptionsLabel";
            this.OptionsLabel.Size = new System.Drawing.Size(104, 26);
            this.OptionsLabel.TabIndex = 7;
            this.OptionsLabel.Text = "Options:";
            this.OptionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WaitForExitCheckBox
            // 
            this.WaitForExitCheckBox._mice = MrngCheckBox.MouseState.HOVER;
            this.WaitForExitCheckBox.AutoSize = true;
            this.WaitForExitCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WaitForExitCheckBox.Location = new System.Drawing.Point(113, 107);
            this.WaitForExitCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.WaitForExitCheckBox.Name = "WaitForExitCheckBox";
            this.WaitForExitCheckBox.Size = new System.Drawing.Size(93, 20);
            this.WaitForExitCheckBox.TabIndex = 7;
            this.WaitForExitCheckBox.Text = "Wait for exit";
            this.WaitForExitCheckBox.UseVisualStyleBackColor = true;
            this.WaitForExitCheckBox.Click += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            this.WaitForExitCheckBox.LostFocus += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // VariablesButton
            // 
            this.VariablesButton._mice = MrngButton.MouseState.HOVER;
            this.VariablesButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VariablesButton.Location = new System.Drawing.Point(723, 55);
            this.VariablesButton.Name = "VariablesButton";
            this.VariablesButton.Size = new System.Drawing.Size(95, 20);
            this.VariablesButton.TabIndex = 5;
            this.VariablesButton.Text = "Variables";
            this.VariablesButton.UseVisualStyleBackColor = true;
            this.VariablesButton.Click += new System.EventHandler(this.VariablesButton_Click);
            this.VariablesButton.LostFocus += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // BrowseButton
            // 
            this.BrowseButton._mice = MrngButton.MouseState.HOVER;
            this.BrowseButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowseButton.Location = new System.Drawing.Point(723, 29);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(95, 20);
            this.BrowseButton.TabIndex = 3;
            this.BrowseButton.Text = "Browse...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            this.BrowseButton.LostFocus += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // BrowseWorkingDir
            // 
            this.BrowseWorkingDir._mice = MrngButton.MouseState.HOVER;
            this.BrowseWorkingDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowseWorkingDir.Location = new System.Drawing.Point(723, 81);
            this.BrowseWorkingDir.Name = "BrowseWorkingDir";
            this.BrowseWorkingDir.Size = new System.Drawing.Size(95, 20);
            this.BrowseWorkingDir.TabIndex = 6;
            this.BrowseWorkingDir.Text = "Browse...";
            this.BrowseWorkingDir.UseVisualStyleBackColor = true;
            this.BrowseWorkingDir.Click += new System.EventHandler(this.BrowseWorkingDir_Click);
            // 
            // AuthenticationTypeLabel
            // 
            this.AuthenticationTypeLabel.AutoSize = true;
            this.AuthenticationTypeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AuthenticationTypeLabel.Location = new System.Drawing.Point(3, 208);
            this.AuthenticationTypeLabel.Name = "AuthenticationTypeLabel";
            this.AuthenticationTypeLabel.Size = new System.Drawing.Size(104, 26);
            this.AuthenticationTypeLabel.TabIndex = 26;
            this.AuthenticationTypeLabel.Text = "Authentication Type:";
            this.AuthenticationTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AuthenticationTypeTextBox
            // 
            this.AuthenticationTypeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.AuthenticationTypeTextBox, 3);
            this.AuthenticationTypeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AuthenticationTypeTextBox.Location = new System.Drawing.Point(110, 210);
            this.AuthenticationTypeTextBox.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.AuthenticationTypeTextBox.Name = "AuthenticationTypeTextBox";
            this.AuthenticationTypeTextBox.Size = new System.Drawing.Size(607, 22);
            this.AuthenticationTypeTextBox.TabIndex = 13;
            this.AuthenticationTypeTextBox.Leave += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // AuthenticationUsernameLabel
            // 
            this.AuthenticationUsernameLabel.AutoSize = true;
            this.AuthenticationUsernameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AuthenticationUsernameLabel.Location = new System.Drawing.Point(3, 234);
            this.AuthenticationUsernameLabel.Name = "AuthenticationUsernameLabel";
            this.AuthenticationUsernameLabel.Size = new System.Drawing.Size(104, 26);
            this.AuthenticationUsernameLabel.TabIndex = 27;
            this.AuthenticationUsernameLabel.Text = "Authentication Username:";
            this.AuthenticationUsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AuthenticationUsernameTextBox
            // 
            this.AuthenticationUsernameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.AuthenticationUsernameTextBox, 3);
            this.AuthenticationUsernameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AuthenticationUsernameTextBox.Location = new System.Drawing.Point(110, 236);
            this.AuthenticationUsernameTextBox.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.AuthenticationUsernameTextBox.Name = "AuthenticationUsernameTextBox";
            this.AuthenticationUsernameTextBox.Size = new System.Drawing.Size(607, 22);
            this.AuthenticationUsernameTextBox.TabIndex = 14;
            this.AuthenticationUsernameTextBox.Leave += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // AuthenticationPasswordLabel
            // 
            this.AuthenticationPasswordLabel.AutoSize = true;
            this.AuthenticationPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AuthenticationPasswordLabel.Location = new System.Drawing.Point(3, 260);
            this.AuthenticationPasswordLabel.Name = "AuthenticationPasswordLabel";
            this.AuthenticationPasswordLabel.Size = new System.Drawing.Size(104, 26);
            this.AuthenticationPasswordLabel.TabIndex = 28;
            this.AuthenticationPasswordLabel.Text = "Authentication Password:";
            this.AuthenticationPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AuthenticationPasswordTextBox
            // 
            this.AuthenticationPasswordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.AuthenticationPasswordTextBox, 3);
            this.AuthenticationPasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AuthenticationPasswordTextBox.Location = new System.Drawing.Point(110, 262);
            this.AuthenticationPasswordTextBox.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.AuthenticationPasswordTextBox.Name = "AuthenticationPasswordTextBox";
            this.AuthenticationPasswordTextBox.Size = new System.Drawing.Size(607, 22);
            this.AuthenticationPasswordTextBox.TabIndex = 15;
            this.AuthenticationPasswordTextBox.Leave += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // PrivateKeyFileLabel
            // 
            this.PrivateKeyFileLabel.AutoSize = true;
            this.PrivateKeyFileLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PrivateKeyFileLabel.Location = new System.Drawing.Point(3, 286);
            this.PrivateKeyFileLabel.Name = "PrivateKeyFileLabel";
            this.PrivateKeyFileLabel.Size = new System.Drawing.Size(104, 26);
            this.PrivateKeyFileLabel.TabIndex = 29;
            this.PrivateKeyFileLabel.Text = "Private Key File:";
            this.PrivateKeyFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PrivateKeyFileTextBox
            // 
            this.PrivateKeyFileTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.PrivateKeyFileTextBox, 3);
            this.PrivateKeyFileTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PrivateKeyFileTextBox.Location = new System.Drawing.Point(110, 288);
            this.PrivateKeyFileTextBox.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.PrivateKeyFileTextBox.Name = "PrivateKeyFileTextBox";
            this.PrivateKeyFileTextBox.Size = new System.Drawing.Size(607, 22);
            this.PrivateKeyFileTextBox.TabIndex = 16;
            this.PrivateKeyFileTextBox.Leave += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // BrowsePrivateKeyButton
            // 
            this.BrowsePrivateKeyButton._mice = MrngButton.MouseState.HOVER;
            this.BrowsePrivateKeyButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowsePrivateKeyButton.Location = new System.Drawing.Point(723, 289);
            this.BrowsePrivateKeyButton.Name = "BrowsePrivateKeyButton";
            this.BrowsePrivateKeyButton.Size = new System.Drawing.Size(95, 20);
            this.BrowsePrivateKeyButton.TabIndex = 17;
            this.BrowsePrivateKeyButton.Text = "Browse...";
            this.BrowsePrivateKeyButton.UseVisualStyleBackColor = true;
            this.BrowsePrivateKeyButton.Click += new System.EventHandler(this.BrowsePrivateKeyButton_Click);
            // 
            // PassphraseLabel
            // 
            this.PassphraseLabel.AutoSize = true;
            this.PassphraseLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PassphraseLabel.Location = new System.Drawing.Point(3, 312);
            this.PassphraseLabel.Name = "PassphraseLabel";
            this.PassphraseLabel.Size = new System.Drawing.Size(104, 26);
            this.PassphraseLabel.TabIndex = 30;
            this.PassphraseLabel.Text = "Passphrase:";
            this.PassphraseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PassphraseTextBox
            // 
            this.PassphraseTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.PassphraseTextBox, 3);
            this.PassphraseTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PassphraseTextBox.Location = new System.Drawing.Point(110, 314);
            this.PassphraseTextBox.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.PassphraseTextBox.Name = "PassphraseTextBox";
            this.PassphraseTextBox.Size = new System.Drawing.Size(607, 22);
            this.PassphraseTextBox.TabIndex = 18;
            this.PassphraseTextBox.Leave += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
            // 
            // ToolStripContainer
            // 
            // 
            // ToolStripContainer.ContentPanel
            // 
            this.ToolStripContainer.ContentPanel.Controls.Add(this.PropertiesGroupBox);
            this.ToolStripContainer.ContentPanel.Controls.Add(this.ToolsListObjView);
            this.ToolStripContainer.ContentPanel.Size = new System.Drawing.Size(827, 552);
            this.ToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.ToolStripContainer.Name = "ToolStripContainer";
            this.ToolStripContainer.Size = new System.Drawing.Size(827, 577);
            this.ToolStripContainer.TabIndex = 0;
            this.ToolStripContainer.Text = "ToolStripContainer";
            // 
            // ToolStripContainer.TopToolStripPanel
            // 
            this.ToolStripContainer.TopToolStripPanel.BackColor = System.Drawing.SystemColors.Control;
            this.ToolStripContainer.TopToolStripPanel.Controls.Add(this.ToolStrip);
            // 
            // ToolStrip
            // 
            this.ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewToolToolstripButton,
            this.DeleteToolToolstripButton,
            this.ToolStripSeparator2,
            this.LaunchToolToolstripButton});
            this.ToolStrip.Location = new System.Drawing.Point(3, 0);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Size = new System.Drawing.Size(186, 25);
            this.ToolStrip.TabIndex = 0;
            // 
            // NewToolToolstripButton
            // 
            this.NewToolToolstripButton.Image = global::mRemoteNG.Properties.Resources.Add_16x;
            this.NewToolToolstripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewToolToolstripButton.Name = "NewToolToolstripButton";
            this.NewToolToolstripButton.Size = new System.Drawing.Size(51, 22);
            this.NewToolToolstripButton.Text = "New";
            this.NewToolToolstripButton.Click += new System.EventHandler(this.NewTool_Click);
            // 
            // DeleteToolToolstripButton
            // 
            this.DeleteToolToolstripButton.Enabled = false;
            this.DeleteToolToolstripButton.Image = global::mRemoteNG.Properties.Resources.Remove_16x;
            this.DeleteToolToolstripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteToolToolstripButton.Name = "DeleteToolToolstripButton";
            this.DeleteToolToolstripButton.Size = new System.Drawing.Size(60, 22);
            this.DeleteToolToolstripButton.Text = "Delete";
            this.DeleteToolToolstripButton.Click += new System.EventHandler(this.DeleteTool_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // LaunchToolToolstripButton
            // 
            this.LaunchToolToolstripButton.Enabled = false;
            this.LaunchToolToolstripButton.Image = global::mRemoteNG.Properties.Resources.Run_16x;
            this.LaunchToolToolstripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LaunchToolToolstripButton.Name = "LaunchToolToolstripButton";
            this.LaunchToolToolstripButton.Size = new System.Drawing.Size(66, 22);
            this.LaunchToolToolstripButton.Text = "Launch";
            this.LaunchToolToolstripButton.Click += new System.EventHandler(this.LaunchTool_Click);
            // 
            // vsToolStripExtender
            // 
            this.vsToolStripExtender.DefaultRenderer = null;
            // 
            // ExternalToolsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(827, 577);
            this.Controls.Add(this.ToolStripContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ExternalToolsWindow";
            this.TabText = "External Applications";
            this.Text = "External Tools";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExternalTools_FormClosed);
            this.Load += new System.EventHandler(this.ExternalTools_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ToolsListObjView)).EndInit();
            this.ToolsContextMenuStrip.ResumeLayout(false);
            this.PropertiesGroupBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ToolStripContainer.ContentPanel.ResumeLayout(false);
            this.ToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.ToolStripContainer.TopToolStripPanel.PerformLayout();
            this.ToolStripContainer.ResumeLayout(false);
            this.ToolStripContainer.PerformLayout();
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.ResumeLayout(false);

		}
		internal System.Windows.Forms.ToolStripContainer ToolStripContainer;
		internal System.Windows.Forms.ToolStrip ToolStrip;
		internal System.Windows.Forms.ToolStripButton NewToolToolstripButton;
		internal System.Windows.Forms.ToolStripButton DeleteToolToolstripButton;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
		internal System.Windows.Forms.ToolStripButton LaunchToolToolstripButton;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
        #endregion

        private System.ComponentModel.IContainer components;
        internal MrngGroupBox PropertiesGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
