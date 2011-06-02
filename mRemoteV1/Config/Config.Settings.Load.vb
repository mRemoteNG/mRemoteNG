Imports System.IO
Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime
Imports System.Xml
Imports System.Environment

Namespace Config
    Namespace Settings
        Public Class Load
#Region "Public Properties"
            Private _MainForm As frmMain
            Public Property MainForm() As frmMain
                Get
                    Return Me._MainForm
                End Get
                Set(ByVal value As frmMain)
                    Me._MainForm = value
                End Set
            End Property
#End Region

#Region "Public Methods"
            Public Sub New(ByVal MainForm As frmMain)
                Me._MainForm = MainForm
            End Sub

            Public Sub Load()
                Try
                    With Me._MainForm
                        ' Migrate settings from previous version
                        If My.Settings.DoUpgrade Then
                            My.Settings.Upgrade()
                            My.Settings.DoUpgrade = False

                            ' Clear pending update flag
                            ' This is used for automatic updates, not for settings migration, but it
                            ' needs to be cleared here because we know that we just updated.
                            My.Settings.UpdatePending = False
                        End If

                        App.SupportedCultures.InstantiateSingleton()
                        If Not My.Settings.OverrideUICulture = "" And App.SupportedCultures.IsNameSupported(My.Settings.OverrideUICulture) Then
                            Threading.Thread.CurrentThread.CurrentUICulture = New Globalization.CultureInfo(My.Settings.OverrideUICulture)
                            log.InfoFormat("Override Culture: {0}/{1}", Threading.Thread.CurrentThread.CurrentUICulture.Name, Threading.Thread.CurrentThread.CurrentUICulture.NativeName)
                        End If

                        .WindowState = FormWindowState.Normal
                        If My.Settings.MainFormState = FormWindowState.Normal Then
                            If Not My.Settings.MainFormLocation.IsEmpty Then .Location = My.Settings.MainFormLocation
                            If Not My.Settings.MainFormSize.IsEmpty Then .Size = My.Settings.MainFormSize
                        Else
                            If Not My.Settings.MainFormRestoreLocation.IsEmpty Then .Location = My.Settings.MainFormRestoreLocation
                            If Not My.Settings.MainFormRestoreSize.IsEmpty Then .Size = My.Settings.MainFormRestoreSize
                        End If
                        If My.Settings.MainFormState = FormWindowState.Maximized Then
                            .WindowState = FormWindowState.Maximized
                        End If

                        ' Make sure the form is visible on the screen
                        Const minHorizontal As Integer = 300
                        Const minVertical As Integer = 150
                        Dim screenBounds As Drawing.Rectangle = Screen.FromHandle(.Handle).Bounds
                        Dim newBounds As Drawing.Rectangle = .Bounds

                        If newBounds.Right < screenBounds.Left + minHorizontal Then
                            newBounds.X = screenBounds.Left + minHorizontal - newBounds.Width
                        End If
                        If newBounds.Left > screenBounds.Right - minHorizontal Then
                            newBounds.X = screenBounds.Right - minHorizontal
                        End If
                        If newBounds.Bottom < screenBounds.Top + minVertical Then
                            newBounds.Y = screenBounds.Top + minVertical - newBounds.Height
                        End If
                        If newBounds.Top > screenBounds.Bottom - minVertical Then
                            newBounds.Y = screenBounds.Bottom - minVertical
                        End If

                        .Location = newBounds.Location

                        If My.Settings.MainFormKiosk = True Then
                            Tools.Misc.Fullscreen.EnterFullscreen()
                        End If

                        If My.Settings.UseCustomPuttyPath Then
                            Connection.Protocol.PuttyBase.PuttyPath = My.Settings.CustomPuttyPath
                        Else
                            Connection.Protocol.PuttyBase.PuttyPath = My.Application.Info.DirectoryPath & "\Putty.exe"
                        End If

                        If My.Settings.ShowSystemTrayIcon Then
                            App.Runtime.SysTrayIcon = New Tools.Controls.SysTrayIcon()
                        End If

                        If My.Settings.AutoSaveEveryMinutes > 0 Then
                            .tmrAutoSave.Interval = My.Settings.AutoSaveEveryMinutes * 60000
                            .tmrAutoSave.Enabled = True
                        End If

                        My.Settings.ConDefaultPassword = Security.Crypt.Decrypt(My.Settings.ConDefaultPassword, App.Info.General.EncryptionKey)

                        Me.LoadPanelsFromXML()
                        Me.LoadExternalAppsFromXML()

                        If My.Settings.ResetToolbars = False Then
                            LoadToolbarsFromSettings()
                        Else
                            SetToolbarsDefault()
                        End If
                    End With
                Catch ex As Exception
                    App.Runtime.log.Error("Loading settings failed" & vbNewLine & ex.Message)
                    'mC.AddMessage(Messages.MessageClass.ErrorMsg, "Loading settings failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub SetToolbarsDefault()
                With MainForm
                    ToolStripPanelFromString("top").Join(.tsQuickConnect, New Point(300, 0))
                    .tsQuickConnect.Visible = True
                    ToolStripPanelFromString("bottom").Join(.tsExtAppsToolbar, New Point(3, 0))
                    .tsExtAppsToolbar.Visible = False
                End With
            End Sub

            Public Sub LoadToolbarsFromSettings()
                With Me.MainForm
                    If My.Settings.QuickyTBLocation.X > My.Settings.ExtAppsTBLocation.X Then
                        AddDynamicPanels()
                        AddStaticPanels()
                    Else
                        AddStaticPanels()
                        AddDynamicPanels()
                    End If
                End With
            End Sub

            Private Sub AddStaticPanels()
                With MainForm
                    ToolStripPanelFromString(My.Settings.QuickyTBParentDock).Join(.tsQuickConnect, My.Settings.QuickyTBLocation)
                    .tsQuickConnect.Visible = My.Settings.QuickyTBVisible
                End With
            End Sub

            Private Sub AddDynamicPanels()
                With MainForm
                    ToolStripPanelFromString(My.Settings.ExtAppsTBParentDock).Join(.tsExtAppsToolbar, My.Settings.ExtAppsTBLocation)
                    .tsExtAppsToolbar.Visible = My.Settings.ExtAppsTBVisible
                End With
            End Sub

            Private Function ToolStripPanelFromString(ByVal Panel As String) As ToolStripPanel
                Select Case LCase(Panel)
                    Case "top"
                        Return MainForm.tsContainer.TopToolStripPanel
                    Case "bottom"
                        Return MainForm.tsContainer.BottomToolStripPanel
                    Case "left"
                        Return MainForm.tsContainer.LeftToolStripPanel
                    Case "right"
                        Return MainForm.tsContainer.RightToolStripPanel
                    Case Else
                        Return MainForm.tsContainer.TopToolStripPanel
                End Select
            End Function

            Public Sub LoadPanelsFromXML()
                Try
                    With MainForm
                        Windows.treePanel = Nothing
                        Windows.configPanel = Nothing
                        Windows.errorsPanel = Nothing

                        Do While .pnlDock.Contents.Count > 0
                            Dim dc As WeifenLuo.WinFormsUI.Docking.DockContent = .pnlDock.Contents(0)
                            dc.Close()
                        Loop

                        Startup.CreatePanels()

                        Dim oldPath As String = GetFolderPath(SpecialFolder.LocalApplicationData) & "\" & My.Application.Info.ProductName & "\" & App.Info.Settings.LayoutFileName
                        Dim newPath As String = App.Info.Settings.SettingsPath & "\" & App.Info.Settings.LayoutFileName
                        If File.Exists(newPath) Then
                            .pnlDock.LoadFromXml(newPath, AddressOf GetContentFromPersistString)
