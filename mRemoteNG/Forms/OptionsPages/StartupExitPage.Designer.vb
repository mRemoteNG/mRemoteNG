Namespace Forms.OptionsPages
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class StartupExitPage
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(StartupExitPage))
            Me.chkReconnectOnStart = New System.Windows.Forms.CheckBox()
            Me.chkSaveConsOnExit = New System.Windows.Forms.CheckBox()
            Me.chkSingleInstance = New System.Windows.Forms.CheckBox()
            Me.chkProperInstallationOfComponentsAtStartup = New System.Windows.Forms.CheckBox()
            Me.SuspendLayout()
            '
            'chkReconnectOnStart
            '
            Me.chkReconnectOnStart.AutoSize = True
            Me.chkReconnectOnStart.Location = New System.Drawing.Point(3, 24)
            Me.chkReconnectOnStart.Name = "chkReconnectOnStart"
            Me.chkReconnectOnStart.Size = New System.Drawing.Size(273, 17)
            Me.chkReconnectOnStart.TabIndex = 7
            Me.chkReconnectOnStart.Text = "Reconnect to previously opened sessions on startup"
            Me.chkReconnectOnStart.UseVisualStyleBackColor = True
            '
            'chkSaveConsOnExit
            '
            Me.chkSaveConsOnExit.AutoSize = True
            Me.chkSaveConsOnExit.Location = New System.Drawing.Point(3, 0)
            Me.chkSaveConsOnExit.Name = "chkSaveConsOnExit"
            Me.chkSaveConsOnExit.Size = New System.Drawing.Size(146, 17)
            Me.chkSaveConsOnExit.TabIndex = 6
            Me.chkSaveConsOnExit.Text = "Save connections on exit"
            Me.chkSaveConsOnExit.UseVisualStyleBackColor = True
            '
            'chkSingleInstance
            '
            Me.chkSingleInstance.AutoSize = True
            Me.chkSingleInstance.Location = New System.Drawing.Point(3, 48)
            Me.chkSingleInstance.Name = "chkSingleInstance"
            Me.chkSingleInstance.Size = New System.Drawing.Size(366, 17)
            Me.chkSingleInstance.TabIndex = 8
            Me.chkSingleInstance.Text = "Allow only a single instance of the application (mRemote restart required)"
            Me.chkSingleInstance.UseVisualStyleBackColor = True
            '
            'chkProperInstallationOfComponentsAtStartup
            '
            Me.chkProperInstallationOfComponentsAtStartup.AutoSize = True
            Me.chkProperInstallationOfComponentsAtStartup.Location = New System.Drawing.Point(3, 72)
            Me.chkProperInstallationOfComponentsAtStartup.Name = "chkProperInstallationOfComponentsAtStartup"
            Me.chkProperInstallationOfComponentsAtStartup.Size = New System.Drawing.Size(262, 17)
            Me.chkProperInstallationOfComponentsAtStartup.TabIndex = 9
            Me.chkProperInstallationOfComponentsAtStartup.Text = "Check proper installation of components at startup"
            Me.chkProperInstallationOfComponentsAtStartup.UseVisualStyleBackColor = True
            '
            'StartupExitPage
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.chkReconnectOnStart)
            Me.Controls.Add(Me.chkSaveConsOnExit)
            Me.Controls.Add(Me.chkSingleInstance)
            Me.Controls.Add(Me.chkProperInstallationOfComponentsAtStartup)
            Me.Name = "StartupExitPage"
            Me.PageIcon = CType(resources.GetObject("$this.PageIcon"), System.Drawing.Icon)
            Me.Size = New System.Drawing.Size(610, 489)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents chkReconnectOnStart As System.Windows.Forms.CheckBox
        Friend WithEvents chkSaveConsOnExit As System.Windows.Forms.CheckBox
        Friend WithEvents chkSingleInstance As System.Windows.Forms.CheckBox
        Friend WithEvents chkProperInstallationOfComponentsAtStartup As System.Windows.Forms.CheckBox

    End Class
End Namespace