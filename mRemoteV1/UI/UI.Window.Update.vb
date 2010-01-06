Imports WeifenLuo.WinFormsUI.Docking
Imports System.Threading
Imports System.IO
Imports mRemote.App.Runtime

Namespace UI
    Namespace Window
        Public Class Update
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents lblStatus As System.Windows.Forms.Label
            Friend WithEvents txtChangeLog As System.Windows.Forms.TextBox
            Friend WithEvents lblCurrentVersion As System.Windows.Forms.Label
            Friend WithEvents lblAvailableVersion As System.Windows.Forms.Label
            Friend WithEvents prgbDownload As System.Windows.Forms.ProgressBar
            Friend WithEvents Label5 As System.Windows.Forms.Label
            Friend WithEvents Label4 As System.Windows.Forms.Label
            Friend WithEvents btnDownload As System.Windows.Forms.Button
            Friend WithEvents Label7 As System.Windows.Forms.Label
            Friend WithEvents pnlUp As System.Windows.Forms.Panel
            Friend WithEvents pbUpdateImage As System.Windows.Forms.PictureBox
            Friend WithEvents btnCheckForUpdate As System.Windows.Forms.Button

            Private Sub InitializeComponent()
                Me.btnCheckForUpdate = New System.Windows.Forms.Button
                Me.pnlUp = New System.Windows.Forms.Panel
                Me.pbUpdateImage = New System.Windows.Forms.PictureBox
                Me.Label7 = New System.Windows.Forms.Label
                Me.btnDownload = New System.Windows.Forms.Button
                Me.Label4 = New System.Windows.Forms.Label
                Me.Label5 = New System.Windows.Forms.Label
                Me.prgbDownload = New System.Windows.Forms.ProgressBar
                Me.lblAvailableVersion = New System.Windows.Forms.Label
                Me.lblCurrentVersion = New System.Windows.Forms.Label
                Me.txtChangeLog = New System.Windows.Forms.TextBox
                Me.lblStatus = New System.Windows.Forms.Label
                Me.pnlUp.SuspendLayout()
                CType(Me.pbUpdateImage, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.SuspendLayout()
                '
                'btnCheckForUpdate
                '
                Me.btnCheckForUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnCheckForUpdate.Location = New System.Drawing.Point(12, 12)
                Me.btnCheckForUpdate.Name = "btnCheckForUpdate"
                Me.btnCheckForUpdate.Size = New System.Drawing.Size(136, 23)
                Me.btnCheckForUpdate.TabIndex = 10
                Me.btnCheckForUpdate.Text = "Check for update"
                Me.btnCheckForUpdate.UseVisualStyleBackColor = True
                '
                'pnlUp
                '
                Me.pnlUp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlUp.Controls.Add(Me.pbUpdateImage)
                Me.pnlUp.Controls.Add(Me.Label7)
                Me.pnlUp.Controls.Add(Me.btnDownload)
                Me.pnlUp.Controls.Add(Me.Label4)
                Me.pnlUp.Controls.Add(Me.Label5)
                Me.pnlUp.Controls.Add(Me.prgbDownload)
                Me.pnlUp.Controls.Add(Me.lblAvailableVersion)
                Me.pnlUp.Controls.Add(Me.lblCurrentVersion)
                Me.pnlUp.Controls.Add(Me.txtChangeLog)
                Me.pnlUp.Location = New System.Drawing.Point(12, 41)
                Me.pnlUp.Name = "pnlUp"
                Me.pnlUp.Size = New System.Drawing.Size(668, 370)
                Me.pnlUp.TabIndex = 30
                Me.pnlUp.Visible = False
                '
                'pbUpdateImage
                '
                Me.pbUpdateImage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pbUpdateImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.pbUpdateImage.Cursor = System.Windows.Forms.Cursors.Hand
                Me.pbUpdateImage.Location = New System.Drawing.Point(197, 3)
                Me.pbUpdateImage.Name = "pbUpdateImage"
                Me.pbUpdateImage.Size = New System.Drawing.Size(468, 60)
                Me.pbUpdateImage.TabIndex = 31
                Me.pbUpdateImage.TabStop = False
                Me.pbUpdateImage.Visible = False
                '
                'Label7
                '
                Me.Label7.AutoSize = True
                Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.Label7.Location = New System.Drawing.Point(3, 69)
                Me.Label7.Name = "Label7"
                Me.Label7.Size = New System.Drawing.Size(79, 13)
                Me.Label7.TabIndex = 50
                Me.Label7.Text = "Change Log:"
                '
                'btnDownload
                '
                Me.btnDownload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnDownload.Location = New System.Drawing.Point(6, 341)
                Me.btnDownload.Name = "btnDownload"
                Me.btnDownload.Size = New System.Drawing.Size(145, 23)
                Me.btnDownload.TabIndex = 70
                Me.btnDownload.Text = "Download and Install"
                Me.btnDownload.UseVisualStyleBackColor = True
                '
                'Label4
                '
                Me.Label4.AutoSize = True
                Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.Label4.Location = New System.Drawing.Point(3, 39)
                Me.Label4.Name = "Label4"
                Me.Label4.Size = New System.Drawing.Size(109, 13)
                Me.Label4.TabIndex = 30
                Me.Label4.Text = "Available Version:"
                '
                'Label5
                '
                Me.Label5.AutoSize = True
                Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.Label5.Location = New System.Drawing.Point(3, 9)
                Me.Label5.Name = "Label5"
                Me.Label5.Size = New System.Drawing.Size(98, 13)
                Me.Label5.TabIndex = 10
                Me.Label5.Text = "Current Version:"
                '
                'prgbDownload
                '
                Me.prgbDownload.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.prgbDownload.Location = New System.Drawing.Point(157, 341)
                Me.prgbDownload.Name = "prgbDownload"
                Me.prgbDownload.Size = New System.Drawing.Size(508, 23)
                Me.prgbDownload.TabIndex = 80
                '
                'lblAvailableVersion
                '
                Me.lblAvailableVersion.Location = New System.Drawing.Point(118, 39)
                Me.lblAvailableVersion.Name = "lblAvailableVersion"
                Me.lblAvailableVersion.Size = New System.Drawing.Size(67, 13)
                Me.lblAvailableVersion.TabIndex = 40
                Me.lblAvailableVersion.Text = "Version"
                '
                'lblCurrentVersion
                '
                Me.lblCurrentVersion.Location = New System.Drawing.Point(118, 9)
                Me.lblCurrentVersion.Name = "lblCurrentVersion"
                Me.lblCurrentVersion.Size = New System.Drawing.Size(67, 13)
                Me.lblCurrentVersion.TabIndex = 20
                Me.lblCurrentVersion.Text = "Version"
                '
                'txtChangeLog
                '
                Me.txtChangeLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtChangeLog.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.txtChangeLog.Cursor = System.Windows.Forms.Cursors.Arrow
                Me.txtChangeLog.Location = New System.Drawing.Point(121, 69)
                Me.txtChangeLog.Multiline = True
                Me.txtChangeLog.Name = "txtChangeLog"
                Me.txtChangeLog.ReadOnly = True
                Me.txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
                Me.txtChangeLog.Size = New System.Drawing.Size(544, 266)
                Me.txtChangeLog.TabIndex = 60
                Me.txtChangeLog.TabStop = False
                '
                'lblStatus
                '
                Me.lblStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
                Me.lblStatus.Location = New System.Drawing.Point(342, 12)
                Me.lblStatus.Name = "lblStatus"
                Me.lblStatus.Size = New System.Drawing.Size(338, 23)
                Me.lblStatus.TabIndex = 20
                Me.lblStatus.Text = "Status"
                Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight
                '
                'Update
                '
                Me.ClientSize = New System.Drawing.Size(692, 423)
                Me.Controls.Add(Me.btnCheckForUpdate)
                Me.Controls.Add(Me.pnlUp)
                Me.Controls.Add(Me.lblStatus)
                Me.Icon = Global.mRemote.My.Resources.Resources.Update_Icon
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
            End Sub

            Public Sub CheckForUpdate()
                Try
                    uT = New Thread(AddressOf CheckForUpdateBG)
                    uT.IsBackground = True
                    'uT.SetApartmentState(ApartmentState.STA)

                    If Me.IsUpdateCheckHandlerDeclared = False Then
                        AddHandler UpdateCheckCompleted, AddressOf UpdateCheckComplete
                        Me.IsUpdateCheckHandlerDeclared = True
                    End If

                    uT.Start()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "CheckForUpdate (UI.Window.Update) failed" & vbNewLine & ex.Message, True)
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
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "CheckForUpdateBG (UI.Window.Update) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub UpdateCheckComplete(ByVal UpdateAvailable As Boolean)
                Try
                    If UpdateAvailable = True Then
                        Me.SetStatus(Color.ForestGreen, Language.Base.UpdateAvailable)
                        Me.SetVisible(pnlUp, True)

                        Dim uI As App.Update.Info = uD.GetUpdateInfo()
                        Me.SetCurrentVersionText(My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor)
                        Me.SetAvailableVersionText(uI.Version.ToString)
                        Me.SetChangeLogText(uI.ChangeLog)

                        If uI.ImageURL <> String.Empty Then
                            Me.SetImageURL(uI.ImageURL)

                            If uI.ImageURLLink <> String.Empty Then
                                pbUpdateImage.Tag = uI.ImageURLLink
                            End If

                            Me.SetVisible(pbUpdateImage, True)
                        Else
                            Me.SetVisible(pbUpdateImage, False)
                        End If

                        Me.FocusDownloadButton()
                    Else
                        Me.SetStatus(Color.OrangeRed, Language.Base.NoUpdateAvailable)
                        Me.SetVisible(pnlUp, False)
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "UpdateCheckComplete (UI.Window.Update) failed" & vbNewLine & ex.Message, True)
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
                btnCheckForUpdate.Text = Language.Base.CheckForUpdate
                Label7.Text = Language.Base.ChangeLog
                btnDownload.Text = Language.Base.DownloadAndInstall
                Label4.Text = Language.Base.AvailableVersion & ":"
                Label5.Text = Language.Base.CurrentVersion & ":"
                lblAvailableVersion.Text = Language.Base.Version
                lblCurrentVersion.Text = Language.Base.Version
                lblStatus.Text = Language.Base.Status
                TabText = Language.Base.Menu_Update
                Text = Language.Base.Menu_Update
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
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "DownloadUpdate (UI.Window.Update) failed" & vbNewLine & ex.Message, True)
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
                        If MessageBox.Show(Language.Base.UpdateDownloadComplete, Language.Base.Menu_Update, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = System.Windows.Forms.DialogResult.OK Then
                            Try
                                App.Runtime.Shutdown.BeforeQuit()

                                Process.Start(uD.curUI.UpdateLocation)
                            Catch ex As Exception
                                mC.AddMessage(Messages.MessageClass.ErrorMsg, "Error starting update!" & vbNewLine & ex.Message)
                            End Try

                            End
                        Else
                            Try
                                File.Delete(uD.curUI.UpdateLocation)
                            Catch ex As Exception
                                mC.AddMessage(Messages.MessageClass.ErrorMsg, "Error deleting update file!" & vbNewLine & ex.Message)
                            End Try
                        End If
                    Else
                        mC.AddMessage(Messages.MessageClass.ErrorMsg, "Update download failed!")
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "DLCompleted (UI.Window.Update) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

            Private Sub pbUpdateImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbUpdateImage.Click
                If pbUpdateImage.Tag IsNot Nothing Then
                    Process.Start(pbUpdateImage.Tag)
                End If
            End Sub
        End Class
    End Namespace
End Namespace