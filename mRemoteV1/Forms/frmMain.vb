Imports mRemote.App.Runtime
Imports System.Reflection
Imports Crownwood
Imports mRemote.App.Native

Public Class frmMain
    Public prevWindowsState As FormWindowState

#Region "Startup & Shutdown"
    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'insert enable edition code here
        App.Editions.Spanlink.Enabled = False

        ApplyLanguage()

        Debug.Print("---------------------------" & vbNewLine & "[START] - " & Now)

        Startup.ParseCommandLineArgs()


        sL = New Config.Settings.Load(Me)
        sS = New Config.Settings.Save(Me)

        Startup.CreateLogger()

        sL.Load()

        mC = New Messages.Collector(Windows.errorsForm)

        Connection.Protocol.RDP.Resolutions.AddResolutions()
        Connection.Protocol.PuttyBase.BorderSize = New Size(SystemInformation.FrameBorderSize.Width, SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height) 'Size.Subtract(Me.Size, Me.ClientSize)

        wL = New UI.Window.List

        Startup.GetConnectionIcons()
        Startup.GetPuttySessions()
        App.Runtime.GetExtApps()
        Windows.treePanel.Focus()

        Tree.Node.TreeView = Windows.treeForm.tvConnections

        'insert new edition code here
        Dim edSpanlink As New App.Editions.Spanlink

        'LoadCredentials()
        LoadConnections()

        If My.Settings.StartupComponentsCheck Then
            Windows.Show(UI.Window.Type.ComponentsCheck)
        End If

        If Not My.Settings.CheckForUpdatesAsked Then
            Windows.Show(UI.Window.Type.Options)
            Windows.optionsForm.ShowUpdatesTab()
            My.Settings.CheckForUpdatesAsked = True
        End If

        Startup.UpdateCheck()
        Startup.AnnouncementCheck()
        Startup.CreateSQLUpdateHandlerAndStartTimer()

        AddSysMenuItems()
        AddHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanged, AddressOf DisplayChanged
    End Sub

    Private Sub ApplyLanguage()
        mMenFile.Text = Language.Base.Menu_File
        mMenFileNew.Text = Language.Base.Menu_NewConnections
        mMenFileLoad.Text = Language.Base.Menu_OpenConnections
        mMenFileSave.Text = Language.Base.Menu_SaveConnections
        mMenFileSaveAs.Text = Language.Base.Menu_SaveConnectionsAs
        mMenFileExit.Text = Language.Base.Menu_Exit
        mMenView.Text = Language.Base.Menu_View
        mMenViewAddConnectionPanel.Text = Language.Base.Menu_AddConnectionPanel
        mMenViewConnectionPanels.Text = Language.Base.Menu_ConnectionPanels
        mMenViewConnections.Text = Language.Base.Menu_Connections
        mMenViewConfig.Text = Language.Base.Menu_Config
        mMenViewSessions.Text = Language.Base.Menu_Sessions
        mMenViewErrorsAndInfos.Text = Language.Base.Menu_ErrorsAndInfos
        mMenViewScreenshotManager.Text = Language.Base.Menu_ScreenshotManager
        mMenViewJumpTo.Text = Language.Base.Menu_JumpTo
        mMenViewJumpToConnectionsConfig.Text = Language.Base.Menu_ConnectionsConfig
        mMenViewJumpToSessionsScreenshots.Text = Language.Base.Menu_SessionsScreenshots
        mMenViewJumpToErrorsInfos.Text = Language.Base.Menu_ErrorsInfos
        mMenViewQuickConnectToolbar.Text = Language.Base.Menu_QuickyToolbar
        mMenViewExtAppsToolbar.Text = Language.Base.Menu_ExternalApplicationsToolbar
        mMenViewFullscreen.Text = Language.Base.Menu_FullScreenKioskMode
        mMenTools.Text = Language.Base.Menu_Tools
        mMenToolsSSHTransfer.Text = Language.Base.Menu_SSHFileTransfer
        mMenToolsExternalApps.Text = Language.Base.Menu_ExternalApplications
        mMenToolsPortScan.Text = Language.Base.Menu_PortScan
        mMenToolsUpdate.Text = Language.Base.Menu_Update
        mMenToolsOptions.Text = Language.Base.Menu_Options
        mMenInfo.Text = Language.Base.Menu_Info
        mMenInfoHelp.Text = Language.Base.Menu_Help
        mMenInfoForum.Text = Language.Base.Menu_Forum
        mMenInfoBugReport.Text = Language.Base.Menu_BugReport
        mMenInfoDonate.Text = Language.Base.Menu_Donate
        mMenInfoWebsite.Text = Language.Base.Menu_Website
        mMenInfoAbout.Text = Language.Base.Menu_About
        lblQuickConnect.Text = Language.Base.Toolbar_Quicky
        btnQuickyPlay.Text = Language.Base.Toolbar_Play
        mMenQuickyCon.Text = Language.Base.Menu_Connections
        cMenToolbarShowText.Text = Language.Base.Toolbar_ShowText
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If My.Settings.ConfirmExit And wL.Count > 0 Then
            If MsgBox(Language.Base.UnclosedConnectionPanels, MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.No Then
                e.Cancel = True
                Exit Sub
            End If
        End If

        For Each Window As UI.Window.Base In wL
            Window.Close()
        Next

        App.Runtime.Shutdown.BeforeQuit()

        Debug.Print("[END] - " & Now)
    End Sub
#End Region

    Private tmrRuns As Integer = 0
    Private Sub tmrShowUpdate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrShowUpdate.Tick
        If tmrRuns = 5 Then
            Me.tmrShowUpdate.Enabled = False
        End If

        If App.Runtime.IsUpdateAvailable Then
            App.Runtime.Windows.Show(UI.Window.Type.Update)
            Me.tmrShowUpdate.Enabled = False
        End If

        If App.Runtime.IsAnnouncementAvailable Then
            App.Runtime.Windows.Show(UI.Window.Type.Announcement)
            Me.tmrShowUpdate.Enabled = False
        End If

        tmrRuns += 1
    End Sub

    Private Sub tmrAutoSave_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrAutoSave.Tick
        mC.AddMessage(Messages.MessageClass.InformationMsg, "Doing AutoSave", True)
        App.Runtime.SaveConnections()
    End Sub


#Region "Ext Apps Toolbar"
    Private Sub cMenToolbarShowText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenToolbarShowText.Click
        SwitchToolbarText(Not cMenToolbarShowText.Checked)
    End Sub

    Public Sub AddExtAppsToToolbar()
        Try
            'clean up
            tsExtAppsToolbar.Items.Clear()

            'add ext apps
            For Each extA As Tools.ExternalApp In ExtApps
                Dim nItem As New ToolStripButton
                nItem.Text = extA.DisplayName
                nItem.Image = extA.Image

                If cMenToolbarShowText.Checked = True Then
                    nItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                Else
                    If nItem.Image IsNot Nothing Then
                        nItem.DisplayStyle = ToolStripItemDisplayStyle.Image
                    Else
                        nItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                    End If
                End If

                nItem.Tag = extA

                AddHandler nItem.Click, AddressOf tsExtAppEntry_Click

                tsExtAppsToolbar.Items.Add(nItem)
            Next
        Catch ex As Exception
            mC.AddMessage(Messages.MessageClass.ErrorMsg, "AddExtAppsToToolbar failed (frmMain)" & vbNewLine & ex.Message, True)
        End Try
    End Sub

    Private Sub tsExtAppEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim extA As Tools.ExternalApp = sender.Tag

        If Tree.Node.GetNodeType(Tree.Node.SelectedNode) = Tree.Node.Type.Connection Then
            extA.Start(Tree.Node.SelectedNode.Tag)
        Else
            extA.Start()
        End If
    End Sub

    Public Sub SwitchToolbarText(ByVal Show As Boolean)
        For Each tItem As ToolStripButton In tsExtAppsToolbar.Items
            If Show = True Then
                tItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
            Else
                If tItem.Image IsNot Nothing Then
                    tItem.DisplayStyle = ToolStripItemDisplayStyle.Image
                Else
                    tItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                End If
            End If
        Next

        cMenToolbarShowText.Checked = Show
    End Sub
#End Region

#Region "Menu"
#Region "File"
    Private Sub mMenFileNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenFileNew.Click
        NewConnections()
    End Sub

    Private Sub mMenFileLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenFileLoad.Click
        If App.Runtime.ConnectionsFileLoaded Then
            Select Case MsgBox(Language.Base.SaveConnectionsFileBeforeOpeningAnother, MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Question)
                Case MsgBoxResult.Yes
                    App.Runtime.SaveConnections()
                Case MsgBoxResult.Cancel
                    Exit Sub
            End Select
        End If

        LoadConnections(True)
    End Sub

    Private Sub mMenFileSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenFileSave.Click
        SaveConnections()
    End Sub

    Private Sub mMenFileSaveAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenFileSaveAs.Click
        App.Runtime.Windows.Show(UI.Window.Type.SaveAs)
    End Sub

    Private Sub mMenFileExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenFileExit.Click
        App.Runtime.Shutdown.Quit()
    End Sub
#End Region

#Region "View"
    Private Sub mMenView_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles mMenView.DropDownOpening
        Me.mMenViewConnections.Checked = Not Windows.treeForm.IsHidden
        Me.mMenViewConfig.Checked = Not Windows.configForm.IsHidden
        Me.mMenViewErrorsAndInfos.Checked = Not Windows.errorsForm.IsHidden
        Me.mMenViewSessions.Checked = Not Windows.sessionsForm.IsHidden
        Me.mMenViewScreenshotManager.Checked = Not Windows.screenshotForm.IsHidden

        Me.mMenViewExtAppsToolbar.Checked = tsExtAppsToolbar.Visible
        Me.mMenViewQuickConnectToolbar.Checked = tsQuickConnect.Visible

        Me.mMenViewConnectionPanels.DropDownItems.Clear()

        For i As Integer = 0 To wL.Count - 1
            Dim tItem As New ToolStripMenuItem(wL(i).Text, wL(i).Icon.ToBitmap, AddressOf ConnectionPanelMenuItem_Click)
            tItem.Tag = wL(i)

            Me.mMenViewConnectionPanels.DropDownItems.Add(tItem)
        Next
    End Sub

    Private Sub ConnectionPanelMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        TryCast(sender.Tag, UI.Window.Base).Show(Me.pnlDock)
        TryCast(sender.Tag, UI.Window.Base).Focus()
    End Sub

    Private Sub mMenViewSessions_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mMenViewSessions.Click
        If Me.mMenViewSessions.Checked = False Then
            Windows.sessionsPanel.Show(Me.pnlDock)
            Me.mMenViewSessions.Checked = True
        Else
            Windows.sessionsPanel.Hide()
            Me.mMenViewSessions.Checked = False
        End If
    End Sub

    Private Sub mMenViewConnections_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewConnections.Click
        If Me.mMenViewConnections.Checked = False Then
            Windows.treePanel.Show(Me.pnlDock)
            Me.mMenViewConnections.Checked = True
        Else
            Windows.treePanel.Hide()
            Me.mMenViewConnections.Checked = False
        End If
    End Sub

    Private Sub mMenViewConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewConfig.Click
        If Me.mMenViewConfig.Checked = False Then
            Windows.configPanel.Show(Me.pnlDock)
            Me.mMenViewConfig.Checked = True
        Else
            Windows.configPanel.Hide()
            Me.mMenViewConfig.Checked = False
        End If
    End Sub

    Private Sub mMenViewErrorsAndInfos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewErrorsAndInfos.Click
        If Me.mMenViewErrorsAndInfos.Checked = False Then
            Windows.errorsPanel.Show(Me.pnlDock)
            Me.mMenViewErrorsAndInfos.Checked = True
        Else
            Windows.errorsPanel.Hide()
            Me.mMenViewErrorsAndInfos.Checked = False
        End If
    End Sub

    Private Sub mMenViewScreenshotManager_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewScreenshotManager.Click
        If Me.mMenViewScreenshotManager.Checked = False Then
            Windows.screenshotPanel.Show(Me.pnlDock)
            Me.mMenViewScreenshotManager.Checked = True
        Else
            Windows.screenshotPanel.Hide()
            Me.mMenViewScreenshotManager.Checked = False
        End If
    End Sub

    Private Sub mMenViewJumpToConnectionsConfig_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mMenViewJumpToConnectionsConfig.Click
        If pnlDock.ActiveContent Is Windows.treePanel Then
            Windows.configForm.Activate()
        Else
            Windows.treeForm.Activate()
        End If
    End Sub

    Private Sub mMenViewJumpToSessionsScreenshots_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mMenViewJumpToSessionsScreenshots.Click
        If pnlDock.ActiveContent Is Windows.sessionsPanel Then
            Windows.screenshotForm.Activate()
        Else
            Windows.sessionsForm.Activate()
        End If
    End Sub

    Private Sub mMenViewJumpToErrorsInfos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mMenViewJumpToErrorsInfos.Click
        Windows.errorsForm.Activate()
    End Sub

    Private Sub mMenViewAddConnectionPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewAddConnectionPanel.Click
        AddPanel()
    End Sub

    Private Sub mMenViewExtAppsToolbar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewExtAppsToolbar.Click
        If mMenViewExtAppsToolbar.Checked = False Then
            tsExtAppsToolbar.Visible = True
            mMenViewExtAppsToolbar.Checked = True
        Else
            tsExtAppsToolbar.Visible = False
            mMenViewExtAppsToolbar.Checked = False
        End If
    End Sub

    Private Sub mMenViewQuickConnectToolbar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewQuickConnectToolbar.Click
        If mMenViewQuickConnectToolbar.Checked = False Then
            tsQuickConnect.Visible = True
            mMenViewQuickConnectToolbar.Checked = True
        Else
            tsQuickConnect.Visible = False
            mMenViewQuickConnectToolbar.Checked = False
        End If
    End Sub

    Private Sub mMenViewFullscreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewFullscreen.Click
        If Tools.Misc.Fullscreen.FullscreenActive Then
            Tools.Misc.Fullscreen.ExitFullscreen()
            Me.mMenViewFullscreen.Checked = False
        Else
            Tools.Misc.Fullscreen.EnterFullscreen()
            Me.mMenViewFullscreen.Checked = True
        End If
    End Sub
#End Region

#Region "Tools"
    Private Sub mMenToolsUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenToolsUpdate.Click
        App.Runtime.Windows.Show(UI.Window.Type.Update)
    End Sub

    Private Sub mMenToolsSSHTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenToolsSSHTransfer.Click
        App.Runtime.Windows.Show(UI.Window.Type.SSHTransfer)
    End Sub

    Private Sub mMenToolsUVNCSC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenToolsUVNCSC.Click
        App.Runtime.Windows.Show(UI.Window.Type.UltraVNCSC)
    End Sub

    Private Sub mMenToolsExternalApps_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenToolsExternalApps.Click
        App.Runtime.Windows.Show(UI.Window.Type.ExternalApps)
    End Sub

    Private Sub mMenToolsPortScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenToolsPortScan.Click
        App.Runtime.Windows.Show(UI.Window.Type.PortScan, Tools.PortScan.PortScanMode.Normal)
    End Sub

    Private Sub mMenToolsComponentsCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenToolsComponentsCheck.Click
        App.Runtime.Windows.Show(UI.Window.Type.ComponentsCheck)
    End Sub

    Private Sub mMenToolsOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenToolsOptions.Click
        App.Runtime.Windows.Show(UI.Window.Type.Options)
    End Sub
#End Region

#Region "Quick Connect"
    Private Sub btnQuickyPlay_ButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQuickyPlay.ButtonClick
        CreateQuicky(QuickyText)
    End Sub

    Private Sub btnQuickyPlay_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQuickyPlay.DropDownOpening
        CreateQuickyButtons()
    End Sub

    Private Sub CreateQuickyButtons()
        Try
            btnQuickyPlay.DropDownItems.Clear()

            For Each fI As FieldInfo In GetType(mRemote.Connection.Protocol.Protocols).GetFields
                If fI.Name <> "value__" And fI.Name <> "NONE" And fI.Name <> "IntApp" Then
                    Dim nBtn As New ToolStripMenuItem
                    nBtn.Text = fI.Name
                    btnQuickyPlay.DropDownItems.Add(nBtn)
                    AddHandler nBtn.Click, AddressOf QuickyProtocolButton_Click
                End If
            Next
        Catch ex As Exception
            mC.AddMessage(Messages.MessageClass.ErrorMsg, "CreateButtons (frmMain) failed" & vbNewLine & ex.Message, True)
        End Try
    End Sub

    Private Sub QuickyProtocolButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim conI As Connection.Info = CreateQuicky(QuickyText, Tools.Misc.StringToEnum(GetType(mRemote.Connection.Protocol.Protocols), sender.Text))

            If conI.Port = 0 Then
                conI.SetDefaultPort()

                If mRemote.Connection.QuickConnect.History.Exists(conI.Hostname) = False Then
                    mRemote.Connection.QuickConnect.History.Add(conI.Hostname)
                End If
            Else
                If mRemote.Connection.QuickConnect.History.Exists(conI.Hostname) = False Then
                    mRemote.Connection.QuickConnect.History.Add(conI.Hostname & ":" & conI.Port)
                End If
            End If

            App.Runtime.OpenConnection(conI, mRemote.Connection.Info.Force.DoNotJump)
        Catch ex As Exception
            mC.AddMessage(Messages.MessageClass.ErrorMsg, "QuickyProtocolButton_Click (frmMain) failed" & vbNewLine & ex.Message, True)
        End Try
    End Sub

    Private Sub cmbQuickConnect_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbQuickConnect.KeyDown
        If e.KeyCode = Keys.Enter Then
            CreateQuicky(QuickyText)
        End If
    End Sub

    Private Sub lblQuickConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblQuickConnect.Click
        Me.cmbQuickConnect.Focus()
    End Sub

    Private Function QuickyText() As String
        Dim txt As String

        txt = cmbQuickConnect.Text

        If txt.StartsWith(" ") Or txt.EndsWith(" ") Then
            txt = txt.Replace(" ", "")
            cmbQuickConnect.Text = txt
        End If

        Return txt
    End Function
#End Region

#Region "Info"
    Private Sub mMenInfoHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoHelp.Click
        App.Runtime.Windows.Show(UI.Window.Type.Help)
    End Sub

    Private Sub mMenInfoForum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoForum.Click
        App.Runtime.GoToForum()
    End Sub

    Private Sub mMenInfoBugReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoBugReport.Click
        App.Runtime.GoToBugs()
    End Sub

    Private Sub mMenInfoWebsite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoWebsite.Click
        App.Runtime.GoToWebsite()
    End Sub

    Private Sub mMenInfoDonate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoDonate.Click
        App.Runtime.GoToDonate()
    End Sub

    Private Sub mMenInfoAnnouncements_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoAnnouncements.Click
        App.Runtime.Windows.Show(UI.Window.Type.Announcement)
    End Sub

    Private Sub mMenInfoAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoAbout.Click
        App.Runtime.Windows.Show(UI.Window.Type.About)
    End Sub
#End Region
#End Region

#Region "Connections DropDown"
    Private Sub mMenQuickyCon_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles mMenQuickyCon.DropDownOpening
        mMenQuickyCon.DropDownItems.Clear()

        For Each tNode As TreeNode In App.Runtime.Windows.treeForm.tvConnections.Nodes
            AddNodeToMenu(tNode.Nodes, mMenQuickyCon)
        Next
    End Sub

    Private Sub AddNodeToMenu(ByVal tnc As TreeNodeCollection, ByVal menToolStrip As ToolStripMenuItem)
        Try
            For Each tNode As TreeNode In tnc
                Dim tMenItem As New ToolStripMenuItem()
                tMenItem.Text = tNode.Text
                tMenItem.Tag = tNode

                If Tree.Node.GetNodeType(tNode) = Tree.Node.Type.Container Then
                    tMenItem.Image = My.Resources.Folder
                    tMenItem.Tag = tNode.Tag

                    menToolStrip.DropDownItems.Add(tMenItem)
                    AddNodeToMenu(tNode.Nodes, tMenItem)
                ElseIf Tree.Node.GetNodeType(tNode) = Tree.Node.Type.Connection Then
                    tMenItem.Image = Windows.treeForm.imgListTree.Images(tNode.ImageIndex)
                    tMenItem.Tag = tNode.Tag

                    menToolStrip.DropDownItems.Add(tMenItem)
                End If

                AddHandler tMenItem.MouseDown, AddressOf ConMenItem_MouseDown
            Next
        Catch ex As Exception
            mC.AddMessage(Messages.MessageClass.ErrorMsg, "AddNodeToMenu failed" & vbNewLine & ex.Message, True)
        End Try
    End Sub

    Private Sub ConMenItem_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            If TypeOf sender.Tag Is mRemote.Connection.Info Then
                App.Runtime.OpenConnection(sender.Tag)
            End If
        End If
    End Sub
#End Region

    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            If My.Settings.MinimizeToTray Then
                If App.Runtime.SysTrayIcon Is Nothing Then
                    App.Runtime.SysTrayIcon = New Tools.Controls.SysTrayIcon()
                End If
                Me.Hide()
            End If
        Else
            prevWindowsState = Me.WindowState
        End If
    End Sub

#Region "Window Overrides and DockPanel Stuff"
    Private bWmGetTextFlag As Boolean = False
    Private bWmWindowPosChangedFlag As Boolean = False

    Protected Overloads Overrides Sub WndProc(ByRef m As Message)
        Try
            'Debug.Print(m.Msg)

            If m.Msg = WM_GETTEXT Then
                bWmGetTextFlag = True
            ElseIf m.Msg = WM_WINDOWPOSCHANGED Then
                If bWmGetTextFlag Then
                    ActivateConnection()
                End If

                bWmGetTextFlag = False
                bWmWindowPosChangedFlag = True
            ElseIf m.Msg = WM_ACTIVATEAPP Then
                If bWmWindowPosChangedFlag Then
                    ActivateConnection()
                End If
            Else
                bWmGetTextFlag = False
                bWmWindowPosChangedFlag = False
            End If

            If m.Msg = Tools.SystemMenu.Flags.WM_SYSCOMMAND Then
                For i As Integer = 0 To SysMenSubItems.Length - 1
                    If SysMenSubItems(i) = m.WParam Then
                        Screens.SendFormToScreen(Screen.AllScreens(i))
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
        End Try

        MyBase.WndProc(m)
    End Sub

    Private Sub ActivateConnection()
        If TypeOf pnlDock.ActiveDocument Is UI.Window.Connection Then
            Dim cW As UI.Window.Connection = pnlDock.ActiveDocument
            If cW.TabController.SelectedTab IsNot Nothing Then
                Dim tab As Magic.Controls.TabPage = cW.TabController.SelectedTab
                Dim ifc As Connection.InterfaceControl = TryCast(tab.Tag, Connection.InterfaceControl)
                ifc.Protocol.Focus()
                TryCast(ifc.FindForm, UI.Window.Connection).RefreshIC()
            End If
        End If
    End Sub

    Private Sub pnlDock_ActiveDocumentChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlDock.ActiveDocumentChanged
        ActivateConnection()
    End Sub
#End Region

#Region "Screen Stuff"
    Private Sub DisplayChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ResetSysMenuItems()
        AddSysMenuItems()
    End Sub

    Private SysMenSubItems(50) As Integer
    Private Sub ResetSysMenuItems()
        SysMenu.Reset()
    End Sub

    Private Sub AddSysMenuItems()
        SysMenu = New Tools.SystemMenu(Me.Handle)
        Dim popMen As IntPtr = SysMenu.CreatePopupMenuItem()

        For i As Integer = 0 To Screen.AllScreens.Length - 1
            SysMenSubItems(i) = 200 + i
            SysMenu.AppendMenuItem(popMen, Tools.SystemMenu.Flags.MF_STRING, SysMenSubItems(i), Language.Base.Screen & " " & i + 1)
        Next

        SysMenu.InsertMenuItem(SysMenu.SystemMenuHandle, 0, Tools.SystemMenu.Flags.MF_POPUP Or Tools.SystemMenu.Flags.MF_BYPOSITION, popMen, Language.Base.SendTo)
        SysMenu.InsertMenuItem(SysMenu.SystemMenuHandle, 1, Tools.SystemMenu.Flags.MF_BYPOSITION Or Tools.SystemMenu.Flags.MF_SEPARATOR, IntPtr.Zero, Nothing)
    End Sub
#End Region
End Class
