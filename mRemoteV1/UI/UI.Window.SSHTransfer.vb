Imports System.ComponentModel
Imports System.IO
Imports System.Threading
Imports mRemote3G.App
Imports mRemote3G.Messages
Imports Tamir.SharpSsh
Imports WeifenLuo.WinFormsUI.Docking

Namespace UI

    Namespace Window
        Public Class SSHTransfer
            Inherits Base

#Region "Form Init"

            Friend WithEvents pbStatus As ProgressBar
            Friend WithEvents btnTransfer As Button
            Friend WithEvents txtUser As TextBox
            Friend WithEvents txtPassword As TextBox
            Friend WithEvents txtHost As TextBox
            Friend WithEvents txtPort As TextBox
            Friend WithEvents lblHost As Label
            Friend WithEvents lblPort As Label
            Friend WithEvents lblUser As Label
            Friend WithEvents lblPassword As Label
            Friend WithEvents lblProtocol As Label
            Friend WithEvents radProtSCP As RadioButton
            Friend WithEvents radProtSFTP As RadioButton
            Friend WithEvents grpConnection As GroupBox
            Friend WithEvents btnBrowse As Button
            Friend WithEvents lblRemoteFile As Label
            Friend WithEvents txtRemoteFile As TextBox
            Friend WithEvents txtLocalFile As TextBox
            Friend WithEvents lblLocalFile As Label
            Friend WithEvents grpFiles As GroupBox

            Private Sub InitializeComponent()
                Dim resources = New ComponentResourceManager(GetType(SSHTransfer))
                Me.grpFiles = New GroupBox
                Me.lblLocalFile = New Label
                Me.txtLocalFile = New TextBox
                Me.txtRemoteFile = New TextBox
                Me.lblRemoteFile = New Label
                Me.btnBrowse = New Button
                Me.grpConnection = New GroupBox
                Me.radProtSFTP = New RadioButton
                Me.radProtSCP = New RadioButton
                Me.lblProtocol = New Label
                Me.lblPassword = New Label
                Me.lblUser = New Label
                Me.lblPort = New Label
                Me.lblHost = New Label
                Me.txtPort = New TextBox
                Me.txtHost = New TextBox
                Me.txtPassword = New TextBox
                Me.txtUser = New TextBox
                Me.btnTransfer = New Button
                Me.pbStatus = New ProgressBar
                Me.grpFiles.SuspendLayout()
                Me.grpConnection.SuspendLayout()
                Me.SuspendLayout()
                '
                'grpFiles
                '
                Me.grpFiles.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                             Or AnchorStyles.Left) _
                                            Or AnchorStyles.Right),
                                           AnchorStyles)
                Me.grpFiles.Controls.Add(Me.lblLocalFile)
                Me.grpFiles.Controls.Add(Me.txtLocalFile)
                Me.grpFiles.Controls.Add(Me.txtRemoteFile)
                Me.grpFiles.Controls.Add(Me.lblRemoteFile)
                Me.grpFiles.Controls.Add(Me.btnBrowse)
                Me.grpFiles.FlatStyle = FlatStyle.Flat
                Me.grpFiles.Location = New Point(12, 153)
                Me.grpFiles.Name = "grpFiles"
                Me.grpFiles.Size = New Size(668, 194)
                Me.grpFiles.TabIndex = 2000
                Me.grpFiles.TabStop = False
                Me.grpFiles.Text = "Files"
                '
                'lblLocalFile
                '
                Me.lblLocalFile.AutoSize = True
                Me.lblLocalFile.Location = New Point(20, 25)
                Me.lblLocalFile.Name = "lblLocalFile"
                Me.lblLocalFile.Size = New Size(52, 13)
                Me.lblLocalFile.TabIndex = 10
                Me.lblLocalFile.Text = "Local file:"
                '
                'txtLocalFile
                '
                Me.txtLocalFile.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                                Or AnchorStyles.Right),
                                               AnchorStyles)
                Me.txtLocalFile.BorderStyle = BorderStyle.FixedSingle
                Me.txtLocalFile.Location = New Point(105, 23)
                Me.txtLocalFile.Name = "txtLocalFile"
                Me.txtLocalFile.Size = New Size(455, 20)
                Me.txtLocalFile.TabIndex = 20
                '
                'txtRemoteFile
                '
                Me.txtRemoteFile.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                                 Or AnchorStyles.Right),
                                                AnchorStyles)
                Me.txtRemoteFile.BorderStyle = BorderStyle.FixedSingle
                Me.txtRemoteFile.Location = New Point(105, 49)
                Me.txtRemoteFile.Name = "txtRemoteFile"
                Me.txtRemoteFile.Size = New Size(542, 20)
                Me.txtRemoteFile.TabIndex = 50
                '
                'lblRemoteFile
                '
                Me.lblRemoteFile.AutoSize = True
                Me.lblRemoteFile.Location = New Point(20, 51)
                Me.lblRemoteFile.Name = "lblRemoteFile"
                Me.lblRemoteFile.Size = New Size(63, 13)
                Me.lblRemoteFile.TabIndex = 40
                Me.lblRemoteFile.Text = "Remote file:"
                '
                'btnBrowse
                '
                Me.btnBrowse.Anchor = CType((AnchorStyles.Top Or AnchorStyles.Right), AnchorStyles)
                Me.btnBrowse.FlatStyle = FlatStyle.Flat
                Me.btnBrowse.Location = New Point(566, 21)
                Me.btnBrowse.Name = "btnBrowse"
                Me.btnBrowse.Size = New Size(81, 23)
                Me.btnBrowse.TabIndex = 30
                Me.btnBrowse.Text = "Browse"
                Me.btnBrowse.UseVisualStyleBackColor = True
                '
                'grpConnection
                '
                Me.grpConnection.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                                 Or AnchorStyles.Right),
                                                AnchorStyles)
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
                Me.grpConnection.FlatStyle = FlatStyle.Flat
                Me.grpConnection.Location = New Point(12, 12)
                Me.grpConnection.Name = "grpConnection"
                Me.grpConnection.Size = New Size(668, 135)
                Me.grpConnection.TabIndex = 1000
                Me.grpConnection.TabStop = False
                Me.grpConnection.Text = "Connection"
                '
                'radProtSFTP
                '
                Me.radProtSFTP.AutoSize = True
                Me.radProtSFTP.FlatStyle = FlatStyle.Flat
                Me.radProtSFTP.Location = New Point(153, 103)
                Me.radProtSFTP.Name = "radProtSFTP"
                Me.radProtSFTP.Size = New Size(51, 17)
                Me.radProtSFTP.TabIndex = 110
                Me.radProtSFTP.Text = "SFTP"
                Me.radProtSFTP.UseVisualStyleBackColor = True
                '
                'radProtSCP
                '
                Me.radProtSCP.AutoSize = True
                Me.radProtSCP.Checked = True
                Me.radProtSCP.FlatStyle = FlatStyle.Flat
                Me.radProtSCP.Location = New Point(92, 103)
                Me.radProtSCP.Name = "radProtSCP"
                Me.radProtSCP.Size = New Size(45, 17)
                Me.radProtSCP.TabIndex = 100
                Me.radProtSCP.TabStop = True
                Me.radProtSCP.Text = "SCP"
                Me.radProtSCP.UseVisualStyleBackColor = True
                '
                'lblProtocol
                '
                Me.lblProtocol.AutoSize = True
                Me.lblProtocol.Location = New Point(20, 105)
                Me.lblProtocol.Name = "lblProtocol"
                Me.lblProtocol.Size = New Size(49, 13)
                Me.lblProtocol.TabIndex = 90
                Me.lblProtocol.Text = "Protocol:"
                '
                'lblPassword
                '
                Me.lblPassword.AutoSize = True
                Me.lblPassword.Location = New Point(20, 79)
                Me.lblPassword.Name = "lblPassword"
                Me.lblPassword.Size = New Size(56, 13)
                Me.lblPassword.TabIndex = 70
                Me.lblPassword.Text = "Password:"
                '
                'lblUser
                '
                Me.lblUser.AutoSize = True
                Me.lblUser.Location = New Point(20, 53)
                Me.lblUser.Name = "lblUser"
                Me.lblUser.Size = New Size(32, 13)
                Me.lblUser.TabIndex = 50
                Me.lblUser.Text = "User:"
                '
                'lblPort
                '
                Me.lblPort.Anchor = CType((AnchorStyles.Top Or AnchorStyles.Right), AnchorStyles)
                Me.lblPort.AutoSize = True
                Me.lblPort.Location = New Point(582, 27)
                Me.lblPort.Name = "lblPort"
                Me.lblPort.Size = New Size(29, 13)
                Me.lblPort.TabIndex = 30
                Me.lblPort.Text = "Port:"
                '
                'lblHost
                '
                Me.lblHost.AutoSize = True
                Me.lblHost.Location = New Point(20, 27)
                Me.lblHost.Name = "lblHost"
                Me.lblHost.Size = New Size(32, 13)
                Me.lblHost.TabIndex = 10
                Me.lblHost.Text = "Host:"
                '
                'txtPort
                '
                Me.txtPort.Anchor = CType((AnchorStyles.Top Or AnchorStyles.Right), AnchorStyles)
                Me.txtPort.BorderStyle = BorderStyle.FixedSingle
                Me.txtPort.Location = New Point(617, 25)
                Me.txtPort.Name = "txtPort"
                Me.txtPort.Size = New Size(30, 20)
                Me.txtPort.TabIndex = 40
                '
                'txtHost
                '
                Me.txtHost.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                           Or AnchorStyles.Right),
                                          AnchorStyles)
                Me.txtHost.BorderStyle = BorderStyle.FixedSingle
                Me.txtHost.Location = New Point(105, 25)
                Me.txtHost.Name = "txtHost"
                Me.txtHost.Size = New Size(471, 20)
                Me.txtHost.TabIndex = 20
                '
                'txtPassword
                '
                Me.txtPassword.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                               Or AnchorStyles.Right),
                                              AnchorStyles)
                Me.txtPassword.BorderStyle = BorderStyle.FixedSingle
                Me.txtPassword.Location = New Point(105, 77)
                Me.txtPassword.Name = "txtPassword"
                Me.txtPassword.PasswordChar = ChrW(42)
                Me.txtPassword.Size = New Size(471, 20)
                Me.txtPassword.TabIndex = 80
                '
                'txtUser
                '
                Me.txtUser.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                           Or AnchorStyles.Right),
                                          AnchorStyles)
                Me.txtUser.BorderStyle = BorderStyle.FixedSingle
                Me.txtUser.Location = New Point(105, 51)
                Me.txtUser.Name = "txtUser"
                Me.txtUser.Size = New Size(471, 20)
                Me.txtUser.TabIndex = 60
                '
                'btnTransfer
                '
                Me.btnTransfer.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles)
                Me.btnTransfer.FlatStyle = FlatStyle.Flat
                Me.btnTransfer.Image = My.Resources.Resources.SSHTransfer
                Me.btnTransfer.ImageAlign = ContentAlignment.MiddleLeft
                Me.btnTransfer.Location = New Point(597, 382)
                Me.btnTransfer.Name = "btnTransfer"
                Me.btnTransfer.Size = New Size(83, 29)
                Me.btnTransfer.TabIndex = 10000
                Me.btnTransfer.Text = "Transfer"
                Me.btnTransfer.TextAlign = ContentAlignment.MiddleRight
                Me.btnTransfer.UseVisualStyleBackColor = True
                '
                'pbStatus
                '
                Me.pbStatus.Anchor = CType(((AnchorStyles.Bottom Or AnchorStyles.Left) _
                                            Or AnchorStyles.Right),
                                           AnchorStyles)
                Me.pbStatus.Location = New Point(12, 353)
                Me.pbStatus.Name = "pbStatus"
                Me.pbStatus.Size = New Size(668, 23)
                Me.pbStatus.TabIndex = 3000
                '
                'SSHTransfer
                '
                Me.ClientSize = New Size(692, 423)
                Me.Controls.Add(Me.grpFiles)
                Me.Controls.Add(Me.grpConnection)
                Me.Controls.Add(Me.btnTransfer)
                Me.Controls.Add(Me.pbStatus)
                Me.Font = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))
                Me.Icon = CType(resources.GetObject("$this.Icon"), Icon)
                Me.Name = "SSHTransfer"
                Me.TabText = "SSH File Transfer"
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
            Private ReadOnly oDlg As OpenFileDialog

