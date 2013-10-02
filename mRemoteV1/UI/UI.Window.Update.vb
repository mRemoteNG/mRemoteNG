Imports System.ComponentModel
Imports mRemoteNG.My
Imports WeifenLuo.WinFormsUI.Docking
Imports System.IO
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class Update
            Inherits Base
#Region "Public Methods"
            Public Sub New(ByVal panel As DockContent)
                WindowType = Type.Update
                DockPnl = panel
                InitializeComponent()
                FontOverride(Me)
            End Sub
#End Region

#Region "Form Stuff"
            Private Sub Update_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
                ApplyLanguage()
                CheckForUpdate()
            End Sub

            Private Sub ApplyLanguage()
                Text = Language.strMenuCheckForUpdates
                TabText = Language.strMenuCheckForUpdates
                btnCheckForUpdate.Text = Language.strCheckForUpdate
                btnDownload.Text = Language.strDownloadAndInstall
                lblChangeLogLabel.Text = Language.strLabelChangeLog
                lblInstalledVersion.Text = Language.strVersion
                lblInstalledVersionLabel.Text = String.Format("{0}:", Language.strCurrentVersion)
                lblLatestVersion.Text = Language.strVersion
                lblLatestVersionLabel.Text = String.Format("{0}:", Language.strAvailableVersion)
            End Sub

            Private Sub btnCheckForUpdate_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnCheckForUpdate.Click
                CheckForUpdate()
            End Sub

            Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnDownload.Click
                DownloadUpdate()
            End Sub

            Private Sub pbUpdateImage_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles pbUpdateImage.Click
                Dim linkUri As Uri = TryCast(pbUpdateImage.Tag, Uri)
                If linkUri Is Nothing OrElse linkUri.IsFile Or linkUri.IsUnc Or linkUri.IsLoopback Then Return
                Process.Start(linkUri.ToString())
            End Sub
#End Region

#Region "Private Fields"
            Private _appUpdate As App.Update
            Private _isUpdateDownloadHandlerDeclared As Boolean = False
#End Region

