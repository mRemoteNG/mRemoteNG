Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports mRemote3G.App
Imports WeifenLuo.WinFormsUI.Docking

Namespace UI

    Namespace Window
        Public Class Update
            Inherits Base

#Region "Public Methods"

            Public Sub New(panel As DockContent)
                WindowType = Type.Update
                DockPnl = panel
                InitializeComponent()
                Runtime.FontOverride(Me)
            End Sub

#End Region

#Region "Form Stuff"

            Private Sub Update_Load(sender As Object, e As EventArgs) Handles Me.Load
                ApplyLanguage()
                CheckForUpdate()
            End Sub

            Private Sub ApplyLanguage()
                Text = Language.Language.strMenuCheckForUpdates
                TabText = Language.Language.strMenuCheckForUpdates
                btnCheckForUpdate.Text = Language.Language.strCheckForUpdate
                btnDownload.Text = Language.Language.strDownloadAndInstall
                lblChangeLogLabel.Text = Language.Language.strLabelChangeLog
                lblInstalledVersion.Text = Language.Language.strVersion
                lblInstalledVersionLabel.Text = String.Format("{0}:", Language.Language.strCurrentVersion)
                lblLatestVersion.Text = Language.Language.strVersion
                lblLatestVersionLabel.Text = String.Format("{0}:", Language.Language.strAvailableVersion)
            End Sub

            Private Sub btnCheckForUpdate_Click(sender As Object, e As EventArgs) Handles btnCheckForUpdate.Click
                CheckForUpdate()
            End Sub

            Private Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
                DownloadUpdate()
            End Sub

            Private Sub pbUpdateImage_Click(sender As Object, e As EventArgs) Handles pbUpdateImage.Click
                Dim linkUri = TryCast(pbUpdateImage.Tag, Uri)
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

                'lblStatus.Text = Language.strUpdateCheckingLabel
                lblStatus.Text = "Update Check is currently disabled."
                lblStatus.ForeColor = SystemColors.WindowText
                lblLatestVersionLabel.Visible = False
                lblInstalledVersion.Visible = False
                lblInstalledVersionLabel.Visible = False
                lblLatestVersion.Visible = False
                btnCheckForUpdate.Visible = False
                pnlUpdate.Visible = False

                Return

                AddHandler _appUpdate.GetUpdateInfoCompletedEvent, AddressOf GetUpdateInfoCompleted

                _appUpdate.GetUpdateInfoAsync()
            End Sub

            Private Sub GetUpdateInfoCompleted(sender As Object, e As AsyncCompletedEventArgs)
                If InvokeRequired Then
                    Dim myDelegate As New AsyncCompletedEventHandler(AddressOf GetUpdateInfoCompleted)
                    Invoke(myDelegate, New Object() {sender, e})
                    Return
                End If

                Try
                    RemoveHandler _appUpdate.GetUpdateInfoCompletedEvent, AddressOf GetUpdateInfoCompleted

                    lblInstalledVersion.Text = My.Application.Info.Version.ToString
                    lblInstalledVersion.Visible = True
                    lblInstalledVersionLabel.Visible = True
                    btnCheckForUpdate.Visible = True

                    If e.Cancelled Then Return
                    If e.Error IsNot Nothing Then Throw e.Error

                    If _appUpdate.IsUpdateAvailable() Then
                        lblStatus.Text = Language.Language.strUpdateAvailable
                        lblStatus.ForeColor = Color.OrangeRed
                        pnlUpdate.Visible = True

                        Dim updateInfo As App.Update.UpdateInfo = _appUpdate.CurrentUpdateInfo
                        lblLatestVersion.Text = updateInfo.Version.ToString
                        lblLatestVersionLabel.Visible = True
                        lblLatestVersion.Visible = True

                        If _
                            updateInfo.ImageAddress Is Nothing OrElse
                            String.IsNullOrEmpty(updateInfo.ImageAddress.ToString()) Then
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
                        lblStatus.Text = Language.Language.strNoUpdateAvailable
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
                    lblStatus.Text = Language.Language.strUpdateCheckFailedLabel
                    lblStatus.ForeColor = Color.OrangeRed

                    Runtime.MessageCollector.AddExceptionMessage(Language.Language.strUpdateCheckCompleteFailed, ex)
                End Try
            End Sub

            Private Sub GetChangeLogCompleted(sender As Object, e As AsyncCompletedEventArgs)
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
                    Runtime.MessageCollector.AddExceptionMessage(Language.Language.strUpdateGetChangeLogFailed, ex)
                End Try
            End Sub

            Private Sub DownloadUpdate()
                Try
                    btnDownload.Enabled = False
                    prgbDownload.Visible = True
                    prgbDownload.Value = 0

                    If _isUpdateDownloadHandlerDeclared = False Then
                        AddHandler _appUpdate.DownloadUpdateProgressChangedEvent,
                            AddressOf DownloadUpdateProgressChanged
                        AddHandler _appUpdate.DownloadUpdateCompletedEvent, AddressOf DownloadUpdateCompleted
                        _isUpdateDownloadHandlerDeclared = True
                    End If

                    _appUpdate.DownloadUpdateAsync()
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage(Language.Language.strUpdateDownloadFailed, ex)
                End Try
            End Sub

#End Region

#Region "Events"

            Private Sub DownloadUpdateProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)
                prgbDownload.Value = e.ProgressPercentage
            End Sub

            Private Sub DownloadUpdateCompleted(sender As Object, e As AsyncCompletedEventArgs)
                Try
                    btnDownload.Enabled = True
                    prgbDownload.Visible = False

                    If e.Cancelled Then Return
                    If e.Error IsNot Nothing Then Throw e.Error

                    If _
                        MessageBox.Show(Language.Language.strUpdateDownloadComplete,
                                        Language.Language.strMenuCheckForUpdates, MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Warning) = DialogResult.OK Then
                        Runtime.Shutdown.Quit(_appUpdate.CurrentUpdateInfo.UpdateFilePath)
                        Return
                    Else
                        File.Delete(_appUpdate.CurrentUpdateInfo.UpdateFilePath)
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage(Language.Language.strUpdateDownloadCompleteFailed, ex)
                End Try
            End Sub

#End Region
        End Class
    End Namespace

End Namespace