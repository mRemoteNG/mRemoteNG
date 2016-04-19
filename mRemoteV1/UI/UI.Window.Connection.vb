Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports Crownwood.Magic.Controls
Imports mRemote3G.App
Imports mRemote3G.Config
Imports mRemote3G.Connection
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Forms
Imports mRemote3G.Messages
Imports mRemote3G.My
Imports mRemote3G.Tools
Imports PSTaskDialog
Imports WeifenLuo.WinFormsUI.Docking

Namespace UI

    Namespace Window
        Public Class Connection
            Inherits Base

#Region "Form Init"

            Friend WithEvents cmenTab As ContextMenuStrip
            Private components As IContainer
            Friend WithEvents cmenTabFullscreen As ToolStripMenuItem
            Friend WithEvents cmenTabScreenshot As ToolStripMenuItem
            Friend WithEvents cmenTabTransferFile As ToolStripMenuItem
            Friend WithEvents cmenTabSendSpecialKeys As ToolStripMenuItem
            Friend WithEvents cmenTabSep1 As ToolStripSeparator
            Friend WithEvents cmenTabRenameTab As ToolStripMenuItem
            Friend WithEvents cmenTabDuplicateTab As ToolStripMenuItem
            Friend WithEvents cmenTabDisconnect As ToolStripMenuItem
            Friend WithEvents cmenTabSmartSize As ToolStripMenuItem
            Friend WithEvents cmenTabSendSpecialKeysCtrlAltDel As ToolStripMenuItem
            Friend WithEvents cmenTabSendSpecialKeysCtrlEsc As ToolStripMenuItem
            Friend WithEvents cmenTabViewOnly As ToolStripMenuItem
            Friend WithEvents cmenTabReconnect As ToolStripMenuItem
            Friend WithEvents cmenTabExternalApps As ToolStripMenuItem
            Friend WithEvents cmenTabStartChat As ToolStripMenuItem
            Friend WithEvents cmenTabRefreshScreen As ToolStripMenuItem
            Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
            Friend WithEvents cmenTabPuttySettings As ToolStripMenuItem

            Public WithEvents TabController As TabControl

            Private Sub InitializeComponent()
                components = New System.ComponentModel.Container()
                Dim resources = New ComponentResourceManager(GetType(Connection))
                TabController = New TabControl()
                cmenTab = New ContextMenuStrip(components)
                cmenTabFullscreen = New ToolStripMenuItem()
                cmenTabSmartSize = New ToolStripMenuItem()
                cmenTabViewOnly = New ToolStripMenuItem()
                ToolStripSeparator1 = New ToolStripSeparator()
                cmenTabScreenshot = New ToolStripMenuItem()
                cmenTabStartChat = New ToolStripMenuItem()
                cmenTabTransferFile = New ToolStripMenuItem()
                cmenTabRefreshScreen = New ToolStripMenuItem()
                cmenTabSendSpecialKeys = New ToolStripMenuItem()
                cmenTabSendSpecialKeysCtrlAltDel = New ToolStripMenuItem()
                cmenTabSendSpecialKeysCtrlEsc = New ToolStripMenuItem()
                cmenTabPuttySettings = New ToolStripMenuItem()
                cmenTabExternalApps = New ToolStripMenuItem()
                cmenTabSep1 = New ToolStripSeparator()
                cmenTabRenameTab = New ToolStripMenuItem()
                cmenTabDuplicateTab = New ToolStripMenuItem()
                cmenTabReconnect = New ToolStripMenuItem()
                cmenTabDisconnect = New ToolStripMenuItem()
                cmenTab.SuspendLayout()
                SuspendLayout()
                '
                'TabController
                '
                TabController.Anchor = (((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                         Or AnchorStyles.Left) _
                                        Or AnchorStyles.Right)
                TabController.Appearance = TabControl.VisualAppearance.MultiDocument
                TabController.Cursor = Cursors.Hand
                TabController.DragFromControl = False
                TabController.IDEPixelArea = False
                TabController.Location = New Point(0, -1)
                TabController.Name = "TabController"
                TabController.Size = New Size(632, 454)
                TabController.TabIndex = 0
                '
                'cmenTab
                '
                cmenTab.ImageScalingSize = New Size(20, 20)
                cmenTab.Items.AddRange(New ToolStripItem() _
                                             {cmenTabFullscreen, cmenTabSmartSize, cmenTabViewOnly,
                                              ToolStripSeparator1, cmenTabScreenshot, cmenTabStartChat,
                                              cmenTabTransferFile, cmenTabRefreshScreen, cmenTabSendSpecialKeys,
                                              cmenTabPuttySettings, cmenTabExternalApps, cmenTabSep1,
                                              cmenTabRenameTab, cmenTabDuplicateTab, cmenTabReconnect,
                                              cmenTabDisconnect})
                cmenTab.Name = "cmenTab"
                cmenTab.RenderMode = ToolStripRenderMode.Professional
                cmenTab.Size = New Size(245, 380)
                '
                'cmenTabFullscreen
                '
                cmenTabFullscreen.Image = My.Resources.Resources.arrow_out
                cmenTabFullscreen.Name = "cmenTabFullscreen"
                cmenTabFullscreen.Size = New Size(244, 26)
                cmenTabFullscreen.Text = "Fullscreen (RDP)"
                '
                'cmenTabSmartSize
                '
                cmenTabSmartSize.Image = My.Resources.Resources.SmartSize
                cmenTabSmartSize.Name = "cmenTabSmartSize"
                cmenTabSmartSize.Size = New Size(244, 26)
                cmenTabSmartSize.Text = "SmartSize (RDP/VNC)"
                '
                'cmenTabViewOnly
                '
                cmenTabViewOnly.Name = "cmenTabViewOnly"
                cmenTabViewOnly.Size = New Size(244, 26)
                cmenTabViewOnly.Text = "View Only (VNC)"
                '
                'ToolStripSeparator1
                '
                ToolStripSeparator1.Name = "ToolStripSeparator1"
                ToolStripSeparator1.Size = New Size(241, 6)
                '
                'cmenTabScreenshot
                '
                cmenTabScreenshot.Image = My.Resources.Resources.Screenshot_Add
                cmenTabScreenshot.Name = "cmenTabScreenshot"
                cmenTabScreenshot.Size = New Size(244, 26)
                cmenTabScreenshot.Text = "Screenshot"
                '
                'cmenTabStartChat
                '
                cmenTabStartChat.Image = My.Resources.Resources.Chat
                cmenTabStartChat.Name = "cmenTabStartChat"
                cmenTabStartChat.Size = New Size(244, 26)
                cmenTabStartChat.Text = "Start Chat (VNC)"
                cmenTabStartChat.Visible = False
                '
                'cmenTabTransferFile
                '
                cmenTabTransferFile.Image = My.Resources.Resources.SSHTransfer
                cmenTabTransferFile.Name = "cmenTabTransferFile"
                cmenTabTransferFile.Size = New Size(244, 26)
                cmenTabTransferFile.Text = "Transfer File (SSH)"
                '
                'cmenTabRefreshScreen
                '
                cmenTabRefreshScreen.Image = My.Resources.Resources.Refresh
                cmenTabRefreshScreen.Name = "cmenTabRefreshScreen"
                cmenTabRefreshScreen.Size = New Size(244, 26)
                cmenTabRefreshScreen.Text = "Refresh Screen (VNC)"
                '
                'cmenTabSendSpecialKeys
                '
                cmenTabSendSpecialKeys.DropDownItems.AddRange(New ToolStripItem() _
                                                                    {cmenTabSendSpecialKeysCtrlAltDel,
                                                                     cmenTabSendSpecialKeysCtrlEsc})
                cmenTabSendSpecialKeys.Image = My.Resources.Resources.Keyboard
                cmenTabSendSpecialKeys.Name = "cmenTabSendSpecialKeys"
                cmenTabSendSpecialKeys.Size = New Size(244, 26)
                cmenTabSendSpecialKeys.Text = "Send special Keys (VNC)"
                '
                'cmenTabSendSpecialKeysCtrlAltDel
                '
                cmenTabSendSpecialKeysCtrlAltDel.Name = "cmenTabSendSpecialKeysCtrlAltDel"
                cmenTabSendSpecialKeysCtrlAltDel.Size = New Size(169, 26)
                cmenTabSendSpecialKeysCtrlAltDel.Text = "Ctrl+Alt+Del"
                '
                'cmenTabSendSpecialKeysCtrlEsc
                '
                cmenTabSendSpecialKeysCtrlEsc.Name = "cmenTabSendSpecialKeysCtrlEsc"
                cmenTabSendSpecialKeysCtrlEsc.Size = New Size(169, 26)
                cmenTabSendSpecialKeysCtrlEsc.Text = "Ctrl+Esc"
                '
                'cmenTabPuttySettings
                '
                cmenTabPuttySettings.Name = "cmenTabPuttySettings"
                cmenTabPuttySettings.Size = New Size(244, 26)
                cmenTabPuttySettings.Text = "PuTTY Settings"
                '
                'cmenTabExternalApps
                '
                cmenTabExternalApps.Image = CType(resources.GetObject("cmenTabExternalApps.Image"), Image)
                cmenTabExternalApps.Name = "cmenTabExternalApps"
                cmenTabExternalApps.Size = New Size(244, 26)
                cmenTabExternalApps.Text = "External Applications"
                '
                'cmenTabSep1
                '
                cmenTabSep1.Name = "cmenTabSep1"
                cmenTabSep1.Size = New Size(241, 6)
                '
                'cmenTabRenameTab
                '
                cmenTabRenameTab.Image = My.Resources.Resources.Rename
                cmenTabRenameTab.Name = "cmenTabRenameTab"
                cmenTabRenameTab.Size = New Size(244, 26)
                cmenTabRenameTab.Text = "Rename Tab"
                '
                'cmenTabDuplicateTab
                '
                cmenTabDuplicateTab.Name = "cmenTabDuplicateTab"
                cmenTabDuplicateTab.Size = New Size(244, 26)
                cmenTabDuplicateTab.Text = "Duplicate Tab"
                '
                'cmenTabReconnect
                '
                cmenTabReconnect.Image = CType(resources.GetObject("cmenTabReconnect.Image"), Image)
                cmenTabReconnect.Name = "cmenTabReconnect"
                cmenTabReconnect.Size = New Size(244, 26)
                cmenTabReconnect.Text = "Reconnect"
                '
                'cmenTabDisconnect
                '
                cmenTabDisconnect.Image = My.Resources.Resources.Pause1
                cmenTabDisconnect.Name = "cmenTabDisconnect"
                cmenTabDisconnect.Size = New Size(244, 26)
                cmenTabDisconnect.Text = "Disconnect"
                '
                'Connection
                '
                ClientSize = New Size(632, 453)
                Controls.Add(TabController)
                Font = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))
                Icon = My.Resources.Resources.mRemote_Icon
                Name = "Connection"
                TabText = "UI.Window.Connection"
                Text = "UI.Window.Connection"
                cmenTab.ResumeLayout(False)
                ResumeLayout(False)
            End Sub

