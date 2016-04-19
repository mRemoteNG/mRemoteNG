Imports mRemote3G.App
Imports mRemote3G.Config.Settings
Imports mRemote3G.Forms
Imports mRemote3G.Tools
Imports WeifenLuo.WinFormsUI.Docking

Namespace UI

    Namespace Window
        Public Class ExternalTools
            Inherits Base

#Region "Constructors"

            Public Sub New(panel As DockContent)
                InitializeComponent()

                WindowType = Type.ExternalApps
                DockPnl = panel
            End Sub

#End Region

#Region "Private Fields"

            Private _selectedTool As ExternalTool = Nothing

#End Region

#Region "Private Methods"

#Region "Event Handlers"

            Private Sub ExternalTools_Load(sender As Object, e As EventArgs) Handles Me.Load
                ApplyLanguage()
                UpdateToolsListView()
            End Sub

            Private Shared Sub ExternalTools_FormClosed(sender As Object, e As FormClosedEventArgs) _
                Handles MyBase.FormClosed
                Save.SaveExternalAppsToXML()
            End Sub

            Private Sub NewTool_Click(sender As Object, e As EventArgs) _
                Handles NewToolMenuItem.Click, NewToolToolstripButton.Click
                Try
                    Dim externalTool As New ExternalTool(Language.Language.strExternalToolDefaultName)
                    Runtime.ExternalTools.Add(externalTool)
                    UpdateToolsListView(externalTool)
                    DisplayNameTextBox.Focus()
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.NewTool_Click() failed.", ex, ,
                                                                 True)
                End Try
            End Sub

            Private Sub DeleteTool_Click(sender As Object, e As EventArgs) _
                Handles DeleteToolMenuItem.Click, DeleteToolToolstripButton.Click
                Try
                    Dim message As String
                    Select Case ToolsListView.SelectedItems.Count
                        Case Is = 1
                            message = String.Format(Language.Language.strConfirmDeleteExternalTool,
                                                    ToolsListView.SelectedItems(0).Text)
                        Case Is > 1
                            message = String.Format(Language.Language.strConfirmDeleteExternalToolMultiple,
                                                    ToolsListView.SelectedItems.Count)
                        Case Else
                            Return
                    End Select

                    If Not MsgBox(message, MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then Return

                    For Each listViewItem As ListViewItem In ToolsListView.SelectedItems
                        Dim externalTool = TryCast(listViewItem.Tag, ExternalTool)
                        If externalTool Is Nothing Then Continue For

                        Runtime.ExternalTools.Remove(listViewItem.Tag)
                        listViewItem.Remove()
                    Next
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.DeleteTool_Click() failed.",
                                                                 ex, , True)
                End Try
            End Sub

            Private Sub LaunchTool_Click(sender As Object, e As EventArgs) _
                Handles LaunchToolMenuItem.Click, LaunchToolToolstripButton.Click
                LaunchTool()
            End Sub

            Private Sub ToolsListView_SelectedIndexChanged(sender As Object, e As EventArgs) _
                Handles ToolsListView.SelectedIndexChanged
                Try
                    If ToolsListView.SelectedItems.Count = 1 Then
                        PropertiesGroupBox.Enabled = True
                        _selectedTool = TryCast(ToolsListView.SelectedItems(0).Tag, ExternalTool)
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
                    Runtime.MessageCollector.AddExceptionMessage(
                        "UI.Window.ExternalTools.ToolsListView_SelectedIndexChanged() failed.", ex, , True)
                End Try
            End Sub

            Private Sub ToolsListView_DoubleClick(sender As Object, e As EventArgs) Handles ToolsListView.DoubleClick
                If ToolsListView.SelectedItems.Count > 0 Then LaunchTool()
            End Sub

            Private Sub PropertyControl_ChangedOrLostFocus(sender As Object, e As EventArgs) _
                Handles DisplayNameTextBox.LostFocus, ArgumentsCheckBox.LostFocus, FilenameTextBox.LostFocus,
                        BrowseButton.LostFocus, WaitForExitCheckBox.LostFocus, WaitForExitCheckBox.Click,
                        TryToIntegrateCheckBox.Click
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
                    Runtime.MessageCollector.AddExceptionMessage(
                        "UI.Window.ExternalTools.PropertyControl_ChangedOrLostFocus() failed.", ex, , True)
                End Try
            End Sub

            Private Sub BrowseButton_Click(sender As Object, e As EventArgs) Handles BrowseButton.Click
                Try
                    Using browseDialog As New OpenFileDialog()
                        With browseDialog
                            .Filter = String.Join("|",
                                                  New String() _
                                                     {Language.Language.strFilterApplication, "*.exe",
                                                      Language.Language.strFilterAll, "*.*"})
                            If .ShowDialog = DialogResult.OK Then FilenameTextBox.Text = .FileName
                        End With
                    End Using
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.BrowseButton_Click() failed.",
                                                                 ex, , True)
                End Try
            End Sub

            Private Sub TryToIntegrateCheckBox_CheckedChanged(sender As Object, e As EventArgs) _
                Handles TryToIntegrateCheckBox.CheckedChanged
                If TryToIntegrateCheckBox.Checked Then
                    WaitForExitCheckBox.Enabled = False
                    WaitForExitCheckBox.Checked = False
                Else
                    WaitForExitCheckBox.Enabled = True
                End If
            End Sub

