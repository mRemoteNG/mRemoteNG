Imports WeifenLuo.WinFormsUI.Docking
Imports Tamir.SharpSsh
Imports System.IO
Imports System.Threading
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class SSHTransfer
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents pbStatus As System.Windows.Forms.ProgressBar
            Friend WithEvents btnTransfer As System.Windows.Forms.Button
            Friend WithEvents txtUser As System.Windows.Forms.TextBox
            Friend WithEvents txtPassword As System.Windows.Forms.TextBox
            Friend WithEvents txtHost As System.Windows.Forms.TextBox
            Friend WithEvents txtPort As System.Windows.Forms.TextBox
            Friend WithEvents lblHost As System.Windows.Forms.Label
            Friend WithEvents lblPort As System.Windows.Forms.Label
            Friend WithEvents lblUser As System.Windows.Forms.Label
            Friend WithEvents lblPassword As System.Windows.Forms.Label
            Friend WithEvents lblProtocol As System.Windows.Forms.Label
            Friend WithEvents radProtSCP As System.Windows.Forms.RadioButton
            Friend WithEvents radProtSFTP As System.Windows.Forms.RadioButton
            Friend WithEvents grpConnection As System.Windows.Forms.GroupBox
            Friend WithEvents btnBrowse As System.Windows.Forms.Button
            Friend WithEvents lblRemoteFile As System.Windows.Forms.Label
            Friend WithEvents txtRemoteFile As System.Windows.Forms.TextBox
            Friend WithEvents txtLocalFile As System.Windows.Forms.TextBox
            Friend WithEvents lblLocalFile As System.Windows.Forms.Label
            Friend WithEvents grpFiles As System.Windows.Forms.GroupBox

            Private Sub InitializeComponent()
                Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SSHTransfer))
                Me.grpFiles = New System.Windows.Forms.GroupBox
                Me.lblLocalFile = New System.Windows.Forms.Label
                Me.txtLocalFile = New System.Windows.Forms.TextBox
                Me.txtRemoteFile = New System.Windows.Forms.TextBox
                Me.lblRemoteFile = New System.Windows.Forms.Label
                Me.btnBrowse = New System.Windows.Forms.Button
                Me.grpConnection = New System.Windows.Forms.GroupBox
                Me.radProtSFTP = New System.Windows.Forms.RadioButton
                Me.radProtSCP = New System.Windows.Forms.RadioButton
                Me.lblProtocol = New System.Windows.Forms.Label
                Me.lblPassword = New System.Windows.Forms.Label
                Me.lblUser = New System.Windows.Forms.Label
                Me.lblPort = New System.Windows.Forms.Label
                Me.lblHost = New System.Windows.Forms.Label
                Me.txtPort = New System.Windows.Forms.TextBox
                Me.txtHost = New System.Windows.Forms.TextBox
                Me.txtPassword = New System.Windows.Forms.TextBox
                Me.txtUser = New System.Windows.Forms.TextBox
                Me.btnTransfer = New System.Windows.Forms.Button
                Me.pbStatus = New System.Windows.Forms.ProgressBar
                Me.grpFiles.SuspendLayout()
                Me.grpConnection.SuspendLayout()
                Me.SuspendLayout()
                '
                'grpFiles
                '
                Me.grpFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.grpFiles.Controls.Add(Me.lblLocalFile)
                Me.grpFiles.Controls.Add(Me.txtLocalFile)
                Me.grpFiles.Controls.Add(Me.txtRemoteFile)
                Me.grpFiles.Controls.Add(Me.lblRemoteFile)
                Me.grpFiles.Controls.Add(Me.btnBrowse)
                Me.grpFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.grpFiles.Location = New System.Drawing.Point(12, 153)
                Me.grpFiles.Name = "grpFiles"
                Me.grpFiles.Size = New System.Drawing.Size(668, 194)
                Me.grpFiles.TabIndex = 2000
                Me.grpFiles.TabStop = False
                Me.grpFiles.Text = "Files"
                '
                'lblLocalFile
                '
                Me.lblLocalFile.AutoSize = True
                Me.lblLocalFile.Location = New System.Drawing.Point(20, 25)
                Me.lblLocalFile.Name = "lblLocalFile"
                Me.lblLocalFile.Size = New System.Drawing.Size(52, 13)
                Me.lblLocalFile.TabIndex = 10
                Me.lblLocalFile.Text = "Local file:"
                '
                'txtLocalFile
                '
                Me.txtLocalFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtLocalFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtLocalFile.Location = New System.Drawing.Point(105, 23)
                Me.txtLocalFile.Name = "txtLocalFile"
                Me.txtLocalFile.Size = New System.Drawing.Size(455, 20)
                Me.txtLocalFile.TabIndex = 20
                '
                'txtRemoteFile
                '
                Me.txtRemoteFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtRemoteFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtRemoteFile.Location = New System.Drawing.Point(105, 49)
                Me.txtRemoteFile.Name = "txtRemoteFile"
                Me.txtRemoteFile.Size = New System.Drawing.Size(542, 20)
                Me.txtRemoteFile.TabIndex = 50
                '
                'lblRemoteFile
                '
                Me.lblRemoteFile.AutoSize = True
                Me.lblRemoteFile.Location = New System.Drawing.Point(20, 51)
                Me.lblRemoteFile.Name = "lblRemoteFile"
                Me.lblRemoteFile.Size = New System.Drawing.Size(63, 13)
                Me.lblRemoteFile.TabIndex = 40
                Me.lblRemoteFile.Text = "Remote file:"
                '
                'btnBrowse
                '
                Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnBrowse.Location = New System.Drawing.Point(566, 21)
                Me.btnBrowse.Name = "btnBrowse"
                Me.btnBrowse.Size = New System.Drawing.Size(81, 23)
                Me.btnBrowse.TabIndex = 30
                Me.btnBrowse.Text = Global.mRemoteNG.My.Resources.Resources.strButtonBrowse
                Me.btnBrowse.UseVisualStyleBackColor = True
                '
                'grpConnection
                '
                Me.grpConnection.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.grpConnection.Controls.Add(Me.radProtSFTP)
                Me.grpConnection.Controls.Add(Me.radProtSCP)
                Me.grpConnection.Controls.Add(Me.lblProtocol)
                Me.grpConnection.Controls.Add(Me.lblPassword)
                Me.grpConnection.Controls.Add(Me.lblUser)
                Me.grpConnection.Controls.Add(Me.lblPort)
                Me.grpConnection.Controls.Add(Me.lblHost)
                Me.grpConnection.Controls.Add(Me.txtPort)
                Me.grpConnection.Controls.Add(Me.txtHost)
                Me.grpConnection.Controls.Add(Me.txtPassword)
                Me.grpConnection.Controls.Add(Me.txtUser)
                Me.grpConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.grpConnection.Location = New System.Drawing.Point(12, 12)
                Me.grpConnection.Name = "grpConnection"
                Me.grpConnection.Size = New System.Drawing.Size(668, 135)
                Me.grpConnection.TabIndex = 1000
                Me.grpConnection.TabStop = False
                Me.grpConnection.Text = "Connection"
                '
                'radProtSFTP
                '
                Me.radProtSFTP.AutoSize = True
                Me.radProtSFTP.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.radProtSFTP.Location = New System.Drawing.Point(153, 103)
                Me.radProtSFTP.Name = "radProtSFTP"
                Me.radProtSFTP.Size = New System.Drawing.Size(51, 17)
                Me.radProtSFTP.TabIndex = 110
                Me.radProtSFTP.Text = "SFTP"
                Me.radProtSFTP.UseVisualStyleBackColor = True
                '
                'radProtSCP
                '
                Me.radProtSCP.AutoSize = True
                Me.radProtSCP.Checked = True
                Me.radProtSCP.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.radProtSCP.Location = New System.Drawing.Point(92, 103)
                Me.radProtSCP.Name = "radProtSCP"
                Me.radProtSCP.Size = New System.Drawing.Size(45, 17)
                Me.radProtSCP.TabIndex = 100
                Me.radProtSCP.TabStop = True
                Me.radProtSCP.Text = "SCP"
                Me.radProtSCP.UseVisualStyleBackColor = True
                '
                'lblProtocol
                '
                Me.lblProtocol.AutoSize = True
                Me.lblProtocol.Location = New System.Drawing.Point(20, 105)
                Me.lblProtocol.Name = "lblProtocol"
                Me.lblProtocol.Size = New System.Drawing.Size(49, 13)
                Me.lblProtocol.TabIndex = 90
                Me.lblProtocol.Text = "Protocol:"
                '
                'lblPassword
                '
                Me.lblPassword.AutoSize = True
                Me.lblPassword.Location = New System.Drawing.Point(20, 79)
                Me.lblPassword.Name = "lblPassword"
                Me.lblPassword.Size = New System.Drawing.Size(56, 13)
                Me.lblPassword.TabIndex = 70
                Me.lblPassword.Text = "Password:"
                '
                'lblUser
                '
                Me.lblUser.AutoSize = True
                Me.lblUser.Location = New System.Drawing.Point(20, 53)
                Me.lblUser.Name = "lblUser"
                Me.lblUser.Size = New System.Drawing.Size(32, 13)
                Me.lblUser.TabIndex = 50
                Me.lblUser.Text = "User:"
                '
                'lblPort
                '
                Me.lblPort.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lblPort.AutoSize = True
                Me.lblPort.Location = New System.Drawing.Point(582, 27)
                Me.lblPort.Name = "lblPort"
                Me.lblPort.Size = New System.Drawing.Size(29, 13)
                Me.lblPort.TabIndex = 30
                Me.lblPort.Text = "Port:"
                '
                'lblHost
                '
                Me.lblHost.AutoSize = True
                Me.lblHost.Location = New System.Drawing.Point(20, 27)
                Me.lblHost.Name = "lblHost"
                Me.lblHost.Size = New System.Drawing.Size(32, 13)
                Me.lblHost.TabIndex = 10
                Me.lblHost.Text = "Host:"
                '
                'txtPort
                '
                Me.txtPort.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtPort.Location = New System.Drawing.Point(617, 25)
                Me.txtPort.Name = "txtPort"
                Me.txtPort.Size = New System.Drawing.Size(30, 20)
                Me.txtPort.TabIndex = 40
                '
                'txtHost
                '
                Me.txtHost.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtHost.Location = New System.Drawing.Point(105, 25)
                Me.txtHost.Name = "txtHost"
                Me.txtHost.Size = New System.Drawing.Size(471, 20)
                Me.txtHost.TabIndex = 20
                '
                'txtPassword
                '
                Me.txtPassword.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtPassword.Location = New System.Drawing.Point(105, 77)
                Me.txtPassword.Name = "txtPassword"
                Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
                Me.txtPassword.Size = New System.Drawing.Size(471, 20)
                Me.txtPassword.TabIndex = 80
                '
                'txtUser
                '
                Me.txtUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtUser.Location = New System.Drawing.Point(105, 51)
                Me.txtUser.Name = "txtUser"
                Me.txtUser.Size = New System.Drawing.Size(471, 20)
                Me.txtUser.TabIndex = 60
                '
                'btnTransfer
                '
                Me.btnTransfer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnTransfer.Image = Global.mRemoteNG.My.Resources.Resources.SSHTransfer
                Me.btnTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
                Me.btnTransfer.Location = New System.Drawing.Point(597, 382)
                Me.btnTransfer.Name = "btnTransfer"
                Me.btnTransfer.Size = New System.Drawing.Size(83, 29)
                Me.btnTransfer.TabIndex = 10000
                Me.btnTransfer.Text = "Transfer"
                Me.btnTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
                Me.btnTransfer.UseVisualStyleBackColor = True
                '
                'pbStatus
                '
                Me.pbStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pbStatus.Location = New System.Drawing.Point(12, 353)
                Me.pbStatus.Name = "pbStatus"
                Me.pbStatus.Size = New System.Drawing.Size(668, 23)
                Me.pbStatus.TabIndex = 3000
                '
                'SSHTransfer
                '
                Me.ClientSize = New System.Drawing.Size(692, 423)
                Me.Controls.Add(Me.grpFiles)
                Me.Controls.Add(Me.grpConnection)
                Me.Controls.Add(Me.btnTransfer)
                Me.Controls.Add(Me.pbStatus)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
                Me.Name = "SSHTransfer"
                Me.TabText = Global.mRemoteNG.My.Resources.Resources.strMenuSSHFileTransfer
                Me.Text = "SSH File Transfer"
                Me.grpFiles.ResumeLayout(False)
                Me.grpFiles.PerformLayout()
                Me.grpConnection.ResumeLayout(False)
                Me.grpConnection.PerformLayout()
                Me.ResumeLayout(False)

            End Sub