#End Region

#Region "Public Methods"

            Public Sub New(Panel As DockContent, Optional ByVal FormText As String = "")

                If FormText = "" Then
                    FormText = Language.Language.strNewPanel
                End If

                WindowType = Type.Connection
                DockPnl = Panel
                InitializeComponent()
                Text = FormText
                TabText = FormText
            End Sub

            Public Function AddConnectionTab(conI As Info) As TabPage
                Try
                    Dim nTab As New TabPage
                    nTab.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

                    If MySettingsProperty.Settings.ShowProtocolOnTabs Then
                        nTab.Title = conI.Protocol.ToString & ": "
                    Else
                        nTab.Title = ""
                    End If

                    nTab.Title &= conI.Name

                    If MySettingsProperty.Settings.ShowLogonInfoOnTabs Then
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

                    nTab.Title = nTab.Title.Replace("&", "&&")

                    Dim conIcon As Drawing.Icon = mRemote3G.Connection.Icon.FromString(conI.Icon)
                    If conIcon IsNot Nothing Then
                        nTab.Icon = conIcon
                    End If

                    If MySettingsProperty.Settings.OpenTabsRightOfSelected Then
                        TabController.TabPages.Insert(TabController.SelectedIndex + 1, nTab)
                    Else
                        TabController.TabPages.Add(nTab)
                    End If

                    nTab.Selected = True
                    _ignoreChangeSelectedTabClick = False

                    Return nTab
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "AddConnectionTab (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try

                Return Nothing
            End Function

