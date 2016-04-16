Imports System.IO
Imports mRemote3G.App.Info
Imports mRemote3G.Config.Putty
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.My
Imports mRemote3G.Tools
Imports PSTaskDialog

Namespace Forms.OptionsPages
    Public Class AdvancedPage

#Region "Public Methods"

        Public Overrides Property PageName As String
            Get
                Return Language.Language.strTabAdvanced
            End Get
            Set
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            MyBase.ApplyLanguage()

            lblSeconds.Text = Language.Language.strLabelSeconds
            lblMaximumPuttyWaitTime.Text = Language.Language.strLabelPuttyTimeout
            chkAutomaticReconnect.Text = Language.Language.strCheckboxAutomaticReconnect
            lblConfigurePuttySessions.Text = Language.Language.strLabelPuttySessionsConfig
            btnLaunchPutty.Text = Language.Language.strButtonLaunchPutty
            btnBrowseCustomPuttyPath.Text = Language.Language.strButtonBrowse
            chkUseCustomPuttyPath.Text = Language.Language.strCheckboxPuttyPath
            chkAutomaticallyGetSessionInfo.Text = Language.Language.strAutomaticallyGetSessionInfo
            chkWriteLogFile.Text = Language.Language.strWriteLogFile
            lblUVNCSCPort.Text = Language.Language.strUltraVNCSCListeningPort
            chkEncryptCompleteFile.Text = Language.Language.strEncryptCompleteConnectionFile
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
        End Sub

        Public Overrides Sub SaveSettings()
            MyBase.SaveSettings()

            My.Settings.WriteLogFile = chkWriteLogFile.Checked
            My.Settings.EncryptCompleteConnectionsFile = chkEncryptCompleteFile.Checked
            My.Settings.AutomaticallyGetSessionInfo = chkAutomaticallyGetSessionInfo.Checked
            My.Settings.ReconnectOnDisconnect = chkAutomaticReconnect.Checked

            Dim puttyPathChanged = False
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
                Sessions.AddSessionsToTree()
            End If

            My.Settings.MaxPuttyWaitTime = numPuttyWaitTime.Value

            My.Settings.UVNCSCPort = numUVNCSCPort.Value
        End Sub

#End Region

#Region "Private Methods"

#Region "Event Handlers"

        Private Sub chkUseCustomPuttyPath_CheckedChanged(sender As Object, e As EventArgs) _
            Handles chkUseCustomPuttyPath.CheckedChanged
            txtCustomPuttyPath.Enabled = chkUseCustomPuttyPath.Checked
            btnBrowseCustomPuttyPath.Enabled = chkUseCustomPuttyPath.Checked
            SetPuttyLaunchButtonEnabled()
        End Sub

        Private Sub txtCustomPuttyPath_TextChanged(sender As Object, e As EventArgs) _
            Handles txtCustomPuttyPath.TextChanged
            SetPuttyLaunchButtonEnabled()
        End Sub

        Private Sub btnBrowseCustomPuttyPath_Click(sender As Object, e As EventArgs) _
            Handles btnBrowseCustomPuttyPath.Click
            Using openFileDialog As New OpenFileDialog()
                With openFileDialog
                    .Filter = String.Format("{0}|*.exe|{1}|*.*", Language.Language.strFilterApplication,
                                            Language.Language.strFilterAll)
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

        Private Sub btnLaunchPutty_Click(sender As Object, e As EventArgs) Handles btnLaunchPutty.Click
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
                cTaskDialog.MessageBox(Application.Info.ProductName, Language.Language.strErrorCouldNotLaunchPutty, "",
                                       ex.ToString(), "", "", eTaskDialogButtons.OK, eSysIcons.Error, Nothing)
            End Try
        End Sub

#End Region

        Private Sub SetPuttyLaunchButtonEnabled()
            Dim puttyPath As String
            If chkUseCustomPuttyPath.Checked Then
                puttyPath = txtCustomPuttyPath.Text
            Else
                puttyPath = General.PuttyPath
            End If

            Dim exists = False
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