#End Region

#Region "Private Properties"
            Private sshT As SshTransferProtocolBase
            Private oDlg As OpenFileDialog
#End Region

#Region "Public Properties"
            Public Property Hostname() As String
                Get
                    Return Me.txtHost.Text
                End Get
                Set(ByVal value As String)
                    Me.txtHost.Text = value
                End Set
            End Property

            Public Property Port() As String
                Get
                    Return Me.txtPort.Text
                End Get
                Set(ByVal value As String)
                    Me.txtPort.Text = value
                End Set
            End Property

            Public Property Username() As String
                Get
                    Return Me.txtUser.Text
                End Get
                Set(ByVal value As String)
                    Me.txtUser.Text = value
                End Set
            End Property

            Public Property Password() As String
                Get
                    Return Me.txtPassword.Text
                End Get
                Set(ByVal value As String)
                    Me.txtPassword.Text = value
                End Set
            End Property
#End Region

#Region "Form Stuff"
            Private Sub SSHTransfer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()
            End Sub

            Private Sub ApplyLanguage()
                grpFiles.Text = My.Resources.strGroupboxFiles
                lblLocalFile.Text = My.Resources.strLocalFile & ":"
                lblRemoteFile.Text = My.Resources.strRemoteFile & ":"
                btnBrowse.Text = My.Resources.strButtonBrowse
                grpConnection.Text = My.Resources.strGroupboxConnection
                lblProtocol.Text = My.Resources.strLabelProtocol
                lblPassword.Text = My.Resources.strLabelPassword
                lblUser.Text = My.Resources.strUser & ":"
                lblPort.Text = My.Resources.strLabelPort
                lblHost.Text = My.Resources.strHost & ":"
                btnTransfer.Text = My.Resources.strTransfer
                TabText = My.Resources.strMenuSSHFileTransfer
                Text = My.Resources.strMenuSSHFileTransfer
            End Sub