#End Region

#Region "Private Methods"

            Public Sub UpdateSelectedConnection()
                If TabController.SelectedTab Is Nothing Then
                    frmMain.SelectedConnection = Nothing
                Else
                    Dim interfaceControl = TryCast(TabController.SelectedTab.Tag, InterfaceControl)
                    If interfaceControl Is Nothing Then
                        frmMain.SelectedConnection = Nothing
                    Else
                        frmMain.SelectedConnection = interfaceControl.Info
                    End If
                End If
            End Sub

#End Region

#Region "Form"

            Private Sub Connection_Load(sender As Object, e As EventArgs) Handles Me.Load
                ApplyLanguage()
            End Sub

            Private _documentHandlersAdded As Boolean = False
            Private _floatHandlersAdded As Boolean = False

            Private Sub Connection_DockStateChanged(sender As Object, e As EventArgs) Handles Me.DockStateChanged
                If DockState = DockState.Float Then
                    If _documentHandlersAdded Then
                        RemoveHandler frmMain.ResizeBegin, AddressOf Connection_ResizeBegin
                        RemoveHandler frmMain.ResizeEnd, AddressOf Connection_ResizeEnd
                        _documentHandlersAdded = False
                    End If
                    AddHandler DockHandler.FloatPane.FloatWindow.ResizeBegin, AddressOf Connection_ResizeBegin
                    AddHandler DockHandler.FloatPane.FloatWindow.ResizeEnd, AddressOf Connection_ResizeEnd
                    _floatHandlersAdded = True
                ElseIf DockState = DockState.Document Then
                    If _floatHandlersAdded Then
                        RemoveHandler DockHandler.FloatPane.FloatWindow.ResizeBegin, AddressOf Connection_ResizeBegin
                        RemoveHandler DockHandler.FloatPane.FloatWindow.ResizeEnd, AddressOf Connection_ResizeEnd
                        _floatHandlersAdded = False
                    End If
                    AddHandler frmMain.ResizeBegin, AddressOf Connection_ResizeBegin
                    AddHandler frmMain.ResizeEnd, AddressOf Connection_ResizeEnd
                    _documentHandlersAdded = True
                End If
            End Sub

            Private Sub ApplyLanguage()
                cmenTabFullscreen.Text = Language.Language.strMenuFullScreenRDP
                cmenTabSmartSize.Text = Language.Language.strMenuSmartSize
                cmenTabViewOnly.Text = Language.Language.strMenuViewOnly
                cmenTabScreenshot.Text = Language.Language.strMenuScreenshot
                cmenTabStartChat.Text = Language.Language.strMenuStartChat
                cmenTabTransferFile.Text = Language.Language.strMenuTransferFile
                cmenTabRefreshScreen.Text = Language.Language.strMenuRefreshScreen
                cmenTabSendSpecialKeys.Text = Language.Language.strMenuSendSpecialKeys
                cmenTabSendSpecialKeysCtrlAltDel.Text = Language.Language.strMenuCtrlAltDel
                cmenTabSendSpecialKeysCtrlEsc.Text = Language.Language.strMenuCtrlEsc
                cmenTabExternalApps.Text = Language.Language.strMenuExternalTools
                cmenTabRenameTab.Text = Language.Language.strMenuRenameTab
                cmenTabDuplicateTab.Text = Language.Language.strMenuDuplicateTab
                cmenTabReconnect.Text = Language.Language.strMenuReconnect
                cmenTabDisconnect.Text = Language.Language.strMenuDisconnect
                cmenTabPuttySettings.Text = Language.Language.strPuttySettings
            End Sub

            Private Sub Connection_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
                If Not frmMain.IsClosing And (
                        (MySettingsProperty.Settings.ConfirmCloseConnection = ConfirmClose.All And TabController.TabPages.Count > 0) Or
                        (MySettingsProperty.Settings.ConfirmCloseConnection = ConfirmClose.Multiple And TabController.TabPages.Count > 1)) Then
                    Dim result As DialogResult = cTaskDialog.MessageBox(Me, Application.Info.ProductName, String.Format(Language.Language.strConfirmCloseConnectionPanelMainInstruction, Text), "", "", "", Language.Language.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.YesNo, eSysIcons.Question, Nothing)
                    If cTaskDialog.VerificationChecked Then
                        MySettingsProperty.Settings.ConfirmCloseConnection =
                            MySettingsProperty.Settings.ConfirmCloseConnection - 1
                    End If
                    If result = DialogResult.No Then
                        e.Cancel = True
                        Exit Sub
                    End If
                End If

                Try
                    For Each tabP As TabPage In TabController.TabPages
                        If tabP.Tag IsNot Nothing Then
                            Dim interfaceControl As InterfaceControl = tabP.Tag
                            interfaceControl.Protocol.Close()
                        End If
                    Next
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "UI.Window.Connection.Connection_FormClosing() failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Public Shadows Event ResizeBegin As EventHandler

            Private Sub Connection_ResizeBegin(sender As Object, e As EventArgs)
                RaiseEvent ResizeBegin(Me, e)
            End Sub

            Public Shadows Event ResizeEnd As EventHandler

            Public Sub Connection_ResizeEnd(sender As Object, e As EventArgs)
                RaiseEvent ResizeEnd(sender, e)
            End Sub

