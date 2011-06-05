Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class ExternalApps
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents clmFilename As System.Windows.Forms.ColumnHeader
            Friend WithEvents clmArguments As System.Windows.Forms.ColumnHeader
            Friend WithEvents grpEditor As System.Windows.Forms.GroupBox
            Friend WithEvents txtDisplayName As System.Windows.Forms.TextBox
            Friend WithEvents Label1 As System.Windows.Forms.Label
            Friend WithEvents txtArguments As System.Windows.Forms.TextBox
            Friend WithEvents txtFilename As System.Windows.Forms.TextBox
            Friend WithEvents Label3 As System.Windows.Forms.Label
            Friend WithEvents Label2 As System.Windows.Forms.Label
            Friend WithEvents btnBrowse As System.Windows.Forms.Button
            Friend WithEvents clmDisplayName As System.Windows.Forms.ColumnHeader
            Friend WithEvents cMenApps As System.Windows.Forms.ContextMenuStrip
            Private components As System.ComponentModel.IContainer
            Friend WithEvents cMenAppsAdd As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents dlgOpenFile As System.Windows.Forms.OpenFileDialog
            Friend WithEvents cMenAppsRemove As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenAppsSep1 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents cMenAppsStart As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents clmWaitForExit As System.Windows.Forms.ColumnHeader
            Friend WithEvents chkWaitForExit As System.Windows.Forms.CheckBox
            Friend WithEvents Label4 As System.Windows.Forms.Label
            Friend WithEvents chkTryIntegrate As System.Windows.Forms.CheckBox
            Friend WithEvents clmTryIntegrate As System.Windows.Forms.ColumnHeader
            Friend WithEvents lvApps As System.Windows.Forms.ListView

            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container
                Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ExternalApps))
                Me.lvApps = New System.Windows.Forms.ListView
                Me.clmDisplayName = New System.Windows.Forms.ColumnHeader
                Me.clmFilename = New System.Windows.Forms.ColumnHeader
                Me.clmArguments = New System.Windows.Forms.ColumnHeader
                Me.clmWaitForExit = New System.Windows.Forms.ColumnHeader
                Me.clmTryIntegrate = New System.Windows.Forms.ColumnHeader
                Me.cMenApps = New System.Windows.Forms.ContextMenuStrip(Me.components)
                Me.cMenAppsAdd = New System.Windows.Forms.ToolStripMenuItem
                Me.cMenAppsRemove = New System.Windows.Forms.ToolStripMenuItem
                Me.cMenAppsSep1 = New System.Windows.Forms.ToolStripSeparator
                Me.cMenAppsStart = New System.Windows.Forms.ToolStripMenuItem
                Me.grpEditor = New System.Windows.Forms.GroupBox
                Me.chkTryIntegrate = New System.Windows.Forms.CheckBox
                Me.Label4 = New System.Windows.Forms.Label
                Me.chkWaitForExit = New System.Windows.Forms.CheckBox
                Me.btnBrowse = New System.Windows.Forms.Button
                Me.txtArguments = New System.Windows.Forms.TextBox
                Me.txtFilename = New System.Windows.Forms.TextBox
                Me.txtDisplayName = New System.Windows.Forms.TextBox
                Me.Label3 = New System.Windows.Forms.Label
                Me.Label2 = New System.Windows.Forms.Label
                Me.Label1 = New System.Windows.Forms.Label
                Me.dlgOpenFile = New System.Windows.Forms.OpenFileDialog
                Me.cMenApps.SuspendLayout()
                Me.grpEditor.SuspendLayout()
                Me.SuspendLayout()
                '
                'lvApps
                '
                Me.lvApps.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lvApps.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.lvApps.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmDisplayName, Me.clmFilename, Me.clmArguments, Me.clmWaitForExit, Me.clmTryIntegrate})
                Me.lvApps.ContextMenuStrip = Me.cMenApps
                Me.lvApps.FullRowSelect = True
                Me.lvApps.GridLines = True
                Me.lvApps.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
                Me.lvApps.HideSelection = False
                Me.lvApps.Location = New System.Drawing.Point(0, 1)
                Me.lvApps.Name = "lvApps"
                Me.lvApps.Size = New System.Drawing.Size(684, 193)
                Me.lvApps.Sorting = System.Windows.Forms.SortOrder.Ascending
                Me.lvApps.TabIndex = 0
                Me.lvApps.UseCompatibleStateImageBehavior = False
                Me.lvApps.View = System.Windows.Forms.View.Details
                '
                'clmDisplayName
                '
                Me.clmDisplayName.Text = "Display Name"
                Me.clmDisplayName.Width = 130
                '
                'clmFilename
                '
                Me.clmFilename.Text = "Filename"
                Me.clmFilename.Width = 200
                '
                'clmArguments
                '
                Me.clmArguments.Text = "Arguments"
                Me.clmArguments.Width = 160
                '
                'clmWaitForExit
                '
                Me.clmWaitForExit.Text = "Wait for exit"
                Me.clmWaitForExit.Width = 75
                '
                'clmTryIntegrate
                '
                Me.clmTryIntegrate.Text = "Try To Integrate"
                Me.clmTryIntegrate.Width = 95
                '
                'cMenApps
                '
                Me.cMenApps.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cMenAppsAdd, Me.cMenAppsRemove, Me.cMenAppsSep1, Me.cMenAppsStart})
                Me.cMenApps.Name = "cMenApps"
                Me.cMenApps.Size = New System.Drawing.Size(148, 76)
                '
                'cMenAppsAdd
                '
                Me.cMenAppsAdd.Image = Global.mRemoteNG.My.Resources.Resources.ExtApp_Add
                Me.cMenAppsAdd.Name = "cMenAppsAdd"
                Me.cMenAppsAdd.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
                Me.cMenAppsAdd.Size = New System.Drawing.Size(147, 22)
                Me.cMenAppsAdd.Text = "Add"
                '
                'cMenAppsRemove
                '
                Me.cMenAppsRemove.Image = Global.mRemoteNG.My.Resources.Resources.ExtApp_Delete
                Me.cMenAppsRemove.Name = "cMenAppsRemove"
                Me.cMenAppsRemove.ShortcutKeys = System.Windows.Forms.Keys.Delete
                Me.cMenAppsRemove.Size = New System.Drawing.Size(147, 22)
                Me.cMenAppsRemove.Text = "Remove"
                '
                'cMenAppsSep1
                '
                Me.cMenAppsSep1.Name = "cMenAppsSep1"
                Me.cMenAppsSep1.Size = New System.Drawing.Size(144, 6)
                '
                'cMenAppsStart
                '
                Me.cMenAppsStart.Image = Global.mRemoteNG.My.Resources.Resources.ExtApp_Start
                Me.cMenAppsStart.Name = "cMenAppsStart"
                Me.cMenAppsStart.Size = New System.Drawing.Size(147, 22)
                Me.cMenAppsStart.Text = "Start"
                '
                'grpEditor
                '
                Me.grpEditor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.grpEditor.Controls.Add(Me.chkTryIntegrate)
                Me.grpEditor.Controls.Add(Me.Label4)
                Me.grpEditor.Controls.Add(Me.chkWaitForExit)
                Me.grpEditor.Controls.Add(Me.btnBrowse)
                Me.grpEditor.Controls.Add(Me.txtArguments)
                Me.grpEditor.Controls.Add(Me.txtFilename)
                Me.grpEditor.Controls.Add(Me.txtDisplayName)
                Me.grpEditor.Controls.Add(Me.Label3)
                Me.grpEditor.Controls.Add(Me.Label2)
                Me.grpEditor.Controls.Add(Me.Label1)
                Me.grpEditor.Enabled = False
                Me.grpEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.grpEditor.Location = New System.Drawing.Point(1, 200)
                Me.grpEditor.Name = "grpEditor"
                Me.grpEditor.Size = New System.Drawing.Size(683, 123)
                Me.grpEditor.TabIndex = 10
                Me.grpEditor.TabStop = False
                Me.grpEditor.Text = "Application Editor"
                '
                'chkTryIntegrate
                '
                Me.chkTryIntegrate.AutoSize = True
                Me.chkTryIntegrate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkTryIntegrate.Location = New System.Drawing.Point(243, 97)
                Me.chkTryIntegrate.Name = "chkTryIntegrate"
                Me.chkTryIntegrate.Size = New System.Drawing.Size(94, 17)
                Me.chkTryIntegrate.TabIndex = 71
                Me.chkTryIntegrate.Text = "Try to integrate"
                Me.chkTryIntegrate.UseVisualStyleBackColor = True
                '
                'Label4
                '
                Me.Label4.AutoSize = True
                Me.Label4.Location = New System.Drawing.Point(15, 98)
                Me.Label4.Name = "Label4"
                Me.Label4.Size = New System.Drawing.Size(46, 13)
                Me.Label4.TabIndex = 62
                Me.Label4.Text = "Options:"
                '
                'chkWaitForExit
                '
                Me.chkWaitForExit.AutoSize = True
                Me.chkWaitForExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkWaitForExit.Location = New System.Drawing.Point(104, 96)
                Me.chkWaitForExit.Name = "chkWaitForExit"
                Me.chkWaitForExit.Size = New System.Drawing.Size(79, 17)
                Me.chkWaitForExit.TabIndex = 70
                Me.chkWaitForExit.Text = "Wait for exit"
                Me.chkWaitForExit.UseVisualStyleBackColor = True
                '
                'btnBrowse
                '
                Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnBrowse.Location = New System.Drawing.Point(576, 40)
                Me.btnBrowse.Name = "btnBrowse"
                Me.btnBrowse.Size = New System.Drawing.Size(95, 23)
                Me.btnBrowse.TabIndex = 40
                Me.btnBrowse.Text = "Browse..."
                Me.btnBrowse.UseVisualStyleBackColor = True
                '
                'txtArguments
                '
                Me.txtArguments.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtArguments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtArguments.Location = New System.Drawing.Point(104, 68)
                Me.txtArguments.Name = "txtArguments"
                Me.txtArguments.Size = New System.Drawing.Size(567, 20)
                Me.txtArguments.TabIndex = 60
                '
                'txtFilename
                '
                Me.txtFilename.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtFilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtFilename.Location = New System.Drawing.Point(104, 42)
                Me.txtFilename.Name = "txtFilename"
                Me.txtFilename.Size = New System.Drawing.Size(464, 20)
                Me.txtFilename.TabIndex = 30
                '
                'txtDisplayName
                '
                Me.txtDisplayName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtDisplayName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtDisplayName.Location = New System.Drawing.Point(104, 16)
                Me.txtDisplayName.Name = "txtDisplayName"
                Me.txtDisplayName.Size = New System.Drawing.Size(567, 20)
                Me.txtDisplayName.TabIndex = 10
                '
                'Label3
                '
                Me.Label3.AutoSize = True
                Me.Label3.Location = New System.Drawing.Point(15, 70)
                Me.Label3.Name = "Label3"
                Me.Label3.Size = New System.Drawing.Size(60, 13)
                Me.Label3.TabIndex = 50
                Me.Label3.Text = "Arguments:"
                '
                'Label2
                '
                Me.Label2.AutoSize = True
                Me.Label2.Location = New System.Drawing.Point(15, 44)
                Me.Label2.Name = "Label2"
                Me.Label2.Size = New System.Drawing.Size(52, 13)
                Me.Label2.TabIndex = 20
                Me.Label2.Text = "Filename:"
                '
                'Label1
                '
                Me.Label1.AutoSize = True
                Me.Label1.Location = New System.Drawing.Point(15, 19)
                Me.Label1.Name = "Label1"
                Me.Label1.Size = New System.Drawing.Size(75, 13)
                Me.Label1.TabIndex = 0
                Me.Label1.Text = "Display Name:"
                '
                'dlgOpenFile
                '
                Me.dlgOpenFile.Filter = "All Files (*.*)|*.*"
                '
                'ExternalApps
                '
                Me.ClientSize = New System.Drawing.Size(684, 323)
                Me.Controls.Add(Me.grpEditor)
                Me.Controls.Add(Me.lvApps)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
                Me.Name = "ExternalApps"
                Me.TabText = "External Applications"
                Me.Text = "External Applications"
                Me.cMenApps.ResumeLayout(False)
                Me.grpEditor.ResumeLayout(False)
                Me.grpEditor.PerformLayout()
                Me.ResumeLayout(False)

            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.ExternalApps
                Me.DockPnl = Panel
                Me.InitializeComponent()
            End Sub