#End Region

#Region "Private Methods"
            Private Sub StartTransfer(ByVal Protocol As SSHTransferProtocol)
                If AllFieldsSet() = False Then
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strPleaseFillAllFields)
                    Exit Sub
                End If

                If File.Exists(Me.txtLocalFile.Text) = False Then
                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Resources.strLocalFileDoesNotExist)
                    Exit Sub
                End If

                Try
                    If Protocol = SSHTransferProtocol.SCP Then
                        Me.sshT = New Scp(txtHost.Text, txtUser.Text, txtPassword.Text)
                    ElseIf Protocol = SSHTransferProtocol.SFTP Then
                        Me.sshT = New Sftp(txtHost.Text, txtUser.Text, txtPassword.Text)
                    End If

                    AddHandler sshT.OnTransferStart, AddressOf SshTransfer_Start
                    AddHandler sshT.OnTransferProgress, AddressOf SshTransfer_Progress
                    AddHandler sshT.OnTransferEnd, AddressOf SshTransfer_End

                    Me.sshT.Connect(Me.txtPort.Text)

                    LocalFile = Me.txtLocalFile.Text
                    RemoteFile = Me.txtRemoteFile.Text

                    Dim t As New Thread(AddressOf StartTransferBG)
                    t.SetApartmentState(ApartmentState.STA)
                    t.IsBackground = True
                    t.Start()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strSSHTransferFailed & vbNewLine & ex.Message)
                    Me.sshT.Close()
                End Try
            End Sub

            Private LocalFile As String
            Private RemoteFile As String

            Private Sub StartTransferBG()
                Try
                    DisableButtons()
                    Me.sshT.Put(LocalFile, RemoteFile)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strSSHStartTransferBG & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Function AllFieldsSet() As Boolean
                If Me.txtHost.Text <> "" And Me.txtPort.Text <> "" And Me.txtUser.Text <> "" And Me.txtLocalFile.Text <> "" And Me.txtRemoteFile.Text <> "" Then
                    If Me.txtPassword.Text = "" Then
                        If MsgBox(My.Resources.strEmptyPasswordContinue, MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                            Return False
                        End If
                    End If

                    If Me.txtRemoteFile.Text.EndsWith("/") Or Me.txtRemoteFile.Text.EndsWith("\") Then
                        Me.txtRemoteFile.Text += Me.txtLocalFile.Text.Substring(Me.txtLocalFile.Text.LastIndexOf("\") + 1)
                    End If

                    Return True
                Else
                    Return False
                End If
            End Function


            Private maxVal As Long
            Private curVal As Long

            Delegate Sub SetStatusCB()
            Private Sub SetStatus()
                If pbStatus.InvokeRequired Then
                    Dim d As New SetStatusCB(AddressOf SetStatus)
                    Me.pbStatus.Invoke(d)
                Else
                    pbStatus.Maximum = maxVal
                    pbStatus.Value = curVal
                End If
            End Sub

            Delegate Sub EnableButtonsCB()
            Private Sub EnableButtons()
                If btnTransfer.InvokeRequired Then
                    Dim d As New EnableButtonsCB(AddressOf EnableButtons)
                    Me.btnTransfer.Invoke(d)
                Else
                    btnTransfer.Enabled = True
                End If
            End Sub

            Delegate Sub DisableButtonsCB()
            Private Sub DisableButtons()
                If btnTransfer.InvokeRequired Then
                    Dim d As New DisableButtonsCB(AddressOf DisableButtons)
                    Me.btnTransfer.Invoke(d)
                Else
                    btnTransfer.Enabled = False
                End If
            End Sub

            Private Sub SshTransfer_Start(ByVal src As String, ByVal dst As String, ByVal transferredBytes As Integer, ByVal totalBytes As Integer, ByVal message As String)
                maxVal = totalBytes
                curVal = transferredBytes

                SetStatus()
            End Sub

            Private Sub SshTransfer_Progress(ByVal src As String, ByVal dst As String, ByVal transferredBytes As Integer, ByVal totalBytes As Integer, ByVal message As String)
                maxVal = totalBytes
                curVal = transferredBytes

                SetStatus()
            End Sub

            Private Sub SshTransfer_End(ByVal src As String, ByVal dst As String, ByVal transferredBytes As Integer, ByVal totalBytes As Integer, ByVal message As String)
                Try
                    maxVal = totalBytes
                    curVal = transferredBytes

                    EnableButtons()
                    SetStatus()

                    MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Resources.strSSHTransferFailed)

                    If Me.sshT IsNot Nothing Then
                        Me.sshT.Close()
                    ElseIf Me.sshT IsNot Nothing Then
                        Me.sshT.Close()
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strSSHTransferEndFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.SSHTransfer
                Me.DockPnl = Panel
                Me.InitializeComponent()

                Me.oDlg = New OpenFileDialog
                Me.oDlg.Filter = "All Files (*.*)|*.*"
                Me.oDlg.CheckFileExists = True
            End Sub
#End Region

#Region "Form Stuff"
            Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
                If Me.oDlg.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                    If Me.oDlg.FileName <> "" Then
                        Me.txtLocalFile.Text = Me.oDlg.FileName
                    End If
                End If
            End Sub

            Private Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
                If Me.radProtSCP.Checked Then
                    Me.StartTransfer(SSHTransferProtocol.SCP)
                ElseIf Me.radProtSFTP.Checked Then
                    Me.StartTransfer(SSHTransferProtocol.SFTP)
                End If
            End Sub
#End Region

            Private Enum SSHTransferProtocol
                SCP = 0
                SFTP = 1
            End Enum
        End Class
    End Namespace
End Namespace