#End Region

            Private Sub ApplyLanguage()
                Text = Language.Language.strMenuExternalTools
                TabText = Language.Language.strMenuExternalTools

                NewToolToolstripButton.Text = Language.Language.strButtonNew
                DeleteToolToolstripButton.Text = Language.Language.strOptionsKeyboardButtonDelete
                LaunchToolToolstripButton.Text = Language.Language.strButtonLaunch

                DisplayNameColumnHeader.Text = Language.Language.strColumnDisplayName
                FilenameColumnHeader.Text = Language.Language.strColumnFilename
                ArgumentsColumnHeader.Text = Language.Language.strColumnArguments
                WaitForExitColumnHeader.Text = Language.Language.strColumnWaitForExit
                TryToIntegrateCheckBox.Text = Language.Language.strTryIntegrate

                PropertiesGroupBox.Text = Language.Language.strGroupboxExternalToolProperties

                DisplayNameLabel.Text = Language.Language.strLabelDisplayName
                FilenameLabel.Text = Language.Language.strLabelFilename
                ArgumentsLabel.Text = Language.Language.strLabelArguments
                OptionsLabel.Text = Language.Language.strLabelOptions
                WaitForExitCheckBox.Text = Language.Language.strCheckboxWaitForExit
                BrowseButton.Text = Language.Language.strButtonBrowse

                NewToolMenuItem.Text = Language.Language.strMenuNewExternalTool
                DeleteToolMenuItem.Text = Language.Language.strMenuDeleteExternalTool
                LaunchToolMenuItem.Text = Language.Language.strMenuLaunchExternalTool
            End Sub

            Private Sub UpdateToolsListView(Optional ByVal selectTool As ExternalTool = Nothing)
                Try
                    Dim selectedTools As New List(Of ExternalTool)
                    If selectTool Is Nothing Then
                        For Each listViewItem As ListViewItem In ToolsListView.SelectedItems
                            Dim externalTool = TryCast(listViewItem.Tag, ExternalTool)
                            If externalTool IsNot Nothing Then selectedTools.Add(externalTool)
                        Next
                    Else
                        selectedTools.Add(selectTool)
                    End If

                    ToolsListView.BeginUpdate()
                    ToolsListView.Items.Clear()

                    For Each externalTool As ExternalTool In Runtime.ExternalTools
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
                    Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.PopulateToolsListView()", ex, ,
                                                                 True)
                End Try
            End Sub

            Private Sub LaunchTool()
                Try
                    For Each listViewItem As ListViewItem In ToolsListView.SelectedItems
                        Dim externalTool = TryCast(listViewItem.Tag, ExternalTool)
                        If externalTool Is Nothing Then Continue For

                        externalTool.Start()
                    Next
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.LaunchTool() failed.", ex, ,
                                                                 True)
                End Try
            End Sub

#End Region
        End Class
    End Namespace

End Namespace