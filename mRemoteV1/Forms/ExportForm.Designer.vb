Namespace Forms
    Partial Public Class ExportForm
        Inherits Form
#Region " Windows Form Designer generated code "

        Private Sub InitializeComponent()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.btnOK = New System.Windows.Forms.Button()
            Me.lblUncheckProperties = New System.Windows.Forms.Label()
            Me.chkUsername = New System.Windows.Forms.CheckBox()
            Me.chkPassword = New System.Windows.Forms.CheckBox()
            Me.chkDomain = New System.Windows.Forms.CheckBox()
            Me.chkInheritance = New System.Windows.Forms.CheckBox()
            Me.txtFileName = New System.Windows.Forms.TextBox()
            Me.btnBrowse = New System.Windows.Forms.Button()
            Me.grpProperties = New System.Windows.Forms.GroupBox()
            Me.grpFile = New System.Windows.Forms.GroupBox()
            Me.lblFileFormat = New System.Windows.Forms.Label()
            Me.lblFileName = New System.Windows.Forms.Label()
            Me.cboFileFormat = New System.Windows.Forms.ComboBox()
            Me.grpItems = New System.Windows.Forms.GroupBox()
            Me.lblSelectedConnection = New System.Windows.Forms.Label()
            Me.lblSelectedFolder = New System.Windows.Forms.Label()
            Me.rdoExportSelectedConnection = New System.Windows.Forms.RadioButton()
            Me.rdoExportSelectedFolder = New System.Windows.Forms.RadioButton()
            Me.rdoExportEverything = New System.Windows.Forms.RadioButton()
            Me.grpProperties.SuspendLayout()
            Me.grpFile.SuspendLayout()
            Me.grpItems.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(447, 473)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 3
            Me.btnCancel.Text = "&Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnOK
            '
            Me.btnOK.Location = New System.Drawing.Point(366, 473)
            Me.btnOK.Name = "btnOK"
            Me.btnOK.Size = New System.Drawing.Size(75, 23)
            Me.btnOK.TabIndex = 2
            Me.btnOK.Text = "&OK"
            Me.btnOK.UseVisualStyleBackColor = True
            '
            'lblUncheckProperties
            '
            Me.lblUncheckProperties.AutoSize = True
            Me.lblUncheckProperties.Location = New System.Drawing.Point(12, 134)
            Me.lblUncheckProperties.Name = "lblUncheckProperties"
            Me.lblUncheckProperties.Size = New System.Drawing.Size(244, 13)
            Me.lblUncheckProperties.TabIndex = 4
            Me.lblUncheckProperties.Text = "Uncheck the properties you want not to be saved!"
            '
            'chkUsername
            '
            Me.chkUsername.AutoSize = True
            Me.chkUsername.Checked = True
            Me.chkUsername.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkUsername.Location = New System.Drawing.Point(15, 32)
            Me.chkUsername.Name = "chkUsername"
            Me.chkUsername.Size = New System.Drawing.Size(74, 17)
            Me.chkUsername.TabIndex = 0
            Me.chkUsername.Text = "Username"
            Me.chkUsername.UseVisualStyleBackColor = True
            '
            'chkPassword
            '
            Me.chkPassword.AutoSize = True
            Me.chkPassword.Checked = True
            Me.chkPassword.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkPassword.Location = New System.Drawing.Point(15, 55)
            Me.chkPassword.Name = "chkPassword"
            Me.chkPassword.Size = New System.Drawing.Size(72, 17)
            Me.chkPassword.TabIndex = 1
            Me.chkPassword.Text = "Password"
            Me.chkPassword.UseVisualStyleBackColor = True
            '
            'chkDomain
            '
            Me.chkDomain.AutoSize = True
            Me.chkDomain.Checked = True
            Me.chkDomain.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkDomain.Location = New System.Drawing.Point(15, 78)
            Me.chkDomain.Name = "chkDomain"
            Me.chkDomain.Size = New System.Drawing.Size(62, 17)
            Me.chkDomain.TabIndex = 2
            Me.chkDomain.Text = "Domain"
            Me.chkDomain.UseVisualStyleBackColor = True
            '
            'chkInheritance
            '
            Me.chkInheritance.AutoSize = True
            Me.chkInheritance.Checked = True
            Me.chkInheritance.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkInheritance.Location = New System.Drawing.Point(15, 101)
            Me.chkInheritance.Name = "chkInheritance"
            Me.chkInheritance.Size = New System.Drawing.Size(79, 17)
            Me.chkInheritance.TabIndex = 3
            Me.chkInheritance.Text = "Inheritance"
            Me.chkInheritance.UseVisualStyleBackColor = True
            '
            'txtFileName
            '
            Me.txtFileName.Location = New System.Drawing.Point(15, 48)
            Me.txtFileName.Name = "txtFileName"
            Me.txtFileName.Size = New System.Drawing.Size(396, 20)
            Me.txtFileName.TabIndex = 1
            '
            'btnBrowse
            '
            Me.btnBrowse.Location = New System.Drawing.Point(417, 46)
            Me.btnBrowse.Name = "btnBrowse"
            Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
            Me.btnBrowse.TabIndex = 2
            Me.btnBrowse.Text = "&Browse"
            Me.btnBrowse.UseVisualStyleBackColor = True
            '
            'grpProperties
            '
            Me.grpProperties.Controls.Add(Me.lblUncheckProperties)
            Me.grpProperties.Controls.Add(Me.chkInheritance)
            Me.grpProperties.Controls.Add(Me.chkUsername)
            Me.grpProperties.Controls.Add(Me.chkDomain)
            Me.grpProperties.Controls.Add(Me.chkPassword)
            Me.grpProperties.Location = New System.Drawing.Point(12, 304)
            Me.grpProperties.Name = "grpProperties"
            Me.grpProperties.Size = New System.Drawing.Size(510, 163)
            Me.grpProperties.TabIndex = 1
            Me.grpProperties.TabStop = False
            Me.grpProperties.Text = "Export Properties"
            '
            'grpFile
            '
            Me.grpFile.Controls.Add(Me.lblFileFormat)
            Me.grpFile.Controls.Add(Me.lblFileName)
            Me.grpFile.Controls.Add(Me.cboFileFormat)
            Me.grpFile.Controls.Add(Me.txtFileName)
            Me.grpFile.Controls.Add(Me.btnBrowse)
            Me.grpFile.Location = New System.Drawing.Point(12, 12)
            Me.grpFile.Name = "grpFile"
            Me.grpFile.Size = New System.Drawing.Size(510, 140)
            Me.grpFile.TabIndex = 0
            Me.grpFile.TabStop = False
            Me.grpFile.Text = "Export File"
            '
            'lblFileFormat
            '
            Me.lblFileFormat.AutoSize = True
            Me.lblFileFormat.Location = New System.Drawing.Point(12, 84)
            Me.lblFileFormat.Name = "lblFileFormat"
            Me.lblFileFormat.Size = New System.Drawing.Size(61, 13)
            Me.lblFileFormat.TabIndex = 3
            Me.lblFileFormat.Text = "File &Format:"
            '
            'lblFileName
            '
            Me.lblFileName.AutoSize = True
            Me.lblFileName.Location = New System.Drawing.Point(12, 32)
            Me.lblFileName.Name = "lblFileName"
            Me.lblFileName.Size = New System.Drawing.Size(52, 13)
            Me.lblFileName.TabIndex = 0
            Me.lblFileName.Text = "Filename:"
            '
            'cboFileFormat
            '
            Me.cboFileFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboFileFormat.FormattingEnabled = True
            Me.cboFileFormat.Location = New System.Drawing.Point(15, 100)
            Me.cboFileFormat.Name = "cboFileFormat"
            Me.cboFileFormat.Size = New System.Drawing.Size(294, 21)
            Me.cboFileFormat.TabIndex = 4
            '
            'grpItems
            '
            Me.grpItems.Controls.Add(Me.lblSelectedConnection)
            Me.grpItems.Controls.Add(Me.lblSelectedFolder)
            Me.grpItems.Controls.Add(Me.rdoExportSelectedConnection)
            Me.grpItems.Controls.Add(Me.rdoExportSelectedFolder)
            Me.grpItems.Controls.Add(Me.rdoExportEverything)
            Me.grpItems.Location = New System.Drawing.Point(12, 158)
            Me.grpItems.Name = "grpItems"
            Me.grpItems.Size = New System.Drawing.Size(510, 140)
            Me.grpItems.TabIndex = 4
            Me.grpItems.TabStop = False
            Me.grpItems.Text = "Export Items"
            '
            'lblSelectedConnection
            '
            Me.lblSelectedConnection.AutoSize = True
            Me.lblSelectedConnection.Location = New System.Drawing.Point(48, 111)
            Me.lblSelectedConnection.Name = "lblSelectedConnection"
            Me.lblSelectedConnection.Size = New System.Drawing.Size(92, 13)
            Me.lblSelectedConnection.TabIndex = 4
            Me.lblSelectedConnection.Text = "Connection Name"
            '
            'lblSelectedFolder
            '
            Me.lblSelectedFolder.AutoSize = True
            Me.lblSelectedFolder.Location = New System.Drawing.Point(48, 75)
            Me.lblSelectedFolder.Name = "lblSelectedFolder"
            Me.lblSelectedFolder.Size = New System.Drawing.Size(67, 13)
            Me.lblSelectedFolder.TabIndex = 3
            Me.lblSelectedFolder.Text = "Folder Name"
            '
            'rdoExportSelectedConnection
            '
            Me.rdoExportSelectedConnection.AutoSize = True
            Me.rdoExportSelectedConnection.Location = New System.Drawing.Point(15, 91)
            Me.rdoExportSelectedConnection.Name = "rdoExportSelectedConnection"
            Me.rdoExportSelectedConnection.Size = New System.Drawing.Size(215, 17)
            Me.rdoExportSelectedConnection.TabIndex = 2
            Me.rdoExportSelectedConnection.TabStop = True
            Me.rdoExportSelectedConnection.Text = "Export the currently selected connection"
            Me.rdoExportSelectedConnection.UseVisualStyleBackColor = True
            '
            'rdoExportSelectedFolder
            '
            Me.rdoExportSelectedFolder.AutoSize = True
            Me.rdoExportSelectedFolder.Location = New System.Drawing.Point(15, 55)
            Me.rdoExportSelectedFolder.Name = "rdoExportSelectedFolder"
            Me.rdoExportSelectedFolder.Size = New System.Drawing.Size(188, 17)
            Me.rdoExportSelectedFolder.TabIndex = 1
            Me.rdoExportSelectedFolder.TabStop = True
            Me.rdoExportSelectedFolder.Text = "Export the currently selected folder"
            Me.rdoExportSelectedFolder.UseVisualStyleBackColor = True
            '
            'rdoExportEverything
            '
            Me.rdoExportEverything.AutoSize = True
            Me.rdoExportEverything.Checked = True
            Me.rdoExportEverything.Location = New System.Drawing.Point(15, 32)
            Me.rdoExportEverything.Name = "rdoExportEverything"
            Me.rdoExportEverything.Size = New System.Drawing.Size(107, 17)
            Me.rdoExportEverything.TabIndex = 0
            Me.rdoExportEverything.TabStop = True
            Me.rdoExportEverything.Text = "Export everything"
            Me.rdoExportEverything.UseVisualStyleBackColor = True
            '
            'ExportForm
            '
            Me.AcceptButton = Me.btnOK
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(534, 508)
            Me.Controls.Add(Me.grpItems)
            Me.Controls.Add(Me.grpFile)
            Me.Controls.Add(Me.grpProperties)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOK)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Icon = Global.mRemote3G.My.Resources.Resources.Connections_SaveAs_Icon
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "ExportForm"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Export Connections"
            Me.grpProperties.ResumeLayout(False)
            Me.grpProperties.PerformLayout()
            Me.grpFile.ResumeLayout(False)
            Me.grpFile.PerformLayout()
            Me.grpItems.ResumeLayout(False)
            Me.grpItems.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents btnCancel As System.Windows.Forms.Button
        Private WithEvents btnOK As System.Windows.Forms.Button
        Private WithEvents lblUncheckProperties As System.Windows.Forms.Label
        Private WithEvents chkUsername As System.Windows.Forms.CheckBox
        Private WithEvents chkPassword As System.Windows.Forms.CheckBox
        Private WithEvents chkDomain As System.Windows.Forms.CheckBox
        Private WithEvents chkInheritance As System.Windows.Forms.CheckBox
        Private WithEvents txtFileName As System.Windows.Forms.TextBox
        Private WithEvents btnBrowse As System.Windows.Forms.Button
        Private WithEvents grpProperties As System.Windows.Forms.GroupBox
        Private WithEvents grpFile As System.Windows.Forms.GroupBox
        Private WithEvents lblFileFormat As System.Windows.Forms.Label
        Private WithEvents lblFileName As System.Windows.Forms.Label
        Private WithEvents cboFileFormat As System.Windows.Forms.ComboBox
        Private WithEvents grpItems As System.Windows.Forms.GroupBox
        Private WithEvents lblSelectedConnection As System.Windows.Forms.Label
        Private WithEvents lblSelectedFolder As System.Windows.Forms.Label
        Private WithEvents rdoExportSelectedConnection As System.Windows.Forms.RadioButton
        Private WithEvents rdoExportSelectedFolder As System.Windows.Forms.RadioButton
        Private WithEvents rdoExportEverything As System.Windows.Forms.RadioButton
#End Region
    End Class
End Namespace
