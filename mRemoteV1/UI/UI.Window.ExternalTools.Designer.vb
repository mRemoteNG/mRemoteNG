Namespace UI
    Namespace Window
        Partial Public Class ExternalTools
            Inherits Base
#Region " Windows Form Designer generated code "
            Friend WithEvents FilenameColumnHeader As System.Windows.Forms.ColumnHeader
            Friend WithEvents ArgumentsColumnHeader As System.Windows.Forms.ColumnHeader
            Friend WithEvents PropertiesGroupBox As System.Windows.Forms.GroupBox
            Friend WithEvents DisplayNameTextBox As System.Windows.Forms.TextBox
            Friend WithEvents DisplayNameLabel As System.Windows.Forms.Label
            Friend WithEvents ArgumentsCheckBox As System.Windows.Forms.TextBox
            Friend WithEvents FilenameTextBox As System.Windows.Forms.TextBox
            Friend WithEvents ArgumentsLabel As System.Windows.Forms.Label
            Friend WithEvents FilenameLabel As System.Windows.Forms.Label
            Friend WithEvents BrowseButton As System.Windows.Forms.Button
            Friend WithEvents DisplayNameColumnHeader As System.Windows.Forms.ColumnHeader
            Friend WithEvents ToolsContextMenuStrip As System.Windows.Forms.ContextMenuStrip
            Private components As System.ComponentModel.IContainer
            Friend WithEvents NewToolMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents DeleteToolMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents LaunchToolMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents WaitForExitColumnHeader As System.Windows.Forms.ColumnHeader
            Friend WithEvents WaitForExitCheckBox As System.Windows.Forms.CheckBox
            Friend WithEvents OptionsLabel As System.Windows.Forms.Label
            Friend WithEvents TryToIntegrateCheckBox As System.Windows.Forms.CheckBox
            Friend WithEvents TryToIntegrateColumnHeader As System.Windows.Forms.ColumnHeader
            Friend WithEvents ToolsListView As System.Windows.Forms.ListView

            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container()
                Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ExternalTools))
                Me.ToolsListView = New System.Windows.Forms.ListView()
                Me.DisplayNameColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.FilenameColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.ArgumentsColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.WaitForExitColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.TryToIntegrateColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.ToolsContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
                Me.NewToolMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.DeleteToolMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
                Me.LaunchToolMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.PropertiesGroupBox = New System.Windows.Forms.GroupBox()
                Me.TryToIntegrateCheckBox = New System.Windows.Forms.CheckBox()
                Me.OptionsLabel = New System.Windows.Forms.Label()
                Me.WaitForExitCheckBox = New System.Windows.Forms.CheckBox()
                Me.BrowseButton = New System.Windows.Forms.Button()
                Me.ArgumentsCheckBox = New System.Windows.Forms.TextBox()
                Me.FilenameTextBox = New System.Windows.Forms.TextBox()
                Me.DisplayNameTextBox = New System.Windows.Forms.TextBox()
                Me.ArgumentsLabel = New System.Windows.Forms.Label()
                Me.FilenameLabel = New System.Windows.Forms.Label()
                Me.DisplayNameLabel = New System.Windows.Forms.Label()
                Me.ToolStripContainer = New System.Windows.Forms.ToolStripContainer()
                Me.ToolStrip = New System.Windows.Forms.ToolStrip()
                Me.NewToolToolstripButton = New System.Windows.Forms.ToolStripButton()
                Me.DeleteToolToolstripButton = New System.Windows.Forms.ToolStripButton()
                Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
                Me.LaunchToolToolstripButton = New System.Windows.Forms.ToolStripButton()
                Me.ToolsContextMenuStrip.SuspendLayout()
                Me.PropertiesGroupBox.SuspendLayout()
                Me.ToolStripContainer.ContentPanel.SuspendLayout()
                Me.ToolStripContainer.TopToolStripPanel.SuspendLayout()
                Me.ToolStripContainer.SuspendLayout()
                Me.ToolStrip.SuspendLayout()
                Me.SuspendLayout()
                '
                'ToolsListView
                '
                Me.ToolsListView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.ToolsListView.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.ToolsListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.DisplayNameColumnHeader, Me.FilenameColumnHeader, Me.ArgumentsColumnHeader, Me.WaitForExitColumnHeader, Me.TryToIntegrateColumnHeader})
                Me.ToolsListView.ContextMenuStrip = Me.ToolsContextMenuStrip
                Me.ToolsListView.FullRowSelect = True
                Me.ToolsListView.GridLines = True
                Me.ToolsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
                Me.ToolsListView.HideSelection = False
                Me.ToolsListView.Location = New System.Drawing.Point(0, 0)
                Me.ToolsListView.Name = "ToolsListView"
                Me.ToolsListView.Size = New System.Drawing.Size(684, 157)
                Me.ToolsListView.Sorting = System.Windows.Forms.SortOrder.Ascending
                Me.ToolsListView.TabIndex = 0
                Me.ToolsListView.UseCompatibleStateImageBehavior = False
                Me.ToolsListView.View = System.Windows.Forms.View.Details
                '
                'DisplayNameColumnHeader
                '
                Me.DisplayNameColumnHeader.Text = "Display Name"
                Me.DisplayNameColumnHeader.Width = 130
                '
                'FilenameColumnHeader
                '
                Me.FilenameColumnHeader.Text = "Filename"
                Me.FilenameColumnHeader.Width = 200
                '
                'ArgumentsColumnHeader
                '
                Me.ArgumentsColumnHeader.Text = "Arguments"
                Me.ArgumentsColumnHeader.Width = 160
                '
                'WaitForExitColumnHeader
                '
                Me.WaitForExitColumnHeader.Text = "Wait for exit"
                Me.WaitForExitColumnHeader.Width = 75
                '
                'TryToIntegrateColumnHeader
                '
                Me.TryToIntegrateColumnHeader.Text = "Try To Integrate"
                Me.TryToIntegrateColumnHeader.Width = 95
                '
                'ToolsContextMenuStrip
                '
                Me.ToolsContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolMenuItem, Me.DeleteToolMenuItem, Me.ToolStripSeparator1, Me.LaunchToolMenuItem})
                Me.ToolsContextMenuStrip.Name = "cMenApps"
                Me.ToolsContextMenuStrip.Size = New System.Drawing.Size(221, 76)
                '
                'NewToolMenuItem
                '
                Me.NewToolMenuItem.Image = Global.mRemote3G.My.Resources.Resources.ExtApp_Add
                Me.NewToolMenuItem.Name = "NewToolMenuItem"
                Me.NewToolMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
                Me.NewToolMenuItem.Size = New System.Drawing.Size(220, 22)
                Me.NewToolMenuItem.Text = "New External Tool"
                '
                'DeleteToolMenuItem
                '
                Me.DeleteToolMenuItem.Image = Global.mRemote3G.My.Resources.Resources.ExtApp_Delete
                Me.DeleteToolMenuItem.Name = "DeleteToolMenuItem"
                Me.DeleteToolMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
                Me.DeleteToolMenuItem.Size = New System.Drawing.Size(220, 22)
                Me.DeleteToolMenuItem.Text = "Delete External Tool..."
                '
                'ToolStripSeparator1
                '
                Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
                Me.ToolStripSeparator1.Size = New System.Drawing.Size(217, 6)
                '
                'LaunchToolMenuItem
                '
                Me.LaunchToolMenuItem.Image = Global.mRemote3G.My.Resources.Resources.ExtApp_Start
                Me.LaunchToolMenuItem.Name = "LaunchToolMenuItem"
                Me.LaunchToolMenuItem.Size = New System.Drawing.Size(220, 22)
                Me.LaunchToolMenuItem.Text = "Launch External Tool"
                '
                'PropertiesGroupBox
                '
                Me.PropertiesGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.PropertiesGroupBox.Controls.Add(Me.TryToIntegrateCheckBox)
                Me.PropertiesGroupBox.Controls.Add(Me.OptionsLabel)
                Me.PropertiesGroupBox.Controls.Add(Me.WaitForExitCheckBox)
                Me.PropertiesGroupBox.Controls.Add(Me.BrowseButton)
                Me.PropertiesGroupBox.Controls.Add(Me.ArgumentsCheckBox)
                Me.PropertiesGroupBox.Controls.Add(Me.FilenameTextBox)
                Me.PropertiesGroupBox.Controls.Add(Me.DisplayNameTextBox)
                Me.PropertiesGroupBox.Controls.Add(Me.ArgumentsLabel)
                Me.PropertiesGroupBox.Controls.Add(Me.FilenameLabel)
                Me.PropertiesGroupBox.Controls.Add(Me.DisplayNameLabel)
                Me.PropertiesGroupBox.Enabled = False
                Me.PropertiesGroupBox.Location = New System.Drawing.Point(3, 163)
                Me.PropertiesGroupBox.Name = "PropertiesGroupBox"
                Me.PropertiesGroupBox.Size = New System.Drawing.Size(678, 132)
                Me.PropertiesGroupBox.TabIndex = 1
                Me.PropertiesGroupBox.TabStop = False
                Me.PropertiesGroupBox.Text = "External Tool Properties"
                '
                'TryToIntegrateCheckBox
                '
                Me.TryToIntegrateCheckBox.AutoSize = True
                Me.TryToIntegrateCheckBox.Location = New System.Drawing.Point(280, 106)
                Me.TryToIntegrateCheckBox.Name = "TryToIntegrateCheckBox"
                Me.TryToIntegrateCheckBox.Size = New System.Drawing.Size(97, 17)
                Me.TryToIntegrateCheckBox.TabIndex = 9
                Me.TryToIntegrateCheckBox.Text = "Try to integrate"
                Me.TryToIntegrateCheckBox.UseVisualStyleBackColor = True
                '
                'OptionsLabel
                '
                Me.OptionsLabel.AutoSize = True
                Me.OptionsLabel.Location = New System.Drawing.Point(58, 107)
                Me.OptionsLabel.Name = "OptionsLabel"
                Me.OptionsLabel.Size = New System.Drawing.Size(46, 13)
                Me.OptionsLabel.TabIndex = 7
                Me.OptionsLabel.Text = "Options:"
                Me.OptionsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
                '
                'WaitForExitCheckBox
                '
                Me.WaitForExitCheckBox.AutoSize = True
                Me.WaitForExitCheckBox.Location = New System.Drawing.Point(110, 106)
                Me.WaitForExitCheckBox.Name = "WaitForExitCheckBox"
                Me.WaitForExitCheckBox.Size = New System.Drawing.Size(82, 17)
                Me.WaitForExitCheckBox.TabIndex = 8
                Me.WaitForExitCheckBox.Text = "Wait for exit"
                Me.WaitForExitCheckBox.UseVisualStyleBackColor = True
                '
                'BrowseButton
                '
                Me.BrowseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.BrowseButton.Location = New System.Drawing.Point(574, 45)
                Me.BrowseButton.Name = "BrowseButton"
                Me.BrowseButton.Size = New System.Drawing.Size(95, 23)
                Me.BrowseButton.TabIndex = 4
                Me.BrowseButton.Text = "Browse..."
                Me.BrowseButton.UseVisualStyleBackColor = True
                '
                'ArgumentsCheckBox
                '
                Me.ArgumentsCheckBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.ArgumentsCheckBox.Location = New System.Drawing.Point(110, 76)
                Me.ArgumentsCheckBox.Name = "ArgumentsCheckBox"
                Me.ArgumentsCheckBox.Size = New System.Drawing.Size(559, 20)
                Me.ArgumentsCheckBox.TabIndex = 6
                '
                'FilenameTextBox
                '
                Me.FilenameTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.FilenameTextBox.Location = New System.Drawing.Point(110, 47)
                Me.FilenameTextBox.Name = "FilenameTextBox"
                Me.FilenameTextBox.Size = New System.Drawing.Size(458, 20)
                Me.FilenameTextBox.TabIndex = 3
                '
                'DisplayNameTextBox
                '
                Me.DisplayNameTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.DisplayNameTextBox.Location = New System.Drawing.Point(110, 19)
                Me.DisplayNameTextBox.Name = "DisplayNameTextBox"
                Me.DisplayNameTextBox.Size = New System.Drawing.Size(559, 20)
                Me.DisplayNameTextBox.TabIndex = 1
                '
                'ArgumentsLabel
                '
                Me.ArgumentsLabel.AutoSize = True
                Me.ArgumentsLabel.Location = New System.Drawing.Point(44, 79)
                Me.ArgumentsLabel.Name = "ArgumentsLabel"
                Me.ArgumentsLabel.Size = New System.Drawing.Size(60, 13)
                Me.ArgumentsLabel.TabIndex = 5
                Me.ArgumentsLabel.Text = "Arguments:"
                Me.ArgumentsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
                '
                'FilenameLabel
                '
                Me.FilenameLabel.AutoSize = True
                Me.FilenameLabel.Location = New System.Drawing.Point(52, 50)
                Me.FilenameLabel.Name = "FilenameLabel"
                Me.FilenameLabel.Size = New System.Drawing.Size(52, 13)
                Me.FilenameLabel.TabIndex = 2
                Me.FilenameLabel.Text = "Filename:"
                Me.FilenameLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
                '
                'DisplayNameLabel
                '
                Me.DisplayNameLabel.AutoSize = True
                Me.DisplayNameLabel.Location = New System.Drawing.Point(29, 22)
                Me.DisplayNameLabel.Name = "DisplayNameLabel"
                Me.DisplayNameLabel.Size = New System.Drawing.Size(75, 13)
                Me.DisplayNameLabel.TabIndex = 0
                Me.DisplayNameLabel.Text = "Display Name:"
                Me.DisplayNameLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
                '
                'ToolStripContainer
                '
                '
                'ToolStripContainer.ContentPanel
                '
                Me.ToolStripContainer.ContentPanel.Controls.Add(Me.PropertiesGroupBox)
                Me.ToolStripContainer.ContentPanel.Controls.Add(Me.ToolsListView)
                Me.ToolStripContainer.ContentPanel.Size = New System.Drawing.Size(684, 298)
                Me.ToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill
                Me.ToolStripContainer.Location = New System.Drawing.Point(0, 0)
                Me.ToolStripContainer.Name = "ToolStripContainer"
                Me.ToolStripContainer.Size = New System.Drawing.Size(684, 323)
                Me.ToolStripContainer.TabIndex = 0
                Me.ToolStripContainer.Text = "ToolStripContainer"
                '
                'ToolStripContainer.TopToolStripPanel
                '
                Me.ToolStripContainer.TopToolStripPanel.Controls.Add(Me.ToolStrip)
                '
                'ToolStrip
                '
                Me.ToolStrip.Dock = System.Windows.Forms.DockStyle.None
                Me.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
                Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolToolstripButton, Me.DeleteToolToolstripButton, Me.ToolStripSeparator2, Me.LaunchToolToolstripButton})
                Me.ToolStrip.Location = New System.Drawing.Point(3, 0)
                Me.ToolStrip.Name = "ToolStrip"
                Me.ToolStrip.Size = New System.Drawing.Size(186, 25)
                Me.ToolStrip.TabIndex = 0
                '
                'NewToolToolstripButton
                '
                Me.NewToolToolstripButton.Image = Global.mRemote3G.My.Resources.Resources.ExtApp_Add
                Me.NewToolToolstripButton.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.NewToolToolstripButton.Name = "NewToolToolstripButton"
                Me.NewToolToolstripButton.Size = New System.Drawing.Size(51, 22)
                Me.NewToolToolstripButton.Text = "New"
                '
                'DeleteToolToolstripButton
                '
                Me.DeleteToolToolstripButton.Image = Global.mRemote3G.My.Resources.Resources.ExtApp_Delete
                Me.DeleteToolToolstripButton.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.DeleteToolToolstripButton.Name = "DeleteToolToolstripButton"
                Me.DeleteToolToolstripButton.Size = New System.Drawing.Size(60, 22)
                Me.DeleteToolToolstripButton.Text = "Delete"
                '
                'ToolStripSeparator2
                '
                Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
                Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
                '
                'LaunchToolToolstripButton
                '
                Me.LaunchToolToolstripButton.Image = Global.mRemote3G.My.Resources.Resources.ExtApp_Start
                Me.LaunchToolToolstripButton.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.LaunchToolToolstripButton.Name = "LaunchToolToolstripButton"
                Me.LaunchToolToolstripButton.Size = New System.Drawing.Size(66, 22)
                Me.LaunchToolToolstripButton.Text = "Launch"
                '
                'ExternalTools
                '
                Me.ClientSize = New System.Drawing.Size(684, 323)
                Me.Controls.Add(Me.ToolStripContainer)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
                Me.Name = "ExternalTools"
                Me.TabText = "External Applications"
                Me.Text = "External Tools"
                Me.ToolsContextMenuStrip.ResumeLayout(False)
                Me.PropertiesGroupBox.ResumeLayout(False)
                Me.PropertiesGroupBox.PerformLayout()
                Me.ToolStripContainer.ContentPanel.ResumeLayout(False)
                Me.ToolStripContainer.TopToolStripPanel.ResumeLayout(False)
                Me.ToolStripContainer.TopToolStripPanel.PerformLayout()
                Me.ToolStripContainer.ResumeLayout(False)
                Me.ToolStripContainer.PerformLayout()
                Me.ToolStrip.ResumeLayout(False)
                Me.ToolStrip.PerformLayout()
                Me.ResumeLayout(False)

            End Sub
            Friend WithEvents ToolStripContainer As System.Windows.Forms.ToolStripContainer
            Friend WithEvents ToolStrip As System.Windows.Forms.ToolStrip
            Friend WithEvents NewToolToolstripButton As System.Windows.Forms.ToolStripButton
            Friend WithEvents DeleteToolToolstripButton As System.Windows.Forms.ToolStripButton
            Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents LaunchToolToolstripButton As System.Windows.Forms.ToolStripButton
#End Region
        End Class
    End Namespace
End Namespace