#End Region

#Region "Public Properties"

            Public Property Hostname As String
                Get
                    Return Me.txtHost.Text
                End Get
                Set
                    Me.txtHost.Text = value
                End Set
            End Property

            Public Property Port As String
                Get
                    Return Me.txtPort.Text
                End Get
                Set
                    Me.txtPort.Text = value
                End Set
            End Property

            Public Property Username As String
                Get
                    Return Me.txtUser.Text
                End Get
                Set
                    Me.txtUser.Text = value
                End Set
            End Property

            Public Property Password As String
                Get
                    Return Me.txtPassword.Text
                End Get
                Set
                    Me.txtPassword.Text = value
                End Set
            End Property

#End Region

#Region "Form Stuff"

            Private Sub SSHTransfer_Load(sender As Object, e As EventArgs) Handles Me.Load
                ApplyLanguage()
            End Sub

            Private Sub ApplyLanguage()
                grpFiles.Text = Language.Language.strGroupboxFiles
                lblLocalFile.Text = Language.Language.strLocalFile & ":"
                lblRemoteFile.Text = Language.Language.strRemoteFile & ":"
                btnBrowse.Text = Language.Language.strButtonBrowse
                grpConnection.Text = Language.Language.strGroupboxConnection
                lblProtocol.Text = Language.Language.strLabelProtocol
                lblPassword.Text = Language.Language.strLabelPassword
                lblUser.Text = Language.Language.strUser & ":"
                lblPort.Text = Language.Language.strLabelPort
                lblHost.Text = Language.Language.strHost & ":"
                btnTransfer.Text = Language.Language.strTransfer
                TabText = Language.Language.strMenuSSHFileTransfer
                Text = Language.Language.strMenuSSHFileTransfer
            End Sub

