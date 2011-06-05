Imports System
Imports System.Windows
Imports System.Windows.Forms
Imports Crownwood
Imports WeifenLuo.WinFormsUI.Docking
Imports PSTaskDialog
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class Connection
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents cmenTab As System.Windows.Forms.ContextMenuStrip
            Private components As System.ComponentModel.IContainer
            Friend WithEvents cmenTabFullscreen As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabScreenshot As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabTransferFile As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabSendSpecialKeys As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabSep1 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents cmenTabRenameTab As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabDuplicateTab As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabDisconnect As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabSmartSize As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabSendSpecialKeysCtrlAltDel As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabSendSpecialKeysCtrlEsc As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabViewOnly As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabReconnect As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabExternalApps As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabStartChat As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cmenTabRefreshScreen As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents cmenTabPuttySettings As System.Windows.Forms.ToolStripMenuItem

            Public WithEvents TabController As Crownwood.Magic.Controls.TabControl
            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container
                Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Connection))
                Me.TabController = New Crownwood.Magic.Controls.TabControl
                Me.cmenTab = New System.Windows.Forms.ContextMenuStrip(Me.components)
                Me.cmenTabFullscreen = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabSmartSize = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabViewOnly = New System.Windows.Forms.ToolStripMenuItem
                Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
                Me.cmenTabScreenshot = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabStartChat = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabTransferFile = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabRefreshScreen = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabSendSpecialKeys = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabSendSpecialKeysCtrlAltDel = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabSendSpecialKeysCtrlEsc = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabExternalApps = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabSep1 = New System.Windows.Forms.ToolStripSeparator
                Me.cmenTabRenameTab = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabDuplicateTab = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabReconnect = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabDisconnect = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTabPuttySettings = New System.Windows.Forms.ToolStripMenuItem
                Me.cmenTab.SuspendLayout()
                Me.SuspendLayout()
                '
                'TabController
                '
                Me.TabController.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.TabController.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument
                Me.TabController.Cursor = System.Windows.Forms.Cursors.Hand
                Me.TabController.IDEPixelArea = True
                Me.TabController.IDEPixelBorder = False
                Me.TabController.Location = New System.Drawing.Point(0, -1)
                Me.TabController.Name = "TabController"
                Me.TabController.Size = New System.Drawing.Size(632, 454)
                Me.TabController.TabIndex = 0
                '
                'cmenTab
                '
                Me.cmenTab.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmenTabFullscreen, Me.cmenTabSmartSize, Me.cmenTabViewOnly, Me.ToolStripSeparator1, Me.cmenTabScreenshot, Me.cmenTabStartChat, Me.cmenTabTransferFile, Me.cmenTabRefreshScreen, Me.cmenTabSendSpecialKeys, Me.cmenTabPuttySettings, Me.cmenTabExternalApps, Me.cmenTabSep1, Me.cmenTabRenameTab, Me.cmenTabDuplicateTab, Me.cmenTabReconnect, Me.cmenTabDisconnect})
                Me.cmenTab.Name = "cmenTab"
                Me.cmenTab.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
                Me.cmenTab.Size = New System.Drawing.Size(202, 346)
                '
                'cmenTabFullscreen
                '
                Me.cmenTabFullscreen.Image = Global.mRemoteNG.My.Resources.Resources.Fullscreen
                Me.cmenTabFullscreen.Name = "cmenTabFullscreen"
                Me.cmenTabFullscreen.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabFullscreen.Text = "Fullscreen (RDP)"
                '
                'cmenTabSmartSize
                '
                Me.cmenTabSmartSize.Image = Global.mRemoteNG.My.Resources.Resources.SmartSize
                Me.cmenTabSmartSize.Name = "cmenTabSmartSize"
                Me.cmenTabSmartSize.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabSmartSize.Text = "SmartSize (RDP/VNC)"
                '
                'cmenTabViewOnly
                '
                Me.cmenTabViewOnly.Name = "cmenTabViewOnly"
                Me.cmenTabViewOnly.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabViewOnly.Text = "View Only (VNC)"
                '
                'ToolStripSeparator1
                '
                Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
                Me.ToolStripSeparator1.Size = New System.Drawing.Size(198, 6)
                '
                'cmenTabScreenshot
                '
                Me.cmenTabScreenshot.Image = Global.mRemoteNG.My.Resources.Resources.Screenshot_Add
                Me.cmenTabScreenshot.Name = "cmenTabScreenshot"
                Me.cmenTabScreenshot.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabScreenshot.Text = "Screenshot"
                '
                'cmenTabStartChat
                '
                Me.cmenTabStartChat.Image = Global.mRemoteNG.My.Resources.Resources.Chat
                Me.cmenTabStartChat.Name = "cmenTabStartChat"
                Me.cmenTabStartChat.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabStartChat.Text = "Start Chat (VNC)"
                Me.cmenTabStartChat.Visible = False
                '
                'cmenTabTransferFile
                '
                Me.cmenTabTransferFile.Image = Global.mRemoteNG.My.Resources.Resources.SSHTransfer
                Me.cmenTabTransferFile.Name = "cmenTabTransferFile"
                Me.cmenTabTransferFile.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabTransferFile.Text = "Transfer File (SSH)"
                '
                'cmenTabRefreshScreen
                '
                Me.cmenTabRefreshScreen.Image = Global.mRemoteNG.My.Resources.Resources.Refresh
                Me.cmenTabRefreshScreen.Name = "cmenTabRefreshScreen"
                Me.cmenTabRefreshScreen.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabRefreshScreen.Text = "Refresh Screen (VNC)"
                '
                'cmenTabSendSpecialKeys
                '
                Me.cmenTabSendSpecialKeys.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmenTabSendSpecialKeysCtrlAltDel, Me.cmenTabSendSpecialKeysCtrlEsc})
                Me.cmenTabSendSpecialKeys.Image = Global.mRemoteNG.My.Resources.Resources.Keyboard
                Me.cmenTabSendSpecialKeys.Name = "cmenTabSendSpecialKeys"
                Me.cmenTabSendSpecialKeys.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabSendSpecialKeys.Text = "Send special Keys (VNC)"
                '
                'cmenTabSendSpecialKeysCtrlAltDel
                '
                Me.cmenTabSendSpecialKeysCtrlAltDel.Name = "cmenTabSendSpecialKeysCtrlAltDel"
                Me.cmenTabSendSpecialKeysCtrlAltDel.Size = New System.Drawing.Size(141, 22)
                Me.cmenTabSendSpecialKeysCtrlAltDel.Text = "Ctrl+Alt+Del"
                '
                'cmenTabSendSpecialKeysCtrlEsc
                '
                Me.cmenTabSendSpecialKeysCtrlEsc.Name = "cmenTabSendSpecialKeysCtrlEsc"
                Me.cmenTabSendSpecialKeysCtrlEsc.Size = New System.Drawing.Size(141, 22)
                Me.cmenTabSendSpecialKeysCtrlEsc.Text = "Ctrl+Esc"
                '
                'cmenTabExternalApps
                '
                Me.cmenTabExternalApps.Image = CType(resources.GetObject("cmenTabExternalApps.Image"), System.Drawing.Image)
                Me.cmenTabExternalApps.Name = "cmenTabExternalApps"
                Me.cmenTabExternalApps.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabExternalApps.Text = "External Applications"
                '
                'cmenTabSep1
                '
                Me.cmenTabSep1.Name = "cmenTabSep1"
                Me.cmenTabSep1.Size = New System.Drawing.Size(198, 6)
                '
                'cmenTabRenameTab
                '
                Me.cmenTabRenameTab.Image = Global.mRemoteNG.My.Resources.Resources.Rename
                Me.cmenTabRenameTab.Name = "cmenTabRenameTab"
                Me.cmenTabRenameTab.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabRenameTab.Text = "Rename Tab"
                '
                'cmenTabDuplicateTab
                '
                Me.cmenTabDuplicateTab.Name = "cmenTabDuplicateTab"
                Me.cmenTabDuplicateTab.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabDuplicateTab.Text = "Duplicate Tab"
                '
                'cmenTabReconnect
                '
                Me.cmenTabReconnect.Image = CType(resources.GetObject("cmenTabReconnect.Image"), System.Drawing.Image)
                Me.cmenTabReconnect.Name = "cmenTabReconnect"
                Me.cmenTabReconnect.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabReconnect.Text = "Reconnect"
                '
                'cmenTabDisconnect
                '
                Me.cmenTabDisconnect.Image = Global.mRemoteNG.My.Resources.Resources.Pause
                Me.cmenTabDisconnect.Name = "cmenTabDisconnect"
                Me.cmenTabDisconnect.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabDisconnect.Text = "Disconnect"
                '
                'cmenTabPuttySettings
                '
                Me.cmenTabPuttySettings.Name = "cmenTabPuttySettings"
                Me.cmenTabPuttySettings.Size = New System.Drawing.Size(201, 22)
                Me.cmenTabPuttySettings.Text = "PuTTY Settings"
                '
                'Connection
                '
                Me.ClientSize = New System.Drawing.Size(632, 453)
                Me.Controls.Add(Me.TabController)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.Icon = Global.mRemoteNG.My.Resources.Resources.mRemote_Icon
                Me.Name = "Connection"
                Me.TabText = "UI.Window.Connection"
                Me.Text = "UI.Window.Connection"
                Me.cmenTab.ResumeLayout(False)
                Me.ResumeLayout(False)

            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent, Optional ByVal FormText As String = "")

                If FormText = "" Then
                    FormText = My.Resources.strNewPanel
                End If

                Me.WindowType = Type.Connection
                Me.DockPnl = Panel
                Me.InitializeComponent()
                Me.Text = FormText
                Me.TabText = FormText
            End Sub

            Public Function AddConnectionTab(ByVal conI As mRemoteNG.Connection.Info) As Magic.Controls.TabPage
                Try
                    Dim nTab As New Magic.Controls.TabPage
                    nTab.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

                    If My.Settings.ShowProtocolOnTabs Then
                        nTab.Title = conI.Protocol.ToString & ": "
                    Else
                        nTab.Title = ""
                    End If

                    nTab.Title &= conI.Name

                    If My.Settings.ShowLogonInfoOnTabs Then
                        nTab.Title &= " ("

                        If conI.Domain <> "" Then
                            nTab.Title &= conI.Domain
                        End If

                        If conI.Username <> "" Then
                            If conI.Domain <> "" Then
                                nTab.Title &= "\"
                            End If

                            nTab.Title &= conI.Username
                        End If

                        nTab.Title &= ")"
                    End If

                    Dim conIcon As Icon = mRemoteNG.Connection.Icon.FromString(conI.Icon)
                    If conIcon IsNot Nothing Then
                        nTab.Icon = conIcon
                    End If

                    If My.Settings.OpenTabsRightOfSelected Then
                        Me.TabController.TabPages.Insert(Me.TabController.SelectedIndex + 1, nTab)
                    Else
                        Me.TabController.TabPages.Add(nTab)
                    End If

                    nTab.Selected = True

                    Return nTab
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AddConnectionTab (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try

                Return Nothing
            End Function
#End Region

#Region "Form"
            Private Sub Connection_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()
            End Sub

            Private Sub ApplyLanguage()
                cmenTabFullscreen.Text = My.Resources.strMenuFullScreenRDP
                cmenTabSmartSize.Text = My.Resources.strMenuSmartSize
                cmenTabViewOnly.Text = My.Resources.strMenuViewOnly
                cmenTabScreenshot.Text = My.Resources.strMenuScreenshot
                cmenTabStartChat.Text = My.Resources.strMenuStartChat
                cmenTabTransferFile.Text = My.Resources.strMenuTransferFile
                cmenTabRefreshScreen.Text = My.Resources.strMenuRefreshScreen
                cmenTabSendSpecialKeys.Text = My.Resources.strMenuSendSpecialKeys
                cmenTabSendSpecialKeysCtrlAltDel.Text = My.Resources.strMenuCtrlAltDel
                cmenTabSendSpecialKeysCtrlEsc.Text = My.Resources.strMenuCtrlEsc
                cmenTabExternalApps.Text = My.Resources.strMenuExternalTools
                cmenTabRenameTab.Text = My.Resources.strMenuRenameTab
                cmenTabDuplicateTab.Text = My.Resources.strMenuDuplicateTab
                cmenTabReconnect.Text = My.Resources.strMenuReconnect
                cmenTabDisconnect.Text = My.Resources.strMenuDisconnect
                cmenTabPuttySettings.Text = My.Resources.strPuttySettings
            End Sub

            Private Sub Connection_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
                If Not frmMain.IsClosing And My.Settings.ConfirmCloseConnection And TabController.TabPages.Count > 0 Then
                    Dim Result As DialogResult = cTaskDialog.MessageBox(Me, My.Application.Info.ProductName, String.Format(My.Resources.strConfirmCloseConnectionPanelMainInstruction, Me.Text), "", "", "", My.Resources.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.YesNo, eSysIcons.Question, Nothing)
                    If cTaskDialog.VerificationChecked Then
                        My.Settings.ConfirmCloseConnection = False
                    End If
                    If Result = DialogResult.No Then
                        e.Cancel = True
                        Exit Sub
                    End If
                End If

                Try
                    For Each tabP As Magic.Controls.TabPage In Me.TabController.TabPages
                        If tabP.Tag IsNot Nothing Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = tabP.Tag
                            IC.Protocol.Close()
                        End If
                    Next
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Connection_FormClosing (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub Connection_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
                Try
                    For Each tabP As Magic.Controls.TabPage In Me.TabController.TabPages
                        If tabP.Tag IsNot Nothing Then
                            If TypeOf tabP.Tag Is mRemoteNG.Connection.InterfaceControl Then
                                Dim IC As mRemoteNG.Connection.InterfaceControl = tabP.Tag
                                IC.Protocol.Resize()
                            End If
                        End If
                    Next
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Connection_Resize (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "TabController"
            Private Sub TabController_ClosePressed(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabController.ClosePressed
                If Me.TabController.SelectedTab Is Nothing Then
                    Exit Sub
                End If

                Me.CloseConnectionTab()
            End Sub

            Private Sub CloseConnectionTab()
                Dim SelectedTab As Crownwood.Magic.Controls.TabPage = Me.TabController.SelectedTab
                If My.Settings.ConfirmCloseConnection Then
                    Dim Result As DialogResult = cTaskDialog.MessageBox(Me, My.Application.Info.ProductName, String.Format(My.Resources.strConfirmCloseConnectionMainInstruction, SelectedTab.Title), "", "", "", My.Resources.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.YesNo, eSysIcons.Question, Nothing)
                    If cTaskDialog.VerificationChecked Then
                        My.Settings.ConfirmCloseConnection = False
                    End If
                    If Result = DialogResult.No Then
                        Exit Sub
                    End If
                End If

                Try
                    If SelectedTab.Tag IsNot Nothing Then
                        Dim IC As mRemoteNG.Connection.InterfaceControl = SelectedTab.Tag
                        IC.Protocol.Close()
                    Else
                        Me.CloseTab(SelectedTab)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "CloseConnectionTab (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub TabController_DoubleClickTab(ByVal sender As Crownwood.Magic.Controls.TabControl, ByVal page As Crownwood.Magic.Controls.TabPage) Handles TabController.DoubleClickTab
                If My.Settings.DoubleClickOnTabClosesIt Then
                    Me.CloseConnectionTab()
                End If
            End Sub

#Region "Drag and Drop"
            Private Sub TabController_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TabController.DragDrop
                If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) Then
                    App.Runtime.OpenConnection(e.Data.GetData("System.Windows.Forms.TreeNode", True).Tag, Me, mRemoteNG.Connection.Info.Force.DoNotJump)
                End If
            End Sub

            Private Sub TabController_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TabController.DragEnter
                If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) Then
                    e.Effect = DragDropEffects.Move
                End If
            End Sub

            Private Sub TabController_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TabController.DragOver
                e.Effect = DragDropEffects.Move
            End Sub
#End Region
#End Region

#Region "Tab Menu"
            Private Sub ShowHideMenuButtons()
                Try
                    If Me.TabController.SelectedTab Is Nothing Then
                        Exit Sub
                    End If

                    Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                    If IC Is Nothing Then
                        Exit Sub
                    End If

                    If IC.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.RDP Then
                        Me.cmenTabFullscreen.Enabled = True
                        Me.cmenTabSmartSize.Enabled = True

                        Dim rdp As mRemoteNG.Connection.Protocol.RDP = IC.Protocol
                        Me.cmenTabSmartSize.Checked = rdp.SmartSize
                        Me.cmenTabFullscreen.Checked = rdp.Fullscreen
                    Else
                        Me.cmenTabFullscreen.Enabled = False
                        Me.cmenTabSmartSize.Enabled = False
                    End If

                    If IC.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.VNC Then
                        Me.cmenTabSendSpecialKeys.Enabled = True
                        Me.cmenTabViewOnly.Enabled = True

                        Me.cmenTabSmartSize.Enabled = True
                        Me.cmenTabStartChat.Enabled = True
                        Me.cmenTabRefreshScreen.Enabled = True
                        Me.cmenTabTransferFile.Enabled = False

                        Dim vnc As mRemoteNG.Connection.Protocol.VNC = IC.Protocol
                        Me.cmenTabSmartSize.Checked = vnc.SmartSize
                        Me.cmenTabViewOnly.Checked = vnc.ViewOnly
                    Else
                        Me.cmenTabSendSpecialKeys.Enabled = False
                        Me.cmenTabViewOnly.Enabled = False
                        Me.cmenTabStartChat.Enabled = False
                        Me.cmenTabRefreshScreen.Enabled = False
                        Me.cmenTabTransferFile.Enabled = False
                    End If

                    If IC.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH1 Or IC.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH2 Then
                        Me.cmenTabTransferFile.Enabled = True
                    End If

                    If TypeOf IC.Protocol Is mRemoteNG.Connection.Protocol.PuttyBase Then
                        Me.cmenTabPuttySettings.Enabled = True
                    Else
                        Me.cmenTabPuttySettings.Enabled = False
                    End If

                    AddExternalApps()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ShowHideMenuButtons (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub cmenTabScreenshot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmenTabScreenshot.Click
                cmenTab.Close()
                Application.DoEvents()
                Windows.screenshotForm.AddScreenshot(Tools.Misc.TakeScreenshot(Me))
            End Sub

            Private Sub cmenTabSmartSize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmenTabSmartSize.Click
                Me.ToggleSmartSize()
            End Sub

            Private Sub cmenTabReconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmenTabReconnect.Click
                Me.Reconnect()
            End Sub

            Private Sub cmenTabTransferFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmenTabTransferFile.Click
                Me.TransferFile()
            End Sub

            Private Sub cmenTabViewOnly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmenTabViewOnly.Click
                Me.ToggleViewOnly()
            End Sub

            Private Sub cmenTabStartChat_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmenTabStartChat.Click
                Me.StartChat()
            End Sub

            Private Sub cmenTabRefreshScreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmenTabRefreshScreen.Click
                Me.RefreshScreen()
            End Sub

            Private Sub cmenTabSendSpecialKeysCtrlAltDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmenTabSendSpecialKeysCtrlAltDel.Click
                Me.SendSpecialKeys(mRemoteNG.Connection.Protocol.VNC.SpecialKeys.CtrlAltDel)
            End Sub

            Private Sub cmenTabSendSpecialKeysCtrlEsc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmenTabSendSpecialKeysCtrlEsc.Click
                Me.SendSpecialKeys(mRemoteNG.Connection.Protocol.VNC.SpecialKeys.CtrlEsc)
            End Sub

            Private Sub cmenTabFullscreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmenTabFullscreen.Click
                Me.ToggleFullscreen()
            End Sub

            Private Sub cmenTabPuttySettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmenTabPuttySettings.Click
                Me.ShowPuttySettingsDialog()
            End Sub

            Private Sub cmenTabExternalAppsEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs)
                StartExternalApp(sender.Tag)
            End Sub

            Private Sub cmenTabDisconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmenTabDisconnect.Click
                Me.CloseTabMenu()
            End Sub

            Private Sub cmenTabDuplicateTab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmenTabDuplicateTab.Click
                Me.DuplicateTab()
            End Sub

            Private Sub cmenTabRenameTab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmenTabRenameTab.Click
                Me.RenameTab()
            End Sub
#End Region

#Region "Tab Actions"
            Private Sub ToggleSmartSize()
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                            If TypeOf IC.Protocol Is mRemoteNG.Connection.Protocol.RDP Then
                                Dim rdp As mRemoteNG.Connection.Protocol.RDP = IC.Protocol
                                rdp.ToggleSmartSize()
                            ElseIf TypeOf IC.Protocol Is mRemoteNG.Connection.Protocol.VNC Then
                                Dim vnc As mRemoteNG.Connection.Protocol.VNC = IC.Protocol
                                vnc.ToggleSmartSize()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ToggleSmartSize (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub TransferFile()
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                            If IC.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH1 Or IC.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH2 Then
                                SSHTransferFile()
                            ElseIf IC.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.VNC Then
                                VNCTransferFile()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "TransferFile (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SSHTransferFile()
                Try

                    Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                    Windows.Show(Type.SSHTransfer)

                    Dim conI As mRemoteNG.Connection.Info = IC.Info

                    Windows.sshtransferForm.Hostname = conI.Hostname
                    Windows.sshtransferForm.Username = conI.Username
                    Windows.sshtransferForm.Password = conI.Password
                    Windows.sshtransferForm.Port = conI.Port
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SSHTransferFile (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub VNCTransferFile()
                Try
                    Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag
                    Dim vnc As mRemoteNG.Connection.Protocol.VNC = IC.Protocol
                    vnc.StartFileTransfer()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "VNCTransferFile (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub ToggleViewOnly()
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                            If TypeOf IC.Protocol Is mRemoteNG.Connection.Protocol.VNC Then
                                cmenTabViewOnly.Checked = Not cmenTabViewOnly.Checked

                                Dim vnc As mRemoteNG.Connection.Protocol.VNC = IC.Protocol
                                vnc.ToggleViewOnly()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ToggleViewOnly (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub StartChat()
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                            If TypeOf IC.Protocol Is mRemoteNG.Connection.Protocol.VNC Then
                                Dim vnc As mRemoteNG.Connection.Protocol.VNC = IC.Protocol
                                vnc.StartChat()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "StartChat (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub RefreshScreen()
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                            If TypeOf IC.Protocol Is mRemoteNG.Connection.Protocol.VNC Then
                                Dim vnc As mRemoteNG.Connection.Protocol.VNC = IC.Protocol
                                vnc.RefreshScreen()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "RefreshScreen (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SendSpecialKeys(ByVal Keys As mRemoteNG.Connection.Protocol.VNC.SpecialKeys)
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                            If TypeOf IC.Protocol Is mRemoteNG.Connection.Protocol.VNC Then
                                Dim vnc As mRemoteNG.Connection.Protocol.VNC = IC.Protocol
                                vnc.SendSpecialKeys(Keys)
                            End If
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SendSpecialKeys (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub ToggleFullscreen()
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                            If TypeOf IC.Protocol Is mRemoteNG.Connection.Protocol.RDP Then
                                Dim rdp As mRemoteNG.Connection.Protocol.RDP = IC.Protocol
                                rdp.ToggleFullscreen()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ToggleFullscreen (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub ShowPuttySettingsDialog()
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim objInterfaceControl As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                            If TypeOf objInterfaceControl.Protocol Is mRemoteNG.Connection.Protocol.PuttyBase Then
                                Dim objPuttyBase As mRemoteNG.Connection.Protocol.PuttyBase = objInterfaceControl.Protocol

                                objPuttyBase.ShowSettingsDialog()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ShowPuttySettingsDialog (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub AddExternalApps()
                Try
                    'clean up
                    cmenTabExternalApps.DropDownItems.Clear()

                    'add ext apps
                    For Each extA As Tools.ExternalApp In ExternalTools
                        Dim nItem As New ToolStripMenuItem
                        nItem.Text = extA.DisplayName
                        nItem.Tag = extA

                        nItem.Image = extA.Image

                        AddHandler nItem.Click, AddressOf cmenTabExternalAppsEntry_Click

                        cmenTabExternalApps.DropDownItems.Add(nItem)
                    Next
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "cMenTreeTools_DropDownOpening failed (UI.Window.Tree)" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub StartExternalApp(ByVal ExtA As Tools.ExternalApp)
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                            ExtA.Start(IC.Info)
                        End If
                    End If

                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "cmenTabExternalAppsEntry_Click failed (UI.Window.Tree)" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub CloseTabMenu()
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                            IC.Protocol.Close()
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "CloseTabMenu (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub DuplicateTab()
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                            App.Runtime.OpenConnection(IC.Info, mRemoteNG.Connection.Info.Force.DoNotJump)
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "DuplicateTab (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub Reconnect()
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag
                            Dim conI As mRemoteNG.Connection.Info = IC.Info

                            IC.Protocol.Close()

                            App.Runtime.OpenConnection(conI, mRemoteNG.Connection.Info.Force.DoNotJump)
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Reconnect (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub RenameTab()
                Try
                    Dim nTitle As String = InputBox(My.Resources.strNewTitle & ":", , Me.TabController.SelectedTab.Title.Replace("&&", "&"))

                    If nTitle <> "" Then
                        Me.TabController.SelectedTab.Title = nTitle.Replace("&", "&&")
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "RenameTab (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Protocols"
            Public Sub Prot_Event_Closed(ByVal sender As Object)
                Dim Prot As mRemoteNG.Connection.Protocol.Base = sender
                CloseTab(Prot.InterfaceControl.Parent)
            End Sub
#End Region

#Region "Tabs"
            Private Delegate Sub CloseTabCB(ByVal TabToBeClosed As Magic.Controls.TabPage)
            Private Sub CloseTab(ByVal TabToBeClosed As Magic.Controls.TabPage)
                If Me.TabController.InvokeRequired Then
                    Dim s As New CloseTabCB(AddressOf CloseTab)

                    Try
                        Me.TabController.Invoke(s, TabToBeClosed)
                    Catch comEx As System.Runtime.InteropServices.COMException
                        Me.TabController.Invoke(s, TabToBeClosed)
                    Catch ex As Exception
                        MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't close tab" & vbNewLine & ex.Message, True)
                    End Try
                Else
                    Try
                        Me.TabController.TabPages.Remove(TabToBeClosed)
                    Catch comEx As System.Runtime.InteropServices.COMException
                        CloseTab(TabToBeClosed)
                    Catch ex As Exception
                        MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't close tab" & vbNewLine & ex.Message, True)
                    End Try

                    If Me.TabController.TabPages.Count = 0 Then
                        Me.Close()
                    End If
                End If
            End Sub

            Private Sub TabController_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabController.SelectionChanged
                Me.FocusIC()
                Me.RefreshIC()
            End Sub

            Private Sub FocusIC()
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag
                            IC.Protocol.Focus()
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "FocusIC (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub RefreshIC()
                Try
                    If Me.TabController.SelectedTab IsNot Nothing Then
                        If TypeOf Me.TabController.SelectedTab.Tag Is mRemoteNG.Connection.InterfaceControl Then
                            Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                            If IC.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.VNC Then
                                TryCast(IC.Protocol, mRemoteNG.Connection.Protocol.VNC).RefreshScreen()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "RefreshIC (UI.Window.Connection) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Window Overrides"
            Protected Overloads Overrides Sub WndProc(ByRef WndMsg As Message)
                Try
                    If WndMsg.Msg = 33 Then
                        Dim curTab As Magic.Controls.TabPage = Me.TabController.SelectedTab
                        Dim curRect As Rectangle = curTab.RectangleToScreen(curTab.ClientRectangle)

                        If curRect.Contains(Form.MousePosition) Then
                            If curTab IsNot Nothing Then
                                Dim IC As mRemoteNG.Connection.InterfaceControl = Me.TabController.SelectedTab.Tag

                                If IC.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.RDP Then
                                    IC.Protocol.Focus()
                                End If
                            End If
                        End If
                    End If
                Catch ex As Exception
                End Try

                MyBase.WndProc(WndMsg)
            End Sub
#End Region

#Region "Tab drag and drop"
            Dim TabDragging As Boolean

            Private Sub TabController_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabController.MouseLeave
                Try
                    If TabDragging = True Then
                        TabDragging = False
                        Me.Cursor = Cursors.Default
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "TabController_MouseLeave (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub TabController_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TabController.MouseMove
                Try
                    If e.Button = Forms.MouseButtons.Left Then
                        TabDragging = True
                        Me.Cursor = Cursors.SizeWE
                    ElseIf e.Button = Forms.MouseButtons.None Then
                        If TabDragging = True Then
                            TabDragging = False
                            Me.Cursor = Cursors.Default
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "TabController_MouseMove (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub TabController_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TabController.MouseUp
                Try
                    If e.Button = Forms.MouseButtons.Left Then
                        If TabDragging = True Then
                            Dim sourceTab As Crownwood.Magic.Controls.TabPage = TabController.SelectedTab
                            Dim targetTab As Crownwood.Magic.Controls.TabPage = TabController.TabPageFromPoint(e.Location)
                            Dim targetIndex As Integer = -1

                            For i As Integer = 0 To TabController.TabPages.Count - 1
                                If TabController.TabPages(i) Is targetTab Then
                                    targetIndex = i
                                End If
                            Next

                            If sourceTab IsNot targetTab And targetIndex > -1 Then
                                TabController.TabPages.Remove(sourceTab)
                                TabController.TabPages.Insert(targetIndex, sourceTab)
                                TabController.SelectedTab = sourceTab
                            End If
                        Else
                            Me.FocusIC()
                        End If
                    ElseIf e.Button = Forms.MouseButtons.Middle Then
                        If TabDragging = False Then
                            Me.TabController.SelectedTab = Me.TabController.TabPageFromPoint(e.Location)
                            Me.CloseConnectionTab()
                        End If
                    ElseIf e.Button = Forms.MouseButtons.Right Then
                        If TabDragging = False Then
                            Me.ShowHideMenuButtons()
                            Me.cmenTab.Show(Me.TabController, e.Location)
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "TabController_MouseUp (UI.Window.Connections) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region
        End Class
    End Namespace
End Namespace