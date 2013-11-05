Namespace Forms.OptionsPages

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class SqlServerPage
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SqlServerPage))
            Me.lblSQLDatabaseName = New System.Windows.Forms.Label()
            Me.txtSQLDatabaseName = New System.Windows.Forms.TextBox()
            Me.lblExperimental = New System.Windows.Forms.Label()
            Me.chkUseSQLServer = New System.Windows.Forms.CheckBox()
            Me.lblSQLUsername = New System.Windows.Forms.Label()
            Me.txtSQLPassword = New System.Windows.Forms.TextBox()
            Me.lblSQLInfo = New System.Windows.Forms.Label()
            Me.lblSQLServer = New System.Windows.Forms.Label()
            Me.txtSQLUsername = New System.Windows.Forms.TextBox()
            Me.txtSQLServer = New System.Windows.Forms.TextBox()
            Me.lblSQLPassword = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'lblSQLDatabaseName
            '
            Me.lblSQLDatabaseName.Enabled = False
            Me.lblSQLDatabaseName.Location = New System.Drawing.Point(23, 132)
            Me.lblSQLDatabaseName.Name = "lblSQLDatabaseName"
            Me.lblSQLDatabaseName.Size = New System.Drawing.Size(111, 13)
            Me.lblSQLDatabaseName.TabIndex = 16
            Me.lblSQLDatabaseName.Text = "Database:"
            Me.lblSQLDatabaseName.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'txtSQLDatabaseName
            '
            Me.txtSQLDatabaseName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtSQLDatabaseName.Enabled = False
            Me.txtSQLDatabaseName.Location = New System.Drawing.Point(140, 130)
            Me.txtSQLDatabaseName.Name = "txtSQLDatabaseName"
            Me.txtSQLDatabaseName.Size = New System.Drawing.Size(153, 20)
            Me.txtSQLDatabaseName.TabIndex = 17
            '
            'lblExperimental
            '
            Me.lblExperimental.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblExperimental.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World)
            Me.lblExperimental.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.lblExperimental.Location = New System.Drawing.Point(3, 0)
            Me.lblExperimental.Name = "lblExperimental"
            Me.lblExperimental.Size = New System.Drawing.Size(596, 25)
            Me.lblExperimental.TabIndex = 11
            Me.lblExperimental.Text = "EXPERIMENTAL"
            Me.lblExperimental.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'chkUseSQLServer
            '
            Me.chkUseSQLServer.AutoSize = True
            Me.chkUseSQLServer.Location = New System.Drawing.Point(3, 76)
            Me.chkUseSQLServer.Name = "chkUseSQLServer"
            Me.chkUseSQLServer.Size = New System.Drawing.Size(234, 17)
            Me.chkUseSQLServer.TabIndex = 13
            Me.chkUseSQLServer.Text = "Use SQL Server to load && save connections"
            Me.chkUseSQLServer.UseVisualStyleBackColor = True
            '
            'lblSQLUsername
            '
            Me.lblSQLUsername.Enabled = False
            Me.lblSQLUsername.Location = New System.Drawing.Point(23, 158)
            Me.lblSQLUsername.Name = "lblSQLUsername"
            Me.lblSQLUsername.Size = New System.Drawing.Size(111, 13)
            Me.lblSQLUsername.TabIndex = 18
            Me.lblSQLUsername.Text = "Username:"
            Me.lblSQLUsername.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'txtSQLPassword
            '
            Me.txtSQLPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtSQLPassword.Enabled = False
            Me.txtSQLPassword.Location = New System.Drawing.Point(140, 182)
            Me.txtSQLPassword.Name = "txtSQLPassword"
            Me.txtSQLPassword.Size = New System.Drawing.Size(153, 20)
            Me.txtSQLPassword.TabIndex = 21
            Me.txtSQLPassword.UseSystemPasswordChar = True
            '
            'lblSQLInfo
            '
            Me.lblSQLInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblSQLInfo.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World)
            Me.lblSQLInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.lblSQLInfo.Location = New System.Drawing.Point(3, 25)
            Me.lblSQLInfo.Name = "lblSQLInfo"
            Me.lblSQLInfo.Size = New System.Drawing.Size(596, 25)
            Me.lblSQLInfo.TabIndex = 12
            Me.lblSQLInfo.Text = "Please see Help - Getting started - SQL Configuration for more Info!"
            Me.lblSQLInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblSQLServer
            '
            Me.lblSQLServer.Enabled = False
            Me.lblSQLServer.Location = New System.Drawing.Point(23, 106)
            Me.lblSQLServer.Name = "lblSQLServer"
            Me.lblSQLServer.Size = New System.Drawing.Size(111, 13)
            Me.lblSQLServer.TabIndex = 14
            Me.lblSQLServer.Text = "SQL Server:"
            Me.lblSQLServer.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'txtSQLUsername
            '
            Me.txtSQLUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtSQLUsername.Enabled = False
            Me.txtSQLUsername.Location = New System.Drawing.Point(140, 156)
            Me.txtSQLUsername.Name = "txtSQLUsername"
            Me.txtSQLUsername.Size = New System.Drawing.Size(153, 20)
            Me.txtSQLUsername.TabIndex = 19
            '
            'txtSQLServer
            '
            Me.txtSQLServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtSQLServer.Enabled = False
            Me.txtSQLServer.Location = New System.Drawing.Point(140, 103)
            Me.txtSQLServer.Name = "txtSQLServer"
            Me.txtSQLServer.Size = New System.Drawing.Size(153, 20)
            Me.txtSQLServer.TabIndex = 15
            '
            'lblSQLPassword
            '
            Me.lblSQLPassword.Enabled = False
            Me.lblSQLPassword.Location = New System.Drawing.Point(23, 184)
            Me.lblSQLPassword.Name = "lblSQLPassword"
            Me.lblSQLPassword.Size = New System.Drawing.Size(111, 13)
            Me.lblSQLPassword.TabIndex = 20
            Me.lblSQLPassword.Text = "Password:"
            Me.lblSQLPassword.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'SqlServerPage
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.lblSQLDatabaseName)
            Me.Controls.Add(Me.txtSQLDatabaseName)
            Me.Controls.Add(Me.lblExperimental)
            Me.Controls.Add(Me.chkUseSQLServer)
            Me.Controls.Add(Me.lblSQLUsername)
            Me.Controls.Add(Me.txtSQLPassword)
            Me.Controls.Add(Me.lblSQLInfo)
            Me.Controls.Add(Me.lblSQLServer)
            Me.Controls.Add(Me.txtSQLUsername)
            Me.Controls.Add(Me.txtSQLServer)
            Me.Controls.Add(Me.lblSQLPassword)
            Me.Name = "SqlServerPage"
            Me.PageIcon = CType(resources.GetObject("$this.PageIcon"), System.Drawing.Icon)
            Me.Size = New System.Drawing.Size(610, 489)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents lblSQLDatabaseName As System.Windows.Forms.Label
        Friend WithEvents txtSQLDatabaseName As System.Windows.Forms.TextBox
        Friend WithEvents lblExperimental As System.Windows.Forms.Label
        Friend WithEvents chkUseSQLServer As System.Windows.Forms.CheckBox
        Friend WithEvents lblSQLUsername As System.Windows.Forms.Label
        Friend WithEvents txtSQLPassword As System.Windows.Forms.TextBox
        Friend WithEvents lblSQLInfo As System.Windows.Forms.Label
        Friend WithEvents lblSQLServer As System.Windows.Forms.Label
        Friend WithEvents txtSQLUsername As System.Windows.Forms.TextBox
        Friend WithEvents txtSQLServer As System.Windows.Forms.TextBox
        Friend WithEvents lblSQLPassword As System.Windows.Forms.Label

    End Class
End Namespace