#End Region

#Region "TabController"

            Private Sub TabController_ClosePressed(sender As Object, e As EventArgs) Handles TabController.ClosePressed
                If TabController.SelectedTab Is Nothing Then
                    Exit Sub
                End If

                CloseConnectionTab()
            End Sub

            Private Sub CloseConnectionTab()
                Dim selectedTab As TabPage = TabController.SelectedTab
                If MySettingsProperty.Settings.ConfirmCloseConnection = ConfirmClose.All Then
                    Dim result As DialogResult = cTaskDialog.MessageBox(Me, Application.Info.ProductName, String.Format(Language.Language.strConfirmCloseConnectionMainInstruction, selectedTab.Title), "", "", "", Language.Language.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.YesNo, eSysIcons.Question, Nothing)
                    If cTaskDialog.VerificationChecked Then
                        MySettingsProperty.Settings.ConfirmCloseConnection =
                            MySettingsProperty.Settings.ConfirmCloseConnection - 1
                    End If
                    If result = DialogResult.No Then
                        Exit Sub
                    End If
                End If

                Try
                    If selectedTab.Tag IsNot Nothing Then
                        Dim interfaceControl As InterfaceControl = selectedTab.Tag
                        interfaceControl.Protocol.Close()
                    Else
                        CloseTab(selectedTab)
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "UI.Window.Connection.CloseConnectionTab() failed" & vbNewLine & ex.ToString(), True)
                End Try

                UpdateSelectedConnection()
            End Sub

            Private Sub TabController_DoubleClickTab(sender As TabControl, page As TabPage) _
                Handles TabController.DoubleClickTab
                _firstClickTicks = 0
                If MySettingsProperty.Settings.DoubleClickOnTabClosesIt Then
                    CloseConnectionTab()
                End If
            End Sub

