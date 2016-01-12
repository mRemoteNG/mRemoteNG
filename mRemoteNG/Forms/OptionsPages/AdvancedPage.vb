Imports System.IO
Imports mRemoteNG.App.Info
Imports mRemoteNG.My
Imports mRemoteNG.Connection.Protocol
Imports mRemoteNG.Tools
Imports PSTaskDialog

Namespace Forms.OptionsPages
    Public Class AdvancedPage
#Region "Public Methods"
        Public Overrides Property PageName() As String
            Get
                Return Language.strTabAdvanced
            End Get
            Set(value As String)
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            MyBase.ApplyLanguage()

            lblSeconds.Text = Language.strLabelSeconds
            lblMaximumPuttyWaitTime.Text = Language.strLabelPuttyTimeout
            chkAutomaticReconnect.Text = Language.strCheckboxAutomaticReconnect
            lblConfigurePuttySessions.Text = Language.strLabelPuttySessionsConfig
            btnLaunchPutty.Text = Language.strButtonLaunchPutty
            btnBrowseCustomPuttyPath.Text = Language.strButtonBrowse
            chkUseCustomPuttyPath.Text = Language.strCheckboxPuttyPath
            chkAutomaticallyGetSessionInfo.Text = Language.strAutomaticallyGetSessionInfo
            chkWriteLogFile.Text = Language.strWriteLogFile
            lblUVNCSCPort.Text = Language.strUltraVNCSCListeningPort
            lblXulRunnerPath.Text = Language.strXULrunnerPath
            btnBrowseXulRunnerPath.Text = Language.strButtonBrowse
            chkEncryptCompleteFile.Text = Language.strEncryptCompleteConnectionFile
        End Sub

        Public Overrides Sub LoadSettings()
            MyBase.SaveSettings()

            chkWriteLogFile.Checked = My.Settings.WriteLogFile
            chkEncryptCompleteFile.Checked = My.Settings.EncryptCompleteConnectionsFile
            chkAutomaticallyGetSessionInfo.Checked = My.Settings.AutomaticallyGetSessionInfo
            chkAutomaticReconnect.Checked = My.Settings.ReconnectOnDisconnect
            numPuttyWaitTime.Value = My.Settings.MaxPuttyWaitTime

            chkUseCustomPuttyPath.Checked = MySettingsProperty.Settings.UseCustomPuttyPath
            txtCustomPuttyPath.Text = MySettingsProperty.Settings.CustomPuttyPath
            SetPuttyLaunchButtonEnabled()

            numUVNCSCPort.Value = My.Settings.UVNCSCPort

            txtXULrunnerPath.Text = My.Settings.XULRunnerPath
        End Sub

        Public Overrides Sub SaveSettings()
            MyBase.SaveSettings()

            My.Settings.WriteLogFile = chkWriteLogFile.Checked
            My.Settings.EncryptCompleteConnectionsFile = chkEncryptCompleteFile.Checked
            My.Settings.AutomaticallyGetSessionInfo = chkAutomaticallyGetSessionInfo.Checked
            My.Settings.ReconnectOnDisconnect = chkAutomaticReconnect.Checked

            Dim puttyPathChanged As Boolean = False
            If Not MySettingsProperty.Settings.CustomPuttyPath = txtCustomPuttyPath.Text Then
                puttyPathChanged = True
                MySettingsProperty.Settings.CustomPuttyPath = txtCustomPuttyPath.Text
            End If
            If Not MySettingsProperty.Settings.UseCustomPuttyPath = chkUseCustomPuttyPath.Checked Then
                puttyPathChanged = True
                MySettingsProperty.Settings.UseCustomPuttyPath = chkUseCustomPuttyPath.Checked
            End If
            If puttyPathChanged Then
                If MySettingsProperty.Settings.UseCustomPuttyPath Then
                    PuttyBase.PuttyPath = MySettingsProperty.Settings.CustomPuttyPath
                Else
                    PuttyBase.PuttyPath = General.PuttyPath
                End If
                Config.Putty.Sessions.AddSessionsToTree()
            End If

            My.Settings.MaxPuttyWaitTime = numPuttyWaitTime.Value

            My.Settings.UVNCSCPort = numUVNCSCPort.Value

            My.Settings.XULRunnerPath = txtXULrunnerPath.Text
        End Sub
#End Region

#Region "Private Methods"
#Region "Event Handlers"
        Private Sub chkUseCustomPuttyPath_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkUseCustomPuttyPath.CheckedChanged
            txtCustomPuttyPath.Enabled = chkUseCustomPuttyPath.Checked
            btnBrowseCustomPuttyPath.Enabled = chkUseCustomPuttyPath.Checked
            SetPuttyLaunchButtonEnabled()
        End Sub

        Private Sub txtCustomPuttyPath_TextChanged(sender As Object, e As EventArgs) Handles txtCustomPuttyPath.TextChanged
            SetPuttyLaunchButtonEnabled()
        End Sub

        Private Sub btnBrowseCustomPuttyPath_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowseCustomPuttyPath.Click
            Using openFileDialog As New OpenFileDialog()
                With openFileDialog
                    .Filter = String.Format("{0}|*.exe|{1}|*.*", Language.strFilterApplication, Language.strFilterAll)
                    .FileName = Path.GetFileName(General.PuttyPath)
                    .CheckFileExists = True
                    .Multiselect = False

                    If .ShowDialog = DialogResult.OK Then
                        txtCustomPuttyPath.Text = .FileName
                        SetPuttyLaunchButtonEnabled()
                    End If
                End With
            End Using
        End Sub

        Private Sub btnLaunchPutty_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLaunchPutty.Click
            Try
                Dim puttyProcess As New PuttyProcessController
                Dim fileName As String
                If chkUseCustomPuttyPath.Checked Then
                    fileName = txtCustomPuttyPath.Text
                Else
                    fileName = General.PuttyPath
                End If
                puttyProcess.Start(fileName)
                puttyProcess.SetControlText("Button", "&Cancel", "&Close")
                puttyProcess.SetControlVisible("Button", "&Open", False)
                puttyProcess.WaitForExit()
            Catch ex As Exception
                cTaskDialog.MessageBox(Application.Info.ProductName, Language.strErrorCouldNotLaunchPutty, "", ex.Message, "", "", eTaskDialogButtons.OK, eSysIcons.Error, Nothing)
            End Try
        End Sub

        Private Sub btnBrowseXulRunnerPath_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowseXulRunnerPath.Click
            Dim oDlg As New FolderBrowserDialog
            oDlg.ShowNewFolderButton = False

            If oDlg.ShowDialog = DialogResult.OK Then
                txtXULrunnerPath.Text = oDlg.SelectedPath
            End If

            oDlg.Dispose()
        End Sub
#End Region

        Private Sub SetPuttyLaunchButtonEnabled()
            Dim puttyPath As String
            If chkUseCustomPuttyPath.Checked Then
                puttyPath = txtCustomPuttyPath.Text
            Else
                puttyPath = General.PuttyPath
            End If

            Dim exists As Boolean = False
            Try
                exists = File.Exists(puttyPath)
            Catch
            End Try

            If exists Then
                lblConfigurePuttySessions.Enabled = True
                btnLaunchPutty.Enabled = True
            Else
                lblConfigurePuttySessions.Enabled = False
                btnLaunchPutty.Enabled = False
            End If
        End Sub
#End Region
    End Class
End Namespace