#Region "Private Methods"
            Private Sub CheckForUpdate()
                If _appUpdate Is Nothing Then
                    _appUpdate = New App.Update
                ElseIf _appUpdate.IsGetUpdateInfoRunning Then
                    Return
                End If

                lblStatus.Text = Language.strUpdateCheckingLabel
                lblStatus.ForeColor = SystemColors.WindowText
                lblLatestVersionLabel.Visible = False
                lblInstalledVersion.Visible = False
                lblInstalledVersionLabel.Visible = False
                lblLatestVersion.Visible = False
                btnCheckForUpdate.Visible = False
                pnlUpdate.Visible = False

                AddHandler _appUpdate.GetUpdateInfoCompletedEvent, AddressOf GetUpdateInfoCompleted

                _appUpdate.GetUpdateInfoAsync()
            End Sub

            Private Sub GetUpdateInfoCompleted(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
                If InvokeRequired Then
                    Dim myDelegate As New AsyncCompletedEventHandler(AddressOf GetUpdateInfoCompleted)
                    Invoke(myDelegate, New Object() {sender, e})
                    Return
                End If

                Try
                    RemoveHandler _appUpdate.GetUpdateInfoCompletedEvent, AddressOf GetUpdateInfoCompleted

                    lblInstalledVersion.Text = Application.Info.Version.ToString
                    lblInstalledVersion.Visible = True
                    lblInstalledVersionLabel.Visible = True
                    btnCheckForUpdate.Visible = True

                    If e.Cancelled Then Return
                    If e.Error IsNot Nothing Then Throw e.Error

                    If _appUpdate.IsUpdateAvailable() Then
                        lblStatus.Text = Language.strUpdateAvailable
                        lblStatus.ForeColor = Color.OrangeRed
                        pnlUpdate.Visible = True

                        Dim updateInfo As App.Update.UpdateInfo = _appUpdate.CurrentUpdateInfo
                        lblLatestVersion.Text = updateInfo.Version.ToString
                        lblLatestVersionLabel.Visible = True
                        lblLatestVersion.Visible = True

                        If updateInfo.ImageAddress Is Nothing OrElse String.IsNullOrEmpty(updateInfo.ImageAddress.ToString()) Then
                            pbUpdateImage.Visible = False
                        Else
                            pbUpdateImage.ImageLocation = updateInfo.ImageAddress.ToString()
                            pbUpdateImage.Tag = updateInfo.ImageLinkAddress
                            pbUpdateImage.Visible = True
                        End If

                        AddHandler _appUpdate.GetChangeLogCompletedEvent, AddressOf GetChangeLogCompleted
                        _appUpdate.GetChangeLogAsync()

                        btnDownload.Focus()
                    Else
                        lblStatus.Text = Language.strNoUpdateAvailable
                        lblStatus.ForeColor = Color.ForestGreen

                        If _appUpdate.CurrentUpdateInfo IsNot Nothing Then
                            Dim updateInfo As App.Update.UpdateInfo = _appUpdate.CurrentUpdateInfo
                            If updateInfo.IsValid And updateInfo.Version IsNot Nothing Then
                                lblLatestVersion.Text = updateInfo.Version.ToString
                                lblLatestVersionLabel.Visible = True
                                lblLatestVersion.Visible = True
                            End If
                        End If
                    End If
                Catch ex As Exception
                    lblStatus.Text = Language.strUpdateCheckFailedLabel
                    lblStatus.ForeColor = Color.OrangeRed

                    MessageCollector.AddExceptionMessage(Language.strUpdateCheckCompleteFailed, ex)
                End Try
            End Sub

            Private Sub GetChangeLogCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
                If InvokeRequired Then
                    Dim myDelegate As New AsyncCompletedEventHandler(AddressOf GetChangeLogCompleted)
                    Invoke(myDelegate, New Object() {sender, e})
                    Return
                End If

                Try
                    RemoveHandler _appUpdate.GetChangeLogCompletedEvent, AddressOf GetChangeLogCompleted

                    If e.Cancelled Then Return
                    If e.Error IsNot Nothing Then Throw e.Error

                    txtChangeLog.Text = _appUpdate.ChangeLog
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage(Language.strUpdateGetChangeLogFailed, ex)
                End Try
            End Sub

            Private Sub DownloadUpdate()
                Try
                    btnDownload.Enabled = False
                    prgbDownload.Visible = True
                    prgbDownload.Value = 0

                    If _isUpdateDownloadHandlerDeclared = False Then
                        AddHandler _appUpdate.DownloadUpdateProgressChangedEvent, AddressOf DownloadUpdateProgressChanged
                        AddHandler _appUpdate.DownloadUpdateCompletedEvent, AddressOf DownloadUpdateCompleted
                        _isUpdateDownloadHandlerDeclared = True
                    End If

                    _appUpdate.DownloadUpdateAsync()
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage(Language.strUpdateDownloadFailed, ex)
                End Try
            End Sub
#End Region

#Region "Events"
            Private Sub DownloadUpdateProgressChanged(ByVal sender As Object, ByVal e As Net.DownloadProgressChangedEventArgs)
                prgbDownload.Value = e.ProgressPercentage
            End Sub

            Private Sub DownloadUpdateCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
                Try
                    btnDownload.Enabled = True
                    prgbDownload.Visible = False

                    If e.Cancelled Then Return
                    If e.Error IsNot Nothing Then Throw e.Error

                    If MessageBox.Show(Language.strUpdateDownloadComplete, Language.strMenuCheckForUpdates, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = System.Windows.Forms.DialogResult.OK Then
                        Shutdown.Quit(_appUpdate.CurrentUpdateInfo.UpdateFilePath)
                        Return
                    Else
                        File.Delete(_appUpdate.CurrentUpdateInfo.UpdateFilePath)
                    End If
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage(Language.strUpdateDownloadCompleteFailed, ex)
                End Try
            End Sub
#End Region
        End Class
    End Namespace
End Namespace