#End Region

#Region "Private Properties"
            Private _SelApp As Tools.ExternalApp
#End Region

#Region "Private Methods"
            Private Sub LoadApps()
                Try
                    lvApps.Items.Clear()

                    For Each extA As Tools.ExternalApp In ExternalTools
                        Dim lvItem As New ListViewItem
                        lvItem.Text = extA.DisplayName
                        lvItem.SubItems.Add(extA.FileName)
                        lvItem.SubItems.Add(extA.Arguments)
                        lvItem.SubItems.Add(extA.WaitForExit)
                        lvItem.SubItems.Add(extA.TryIntegrate)
                        lvItem.Tag = extA

                        lvApps.Items.Add(lvItem)

                        If _SelApp IsNot Nothing Then
                            If extA Is _SelApp Then
                                lvItem.Selected = True
                            End If
                        End If
                    Next

                    RefreshToolbar()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "LoadApps failed (UI.Window.ExternalApps)" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub GetAppProperties(ByVal SelApp As Tools.ExternalApp)
                Try
                    If SelApp IsNot Nothing Then
                        txtDisplayName.Text = SelApp.DisplayName
                        txtFilename.Text = SelApp.FileName
                        txtArguments.Text = SelApp.Arguments
                        chkWaitForExit.Checked = SelApp.WaitForExit
                        chkTryIntegrate.Checked = SelApp.TryIntegrate
                        _SelApp = SelApp
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "GetAppProperties failed (UI.Window.ExternalApps)" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetAppProperties(ByVal SelApp As Tools.ExternalApp)
                Try
                    If SelApp IsNot Nothing Then
                        SelApp.DisplayName = txtDisplayName.Text
                        SelApp.FileName = txtFilename.Text
                        SelApp.Arguments = txtArguments.Text
                        SelApp.WaitForExit = chkWaitForExit.Checked
                        SelApp.TryIntegrate = chkTryIntegrate.Checked

                        LoadApps()
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SetAppProperties failed (UI.Window.ExternalApps)" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub AddNewApp()
                Try
                    Dim nExtA As New Tools.ExternalApp("New Application")
                    ExternalTools.Add(nExtA)
                    LoadApps()
                    lvApps.SelectedItems.Clear()

                    For Each lvItem As ListViewItem In lvApps.Items
                        If lvItem.Tag Is nExtA Then
                            lvItem.Selected = True
                            txtDisplayName.Focus()
                            Exit For
                        End If
                    Next
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AddNewApp failed (UI.Window.ExternalApps)" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub RemoveApps()
                Try
                    Dim Prompt As String = ""
                    Select Case lvApps.SelectedItems.Count
                        Case Is < 1
                            Exit Sub
                        Case Is = 1
                            Prompt = String.Format(My.Resources.strConfirmDeleteExternalTool, lvApps.SelectedItems(0).Text)
                        Case Is > 1
                            Prompt = String.Format(My.Resources.strConfirmDeleteExternalToolMultiple, lvApps.SelectedItems.Count)
                    End Select

                    If MsgBox(Prompt, MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        For Each lvItem As ListViewItem In lvApps.SelectedItems
                            ExternalTools.Remove(lvItem.Tag)
                            lvItem.Tag = Nothing
                            lvItem.Remove()
                        Next

                        RefreshToolbar()
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "RemoveApps failed (UI.Window.ExternalApps)" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub StartApp()
                Try
                    For Each lvItem As ListViewItem In lvApps.SelectedItems
                        TryCast(lvItem.Tag, mRemoteNG.Tools.ExternalApp).Start()
                    Next
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "StartApp failed (UI.Window.ExternalApps" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub RefreshToolbar()
                frmMain.AddExtAppsToToolbar()
                App.Runtime.GetExtApps()
            End Sub
#End Region

#Region "Form Stuff"
            Private Sub ExternalApps_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()
                LoadApps()
            End Sub

            Private Sub ApplyLanguage()
                clmDisplayName.Text = My.Resources.strColumnDisplayName
                clmFilename.Text = My.Resources.strColumnFilename
                clmArguments.Text = My.Resources.strColumnArguments
                clmWaitForExit.Text = My.Resources.strColumnWaitForExit
                cMenAppsAdd.Text = My.Resources.strMenuNewExternalTool
                cMenAppsRemove.Text = My.Resources.strMenuDeleteExternalTool
                cMenAppsStart.Text = My.Resources.strMenuLaunchExternalTool
                grpEditor.Text = My.Resources.strGroupboxExternalToolProperties
                Label4.Text = My.Resources.strLabelOptions
                chkWaitForExit.Text = My.Resources.strCheckboxWaitForExit
                chkTryIntegrate.Text = My.Resources.strTryIntegrate
                btnBrowse.Text = My.Resources.strButtonBrowse
                Label3.Text = My.Resources.strLabelArguments
                Label2.Text = My.Resources.strLabelFilename
                Label1.Text = My.Resources.strLabelDisplayName
                dlgOpenFile.Filter = My.Resources.strFilterApplication & "|*.exe|" & My.Resources.strFilterAll & "|*.*"
                TabText = My.Resources.strMenuExternalTools
                Text = My.Resources.strMenuExternalTools
            End Sub

            Private Sub lvApps_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvApps.DoubleClick
                If lvApps.SelectedItems.Count > 0 Then
                    StartApp()
                End If
            End Sub

            Private Sub lvApps_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvApps.SelectedIndexChanged
                If lvApps.SelectedItems.Count = 1 Then
                    Me.grpEditor.Enabled = True
                    GetAppProperties(lvApps.SelectedItems(0).Tag)
                Else
                    Me.grpEditor.Enabled = False
                End If
            End Sub

            Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
                If dlgOpenFile.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                    txtFilename.Text = dlgOpenFile.FileName
                End If
            End Sub

            Private Sub ApplicationEditorField_ChangedOrLostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDisplayName.LostFocus, txtArguments.LostFocus, txtFilename.LostFocus, btnBrowse.LostFocus, chkWaitForExit.LostFocus, chkWaitForExit.Click, chkTryIntegrate.Click
                SetAppProperties(_SelApp)
            End Sub

            Private Sub chkTryIntegrate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTryIntegrate.CheckedChanged
                If chkTryIntegrate.Checked Then
                    chkWaitForExit.Checked = False
                    chkWaitForExit.Enabled = False
                Else
                    chkWaitForExit.Enabled = True
                End If
            End Sub
#End Region

#Region "Menu"
            Private Sub cMenAppsAddApp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenAppsAdd.Click
                AddNewApp()
            End Sub

            Private Sub cMenAppsRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenAppsRemove.Click
                RemoveApps()
            End Sub

            Private Sub cMenAppsStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenAppsStart.Click
                StartApp()
            End Sub
#End Region

        End Class
    End Namespace
End Namespace