#End Region

#Region "Private Methods"

            Private Sub StartTransfer(Protocol As SSHTransferProtocol)
                If AllFieldsSet() = False Then
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.Language.strPleaseFillAllFields)
                    Exit Sub
                End If

                If File.Exists(Me.txtLocalFile.Text) = False Then
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                        Language.Language.strLocalFileDoesNotExist)
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
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strSSHTransferFailed & vbNewLine &
                                                        ex.ToString())
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
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strSSHStartTransferBG & vbNewLine &
                                                        ex.ToString(), True)
                End Try
            End Sub

            Private Function AllFieldsSet() As Boolean
                If _
                    Me.txtHost.Text <> "" And Me.txtPort.Text <> "" And Me.txtUser.Text <> "" And
                    Me.txtLocalFile.Text <> "" And Me.txtRemoteFile.Text <> "" Then
                    If Me.txtPassword.Text = "" Then
                        If _
                            MsgBox(Language.Language.strEmptyPasswordContinue, MsgBoxStyle.Question Or MsgBoxStyle.YesNo) =
                            MsgBoxResult.No Then
                            Return False
                        End If
                    End If

                    If Me.txtRemoteFile.Text.EndsWith("/") Or Me.txtRemoteFile.Text.EndsWith("\") Then
                        Me.txtRemoteFile.Text +=
                            Me.txtLocalFile.Text.Substring(Me.txtLocalFile.Text.LastIndexOf("\") + 1)
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

            Private Sub SshTransfer_Start(src As String, dst As String, transferredBytes As Integer,
                                          totalBytes As Integer, message As String)
                maxVal = totalBytes
                curVal = transferredBytes

                SetStatus()
            End Sub

            Private Sub SshTransfer_Progress(src As String, dst As String, transferredBytes As Integer,
                                             totalBytes As Integer, message As String)
                maxVal = totalBytes
                curVal = transferredBytes

                SetStatus()
            End Sub

            Private Sub SshTransfer_End(src As String, dst As String, transferredBytes As Integer, totalBytes As Integer,
                                        message As String)
                Try
                    maxVal = totalBytes
                    curVal = transferredBytes

                    EnableButtons()
                    SetStatus()

                    ' TODO: Localize that message or change to My.Language.strSSHTranferSuccessful accordingly.
                    ' Original message was "SSH transfer failed." But that didn't seem to be accurate (since my test was successsful).
                    ' Need to review further to see if there is any other error handlding for failed transfers before this and
                    ' if this should be changed to successful.
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "SSH Transfer Ended.")

                    If Me.sshT IsNot Nothing Then
                        Me.sshT.Close()
                    ElseIf Me.sshT IsNot Nothing Then
                        Me.sshT.Close()
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strSSHTransferEndFailed & vbNewLine &
                                                        ex.ToString(), True)
                End Try
            End Sub

#End Region

#Region "Public Methods"

            Public Sub New(Panel As DockContent)
                Me.WindowType = Type.SSHTransfer
                Me.DockPnl = Panel
                Me.InitializeComponent()

                Me.oDlg = New OpenFileDialog
                Me.oDlg.Filter = "All Files (*.*)|*.*"
                Me.oDlg.CheckFileExists = True
            End Sub

#End Region

#Region "Form Stuff"

            Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
                If Me.oDlg.ShowDialog = DialogResult.OK Then
                    If Me.oDlg.FileName <> "" Then
                        Me.txtLocalFile.Text = Me.oDlg.FileName
                    End If
                End If
            End Sub

            Private Sub btnTransfer_Click(sender As Object, e As EventArgs) Handles btnTransfer.Click
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