#If Not PORTABLE Then
                        ElseIf File.Exists(oldPath) Then
                            .pnlDock.LoadFromXml(oldPath, AddressOf GetContentFromPersistString)
#End If
                        Else
                            Startup.SetDefaultLayout()
                        End If
                    End With
                Catch ex As Exception
                    App.Runtime.log.Error("LoadPanelsFromXML failed" & vbNewLine & ex.Message)
                    'mC.AddMessage(Messages.MessageClass.ErrorMsg, "LoadPanelsFromXML failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub LoadExternalAppsFromXML()
                Dim oldPath As String = GetFolderPath(SpecialFolder.LocalApplicationData) & "\" & My.Application.Info.ProductName & "\" & App.Info.Settings.ExtAppsFilesName
                Dim newPath As String = App.Info.Settings.SettingsPath & "\" & App.Info.Settings.ExtAppsFilesName
                Dim xDom As New XmlDocument()
                If File.Exists(newPath) Then
                    xDom.Load(newPath)
#If Not PORTABLE Then
                ElseIf File.Exists(oldPath) Then
                    xDom.Load(oldPath)
#End If
                Else
                    Exit Sub
                End If

                For Each xEl As XmlElement In xDom.DocumentElement.ChildNodes
                    Dim extA As New Tools.ExternalApp
                    extA.DisplayName = xEl.Attributes("DisplayName").Value
                    extA.FileName = xEl.Attributes("FileName").Value
                    extA.Arguments = xEl.Attributes("Arguments").Value

                    If xEl.HasAttribute("WaitForExit") Then
                        extA.WaitForExit = xEl.Attributes("WaitForExit").Value
                    End If

                    If xEl.HasAttribute("TryToIntegrate") Then
                        extA.TryIntegrate = xEl.Attributes("TryToIntegrate").Value
                    End If

                    ExtApps.Add(extA)
                Next

                MainForm.SwitchToolbarText(My.Settings.ExtAppsTBShowText)

                xDom = Nothing

                frmMain.AddExtAppsToToolbar()
            End Sub
#End Region

#Region "Private Methods"
            Private Function GetContentFromPersistString(ByVal persistString As String) As IDockContent
                ' pnlLayout.xml persistence XML fix for refactoring to mRemoteNG
                If (persistString.StartsWith("mRemote.")) Then
                    persistString = persistString.Replace("mRemote.", "mRemoteNG.")
                End If

                Try
                    If persistString = GetType(UI.Window.Config).ToString Then
                        Return Windows.configPanel
                    End If

                    If persistString = GetType(UI.Window.Tree).ToString Then
                        Return Windows.treePanel
                    End If

                    If persistString = GetType(UI.Window.ErrorsAndInfos).ToString Then
                        Return Windows.errorsPanel
                    End If

                    If persistString = GetType(UI.Window.Sessions).ToString Then
                        Return Windows.sessionsPanel
                    End If

                    If persistString = GetType(UI.Window.ScreenshotManager).ToString Then
                        Return Windows.screenshotPanel
                    End If
                Catch ex As Exception
                    App.Runtime.log.Error("GetContentFromPersistString failed" & vbNewLine & ex.Message)
                    'mC.AddMessage(Messages.MessageClass.ErrorMsg, "GetContentFromPersistString failed" & vbNewLine & ex.Message, True)
                End Try

                Return Nothing
            End Function
#End Region
        End Class
    End Namespace
End Namespace