#Region "Drag and Drop"

            Private Sub TabController_DragDrop(sender As Object, e As DragEventArgs) Handles TabController.DragDrop
                If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) Then
                    Runtime.OpenConnection(e.Data.GetData("System.Windows.Forms.TreeNode", True).Tag, Me,
                                           Info.Force.DoNotJump)
                End If
            End Sub

            Private Sub TabController_DragEnter(sender As Object, e As DragEventArgs) Handles TabController.DragEnter
                If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) Then
                    e.Effect = DragDropEffects.Move
                End If
            End Sub

            Private Sub TabController_DragOver(sender As Object, e As DragEventArgs) Handles TabController.DragOver
                e.Effect = DragDropEffects.Move
            End Sub

#End Region

#End Region

#Region "Tab Menu"

            Private Sub ShowHideMenuButtons()
                Try
                    If TabController.SelectedTab Is Nothing Then
                        Exit Sub
                    End If

                    Dim IC As InterfaceControl = TabController.SelectedTab.Tag

                    If IC Is Nothing Then
                        Exit Sub
                    End If

                    If IC.Info.Protocol = Protocols.RDP Then
                        Dim rdp As RDP = IC.Protocol

                        cmenTabFullscreen.Visible = True
                        cmenTabFullscreen.Checked = rdp.Fullscreen

                        cmenTabSmartSize.Visible = True
                        cmenTabSmartSize.Checked = rdp.SmartSize

                        ToolStripSeparator1.Visible = True
                    Else
                        cmenTabFullscreen.Visible = False
                        cmenTabSmartSize.Visible = False
                        ToolStripSeparator1.Visible = False
                    End If

                    If IC.Info.Protocol = Protocols.VNC Then
                        cmenTabSendSpecialKeys.Visible = True
                        cmenTabViewOnly.Visible = True

                        cmenTabSmartSize.Visible = True
                        cmenTabStartChat.Visible = True
                        cmenTabRefreshScreen.Visible = True
                        cmenTabTransferFile.Visible = False

                        Dim vnc As VNC = IC.Protocol
                        cmenTabSmartSize.Checked = vnc.SmartSize
                        cmenTabViewOnly.Checked = vnc.ViewOnly
                    Else
                        cmenTabSendSpecialKeys.Visible = False
                        cmenTabViewOnly.Visible = False
                        cmenTabStartChat.Visible = False
                        cmenTabRefreshScreen.Visible = False
                        cmenTabTransferFile.Visible = False
                    End If

                    If IC.Info.Protocol = Protocols.SSH1 Or IC.Info.Protocol = Protocols.SSH2 Then
                        cmenTabTransferFile.Visible = True
                    End If

                    If TypeOf IC.Protocol Is PuttyBase Then
                        cmenTabPuttySettings.Visible = True
                    Else
                        cmenTabPuttySettings.Visible = False
                    End If

                    AddExternalApps()
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "ShowHideMenuButtons (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub cmenTabScreenshot_Click(sender As Object, e As EventArgs) Handles cmenTabScreenshot.Click
                cmenTab.Close()
                Application.DoEvents()
                Runtime.Windows.screenshotForm.AddScreenshot(Misc.TakeScreenshot(Me))
            End Sub

            Private Sub cmenTabSmartSize_Click(sender As Object, e As EventArgs) Handles cmenTabSmartSize.Click
                ToggleSmartSize()
            End Sub

            Private Sub cmenTabReconnect_Click(sender As Object, e As EventArgs) Handles cmenTabReconnect.Click
                Reconnect()
            End Sub

            Private Sub cmenTabTransferFile_Click(sender As Object, e As EventArgs) Handles cmenTabTransferFile.Click
                TransferFile()
            End Sub

            Private Sub cmenTabViewOnly_Click(sender As Object, e As EventArgs) Handles cmenTabViewOnly.Click
                ToggleViewOnly()
            End Sub

            Private Sub cmenTabStartChat_Click(sender As Object, e As EventArgs) Handles cmenTabStartChat.Click
                StartChat()
            End Sub

            Private Sub cmenTabRefreshScreen_Click(sender As Object, e As EventArgs) Handles cmenTabRefreshScreen.Click
                RefreshScreen()
            End Sub

            Private Sub cmenTabSendSpecialKeysCtrlAltDel_Click(sender As Object, e As EventArgs) _
                Handles cmenTabSendSpecialKeysCtrlAltDel.Click
                SendSpecialKeys(VNC.SpecialKeys.CtrlAltDel)
            End Sub

            Private Sub cmenTabSendSpecialKeysCtrlEsc_Click(sender As Object, e As EventArgs) _
                Handles cmenTabSendSpecialKeysCtrlEsc.Click
                SendSpecialKeys(VNC.SpecialKeys.CtrlEsc)
            End Sub

            Private Sub cmenTabFullscreen_Click(sender As Object, e As EventArgs) Handles cmenTabFullscreen.Click
                ToggleFullscreen()
            End Sub

            Private Sub cmenTabPuttySettings_Click(sender As Object, e As EventArgs) Handles cmenTabPuttySettings.Click
                ShowPuttySettingsDialog()
            End Sub

            Private Sub cmenTabExternalAppsEntry_Click(sender As Object, e As EventArgs)
                StartExternalApp(sender.Tag)
            End Sub

            Private Sub cmenTabDisconnect_Click(sender As Object, e As EventArgs) Handles cmenTabDisconnect.Click
                CloseTabMenu()
            End Sub

            Private Sub cmenTabDuplicateTab_Click(sender As Object, e As EventArgs) Handles cmenTabDuplicateTab.Click
                DuplicateTab()
            End Sub

            Private Sub cmenTabRenameTab_Click(sender As Object, e As EventArgs) Handles cmenTabRenameTab.Click
                RenameTab()
            End Sub

