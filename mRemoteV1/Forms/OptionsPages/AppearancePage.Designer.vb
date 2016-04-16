Namespace Forms.OptionsPages
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class AppearancePage
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AppearancePage))
            Me.lblLanguageRestartRequired = New System.Windows.Forms.Label()
            Me.cboLanguage = New System.Windows.Forms.ComboBox()
            Me.lblLanguage = New System.Windows.Forms.Label()
            Me.chkShowFullConnectionsFilePathInTitle = New System.Windows.Forms.CheckBox()
            Me.chkShowDescriptionTooltipsInTree = New System.Windows.Forms.CheckBox()
            Me.chkShowSystemTrayIcon = New System.Windows.Forms.CheckBox()
            Me.chkMinimizeToSystemTray = New System.Windows.Forms.CheckBox()
            Me.SuspendLayout()
            '
            'lblLanguageRestartRequired
            '
            Me.lblLanguageRestartRequired.AutoSize = True
            Me.lblLanguageRestartRequired.Location = New System.Drawing.Point(3, 56)
            Me.lblLanguageRestartRequired.Name = "lblLanguageRestartRequired"
            Me.lblLanguageRestartRequired.Size = New System.Drawing.Size(380, 13)
            Me.lblLanguageRestartRequired.TabIndex = 9
            Me.lblLanguageRestartRequired.Text = "mRemote3G must be restarted before changes to the language will take effect."
            '
            'cboLanguage
            '
            Me.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboLanguage.FormattingEnabled = True
            Me.cboLanguage.Location = New System.Drawing.Point(3, 24)
            Me.cboLanguage.Name = "cboLanguage"
            Me.cboLanguage.Size = New System.Drawing.Size(304, 21)
            Me.cboLanguage.Sorted = True
            Me.cboLanguage.TabIndex = 8
            '
            'lblLanguage
            '
            Me.lblLanguage.AutoSize = True
            Me.lblLanguage.Location = New System.Drawing.Point(3, 0)
            Me.lblLanguage.Name = "lblLanguage"
            Me.lblLanguage.Size = New System.Drawing.Size(55, 13)
            Me.lblLanguage.TabIndex = 7
            Me.lblLanguage.Text = "Language"
            '
            'chkShowFullConnectionsFilePathInTitle
            '
            Me.chkShowFullConnectionsFilePathInTitle.AutoSize = True
            Me.chkShowFullConnectionsFilePathInTitle.Location = New System.Drawing.Point(3, 141)
            Me.chkShowFullConnectionsFilePathInTitle.Name = "chkShowFullConnectionsFilePathInTitle"
            Me.chkShowFullConnectionsFilePathInTitle.Size = New System.Drawing.Size(239, 17)
            Me.chkShowFullConnectionsFilePathInTitle.TabIndex = 11
            Me.chkShowFullConnectionsFilePathInTitle.Text = "Show full connections file path in window title"
            Me.chkShowFullConnectionsFilePathInTitle.UseVisualStyleBackColor = True
            '
            'chkShowDescriptionTooltipsInTree
            '
            Me.chkShowDescriptionTooltipsInTree.AutoSize = True
            Me.chkShowDescriptionTooltipsInTree.Location = New System.Drawing.Point(3, 118)
            Me.chkShowDescriptionTooltipsInTree.Name = "chkShowDescriptionTooltipsInTree"
            Me.chkShowDescriptionTooltipsInTree.Size = New System.Drawing.Size(231, 17)
            Me.chkShowDescriptionTooltipsInTree.TabIndex = 10
            Me.chkShowDescriptionTooltipsInTree.Text = "Show description tooltips in connection tree"
            Me.chkShowDescriptionTooltipsInTree.UseVisualStyleBackColor = True
            '
            'chkShowSystemTrayIcon
            '
            Me.chkShowSystemTrayIcon.AutoSize = True
            Me.chkShowSystemTrayIcon.Location = New System.Drawing.Point(3, 187)
            Me.chkShowSystemTrayIcon.Name = "chkShowSystemTrayIcon"
            Me.chkShowSystemTrayIcon.Size = New System.Drawing.Size(172, 17)
            Me.chkShowSystemTrayIcon.TabIndex = 12
            Me.chkShowSystemTrayIcon.Text = "Always show System Tray Icon"
            Me.chkShowSystemTrayIcon.UseVisualStyleBackColor = True
            '
            'chkMinimizeToSystemTray
            '
            Me.chkMinimizeToSystemTray.AutoSize = True
            Me.chkMinimizeToSystemTray.Location = New System.Drawing.Point(3, 210)
            Me.chkMinimizeToSystemTray.Name = "chkMinimizeToSystemTray"
            Me.chkMinimizeToSystemTray.Size = New System.Drawing.Size(139, 17)
            Me.chkMinimizeToSystemTray.TabIndex = 13
            Me.chkMinimizeToSystemTray.Text = "Minimize to System Tray"
            Me.chkMinimizeToSystemTray.UseVisualStyleBackColor = True
            '
            'AppearancePage
            '
            Me.Controls.Add(Me.lblLanguageRestartRequired)
            Me.Controls.Add(Me.cboLanguage)
            Me.Controls.Add(Me.lblLanguage)
            Me.Controls.Add(Me.chkShowFullConnectionsFilePathInTitle)
            Me.Controls.Add(Me.chkShowDescriptionTooltipsInTree)
            Me.Controls.Add(Me.chkShowSystemTrayIcon)
            Me.Controls.Add(Me.chkMinimizeToSystemTray)
            Me.Name = "AppearancePage"
            Me.PageIcon = CType(resources.GetObject("$this.PageIcon"), System.Drawing.Icon)
            Me.Size = New System.Drawing.Size(610, 489)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblLanguageRestartRequired As System.Windows.Forms.Label
        Friend WithEvents cboLanguage As System.Windows.Forms.ComboBox
        Friend WithEvents lblLanguage As System.Windows.Forms.Label
        Friend WithEvents chkShowFullConnectionsFilePathInTitle As System.Windows.Forms.CheckBox
        Friend WithEvents chkShowDescriptionTooltipsInTree As System.Windows.Forms.CheckBox
        Friend WithEvents chkShowSystemTrayIcon As System.Windows.Forms.CheckBox
        Friend WithEvents chkMinimizeToSystemTray As System.Windows.Forms.CheckBox

    End Class
End Namespace