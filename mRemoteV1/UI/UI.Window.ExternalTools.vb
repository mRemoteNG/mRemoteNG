Imports mRemoteNG.App
Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime
Imports mRemoteNG.My

Namespace UI
    Namespace Window
        Public Class ExternalTools
            Inherits Base
#Region "Constructors"
            Public Sub New(ByVal panel As DockContent)
                InitializeComponent()

                WindowType = Type.ExternalApps
                DockPnl = panel
            End Sub
#End Region

#Region "Private Fields"
            Private _selectedTool As Tools.ExternalTool = Nothing
#End Region

#Region "Private Methods"
#Region "Event Handlers"
            Private Sub ExternalTools_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
                ApplyLanguage()
                UpdateToolsListView()
            End Sub

            Private Shared Sub ExternalTools_FormClosed(sender As System.Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
                mRemoteNG.Config.Settings.Save.SaveExternalAppsToXML()
            End Sub

            Private Sub NewTool_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles NewToolMenuItem.Click, NewToolToolstripButton.Click
                Try
                    Dim externalTool As New Tools.ExternalTool(Language.strExternalToolDefaultName)
                    Runtime.ExternalTools.Add(externalTool)
                    UpdateToolsListView(externalTool)
                    DisplayNameTextBox.Focus()
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.NewTool_Click() failed.", ex, , True)
                End Try
            End Sub

            Private Sub DeleteTool_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles DeleteToolMenuItem.Click, DeleteToolToolstripButton.Click
                Try
                    Dim message As String
                    Select Case ToolsListView.SelectedItems.Count
                        Case Is = 1
                            message = String.Format(Language.strConfirmDeleteExternalTool, ToolsListView.SelectedItems(0).Text)
                        Case Is > 1
                            message = String.Format(Language.strConfirmDeleteExternalToolMultiple, ToolsListView.SelectedItems.Count)
                        Case Else
                            Return
                    End Select

                    If Not MsgBox(message, MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then Return

                    For Each listViewItem As ListViewItem In ToolsListView.SelectedItems
                        Dim externalTool As Tools.ExternalTool = TryCast(listViewItem.Tag, Tools.ExternalTool)
                        If externalTool Is Nothing Then Continue For

                        Runtime.ExternalTools.Remove(listViewItem.Tag)
                        listViewItem.Remove()
                    Next
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.DeleteTool_Click() failed.", ex, , True)
                End Try
            End Sub

            Private Sub LaunchTool_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles LaunchToolMenuItem.Click, LaunchToolToolstripButton.Click
                LaunchTool()
            End Sub

            Private Sub ToolsListView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles ToolsListView.SelectedIndexChanged
                Try
                    If ToolsListView.SelectedItems.Count = 1 Then
                        PropertiesGroupBox.Enabled = True
                        _selectedTool = TryCast(ToolsListView.SelectedItems(0).Tag, Tools.ExternalTool)
                        If _selectedTool Is Nothing Then Return

                        With _selectedTool
                            DisplayNameTextBox.Text = .DisplayName
                            FilenameTextBox.Text = .FileName
                            ArgumentsCheckBox.Text = .Arguments
                            WaitForExitCheckBox.Checked = .WaitForExit
                            TryToIntegrateCheckBox.Checked = .TryIntegrate
                        End With
                    Else
                        PropertiesGroupBox.Enabled = False
                    End If
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.ToolsListView_SelectedIndexChanged() failed.", ex, , True)
                End Try
            End Sub

            Private Sub ToolsListView_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles ToolsListView.DoubleClick
                If ToolsListView.SelectedItems.Count > 0 Then LaunchTool()
            End Sub

            Private Sub PropertyControl_ChangedOrLostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles DisplayNameTextBox.LostFocus, ArgumentsCheckBox.LostFocus, FilenameTextBox.LostFocus, BrowseButton.LostFocus, WaitForExitCheckBox.LostFocus, WaitForExitCheckBox.Click, TryToIntegrateCheckBox.Click
                If _selectedTool Is Nothing Then Return

                Try
                    With _selectedTool
                        .DisplayName = DisplayNameTextBox.Text
                        .FileName = FilenameTextBox.Text
                        .Arguments = ArgumentsCheckBox.Text
                        .WaitForExit = WaitForExitCheckBox.Checked
                        .TryIntegrate = TryToIntegrateCheckBox.Checked
                    End With

                    UpdateToolsListView()
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.PropertyControl_ChangedOrLostFocus() failed.", ex, , True)
                End Try
            End Sub

            Private Sub BrowseButton_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles BrowseButton.Click
                Try
                    Using browseDialog As New OpenFileDialog()
                        With browseDialog
                            .Filter = String.Join("|", New String() {Language.strFilterApplication, "*.exe", Language.strFilterAll, "*.*"})
                            If .ShowDialog = DialogResult.OK Then FilenameTextBox.Text = .FileName
                        End With
                    End Using
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.BrowseButton_Click() failed.", ex, , True)
                End Try
            End Sub

            Private Sub TryToIntegrateCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles TryToIntegrateCheckBox.CheckedChanged
                If TryToIntegrateCheckBox.Checked Then
                    WaitForExitCheckBox.Enabled = False
                    WaitForExitCheckBox.Checked = False
                Else
                    WaitForExitCheckBox.Enabled = True
                End If
            End Sub
#End Region

            Private Sub ApplyLanguage()
                Text = Language.strMenuExternalTools
                TabText = Language.strMenuExternalTools

                NewToolToolstripButton.Text = Language.strButtonNew
                DeleteToolToolstripButton.Text = Language.strOptionsKeyboardButtonDelete
                LaunchToolToolstripButton.Text = Language.strButtonLaunch

                DisplayNameColumnHeader.Text = Language.strColumnDisplayName
                FilenameColumnHeader.Text = Language.strColumnFilename
                ArgumentsColumnHeader.Text = Language.strColumnArguments
                WaitForExitColumnHeader.Text = Language.strColumnWaitForExit
                TryToIntegrateCheckBox.Text = Language.strTryIntegrate

                PropertiesGroupBox.Text = Language.strGroupboxExternalToolProperties

                DisplayNameLabel.Text = Language.strLabelDisplayName
                FilenameLabel.Text = Language.strLabelFilename
                ArgumentsLabel.Text = Language.strLabelArguments
                OptionsLabel.Text = Language.strLabelOptions
                WaitForExitCheckBox.Text = Language.strCheckboxWaitForExit
                BrowseButton.Text = Language.strButtonBrowse

                NewToolMenuItem.Text = Language.strMenuNewExternalTool
                DeleteToolMenuItem.Text = Language.strMenuDeleteExternalTool
                LaunchToolMenuItem.Text = Language.strMenuLaunchExternalTool
            End Sub

            Private Sub UpdateToolsListView(Optional ByVal selectTool As Tools.ExternalTool = Nothing)
                Try
                    Dim selectedTools As New List(Of Tools.ExternalTool)
                    If selectTool Is Nothing Then
                        For Each listViewItem As ListViewItem In ToolsListView.SelectedItems
                            Dim externalTool As Tools.ExternalTool = TryCast(listViewItem.Tag, Tools.ExternalTool)
                            If externalTool IsNot Nothing Then selectedTools.Add(externalTool)
                        Next
                    Else
                        selectedTools.Add(selectTool)
                    End If

                    ToolsListView.BeginUpdate()
                    ToolsListView.Items.Clear()

                    For Each externalTool As Tools.ExternalTool In Runtime.ExternalTools
                        Dim listViewItem As New ListViewItem
                        With listViewItem
                            .Text = externalTool.DisplayName
                            .SubItems.Add(externalTool.FileName)
                            .SubItems.Add(externalTool.Arguments)
                            .SubItems.Add(externalTool.WaitForExit)
                            .SubItems.Add(externalTool.TryIntegrate)
                            .Tag = externalTool
                        End With

                        ToolsListView.Items.Add(listViewItem)

                        If selectedTools.Contains(externalTool) Then listViewItem.Selected = True
                    Next

                    ToolsListView.EndUpdate()

                    frmMain.AddExternalToolsToToolBar()
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.PopulateToolsListView()", ex, , True)
                End Try
            End Sub

            Private Sub LaunchTool()
                Try
                    For Each listViewItem As ListViewItem In ToolsListView.SelectedItems
                        Dim externalTool As Tools.ExternalTool = TryCast(listViewItem.Tag, Tools.ExternalTool)
                        If externalTool Is Nothing Then Continue For

                        externalTool.Start()
                    Next
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.LaunchTool() failed.", ex, , True)
                End Try
            End Sub
#End Region
        End Class
    End Namespace
End Namespace