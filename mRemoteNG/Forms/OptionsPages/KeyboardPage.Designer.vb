Namespace Forms.OptionsPages
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class KeyboardPage
        Inherits OptionsPage

        'UserControl overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim LineGroupBox As System.Windows.Forms.GroupBox
            Dim Alignment1 As mRemoteNG.Controls.Alignment = New mRemoteNG.Controls.Alignment()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(KeyboardPage))
            Me.btnDeleteKeyboardShortcut = New System.Windows.Forms.Button()
            Me.btnNewKeyboardShortcut = New System.Windows.Forms.Button()
            Me.grpModifyKeyboardShortcut = New System.Windows.Forms.GroupBox()
            Me.hotModifyKeyboardShortcut = New SharedLibraryNG.HotkeyControl()
            Me.btnResetAllKeyboardShortcuts = New System.Windows.Forms.Button()
            Me.btnResetKeyboardShortcuts = New System.Windows.Forms.Button()
            Me.lblKeyboardCommand = New System.Windows.Forms.Label()
            Me.lstKeyboardShortcuts = New System.Windows.Forms.ListBox()
            Me.lblKeyboardShortcuts = New System.Windows.Forms.Label()
            Me.lvKeyboardCommands = New mRemoteNG.Controls.ListView()
            LineGroupBox = New System.Windows.Forms.GroupBox()
            Me.grpModifyKeyboardShortcut.SuspendLayout()
            Me.SuspendLayout()
            '
            'LineGroupBox
            '
            LineGroupBox.Location = New System.Drawing.Point(212, 20)
            LineGroupBox.Name = "LineGroupBox"
            LineGroupBox.Size = New System.Drawing.Size(398, 3)
            LineGroupBox.TabIndex = 19
            LineGroupBox.TabStop = False
            '
            'btnDeleteKeyboardShortcut
            '
            Me.btnDeleteKeyboardShortcut.Location = New System.Drawing.Point(293, 151)
            Me.btnDeleteKeyboardShortcut.Name = "btnDeleteKeyboardShortcut"
            Me.btnDeleteKeyboardShortcut.Size = New System.Drawing.Size(75, 23)
            Me.btnDeleteKeyboardShortcut.TabIndex = 15
            Me.btnDeleteKeyboardShortcut.Text = "&Delete"
            Me.btnDeleteKeyboardShortcut.UseVisualStyleBackColor = True
            '
            'btnNewKeyboardShortcut
            '
            Me.btnNewKeyboardShortcut.Location = New System.Drawing.Point(212, 151)
            Me.btnNewKeyboardShortcut.Name = "btnNewKeyboardShortcut"
            Me.btnNewKeyboardShortcut.Size = New System.Drawing.Size(75, 23)
            Me.btnNewKeyboardShortcut.TabIndex = 14
            Me.btnNewKeyboardShortcut.Text = "&New"
            Me.btnNewKeyboardShortcut.UseVisualStyleBackColor = True
            '
            'grpModifyKeyboardShortcut
            '
            Me.grpModifyKeyboardShortcut.Controls.Add(Me.hotModifyKeyboardShortcut)
            Me.grpModifyKeyboardShortcut.Location = New System.Drawing.Point(212, 180)
            Me.grpModifyKeyboardShortcut.Name = "grpModifyKeyboardShortcut"
            Me.grpModifyKeyboardShortcut.Size = New System.Drawing.Size(398, 103)
            Me.grpModifyKeyboardShortcut.TabIndex = 17
            Me.grpModifyKeyboardShortcut.TabStop = False
            Me.grpModifyKeyboardShortcut.Text = "Modify Shortcut"
            '
            'hotModifyKeyboardShortcut
            '
            Me.hotModifyKeyboardShortcut.HotkeyModifiers = System.Windows.Forms.Keys.None
            Me.hotModifyKeyboardShortcut.KeyCode = System.Windows.Forms.Keys.None
            Me.hotModifyKeyboardShortcut.Location = New System.Drawing.Point(27, 41)
            Me.hotModifyKeyboardShortcut.Name = "hotModifyKeyboardShortcut"
            Me.hotModifyKeyboardShortcut.Size = New System.Drawing.Size(344, 20)
            Me.hotModifyKeyboardShortcut.TabIndex = 0
            Me.hotModifyKeyboardShortcut.Text = "None"
            '
            'btnResetAllKeyboardShortcuts
            '
            Me.btnResetAllKeyboardShortcuts.Location = New System.Drawing.Point(3, 466)
            Me.btnResetAllKeyboardShortcuts.Name = "btnResetAllKeyboardShortcuts"
            Me.btnResetAllKeyboardShortcuts.Size = New System.Drawing.Size(120, 23)
            Me.btnResetAllKeyboardShortcuts.TabIndex = 18
            Me.btnResetAllKeyboardShortcuts.Text = "Reset &All to Default"
            Me.btnResetAllKeyboardShortcuts.UseVisualStyleBackColor = True
            '
            'btnResetKeyboardShortcuts
            '
            Me.btnResetKeyboardShortcuts.Location = New System.Drawing.Point(490, 151)
            Me.btnResetKeyboardShortcuts.Name = "btnResetKeyboardShortcuts"
            Me.btnResetKeyboardShortcuts.Size = New System.Drawing.Size(120, 23)
            Me.btnResetKeyboardShortcuts.TabIndex = 16
            Me.btnResetKeyboardShortcuts.Text = "&Reset to Default"
            Me.btnResetKeyboardShortcuts.UseVisualStyleBackColor = True
            '
            'lblKeyboardCommand
            '
            Me.lblKeyboardCommand.AutoSize = True
            Me.lblKeyboardCommand.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblKeyboardCommand.Location = New System.Drawing.Point(209, 0)
            Me.lblKeyboardCommand.Name = "lblKeyboardCommand"
            Me.lblKeyboardCommand.Size = New System.Drawing.Size(71, 17)
            Me.lblKeyboardCommand.TabIndex = 11
            Me.lblKeyboardCommand.Text = "Command"
            '
            'lstKeyboardShortcuts
            '
            Me.lstKeyboardShortcuts.FormattingEnabled = True
            Me.lstKeyboardShortcuts.Location = New System.Drawing.Point(212, 50)
            Me.lstKeyboardShortcuts.Name = "lstKeyboardShortcuts"
            Me.lstKeyboardShortcuts.Size = New System.Drawing.Size(398, 95)
            Me.lstKeyboardShortcuts.TabIndex = 13
            '
            'lblKeyboardShortcuts
            '
            Me.lblKeyboardShortcuts.AutoSize = True
            Me.lblKeyboardShortcuts.Location = New System.Drawing.Point(209, 34)
            Me.lblKeyboardShortcuts.Name = "lblKeyboardShortcuts"
            Me.lblKeyboardShortcuts.Size = New System.Drawing.Size(100, 13)
            Me.lblKeyboardShortcuts.TabIndex = 12
            Me.lblKeyboardShortcuts.Text = "Keyboard Shortcuts"
            '
            'lvKeyboardCommands
            '
            Me.lvKeyboardCommands.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lvKeyboardCommands.HideSelection = False
            Me.lvKeyboardCommands.InactiveHighlightBackColor = System.Drawing.SystemColors.Highlight
            Me.lvKeyboardCommands.InactiveHighlightBorderColor = System.Drawing.SystemColors.HotTrack
            Me.lvKeyboardCommands.InactiveHighlightForeColor = System.Drawing.SystemColors.HighlightText
            Alignment1.Horizontal = mRemoteNG.Controls.HorizontalAlignment.Left
            Alignment1.Vertical = mRemoteNG.Controls.VerticalAlignment.Middle
            Me.lvKeyboardCommands.LabelAlignment = Alignment1
            Me.lvKeyboardCommands.LabelWrap = False
            Me.lvKeyboardCommands.Location = New System.Drawing.Point(3, 0)
            Me.lvKeyboardCommands.MultiSelect = False
            Me.lvKeyboardCommands.Name = "lvKeyboardCommands"
            Me.lvKeyboardCommands.OwnerDraw = True
            Me.lvKeyboardCommands.Size = New System.Drawing.Size(200, 460)
            Me.lvKeyboardCommands.TabIndex = 10
            Me.lvKeyboardCommands.TileSize = New System.Drawing.Size(196, 20)
            Me.lvKeyboardCommands.UseCompatibleStateImageBehavior = False
            Me.lvKeyboardCommands.View = System.Windows.Forms.View.Tile
            '
            'KeyboardPage
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(LineGroupBox)
            Me.Controls.Add(Me.btnDeleteKeyboardShortcut)
            Me.Controls.Add(Me.btnNewKeyboardShortcut)
            Me.Controls.Add(Me.grpModifyKeyboardShortcut)
            Me.Controls.Add(Me.btnResetAllKeyboardShortcuts)
            Me.Controls.Add(Me.btnResetKeyboardShortcuts)
            Me.Controls.Add(Me.lblKeyboardCommand)
            Me.Controls.Add(Me.lstKeyboardShortcuts)
            Me.Controls.Add(Me.lblKeyboardShortcuts)
            Me.Controls.Add(Me.lvKeyboardCommands)
            Me.Name = "KeyboardPage"
            Me.PageIcon = CType(resources.GetObject("$this.PageIcon"), System.Drawing.Icon)
            Me.Size = New System.Drawing.Size(610, 489)
            Me.grpModifyKeyboardShortcut.ResumeLayout(False)
            Me.grpModifyKeyboardShortcut.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents btnDeleteKeyboardShortcut As System.Windows.Forms.Button
        Friend WithEvents btnNewKeyboardShortcut As System.Windows.Forms.Button
        Friend WithEvents grpModifyKeyboardShortcut As System.Windows.Forms.GroupBox
        Friend WithEvents hotModifyKeyboardShortcut As SharedLibraryNG.HotkeyControl
        Friend WithEvents btnResetAllKeyboardShortcuts As System.Windows.Forms.Button
        Friend WithEvents btnResetKeyboardShortcuts As System.Windows.Forms.Button
        Friend WithEvents lblKeyboardCommand As System.Windows.Forms.Label
        Friend WithEvents lstKeyboardShortcuts As System.Windows.Forms.ListBox
        Friend WithEvents lblKeyboardShortcuts As System.Windows.Forms.Label
        Friend WithEvents lvKeyboardCommands As mRemoteNG.Controls.ListView

    End Class
End Namespace