#End Region

#Region "Tab Actions"

            Private Sub ToggleSmartSize()
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim IC As InterfaceControl = TabController.SelectedTab.Tag

                            If TypeOf IC.Protocol Is RDP Then
                                Dim rdp As RDP = IC.Protocol
                                rdp.ToggleSmartSize()
                            ElseIf TypeOf IC.Protocol Is VNC Then
                                Dim vnc As VNC = IC.Protocol
                                vnc.ToggleSmartSize()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "ToggleSmartSize (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub TransferFile()
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim IC As InterfaceControl = TabController.SelectedTab.Tag

                            If IC.Info.Protocol = Protocols.SSH1 Or IC.Info.Protocol = Protocols.SSH2 Then
                                SSHTransferFile()
                            ElseIf IC.Info.Protocol = Protocols.VNC Then
                                VNCTransferFile()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "TransferFile (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub SSHTransferFile()
                Try

                    Dim IC As InterfaceControl = TabController.SelectedTab.Tag

                    Runtime.Windows.Show(Type.SSHTransfer)

                    Dim conI As Info = IC.Info

                    Runtime.Windows.sshtransferForm.Hostname = conI.Hostname
                    Runtime.Windows.sshtransferForm.Username = conI.Username
                    Runtime.Windows.sshtransferForm.Password = conI.Password
                    Runtime.Windows.sshtransferForm.Port = conI.Port
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "SSHTransferFile (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub VNCTransferFile()
                Try
                    Dim IC As InterfaceControl = TabController.SelectedTab.Tag
                    Dim vnc As VNC = IC.Protocol
                    vnc.StartFileTransfer()
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "VNCTransferFile (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub ToggleViewOnly()
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim IC As InterfaceControl = TabController.SelectedTab.Tag

                            If TypeOf IC.Protocol Is VNC Then
                                cmenTabViewOnly.Checked = Not cmenTabViewOnly.Checked

                                Dim vnc As VNC = IC.Protocol
                                vnc.ToggleViewOnly()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "ToggleViewOnly (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub StartChat()
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim IC As InterfaceControl = TabController.SelectedTab.Tag

                            If TypeOf IC.Protocol Is VNC Then
                                Dim vnc As VNC = IC.Protocol
                                vnc.StartChat()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "StartChat (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub RefreshScreen()
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim IC As InterfaceControl = TabController.SelectedTab.Tag

                            If TypeOf IC.Protocol Is VNC Then
                                Dim vnc As VNC = IC.Protocol
                                vnc.RefreshScreen()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "RefreshScreen (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub SendSpecialKeys(Keys As VNC.SpecialKeys)
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim IC As InterfaceControl = TabController.SelectedTab.Tag

                            If TypeOf IC.Protocol Is VNC Then
                                Dim vnc As VNC = IC.Protocol
                                vnc.SendSpecialKeys(Keys)
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "SendSpecialKeys (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub ToggleFullscreen()
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim IC As InterfaceControl = TabController.SelectedTab.Tag

                            If TypeOf IC.Protocol Is RDP Then
                                Dim rdp As RDP = IC.Protocol
                                rdp.ToggleFullscreen()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "ToggleFullscreen (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub ShowPuttySettingsDialog()
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim objInterfaceControl As InterfaceControl = TabController.SelectedTab.Tag

                            If TypeOf objInterfaceControl.Protocol Is PuttyBase Then
                                Dim objPuttyBase As PuttyBase = objInterfaceControl.Protocol

                                objPuttyBase.ShowSettingsDialog()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "ShowPuttySettingsDialog (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub AddExternalApps()
                Try
                    'clean up
                    'since new items are added below, we have to dispose of any previous items first
                    If cmenTabExternalApps.DropDownItems.Count > 0 Then
                        For Each mitem As ToolStripMenuItem In cmenTabExternalApps.DropDownItems
                            mitem.Dispose()
                        Next mitem
                        cmenTabExternalApps.DropDownItems.Clear()
                    End If


                    'add ext apps
                    For Each extA As ExternalTool In Runtime.ExternalTools
                        Dim nItem As New ToolStripMenuItem
                        nItem.Text = extA.DisplayName
                        nItem.Tag = extA

                        nItem.Image = extA.Image

                        AddHandler nItem.Click, AddressOf cmenTabExternalAppsEntry_Click

                        cmenTabExternalApps.DropDownItems.Add(nItem)
                    Next
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "cMenTreeTools_DropDownOpening failed (UI.Window.Tree)" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub StartExternalApp(ExtA As ExternalTool)
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim IC As InterfaceControl = TabController.SelectedTab.Tag

                            ExtA.Start(IC.Info)
                        End If
                    End If

                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "cmenTabExternalAppsEntry_Click failed (UI.Window.Tree)" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub CloseTabMenu()
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim IC As InterfaceControl = TabController.SelectedTab.Tag

                            IC.Protocol.Close()
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "CloseTabMenu (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub DuplicateTab()
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim IC As InterfaceControl = TabController.SelectedTab.Tag

                            Runtime.OpenConnection(IC.Info, Info.Force.DoNotJump)
                            _ignoreChangeSelectedTabClick = False
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "DuplicateTab (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub Reconnect()
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim IC As InterfaceControl = TabController.SelectedTab.Tag
                            Dim conI As Info = IC.Info

                            IC.Protocol.Close()

                            Runtime.OpenConnection(conI, Info.Force.DoNotJump)
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Reconnect (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub RenameTab()
                Try
                    Dim nTitle As String = InputBox(Language.Language.strNewTitle & ":", ,
                                                    TabController.SelectedTab.Title.Replace("&&", "&"))

                    If nTitle <> "" Then
                        TabController.SelectedTab.Title = nTitle.Replace("&", "&&")
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "RenameTab (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

#End Region

#Region "Protocols"

            Public Sub Prot_Event_Closed(sender As Object)
                Dim Prot As Protocol.Base = sender
                CloseTab(Prot.InterfaceControl.Parent)
            End Sub

#End Region

#Region "Tabs"

            Private Delegate Sub CloseTabCB(TabToBeClosed As TabPage)

            Private Sub CloseTab(TabToBeClosed As TabPage)
                If TabController.InvokeRequired Then
                    Dim s As New CloseTabCB(AddressOf CloseTab)

                    Try
                        TabController.Invoke(s, TabToBeClosed)
                    Catch comEx As COMException
                        TabController.Invoke(s, TabToBeClosed)
                    Catch ex As Exception
                        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Couldn't close tab" & vbNewLine & ex.ToString(), True)
                    End Try
                Else
                    Try
                        TabController.TabPages.Remove(TabToBeClosed)
                        _ignoreChangeSelectedTabClick = False
                    Catch comEx As COMException
                        CloseTab(TabToBeClosed)
                    Catch ex As Exception
                        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Couldn't close tab" & vbNewLine & ex.ToString(), True)
                    End Try

                    If TabController.TabPages.Count = 0 Then
                        Close()
                    End If
                End If
            End Sub

            Private _ignoreChangeSelectedTabClick As Boolean = False

            Private Sub TabController_SelectionChanged(sender As Object, e As EventArgs) _
                Handles TabController.SelectionChanged
                _ignoreChangeSelectedTabClick = True
                UpdateSelectedConnection()
                FocusIC()
                RefreshIC()
            End Sub

            Private _firstClickTicks As Integer = 0
            Private _doubleClickRectangle As Rectangle

            Private Sub TabController_MouseUp(sender As Object, e As MouseEventArgs) Handles TabController.MouseUp
                Try
                    If Not NativeMethods.GetForegroundWindow() = frmMain.Handle And Not _ignoreChangeSelectedTabClick _
                        Then
                        Dim clickedTab As TabPage = TabController.TabPageFromPoint(e.Location)
                        If clickedTab IsNot Nothing And TabController.SelectedTab IsNot clickedTab Then
                            NativeMethods.SetForegroundWindow(Handle)
                            TabController.SelectedTab = clickedTab
                        End If
                    End If
                    _ignoreChangeSelectedTabClick = False

                    Select Case e.Button
                        Case MouseButtons.Left
                            Dim currentTicks As Integer = Environment.TickCount
                            Dim elapsedTicks As Integer = currentTicks - _firstClickTicks
                            If _
                                elapsedTicks > SystemInformation.DoubleClickTime Or
                                Not _doubleClickRectangle.Contains(MousePosition) Then
                                _firstClickTicks = currentTicks
                                _doubleClickRectangle =
                                    New Rectangle(MousePosition.X - (SystemInformation.DoubleClickSize.Width / 2),
                                                  MousePosition.Y - (SystemInformation.DoubleClickSize.Height / 2),
                                                  SystemInformation.DoubleClickSize.Width,
                                                  SystemInformation.DoubleClickSize.Height)
                                FocusIC()
                            Else
                                TabController.OnDoubleClickTab(TabController.SelectedTab)
                            End If
                        Case MouseButtons.Middle
                            CloseConnectionTab()
                        Case MouseButtons.Right
                            ShowHideMenuButtons()
                            NativeMethods.SetForegroundWindow(Handle)
                            cmenTab.Show(TabController, e.Location)
                    End Select
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "TabController_MouseUp (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub FocusIC()
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim IC As InterfaceControl = TabController.SelectedTab.Tag
                            IC.Protocol.Focus()
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "FocusIC (UI.Window.Connections) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Public Sub RefreshIC()
                Try
                    If TabController.SelectedTab IsNot Nothing Then
                        If TypeOf TabController.SelectedTab.Tag Is InterfaceControl Then
                            Dim IC As InterfaceControl = TabController.SelectedTab.Tag

                            If IC.Info.Protocol = Protocols.VNC Then
                                TryCast(IC.Protocol, VNC).RefreshScreen()
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "RefreshIC (UI.Window.Connection) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

#End Region

#Region "Window Overrides"

            Protected Overloads Overrides Sub WndProc(ByRef m As Windows.Forms.Message)
                Try
                    If m.Msg = NativeMethods.WM_MOUSEACTIVATE Then
                        Dim selectedTab As TabPage = TabController.SelectedTab
                        If selectedTab IsNot Nothing Then
                            Dim tabClientRectangle As Rectangle =
                                    selectedTab.RectangleToScreen(selectedTab.ClientRectangle)
                            If tabClientRectangle.Contains(MousePosition) Then
                                Dim interfaceControl = TryCast(TabController.SelectedTab.Tag, InterfaceControl)
                                If interfaceControl IsNot Nothing AndAlso interfaceControl.Info IsNot Nothing Then
                                    If interfaceControl.Info.Protocol = Protocols.RDP Then
                                        interfaceControl.Protocol.Focus()
                                        Return ' Do not pass to base class
                                    End If
                                End If
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage("UI.Window.Connection.WndProc() failed.", ex, , True)
                End Try

                MyBase.WndProc(m)
            End Sub

#End Region

#Region "Tab drag and drop"

            Public Property InTabDrag As Boolean = False

            Private Sub TabController_PageDragStart(sender As Object, e As MouseEventArgs) _
                Handles TabController.PageDragEnd, TabController.PageDragStart
                Cursor = Cursors.SizeWE
            End Sub

            Private Sub TabController_PageDragMove(sender As Object, e As MouseEventArgs) _
                Handles TabController.PageDragMove
                InTabDrag = True _
                ' For some reason PageDragStart gets raised again after PageDragEnd so set this here instead

                Dim sourceTab As TabPage = TabController.SelectedTab
                Dim destinationTab As TabPage = TabController.TabPageFromPoint(e.Location)

                If Not TabController.TabPages.Contains(destinationTab) Or sourceTab Is destinationTab Then Return

                Dim targetIndex As Integer = TabController.TabPages.IndexOf(destinationTab)

                TabController.TabPages.SuspendEvents()
                TabController.TabPages.Remove(sourceTab)
                TabController.TabPages.Insert(targetIndex, sourceTab)
                TabController.SelectedTab = sourceTab
                TabController.TabPages.ResumeEvents()
            End Sub

            Private Sub TabController_PageDragEnd(sender As Object, e As MouseEventArgs) _
                Handles TabController.PageDragEnd, TabController.PageDragQuit
                Cursor = Cursors.Default
                InTabDrag = False
                Dim interfaceControl = TryCast(TabController.SelectedTab.Tag, InterfaceControl)
                If interfaceControl IsNot Nothing Then interfaceControl.Protocol.Focus()
            End Sub

#End Region
        End Class
    End Namespace

End Namespace