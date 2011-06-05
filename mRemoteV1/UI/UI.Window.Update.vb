Imports WeifenLuo.WinFormsUI.Docking
Imports System.Threading
Imports System.IO
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class Update
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents lblStatus As System.Windows.Forms.Label
            Friend WithEvents txtChangeLog As System.Windows.Forms.TextBox
            Friend WithEvents prgbDownload As System.Windows.Forms.ProgressBar
            Friend WithEvents btnDownload As System.Windows.Forms.Button
            Friend WithEvents lblChangeLogLabel As System.Windows.Forms.Label
            Friend WithEvents pnlUp As System.Windows.Forms.Panel
            Friend WithEvents lblCurrentVersionLabel As System.Windows.Forms.Label
            Friend WithEvents lblInstalledVersionLabel As System.Windows.Forms.Label
            Friend WithEvents lblAvailableVersion As System.Windows.Forms.Label
            Friend WithEvents lblCurrentVersion As System.Windows.Forms.Label
            Friend WithEvents pbUpdateImage As System.Windows.Forms.PictureBox
            Friend WithEvents btnCheckForUpdate As System.Windows.Forms.Button

            Private Sub InitializeComponent()
                Me.btnCheckForUpdate = New System.Windows.Forms.Button
                Me.pnlUp = New System.Windows.Forms.Panel
                Me.lblChangeLogLabel = New System.Windows.Forms.Label
                Me.btnDownload = New System.Windows.Forms.Button
                Me.prgbDownload = New System.Windows.Forms.ProgressBar
                Me.txtChangeLog = New System.Windows.Forms.TextBox
                Me.lblStatus = New System.Windows.Forms.Label
                Me.lblCurrentVersionLabel = New System.Windows.Forms.Label
                Me.lblInstalledVersionLabel = New System.Windows.Forms.Label
                Me.lblAvailableVersion = New System.Windows.Forms.Label
                Me.lblCurrentVersion = New System.Windows.Forms.Label
                Me.pbUpdateImage = New System.Windows.Forms.PictureBox
                Me.pnlUp.SuspendLayout()
                CType(Me.pbUpdateImage, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.SuspendLayout()
                '
                'btnCheckForUpdate
                '
                Me.btnCheckForUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnCheckForUpdate.Location = New System.Drawing.Point(16, 104)
                Me.btnCheckForUpdate.Name = "btnCheckForUpdate"
                Me.btnCheckForUpdate.Size = New System.Drawing.Size(104, 32)
                Me.btnCheckForUpdate.TabIndex = 5
                Me.btnCheckForUpdate.Text = "Check Again"
                Me.btnCheckForUpdate.UseVisualStyleBackColor = True
                '
                'pnlUp
                '
                Me.pnlUp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlUp.Controls.Add(Me.lblChangeLogLabel)
                Me.pnlUp.Controls.Add(Me.btnDownload)
                Me.pnlUp.Controls.Add(Me.prgbDownload)
                Me.pnlUp.Controls.Add(Me.txtChangeLog)
                Me.pnlUp.Location = New System.Drawing.Point(16, 152)
                Me.pnlUp.Name = "pnlUp"
                Me.pnlUp.Size = New System.Drawing.Size(718, 248)
                Me.pnlUp.TabIndex = 6
                Me.pnlUp.Visible = False
                '
                'lblChangeLogLabel
                '
                Me.lblChangeLogLabel.AutoSize = True
                Me.lblChangeLogLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblChangeLogLabel.Location = New System.Drawing.Point(0, 0)
                Me.lblChangeLogLabel.Name = "lblChangeLogLabel"
                Me.lblChangeLogLabel.Size = New System.Drawing.Size(79, 13)
                Me.lblChangeLogLabel.TabIndex = 0
                Me.lblChangeLogLabel.Text = "Change Log:"
                '
                'btnDownload
                '
                Me.btnDownload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnDownload.Location = New System.Drawing.Point(0, 216)
                Me.btnDownload.Name = "btnDownload"
                Me.btnDownload.Size = New System.Drawing.Size(144, 32)
                Me.btnDownload.TabIndex = 2
                Me.btnDownload.Text = "Download and Install"
                Me.btnDownload.UseVisualStyleBackColor = True
                '
                'prgbDownload
                '
                Me.prgbDownload.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.prgbDownload.Location = New System.Drawing.Point(160, 224)
                Me.prgbDownload.Name = "prgbDownload"
                Me.prgbDownload.Size = New System.Drawing.Size(542, 23)
                Me.prgbDownload.TabIndex = 3
                '
                'txtChangeLog
                '
                Me.txtChangeLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtChangeLog.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.txtChangeLog.Cursor = System.Windows.Forms.Cursors.Arrow
                Me.txtChangeLog.Location = New System.Drawing.Point(16, 24)
                Me.txtChangeLog.Multiline = True
                Me.txtChangeLog.Name = "txtChangeLog"
                Me.txtChangeLog.ReadOnly = True
                Me.txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
                Me.txtChangeLog.Size = New System.Drawing.Size(699, 181)
                Me.txtChangeLog.TabIndex = 1
                Me.txtChangeLog.TabStop = False
                '
                'lblStatus
                '
                Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
                Me.lblStatus.Location = New System.Drawing.Point(12, 16)
                Me.lblStatus.Name = "lblStatus"
                Me.lblStatus.Size = New System.Drawing.Size(660, 23)
                Me.lblStatus.TabIndex = 0
                Me.lblStatus.Text = "Status"
                Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                '
                'lblCurrentVersionLabel
                '
                Me.lblCurrentVersionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblCurrentVersionLabel.Location = New System.Drawing.Point(16, 72)
                Me.lblCurrentVersionLabel.Name = "lblCurrentVersionLabel"
                Me.lblCurrentVersionLabel.Size = New System.Drawing.Size(120, 16)
                Me.lblCurrentVersionLabel.TabIndex = 3
                Me.lblCurrentVersionLabel.Text = "Current version:"
                Me.lblCurrentVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
                '
                'lblInstalledVersionLabel
                '
                Me.lblInstalledVersionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblInstalledVersionLabel.Location = New System.Drawing.Point(16, 48)
                Me.lblInstalledVersionLabel.Name = "lblInstalledVersionLabel"
                Me.lblInstalledVersionLabel.Size = New System.Drawing.Size(120, 16)
                Me.lblInstalledVersionLabel.TabIndex = 1
                Me.lblInstalledVersionLabel.Text = "Installed version:"
                Me.lblInstalledVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
                '
                'lblAvailableVersion
                '
                Me.lblAvailableVersion.Location = New System.Drawing.Point(136, 72)
                Me.lblAvailableVersion.Name = "lblAvailableVersion"
                Me.lblAvailableVersion.Size = New System.Drawing.Size(104, 16)
                Me.lblAvailableVersion.TabIndex = 4
                Me.lblAvailableVersion.Text = "Version"
                Me.lblAvailableVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                '
                'lblCurrentVersion
                '
                Me.lblCurrentVersion.Location = New System.Drawing.Point(136, 48)
                Me.lblCurrentVersion.Name = "lblCurrentVersion"
                Me.lblCurrentVersion.Size = New System.Drawing.Size(104, 16)
                Me.lblCurrentVersion.TabIndex = 2
                Me.lblCurrentVersion.Text = "Version"
                Me.lblCurrentVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                '
                'pbUpdateImage
                '
                Me.pbUpdateImage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pbUpdateImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.pbUpdateImage.Cursor = System.Windows.Forms.Cursors.Hand
                Me.pbUpdateImage.Location = New System.Drawing.Point(246, 48)
                Me.pbUpdateImage.Name = "pbUpdateImage"
                Me.pbUpdateImage.Size = New System.Drawing.Size(468, 60)
                Me.pbUpdateImage.TabIndex = 45
                Me.pbUpdateImage.TabStop = False
                Me.pbUpdateImage.Visible = False
                '
                'Update
                '
                Me.ClientSize = New System.Drawing.Size(734, 418)
                Me.Controls.Add(Me.pbUpdateImage)
                Me.Controls.Add(Me.lblCurrentVersionLabel)
                Me.Controls.Add(Me.lblInstalledVersionLabel)
                Me.Controls.Add(Me.lblAvailableVersion)
                Me.Controls.Add(Me.btnCheckForUpdate)
                Me.Controls.Add(Me.lblCurrentVersion)
                Me.Controls.Add(Me.pnlUp)
                Me.Controls.Add(Me.lblStatus)
                Me.Icon = Global.mRemoteNG.My.Resources.Resources.Update_Icon
                Me.Name = "Update"
                Me.TabText = "Update"
                Me.Text = "Update"
                Me.pnlUp.ResumeLayout(False)
                Me.pnlUp.PerformLayout()
                CType(Me.pbUpdateImage, System.ComponentModel.ISupportInitialize).EndInit()
                Me.ResumeLayout(False)

            End Sub
#End Region

#Region "Private Properties"
            Private uD As App.Update
            Private uT As Thread

            Private IsUpdateCheckHandlerDeclared As Boolean = False
            Private IsUpdateDownloadHandlerDeclared As Boolean = False
#End Region

#Region "Public Events"
            Public Event UpdateCheckCompleted(ByVal UpdateAvailable As Boolean)
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.Update
                Me.DockPnl = Panel
                Me.InitializeComponent()
                App.Runtime.FontOverride(Me)
            End Sub

            Public Sub CheckForUpdate()
                Try
                    uT = New Thread(AddressOf CheckForUpdateBG)
                    uT.SetApartmentState(ApartmentState.STA)
                    uT.IsBackground = True

                    If Me.IsUpdateCheckHandlerDeclared = False Then
                        AddHandler UpdateCheckCompleted, AddressOf UpdateCheckComplete
                        Me.IsUpdateCheckHandlerDeclared = True
                    End If

                    uT.Start()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strUpdateCheckFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Private Methods"
            Private Sub CheckForUpdateBG()
                Try
                    uD = New App.Update

                    If uD.IsUpdateAvailable = True Then
                        RaiseEvent UpdateCheckCompleted(True)
                    Else
                        RaiseEvent UpdateCheckCompleted(False)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strUpdateCheckFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub UpdateCheckComplete(ByVal UpdateAvailable As Boolean)
                Try
                    My.Settings.CheckForUpdatesLastCheck = Date.Now
                    SetCurrentVersionText(My.Application.Info.Version.ToString)

                    If UpdateAvailable = True Then
                        My.Settings.UpdatePending = True

                        SetStatus(Color.OrangeRed, My.Resources.strUpdateAvailable)
                        SetVisible(pnlUp, True)

                        Dim uI As App.Update.Info = uD.GetUpdateInfo()
                        SetAvailableVersionText(uI.Version.ToString)
                        SetChangeLogText(uI.ChangeLog)

                        If uI.ImageURL <> String.Empty Then
                            SetImageURL(uI.ImageURL)

                            If uI.ImageURLLink <> String.Empty Then
                                pbUpdateImage.Tag = uI.ImageURLLink
                            End If

                            SetVisible(pbUpdateImage, True)
                        Else
                            SetVisible(pbUpdateImage, False)
                        End If

                        FocusDownloadButton()
                    Else
                        My.Settings.UpdatePending = False

                        SetStatus(Color.ForestGreen, My.Resources.strNoUpdateAvailable)
                        SetVisible(pnlUp, False)

                        Dim uI As App.Update.Info = uD.GetUpdateInfo()
                        SetAvailableVersionText(uI.Version.ToString)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strUpdateCheckCompleteFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Threading Callbacks"
            Private Delegate Sub SetImageURLCB(ByVal img As String)
            Private Sub SetImageURL(ByVal img As String)
                If Me.pbUpdateImage.InvokeRequired = True Then
                    Dim d As New SetImageURLCB(AddressOf SetImageURL)
                    Me.pbUpdateImage.Invoke(d, New Object() {img})
                Else
                    Me.pbUpdateImage.ImageLocation = img
                End If
            End Sub

            Private Delegate Sub SetStatusCB(ByVal Color As Color, ByVal Text As String)
            Private Sub SetStatus(ByVal Color As Color, ByVal Text As String)
                If Me.lblStatus.InvokeRequired = True Then
                    Dim d As New SetStatusCB(AddressOf SetStatus)
                    Me.lblStatus.Invoke(d, New Object() {Color, Text})
                Else
                    Me.lblStatus.ForeColor = Color
                    Me.lblStatus.Text = Text
                End If
            End Sub

            Private Delegate Sub SetVisibleCB(ByVal ctrl As Control, ByVal visible As Boolean)
            Private Sub SetVisible(ByVal ctrl As Control, ByVal visible As Boolean)
                If ctrl.InvokeRequired Then
                    Dim d As New SetVisibleCB(AddressOf SetVisible)
                    ctrl.Invoke(d, New Object() {ctrl, visible})
                Else
                    ctrl.Visible = visible
                End If
            End Sub

            Private Delegate Sub SetCurrentVersionTextCB(ByVal Text As String)
            Private Sub SetCurrentVersionText(ByVal Text As String)
                If Me.lblCurrentVersion.InvokeRequired = True Then
                    Dim d As New SetCurrentVersionTextCB(AddressOf SetCurrentVersionText)
                    Me.lblCurrentVersion.Invoke(d, New Object() {Text})
                Else
                    Me.lblCurrentVersion.Text = Text
                End If
            End Sub

            Private Delegate Sub SetAvailableVersionTextCB(ByVal Text As String)
            Private Sub SetAvailableVersionText(ByVal Text As String)
                If Me.lblAvailableVersion.InvokeRequired = True Then
                    Dim d As New SetAvailableVersionTextCB(AddressOf SetAvailableVersionText)
                    Me.lblAvailableVersion.Invoke(d, New Object() {Text})
                Else
                    Me.lblAvailableVersion.Text = Text
                End If
            End Sub

            Private Delegate Sub SetChangeLogTextCB(ByVal Text As String)
            Private Sub SetChangeLogText(ByVal Text As String)
                If Me.txtChangeLog.InvokeRequired = True Then
                    Dim d As New SetChangeLogTextCB(AddressOf SetChangeLogText)
                    Me.txtChangeLog.Invoke(d, New Object() {Text})
                Else
                    Me.txtChangeLog.Text = Text
                End If
            End Sub

            Private Delegate Sub FocusDownloadButtonCB()
            Private Sub FocusDownloadButton()
                If Me.btnDownload.InvokeRequired = True Then
                    Dim d As New FocusDownloadButtonCB(AddressOf FocusDownloadButton)
                    Me.btnDownload.Invoke(d)
                Else
                    Me.btnDownload.Focus()
                End If
            End Sub
#End Region

#Region "Form Stuff"
            Private Sub Update_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()


                Me.CheckForUpdate()
            End Sub

            Private Sub ApplyLanguage()
                btnCheckForUpdate.Text = My.Resources.strCheckForUpdate
                lblChangeLogLabel.Text = My.Resources.strLabelChangeLog
                btnDownload.Text = My.Resources.strDownloadAndInstall
                lblCurrentVersionLabel.Text = My.Resources.strAvailableVersion & ":"
                lblInstalledVersionLabel.Text = My.Resources.strCurrentVersion & ":"
                lblAvailableVersion.Text = My.Resources.strVersion
                lblCurrentVersion.Text = My.Resources.strVersion
                TabText = My.Resources.strMenuCheckForUpdates
                Text = My.Resources.strMenuCheckForUpdates
            End Sub

            Private Sub btnCheckForUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckForUpdate.Click
                Me.CheckForUpdate()
            End Sub

            Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownload.Click
                Me.DownloadUpdate()
            End Sub

            Private Sub DownloadUpdate()
                Try
                    If uD.DownloadUpdate(uD.curUI.DownloadUrl) = True Then
                        Me.btnDownload.Enabled = False

                        If Me.IsUpdateDownloadHandlerDeclared = False Then
                            AddHandler uD.DownloadProgressChanged, AddressOf DLProgressChanged
                            AddHandler uD.DownloadCompleted, AddressOf DLCompleted
                            Me.IsUpdateDownloadHandlerDeclared = True
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strUpdateDownloadFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Events"
            Private Sub DLProgressChanged(ByVal sender As Object, ByVal e As System.Net.DownloadProgressChangedEventArgs)
                Me.prgbDownload.Value = e.ProgressPercentage
            End Sub

            Private Sub DLCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs, ByVal Success As Boolean)
                Try
                    Me.btnDownload.Enabled = True

                    If Success = True Then
                        If MessageBox.Show(My.Resources.strUpdateDownloadComplete, My.Resources.strMenuCheckForUpdates, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = System.Windows.Forms.DialogResult.OK Then
                            Try
                                App.Runtime.Shutdown.BeforeQuit()

                                Process.Start(uD.curUI.UpdateLocation)
                            Catch ex As Exception
                                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strUpdateStartFailed & vbNewLine & ex.Message)
                            End Try

                            End
                        Else
                            Try
                                File.Delete(uD.curUI.UpdateLocation)
                            Catch ex As Exception
                                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strUpdateDeleteFailed & vbNewLine & ex.Message)
                            End Try
                        End If
                    Else
                        MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strUpdateDownloadFailed)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strUpdateDownloadCompleteFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

            Private Sub pbUpdateImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
                If pbUpdateImage.Tag IsNot Nothing Then
                    Process.Start(pbUpdateImage.Tag)
                End If
            End Sub
        End Class
    End Namespace
End Namespace