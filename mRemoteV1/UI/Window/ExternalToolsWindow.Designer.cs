
namespace mRemoteNG.UI.Window
{
	public partial class ExternalToolsWindow : BaseWindow
	{
        #region  Windows Form Designer generated code
		internal System.Windows.Forms.ColumnHeader FilenameColumnHeader;
		internal System.Windows.Forms.ColumnHeader ArgumentsColumnHeader;
		internal System.Windows.Forms.GroupBox PropertiesGroupBox;
		internal System.Windows.Forms.TextBox DisplayNameTextBox;
		internal System.Windows.Forms.Label DisplayNameLabel;
		internal System.Windows.Forms.TextBox ArgumentsCheckBox;
		internal System.Windows.Forms.TextBox FilenameTextBox;
		internal System.Windows.Forms.Label ArgumentsLabel;
		internal System.Windows.Forms.Label FilenameLabel;
		internal System.Windows.Forms.Button BrowseButton;
		internal System.Windows.Forms.ColumnHeader DisplayNameColumnHeader;
		internal System.Windows.Forms.ContextMenuStrip ToolsContextMenuStrip;
		private System.ComponentModel.Container components = null;
		internal System.Windows.Forms.ToolStripMenuItem NewToolMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem DeleteToolMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
		internal System.Windows.Forms.ToolStripMenuItem LaunchToolMenuItem;
		internal System.Windows.Forms.ColumnHeader WaitForExitColumnHeader;
		internal System.Windows.Forms.CheckBox WaitForExitCheckBox;
		internal System.Windows.Forms.Label OptionsLabel;
		internal System.Windows.Forms.CheckBox TryToIntegrateCheckBox;
		internal System.Windows.Forms.ColumnHeader TryToIntegrateColumnHeader;
		internal System.Windows.Forms.ListView ToolsListView;
				
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Load += new System.EventHandler(ExternalTools_Load);
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ExternalTools_FormClosed);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExternalToolsWindow));
			this.ToolsListView = new System.Windows.Forms.ListView();
			this.ToolsListView.SelectedIndexChanged += new System.EventHandler(this.ToolsListView_SelectedIndexChanged);
			this.ToolsListView.DoubleClick += new System.EventHandler(this.ToolsListView_DoubleClick);
			this.DisplayNameColumnHeader = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.FilenameColumnHeader = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.ArgumentsColumnHeader = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.WaitForExitColumnHeader = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.TryToIntegrateColumnHeader = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.ToolsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.NewToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NewToolMenuItem.Click += new System.EventHandler(this.NewTool_Click);
			this.DeleteToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DeleteToolMenuItem.Click += new System.EventHandler(this.DeleteTool_Click);
			this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.LaunchToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.LaunchToolMenuItem.Click += new System.EventHandler(this.LaunchTool_Click);
			this.PropertiesGroupBox = new System.Windows.Forms.GroupBox();
			this.TryToIntegrateCheckBox = new System.Windows.Forms.CheckBox();
			this.TryToIntegrateCheckBox.Click += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
			this.TryToIntegrateCheckBox.CheckedChanged += new System.EventHandler(this.TryToIntegrateCheckBox_CheckedChanged);
			this.OptionsLabel = new System.Windows.Forms.Label();
			this.WaitForExitCheckBox = new System.Windows.Forms.CheckBox();
			this.WaitForExitCheckBox.LostFocus += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
			this.WaitForExitCheckBox.Click += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
			this.BrowseButton = new System.Windows.Forms.Button();
			this.BrowseButton.LostFocus += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
			this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
			this.ArgumentsCheckBox = new System.Windows.Forms.TextBox();
			this.ArgumentsCheckBox.LostFocus += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
			this.FilenameTextBox = new System.Windows.Forms.TextBox();
			this.FilenameTextBox.LostFocus += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
			this.DisplayNameTextBox = new System.Windows.Forms.TextBox();
			this.DisplayNameTextBox.LostFocus += new System.EventHandler(this.PropertyControl_ChangedOrLostFocus);
			this.ArgumentsLabel = new System.Windows.Forms.Label();
			this.FilenameLabel = new System.Windows.Forms.Label();
			this.DisplayNameLabel = new System.Windows.Forms.Label();
			this.ToolStripContainer = new System.Windows.Forms.ToolStripContainer();
			this.ToolStrip = new System.Windows.Forms.ToolStrip();
			this.NewToolToolstripButton = new System.Windows.Forms.ToolStripButton();
			this.NewToolToolstripButton.Click += new System.EventHandler(this.NewTool_Click);
			this.DeleteToolToolstripButton = new System.Windows.Forms.ToolStripButton();
			this.DeleteToolToolstripButton.Click += new System.EventHandler(this.DeleteTool_Click);
			this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.LaunchToolToolstripButton = new System.Windows.Forms.ToolStripButton();
			this.LaunchToolToolstripButton.Click += new System.EventHandler(this.LaunchTool_Click);
			this.ToolsContextMenuStrip.SuspendLayout();
			this.PropertiesGroupBox.SuspendLayout();
			this.ToolStripContainer.ContentPanel.SuspendLayout();
			this.ToolStripContainer.TopToolStripPanel.SuspendLayout();
			this.ToolStripContainer.SuspendLayout();
			this.ToolStrip.SuspendLayout();
			this.SuspendLayout();
			//
			//ToolsListView
			//
			this.ToolsListView.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.ToolsListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ToolsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this.DisplayNameColumnHeader, this.FilenameColumnHeader, this.ArgumentsColumnHeader, this.WaitForExitColumnHeader, this.TryToIntegrateColumnHeader});
			this.ToolsListView.ContextMenuStrip = this.ToolsContextMenuStrip;
			this.ToolsListView.FullRowSelect = true;
			this.ToolsListView.GridLines = true;
			this.ToolsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.ToolsListView.HideSelection = false;
			this.ToolsListView.Location = new System.Drawing.Point(0, 0);
			this.ToolsListView.Name = "ToolsListView";
			this.ToolsListView.Size = new System.Drawing.Size(684, 157);
			this.ToolsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.ToolsListView.TabIndex = 0;
			this.ToolsListView.UseCompatibleStateImageBehavior = false;
			this.ToolsListView.View = System.Windows.Forms.View.Details;
			//
			//DisplayNameColumnHeader
			//
			this.DisplayNameColumnHeader.Text = "Display Name";
			this.DisplayNameColumnHeader.Width = 130;
			//
			//FilenameColumnHeader
			//
			this.FilenameColumnHeader.Text = "Filename";
			this.FilenameColumnHeader.Width = 200;
			//
			//ArgumentsColumnHeader
			//
			this.ArgumentsColumnHeader.Text = "Arguments";
			this.ArgumentsColumnHeader.Width = 160;
			//
			//WaitForExitColumnHeader
			//
			this.WaitForExitColumnHeader.Text = "Wait for exit";
			this.WaitForExitColumnHeader.Width = 75;
			//
			//TryToIntegrateColumnHeader
			//
			this.TryToIntegrateColumnHeader.Text = "Try To Integrate";
			this.TryToIntegrateColumnHeader.Width = 95;
			//
			//ToolsContextMenuStrip
			//
			this.ToolsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.NewToolMenuItem, this.DeleteToolMenuItem, this.ToolStripSeparator1, this.LaunchToolMenuItem});
			this.ToolsContextMenuStrip.Name = "cMenApps";
			this.ToolsContextMenuStrip.Size = new System.Drawing.Size(221, 76);
			//
			//NewToolMenuItem
			//
			this.NewToolMenuItem.Image = Resources.ExtApp_Add;
			this.NewToolMenuItem.Name = "NewToolMenuItem";
			this.NewToolMenuItem.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F4);
			this.NewToolMenuItem.Size = new System.Drawing.Size(220, 22);
			this.NewToolMenuItem.Text = "New External Tool";
			//
			//DeleteToolMenuItem
			//
			this.DeleteToolMenuItem.Image = Resources.ExtApp_Delete;
			this.DeleteToolMenuItem.Name = "DeleteToolMenuItem";
			this.DeleteToolMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.DeleteToolMenuItem.Size = new System.Drawing.Size(220, 22);
			this.DeleteToolMenuItem.Text = "Delete External Tool...";
			//
			//ToolStripSeparator1
			//
			this.ToolStripSeparator1.Name = "ToolStripSeparator1";
			this.ToolStripSeparator1.Size = new System.Drawing.Size(217, 6);
			//
			//LaunchToolMenuItem
			//
			this.LaunchToolMenuItem.Image = Resources.ExtApp_Start;
			this.LaunchToolMenuItem.Name = "LaunchToolMenuItem";
			this.LaunchToolMenuItem.Size = new System.Drawing.Size(220, 22);
			this.LaunchToolMenuItem.Text = "Launch External Tool";
			//
			//PropertiesGroupBox
			//
			this.PropertiesGroupBox.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.PropertiesGroupBox.Controls.Add(this.TryToIntegrateCheckBox);
			this.PropertiesGroupBox.Controls.Add(this.OptionsLabel);
			this.PropertiesGroupBox.Controls.Add(this.WaitForExitCheckBox);
			this.PropertiesGroupBox.Controls.Add(this.BrowseButton);
			this.PropertiesGroupBox.Controls.Add(this.ArgumentsCheckBox);
			this.PropertiesGroupBox.Controls.Add(this.FilenameTextBox);
			this.PropertiesGroupBox.Controls.Add(this.DisplayNameTextBox);
			this.PropertiesGroupBox.Controls.Add(this.ArgumentsLabel);
			this.PropertiesGroupBox.Controls.Add(this.FilenameLabel);
			this.PropertiesGroupBox.Controls.Add(this.DisplayNameLabel);
			this.PropertiesGroupBox.Enabled = false;
			this.PropertiesGroupBox.Location = new System.Drawing.Point(3, 163);
			this.PropertiesGroupBox.Name = "PropertiesGroupBox";
			this.PropertiesGroupBox.Size = new System.Drawing.Size(678, 132);
			this.PropertiesGroupBox.TabIndex = 1;
			this.PropertiesGroupBox.TabStop = false;
			this.PropertiesGroupBox.Text = "External Tool Properties";
			//
			//TryToIntegrateCheckBox
			//
			this.TryToIntegrateCheckBox.AutoSize = true;
			this.TryToIntegrateCheckBox.Location = new System.Drawing.Point(280, 106);
			this.TryToIntegrateCheckBox.Name = "TryToIntegrateCheckBox";
			this.TryToIntegrateCheckBox.Size = new System.Drawing.Size(97, 17);
			this.TryToIntegrateCheckBox.TabIndex = 9;
			this.TryToIntegrateCheckBox.Text = "Try to integrate";
			this.TryToIntegrateCheckBox.UseVisualStyleBackColor = true;
			//
			//OptionsLabel
			//
			this.OptionsLabel.AutoSize = true;
			this.OptionsLabel.Location = new System.Drawing.Point(58, 107);
			this.OptionsLabel.Name = "OptionsLabel";
			this.OptionsLabel.Size = new System.Drawing.Size(46, 13);
			this.OptionsLabel.TabIndex = 7;
			this.OptionsLabel.Text = "Options:";
			this.OptionsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			//
			//WaitForExitCheckBox
			//
			this.WaitForExitCheckBox.AutoSize = true;
			this.WaitForExitCheckBox.Location = new System.Drawing.Point(110, 106);
			this.WaitForExitCheckBox.Name = "WaitForExitCheckBox";
			this.WaitForExitCheckBox.Size = new System.Drawing.Size(82, 17);
			this.WaitForExitCheckBox.TabIndex = 8;
			this.WaitForExitCheckBox.Text = "Wait for exit";
			this.WaitForExitCheckBox.UseVisualStyleBackColor = true;
			//
			//BrowseButton
			//
			this.BrowseButton.Anchor = (System.Windows.Forms.AnchorStyles) (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.BrowseButton.Location = new System.Drawing.Point(574, 45);
			this.BrowseButton.Name = "BrowseButton";
			this.BrowseButton.Size = new System.Drawing.Size(95, 23);
			this.BrowseButton.TabIndex = 4;
			this.BrowseButton.Text = "Browse...";
			this.BrowseButton.UseVisualStyleBackColor = true;
			//
			//ArgumentsCheckBox
			//
			this.ArgumentsCheckBox.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.ArgumentsCheckBox.Location = new System.Drawing.Point(110, 76);
			this.ArgumentsCheckBox.Name = "ArgumentsCheckBox";
			this.ArgumentsCheckBox.Size = new System.Drawing.Size(559, 20);
			this.ArgumentsCheckBox.TabIndex = 6;
			//
			//FilenameTextBox
			//
			this.FilenameTextBox.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.FilenameTextBox.Location = new System.Drawing.Point(110, 47);
			this.FilenameTextBox.Name = "FilenameTextBox";
			this.FilenameTextBox.Size = new System.Drawing.Size(458, 20);
			this.FilenameTextBox.TabIndex = 3;
			//
			//DisplayNameTextBox
			//
			this.DisplayNameTextBox.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.DisplayNameTextBox.Location = new System.Drawing.Point(110, 19);
			this.DisplayNameTextBox.Name = "DisplayNameTextBox";
			this.DisplayNameTextBox.Size = new System.Drawing.Size(559, 20);
			this.DisplayNameTextBox.TabIndex = 1;
			//
			//ArgumentsLabel
			//
			this.ArgumentsLabel.AutoSize = true;
			this.ArgumentsLabel.Location = new System.Drawing.Point(44, 79);
			this.ArgumentsLabel.Name = "ArgumentsLabel";
			this.ArgumentsLabel.Size = new System.Drawing.Size(60, 13);
			this.ArgumentsLabel.TabIndex = 5;
			this.ArgumentsLabel.Text = "Arguments:";
			this.ArgumentsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			//
			//FilenameLabel
			//
			this.FilenameLabel.AutoSize = true;
			this.FilenameLabel.Location = new System.Drawing.Point(52, 50);
			this.FilenameLabel.Name = "FilenameLabel";
			this.FilenameLabel.Size = new System.Drawing.Size(52, 13);
			this.FilenameLabel.TabIndex = 2;
			this.FilenameLabel.Text = "Filename:";
			this.FilenameLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			//
			//DisplayNameLabel
			//
			this.DisplayNameLabel.AutoSize = true;
			this.DisplayNameLabel.Location = new System.Drawing.Point(29, 22);
			this.DisplayNameLabel.Name = "DisplayNameLabel";
			this.DisplayNameLabel.Size = new System.Drawing.Size(75, 13);
			this.DisplayNameLabel.TabIndex = 0;
			this.DisplayNameLabel.Text = "Display Name:";
			this.DisplayNameLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			//
			//ToolStripContainer
			//
			//
			//ToolStripContainer.ContentPanel
			//
			this.ToolStripContainer.ContentPanel.Controls.Add(this.PropertiesGroupBox);
			this.ToolStripContainer.ContentPanel.Controls.Add(this.ToolsListView);
			this.ToolStripContainer.ContentPanel.Size = new System.Drawing.Size(684, 298);
			this.ToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ToolStripContainer.Location = new System.Drawing.Point(0, 0);
			this.ToolStripContainer.Name = "ToolStripContainer";
			this.ToolStripContainer.Size = new System.Drawing.Size(684, 323);
			this.ToolStripContainer.TabIndex = 0;
			this.ToolStripContainer.Text = "ToolStripContainer";
			//
			//ToolStripContainer.TopToolStripPanel
			//
			this.ToolStripContainer.TopToolStripPanel.Controls.Add(this.ToolStrip);
			//
			//ToolStrip
			//
			this.ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.NewToolToolstripButton, this.DeleteToolToolstripButton, this.ToolStripSeparator2, this.LaunchToolToolstripButton});
			this.ToolStrip.Location = new System.Drawing.Point(3, 0);
			this.ToolStrip.Name = "ToolStrip";
			this.ToolStrip.Size = new System.Drawing.Size(186, 25);
			this.ToolStrip.TabIndex = 0;
			//
			//NewToolToolstripButton
			//
			this.NewToolToolstripButton.Image = Resources.ExtApp_Add;
			this.NewToolToolstripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.NewToolToolstripButton.Name = "NewToolToolstripButton";
			this.NewToolToolstripButton.Size = new System.Drawing.Size(51, 22);
			this.NewToolToolstripButton.Text = "New";
			//
			//DeleteToolToolstripButton
			//
			this.DeleteToolToolstripButton.Image = Resources.ExtApp_Delete;
			this.DeleteToolToolstripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.DeleteToolToolstripButton.Name = "DeleteToolToolstripButton";
			this.DeleteToolToolstripButton.Size = new System.Drawing.Size(60, 22);
			this.DeleteToolToolstripButton.Text = "Delete";
			//
			//ToolStripSeparator2
			//
			this.ToolStripSeparator2.Name = "ToolStripSeparator2";
			this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			//
			//LaunchToolToolstripButton
			//
			this.LaunchToolToolstripButton.Image = Resources.ExtApp_Start;
			this.LaunchToolToolstripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.LaunchToolToolstripButton.Name = "LaunchToolToolstripButton";
			this.LaunchToolToolstripButton.Size = new System.Drawing.Size(66, 22);
			this.LaunchToolToolstripButton.Text = "Launch";
			//
			//ExternalTools
			//
			this.ClientSize = new System.Drawing.Size(684, 323);
			this.Controls.Add(this.ToolStripContainer);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.Icon = (System.Drawing.Icon) (resources.GetObject("$this.Icon"));
			this.Name = "ExternalTools";
			this.TabText = "External Applications";
			this.Text = "External Tools";
			this.ToolsContextMenuStrip.ResumeLayout(false);
			this.PropertiesGroupBox.ResumeLayout(false);
			this.PropertiesGroupBox.PerformLayout();
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
        #endregion
	}
}
