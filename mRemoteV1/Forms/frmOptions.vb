Imports System.IO
Imports System.ComponentModel
Imports mRemoteNG.Messages
Imports mRemoteNG.App.Info
Imports mRemoteNG.Config
Imports mRemoteNG.App
Imports mRemoteNG.My
Imports mRemoteNG.Tools
Imports mRemoteNG.Connection.Protocol
Imports PSTaskDialog
Imports mRemoteNG.Security
Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.Themes
Imports mRemoteNG.UI.Window

Namespace Forms

    Public Class frmOptions
        Inherits Form

#Region "Private Methods"
        Private Sub LoadOptions()
            Try
                chkSaveConsOnExit.Checked = My.Settings.SaveConsOnExit
                Select Case MySettingsProperty.Settings.ConfirmCloseConnection
                    Case ConfirmClose.Never
                        radCloseWarnNever.Checked = True
                    Case ConfirmClose.Exit
                        radCloseWarnExit.Checked = True
                    Case ConfirmClose.Multiple
                        radCloseWarnMultiple.Checked = True
                    Case Else
                        radCloseWarnAll.Checked = True
                End Select
                chkReconnectOnStart.Checked = My.Settings.OpenConsFromLastSession
                chkProperInstallationOfComponentsAtStartup.Checked = My.Settings.StartupComponentsCheck

                cboLanguage.Items.Clear()
                cboLanguage.Items.Add(Language.strLanguageDefault)

                For Each nativeName As String In SupportedCultures.CultureNativeNames
                    cboLanguage.Items.Add(nativeName)
                Next
                If Not My.Settings.OverrideUICulture = "" And SupportedCultures.IsNameSupported(My.Settings.OverrideUICulture) Then
                    cboLanguage.SelectedItem = SupportedCultures.CultureNativeName(My.Settings.OverrideUICulture)
                End If
                If cboLanguage.SelectedIndex = -1 Then
                    cboLanguage.SelectedIndex = 0
                End If

                chkAlwaysShowPanelTabs.Checked = MySettingsProperty.Settings.AlwaysShowPanelTabs
                chkShowDescriptionTooltipsInTree.Checked = My.Settings.ShowDescriptionTooltipsInTree
                chkShowSystemTrayIcon.Checked = My.Settings.ShowSystemTrayIcon
                chkMinimizeToSystemTray.Checked = My.Settings.MinimizeToTray

                chkOpenNewTabRightOfSelected.Checked = My.Settings.OpenTabsRightOfSelected
                chkShowLogonInfoOnTabs.Checked = My.Settings.ShowLogonInfoOnTabs
                chkShowProtocolOnTabs.Checked = My.Settings.ShowProtocolOnTabs
                chkIdentifyQuickConnectTabs.Checked = MySettingsProperty.Settings.IdentifyQuickConnectTabs
                chkShowFullConnectionsFilePathInTitle.Checked = My.Settings.ShowCompleteConsPathInTitle
                chkDoubleClickClosesTab.Checked = My.Settings.DoubleClickOnTabClosesIt
                chkAlwaysShowPanelSelectionDlg.Checked = My.Settings.AlwaysShowPanelSelectionDlg

                chkSingleClickOnConnectionOpensIt.Checked = My.Settings.SingleClickOnConnectionOpensIt
                chkSingleClickOnOpenedConnectionSwitchesToIt.Checked = My.Settings.SingleClickSwitchesToOpenConnection
                chkHostnameLikeDisplayName.Checked = My.Settings.SetHostnameLikeDisplayName
                numRdpReconnectionCount.Value = My.Settings.RdpReconnectionCount
                numAutoSave.Value = My.Settings.AutoSaveEveryMinutes

                chkUseSQLServer.Checked = My.Settings.UseSQLServer
                txtSQLServer.Text = My.Settings.SQLHost
                txtSQLDatabaseName.Text = My.Settings.SQLDatabaseName
                txtSQLUsername.Text = My.Settings.SQLUser
                txtSQLPassword.Text = Crypt.Decrypt(My.Settings.SQLPass, General.EncryptionKey)

                Select Case My.Settings.EmptyCredentials
                    Case "noinfo"
                        radCredentialsNoInfo.Checked = True
                    Case "windows"
                        radCredentialsWindows.Checked = True
                    Case "custom"
                        radCredentialsCustom.Checked = True
                End Select

                txtCredentialsUsername.Text = My.Settings.DefaultUsername
                txtCredentialsPassword.Text = Crypt.Decrypt(My.Settings.DefaultPassword, General.EncryptionKey)
                txtCredentialsDomain.Text = My.Settings.DefaultDomain

                chkUseOnlyErrorsAndInfosPanel.Checked = My.Settings.ShowNoMessageBoxes
                chkMCInformation.Checked = My.Settings.SwitchToMCOnInformation
                chkMCWarnings.Checked = My.Settings.SwitchToMCOnWarning
                chkMCErrors.Checked = My.Settings.SwitchToMCOnError

                chkCheckForUpdatesOnStartup.Checked = My.Settings.CheckForUpdatesOnStartup
                cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked
                cboUpdateCheckFrequency.Items.Clear()
                Dim nDaily As Integer = cboUpdateCheckFrequency.Items.Add(Language.strUpdateFrequencyDaily)
                Dim nWeekly As Integer = cboUpdateCheckFrequency.Items.Add(Language.strUpdateFrequencyWeekly)
                Dim nMonthly As Integer = cboUpdateCheckFrequency.Items.Add(Language.strUpdateFrequencyMonthly)
                Select Case My.Settings.CheckForUpdatesFrequencyDays
                    Case Is < 1
                        chkCheckForUpdatesOnStartup.Checked = False
                        cboUpdateCheckFrequency.SelectedIndex = nDaily
                    Case 1 ' Daily
                        cboUpdateCheckFrequency.SelectedIndex = nDaily
                    Case 7 ' Weekly
                        cboUpdateCheckFrequency.SelectedIndex = nWeekly
                    Case 31 ' Monthly
                        cboUpdateCheckFrequency.SelectedIndex = nMonthly
                    Case Else
                        Dim nCustom As Integer = cboUpdateCheckFrequency.Items.Add(String.Format(Language.strUpdateFrequencyCustom, My.Settings.CheckForUpdatesFrequencyDays))
                        cboUpdateCheckFrequency.SelectedIndex = nCustom
                End Select

                chkWriteLogFile.Checked = My.Settings.WriteLogFile
                chkEncryptCompleteFile.Checked = My.Settings.EncryptCompleteConnectionsFile
                chkAutomaticallyGetSessionInfo.Checked = My.Settings.AutomaticallyGetSessionInfo
                chkAutomaticReconnect.Checked = My.Settings.ReconnectOnDisconnect
                chkSingleInstance.Checked = My.Settings.SingleInstance
                numPuttyWaitTime.Value = My.Settings.MaxPuttyWaitTime

                chkUseCustomPuttyPath.Checked = MySettingsProperty.Settings.UseCustomPuttyPath
                txtCustomPuttyPath.Text = MySettingsProperty.Settings.CustomPuttyPath
                SetPuttyLaunchButtonEnabled()

                chkUseProxyForAutomaticUpdates.Checked = My.Settings.UpdateUseProxy
                btnTestProxy.Enabled = My.Settings.UpdateUseProxy
                pnlProxyBasic.Enabled = My.Settings.UpdateUseProxy

                txtProxyAddress.Text = My.Settings.UpdateProxyAddress
                numProxyPort.Value = My.Settings.UpdateProxyPort

                chkUseProxyAuthentication.Checked = My.Settings.UpdateProxyUseAuthentication
                pnlProxyAuthentication.Enabled = My.Settings.UpdateProxyUseAuthentication

                txtProxyUsername.Text = My.Settings.UpdateProxyAuthUser
                txtProxyPassword.Text = Crypt.Decrypt(My.Settings.UpdateProxyAuthPass, General.EncryptionKey)

                numUVNCSCPort.Value = My.Settings.UVNCSCPort

                txtXULrunnerPath.Text = My.Settings.XULRunnerPath

                _themeList = New BindingList(Of ThemeInfo)(ThemeManager.LoadThemes())
                cboTheme.DataSource = _themeList
                cboTheme.SelectedItem = ThemeManager.ActiveTheme
                cboTheme_SelectionChangeCommitted(Me, New EventArgs())

                ThemePropertyGrid.PropertySort = PropertySort.Categorized

                _originalTheme = ThemeManager.ActiveTheme
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "LoadOptions (UI.Window.Options) failed" & vbNewLine & ex.Message, True)
            End Try
        End Sub

        Private Sub SaveOptions()
            Try
                My.Settings.SaveConsOnExit = chkSaveConsOnExit.Checked
                If radCloseWarnAll.Checked Then MySettingsProperty.Settings.ConfirmCloseConnection = ConfirmClose.All
                If radCloseWarnMultiple.Checked Then MySettingsProperty.Settings.ConfirmCloseConnection = ConfirmClose.Multiple
                If radCloseWarnExit.Checked Then MySettingsProperty.Settings.ConfirmCloseConnection = ConfirmClose.Exit
                If radCloseWarnNever.Checked Then MySettingsProperty.Settings.ConfirmCloseConnection = ConfirmClose.Never
                My.Settings.OpenConsFromLastSession = chkReconnectOnStart.Checked
                My.Settings.StartupComponentsCheck = chkProperInstallationOfComponentsAtStartup.Checked

                If cboLanguage.SelectedIndex > 0 And SupportedCultures.IsNativeNameSupported(cboLanguage.SelectedItem) Then
                    My.Settings.OverrideUICulture = SupportedCultures.CultureName(cboLanguage.SelectedItem)
                Else
                    My.Settings.OverrideUICulture = ""
                End If

                MySettingsProperty.Settings.AlwaysShowPanelTabs = chkAlwaysShowPanelTabs.Checked
                frmMain.ShowHidePanelTabs()

                My.Settings.ShowDescriptionTooltipsInTree = chkShowDescriptionTooltipsInTree.Checked
                My.Settings.ShowSystemTrayIcon = chkShowSystemTrayIcon.Checked
                My.Settings.MinimizeToTray = chkMinimizeToSystemTray.Checked

                If My.Settings.ShowSystemTrayIcon Then
                    If Runtime.NotificationAreaIcon Is Nothing Then
                        Runtime.NotificationAreaIcon = New Tools.Controls.NotificationAreaIcon
                    End If
                Else
                    If Runtime.NotificationAreaIcon IsNot Nothing Then
                        Runtime.NotificationAreaIcon.Dispose()
                        Runtime.NotificationAreaIcon = Nothing
                    End If
                End If

                My.Settings.ShowCompleteConsPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked

                My.Settings.OpenTabsRightOfSelected = chkOpenNewTabRightOfSelected.Checked
                My.Settings.ShowLogonInfoOnTabs = chkShowLogonInfoOnTabs.Checked
                My.Settings.ShowProtocolOnTabs = chkShowProtocolOnTabs.Checked
                MySettingsProperty.Settings.IdentifyQuickConnectTabs = chkIdentifyQuickConnectTabs.Checked
                My.Settings.DoubleClickOnTabClosesIt = chkDoubleClickClosesTab.Checked
                My.Settings.AlwaysShowPanelSelectionDlg = chkAlwaysShowPanelSelectionDlg.Checked

                My.Settings.SingleClickOnConnectionOpensIt = chkSingleClickOnConnectionOpensIt.Checked
                My.Settings.SingleClickSwitchesToOpenConnection = chkSingleClickOnOpenedConnectionSwitchesToIt.Checked
                My.Settings.SetHostnameLikeDisplayName = chkHostnameLikeDisplayName.Checked
                My.Settings.RdpReconnectionCount = numRdpReconnectionCount.Value
                My.Settings.AutoSaveEveryMinutes = numAutoSave.Value

                If My.Settings.AutoSaveEveryMinutes > 0 Then
                    frmMain.tmrAutoSave.Interval = My.Settings.AutoSaveEveryMinutes * 60000
                    frmMain.tmrAutoSave.Enabled = True
                Else
                    frmMain.tmrAutoSave.Enabled = False
                End If

                My.Settings.UseSQLServer = chkUseSQLServer.Checked
                My.Settings.SQLHost = txtSQLServer.Text
                My.Settings.SQLDatabaseName = txtSQLDatabaseName.Text
                My.Settings.SQLUser = txtSQLUsername.Text
                My.Settings.SQLPass = Crypt.Encrypt(txtSQLPassword.Text, General.EncryptionKey)

                If radCredentialsNoInfo.Checked Then
                    My.Settings.EmptyCredentials = "noinfo"
                ElseIf radCredentialsWindows.Checked Then
                    My.Settings.EmptyCredentials = "windows"
                ElseIf radCredentialsCustom.Checked Then
                    My.Settings.EmptyCredentials = "custom"
                End If

                My.Settings.DefaultUsername = txtCredentialsUsername.Text
                My.Settings.DefaultPassword = Crypt.Encrypt(txtCredentialsPassword.Text, General.EncryptionKey)
                My.Settings.DefaultDomain = txtCredentialsDomain.Text

                My.Settings.ShowNoMessageBoxes = chkUseOnlyErrorsAndInfosPanel.Checked
                My.Settings.SwitchToMCOnInformation = chkMCInformation.Checked
                My.Settings.SwitchToMCOnWarning = chkMCWarnings.Checked
                My.Settings.SwitchToMCOnError = chkMCErrors.Checked

                My.Settings.CheckForUpdatesOnStartup = chkCheckForUpdatesOnStartup.Checked
                Select Case cboUpdateCheckFrequency.SelectedItem.ToString()
                    Case Language.strUpdateFrequencyDaily
                        My.Settings.CheckForUpdatesFrequencyDays = 1
                    Case Language.strUpdateFrequencyWeekly
                        My.Settings.CheckForUpdatesFrequencyDays = 7
                    Case Language.strUpdateFrequencyMonthly
                        My.Settings.CheckForUpdatesFrequencyDays = 31
                End Select

                My.Settings.WriteLogFile = chkWriteLogFile.Checked
                My.Settings.EncryptCompleteConnectionsFile = chkEncryptCompleteFile.Checked
                My.Settings.AutomaticallyGetSessionInfo = chkAutomaticallyGetSessionInfo.Checked
                My.Settings.ReconnectOnDisconnect = chkAutomaticReconnect.Checked
                My.Settings.SingleInstance = chkSingleInstance.Checked

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
                    Putty.Sessions.AddSessionsToTree()
                End If

                My.Settings.MaxPuttyWaitTime = numPuttyWaitTime.Value

                My.Settings.UpdateUseProxy = chkUseProxyForAutomaticUpdates.Checked

                My.Settings.UpdateProxyAddress = txtProxyAddress.Text
                My.Settings.UpdateProxyPort = numProxyPort.Value

                My.Settings.UpdateProxyUseAuthentication = chkUseProxyAuthentication.Checked

                My.Settings.UpdateProxyAuthUser = txtProxyUsername.Text
                My.Settings.UpdateProxyAuthPass = Crypt.Encrypt(txtProxyPassword.Text, General.EncryptionKey)

                My.Settings.UVNCSCPort = numUVNCSCPort.Value

                My.Settings.XULRunnerPath = txtXULrunnerPath.Text

                ThemeManager.SaveThemes(_themeList)
                MySettingsProperty.Settings.ThemeName = ThemeManager.ActiveTheme.Name

                KeyboardShortcuts.Map = _keyboardShortcutMap

                Runtime.SetMainFormText(Runtime.GetStartupConnectionFileName())

                Runtime.Startup.DestroySQLUpdateHandlerAndStopTimer()

                If My.Settings.UseSQLServer = True Then
                    Runtime.SetMainFormText("SQL Server")
                    Runtime.Startup.CreateSQLUpdateHandlerAndStartTimer()
                End If
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "SaveOptions (UI.Window.Options) failed" & vbNewLine & ex.Message, True)
            End Try
        End Sub
#End Region

#Region "Private Fields"
        Private _initialTab As Integer = 0
        Private _appUpdate As App.Update
        Private _themeList As BindingList(Of ThemeInfo)
        Private _originalTheme As ThemeInfo
        Private _keyboardShortcutMap As KeyboardShortcutMap
        Private _tabsListViewGroup As ListViewGroup
        Private _previousTabListViewItem As ListViewItem
        Private _nextTabListViewItem As ListViewItem
        Private _ignoreKeyboardShortcutTextChanged As Boolean = False
#End Region

#Region "Public Methods"
        Public Sub New()
            InitializeComponent()
            Runtime.FontOverride(Me)
        End Sub
#End Region

#Region "Form Stuff"
        Private Sub Options_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            ApplyLanguage()
            InitializeKeyboardPage()

            ' Hide the tabs
            tcTabControl.Appearance = Windows.Forms.TabAppearance.FlatButtons
            tcTabControl.Padding = New Point(0, 0)
            tcTabControl.ItemSize = New Point(0, 1)

            ' Switch to the _initialTab
            tcTabControl.SelectedIndex = _initialTab
            lvPages.Items(_initialTab).Selected = True

#If PORTABLE Then
        For Each Control As Control In tcTabControl.TabPages(5).Controls
            If Control IsNot lblUpdatesExplanation Then
                Control.Visible = False
            End If
        Next
#End If
        End Sub

        Private Sub ApplyLanguage()
            lvPages.Items(0).Text = Language.strStartupExit
            lvPages.Items(1).Text = Language.strTabAppearance
            lvPages.Items(2).Text = Language.strTabsAndPanels.Replace("&&", "&")
            lvPages.Items(3).Text = Language.strConnections
            lvPages.Items(4).Text = Language.strSQLServer
            lvPages.Items(5).Text = Language.strTabUpdates
            lvPages.Items(6).Text = Language.strTabAdvanced
            lvPages.Items(7).Text = Language.strOptionsTabTheme
#If Not PORTABLE Then
            lblUpdatesExplanation.Text = Language.strUpdateCheck
#Else
            lblUpdatesExplanation.Text = Language.strUpdateCheckPortableEdition
#End If
            btnTestProxy.Text = Language.strButtonTestProxy
            lblSeconds.Text = Language.strLabelSeconds
            lblMaximumPuttyWaitTime.Text = Language.strLabelPuttyTimeout
            chkAutomaticReconnect.Text = Language.strCheckboxAutomaticReconnect
            lblProxyAddress.Text = Language.strLabelAddress
            lblProxyPort.Text = Language.strLabelPort
            lblProxyUsername.Text = Language.strLabelUsername
            lblProxyPassword.Text = Language.strLabelPassword
            chkUseProxyAuthentication.Text = Language.strCheckboxProxyAuthentication
            chkUseProxyForAutomaticUpdates.Text = Language.strCheckboxUpdateUseProxy
            lblConfigurePuttySessions.Text = Language.strLabelPuttySessionsConfig
            btnLaunchPutty.Text = Language.strButtonLaunchPutty
            btnBrowseCustomPuttyPath.Text = Language.strButtonBrowse
            chkUseCustomPuttyPath.Text = Language.strCheckboxPuttyPath
            chkAutomaticallyGetSessionInfo.Text = Language.strAutomaticallyGetSessionInfo
            chkWriteLogFile.Text = Language.strWriteLogFile
            chkSingleInstance.Text = Language.strAllowOnlySingleInstance
            chkReconnectOnStart.Text = Language.strReconnectAtStartup
            chkCheckForUpdatesOnStartup.Text = Language.strCheckForUpdatesOnStartup
            chkSaveConsOnExit.Text = Language.strSaveConsOnExit
            chkMinimizeToSystemTray.Text = Language.strMinimizeToSysTray
            chkShowFullConnectionsFilePathInTitle.Text = Language.strShowFullConsFilePath
            chkShowSystemTrayIcon.Text = Language.strAlwaysShowSysTrayIcon
            chkAlwaysShowPanelTabs.Text = Language.strAlwaysShowPanelTabs
            chkShowDescriptionTooltipsInTree.Text = Language.strShowDescriptionTooltips
            chkShowProtocolOnTabs.Text = Language.strShowProtocolOnTabs
            chkIdentifyQuickConnectTabs.Text = Language.strIdentifyQuickConnectTabs
            chkShowLogonInfoOnTabs.Text = Language.strShowLogonInfoOnTabs
            chkOpenNewTabRightOfSelected.Text = Language.strOpenNewTabRight
            chkAlwaysShowPanelSelectionDlg.Text = Language.strAlwaysShowPanelSelection
            chkDoubleClickClosesTab.Text = Language.strDoubleClickTabClosesIt
            chkHostnameLikeDisplayName.Text = Language.strSetHostnameLikeDisplayName
            lblExperimental.Text = Language.strExperimental.ToUpper
            chkUseSQLServer.Text = Language.strUseSQLServer
            lblSQLInfo.Text = Language.strSQLInfo
            lblSQLUsername.Text = Language.strLabelUsername
            lblSQLServer.Text = Language.strLabelHostname
            lblSQLDatabaseName.Text = Language.strLabelSQLServerDatabaseName
            lblSQLPassword.Text = Language.strLabelPassword
            lblRdpReconnectionCount.Text = Language.strRdpReconnectCount
            lblAutoSave2.Text = Language.strAutoSaveMins
            lblAutoSave1.Text = Language.strAutoSaveEvery
            lblCredentialsDomain.Text = Language.strLabelDomain
            lblCredentialsPassword.Text = Language.strLabelPassword
            lblCredentialsUsername.Text = Language.strLabelUsername
            radCredentialsCustom.Text = Language.strTheFollowing
            radCredentialsWindows.Text = Language.strMyCurrentWindowsCreds
            radCredentialsNoInfo.Text = Language.strNoInformation
            lblDefaultCredentials.Text = Language.strEmptyUsernamePasswordDomainFields
            lblClosingConnections.Text = Language.strLabelClosingConnections
            radCloseWarnAll.Text = Language.strRadioCloseWarnAll
            radCloseWarnMultiple.Text = Language.strRadioCloseWarnMultiple
            radCloseWarnExit.Text = Language.strRadioCloseWarnExit
            radCloseWarnNever.Text = Language.strRadioCloseWarnNever
            chkSingleClickOnOpenedConnectionSwitchesToIt.Text = Language.strSingleClickOnOpenConnectionSwitchesToIt
            chkSingleClickOnConnectionOpensIt.Text = Language.strSingleClickOnConnectionOpensIt
            lblSwitchToErrorsAndInfos.Text = Language.strSwitchToErrorsAndInfos
            chkMCErrors.Text = Language.strErrors
            chkMCWarnings.Text = Language.strWarnings
            chkMCInformation.Text = Language.strInformations
            chkUseOnlyErrorsAndInfosPanel.Text = Language.strUseOnlyErrorsAndInfosPanel
            btnOK.Text = Language.strButtonOK
            btnCancel.Text = Language.strButtonCancel
            btnUpdateCheckNow.Text = Language.strCheckNow
            Text = Language.strMenuOptions
            lblUVNCSCPort.Text = Language.strUltraVNCSCListeningPort
            chkProperInstallationOfComponentsAtStartup.Text = Language.strCheckProperInstallationOfComponentsAtStartup
            lblXulRunnerPath.Text = Language.strXULrunnerPath
            btnBrowseXulRunnerPath.Text = Language.strButtonBrowse
            chkEncryptCompleteFile.Text = Language.strEncryptCompleteConnectionFile
            lblLanguage.Text = Language.strLanguage
            lblLanguageRestartRequired.Text = String.Format(Language.strLanguageRestartRequired, Application.Info.ProductName)
            btnThemeDelete.Text = Language.strOptionsThemeButtonDelete
            btnThemeNew.Text = Language.strOptionsThemeButtonNew

            ' Keyboard Page
            lvPages.Items(8).Text = Language.strOptionsTabKeyboard
            lblKeyboardShortcuts.Text = Language.strOptionsKeyboardLabelKeyboardShortcuts
            btnNewKeyboardShortcut.Text = Language.strOptionsKeyboardButtonNew
            btnDeleteKeyboardShortcut.Text = Language.strOptionsKeyboardButtonDelete
            btnResetKeyboardShortcuts.Text = Language.strOptionsKeyboardButtonReset
            grpModifyKeyboardShortcut.Text = Language.strOptionsKeyboardGroupModifyShortcut
            btnResetAllKeyboardShortcuts.Text = Language.strOptionsKeyboardButtonResetAll
            _tabsListViewGroup = New ListViewGroup(Language.strOptionsKeyboardCommandsGroupTabs)
            _previousTabListViewItem = New ListViewItem(Language.strOptionsKeyboardCommandsPreviousTab, _tabsListViewGroup)
            _nextTabListViewItem = New ListViewItem(Language.strOptionsKeyboardCommandsNextTab, _tabsListViewGroup)
        End Sub

        Private Sub InitializeKeyboardPage()
            _keyboardShortcutMap = KeyboardShortcuts.Map.Clone()

            lvKeyboardCommands.Groups.Add(_tabsListViewGroup)
            lvKeyboardCommands.Items.Add(_previousTabListViewItem)
            lvKeyboardCommands.Items.Add(_nextTabListViewItem)
            _previousTabListViewItem.Selected = True

            EnableKeyboardShortcutControls(False)
        End Sub

        Public Overloads Sub Show(ByVal dockPanel As DockPanel, Optional ByVal initialTab As Integer = 0)
            Runtime.Windows.optionsForm.LoadOptions()

            _initialTab = initialTab
            ShowDialog(frmMain)
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            ThemeManager.ActiveTheme = _originalTheme
            Close()
        End Sub

        Private Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click
            SaveOptions()
            Close()
        End Sub

        Private Sub lvPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lvPages.SelectedIndexChanged
            If tcTabControl.TabPages.Count < 1 Then Return
            If lvPages.SelectedIndices.Count > 0 AndAlso Not tcTabControl.SelectedIndex = lvPages.SelectedIndices(0) Then
                tcTabControl.SelectedIndex = lvPages.SelectedIndices(0)
            End If
            SelectNextControl(tcTabControl.SelectedTab, True, True, True, True)
        End Sub

        Private Sub lvPages_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles lvPages.MouseUp
            If lvPages.SelectedIndices.Count = 0 Then
                lvPages.Items(tcTabControl.SelectedIndex).Selected = True
            End If
            SelectNextControl(tcTabControl.SelectedTab, True, True, True, True)
        End Sub

        Private Sub tcTabControl_SelectedIndexChanged(sender As System.Object, e As EventArgs) Handles tcTabControl.SelectedIndexChanged
            If lvPages.Items.Count < 1 Then Return
            If lvPages.SelectedIndices.Count > 0 Then
                If lvPages.SelectedIndices(0) = tcTabControl.SelectedIndex Then Return
                lvPages.SelectedIndices.Clear()
            End If
            lvPages.SelectedIndices.Add(tcTabControl.SelectedIndex)
        End Sub

        Private Sub radCredentialsCustom_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radCredentialsCustom.CheckedChanged
            lblCredentialsUsername.Enabled = radCredentialsCustom.Checked
            lblCredentialsPassword.Enabled = radCredentialsCustom.Checked
            lblCredentialsDomain.Enabled = radCredentialsCustom.Checked
            txtCredentialsUsername.Enabled = radCredentialsCustom.Checked
            txtCredentialsPassword.Enabled = radCredentialsCustom.Checked
            txtCredentialsDomain.Enabled = radCredentialsCustom.Checked
        End Sub

        Private Sub chkUseSQLServer_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkUseSQLServer.CheckedChanged
            lblSQLServer.Enabled = chkUseSQLServer.Checked
            lblSQLDatabaseName.Enabled = chkUseSQLServer.Checked
            lblSQLUsername.Enabled = chkUseSQLServer.Checked
            lblSQLPassword.Enabled = chkUseSQLServer.Checked
            txtSQLServer.Enabled = chkUseSQLServer.Checked
            txtSQLDatabaseName.Enabled = chkUseSQLServer.Checked
            txtSQLUsername.Enabled = chkUseSQLServer.Checked
            txtSQLPassword.Enabled = chkUseSQLServer.Checked
        End Sub

        Private Sub chkUseOnlyErrorsAndInfosPanel_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkUseOnlyErrorsAndInfosPanel.CheckedChanged
            chkMCInformation.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked
            chkMCWarnings.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked
            chkMCErrors.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked
        End Sub

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

        Private Sub chkCheckForUpdatesOnStartup_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCheckForUpdatesOnStartup.CheckedChanged
            cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked
        End Sub

        Private Sub btnUpdateCheckNow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateCheckNow.Click
            Runtime.Windows.Show(Type.Update)
        End Sub

        Private Sub chkUseProxyForAutomaticUpdates_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkUseProxyForAutomaticUpdates.CheckedChanged
            pnlProxyBasic.Enabled = chkUseProxyForAutomaticUpdates.Checked
            btnTestProxy.Enabled = chkUseProxyForAutomaticUpdates.Checked

            If chkUseProxyForAutomaticUpdates.Checked Then
                chkUseProxyAuthentication.Enabled = True

                If chkUseProxyAuthentication.Checked Then
                    pnlProxyAuthentication.Enabled = True
                End If
            Else
                chkUseProxyAuthentication.Enabled = False
                pnlProxyAuthentication.Enabled = False
            End If
        End Sub

        Private Sub btnTestProxy_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTestProxy.Click
            If _appUpdate IsNot Nothing Then
                If _appUpdate.IsGetUpdateInfoRunning Then Return
            End If

            _appUpdate = New App.Update
            _appUpdate.SetProxySettings(chkUseProxyForAutomaticUpdates.Checked, txtProxyAddress.Text, numProxyPort.Value, chkUseProxyAuthentication.Checked, txtProxyUsername.Text, txtProxyPassword.Text)

            btnTestProxy.Enabled = False
            btnTestProxy.Text = Language.strOptionsProxyTesting

            AddHandler _appUpdate.GetUpdateInfoCompletedEvent, AddressOf GetUpdateInfoCompleted

            _appUpdate.GetUpdateInfoAsync()
        End Sub

        Private Sub GetUpdateInfoCompleted(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
            If InvokeRequired Then
                Dim myDelegate As New AsyncCompletedEventHandler(AddressOf GetUpdateInfoCompleted)
                Invoke(myDelegate, New Object() {sender, e})
                Return
            End If

            Try
                RemoveHandler _appUpdate.GetUpdateInfoCompletedEvent, AddressOf GetUpdateInfoCompleted

                btnTestProxy.Enabled = True
                btnTestProxy.Text = Language.strButtonTestProxy

                If e.Cancelled Then Return
                If e.Error IsNot Nothing Then Throw e.Error

                cTaskDialog.ShowCommandBox(Me, Application.Info.ProductName, Language.strProxyTestSucceeded, "", Language.strButtonOK, False)
            Catch ex As Exception
                cTaskDialog.ShowCommandBox(Me, Application.Info.ProductName, Language.strProxyTestFailed, Misc.GetExceptionMessageRecursive(ex), "", "", "", Language.strButtonOK, False, eSysIcons.Error, 0)
            End Try
        End Sub

        Private Sub chkUseProxyAuthentication_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkUseProxyAuthentication.CheckedChanged
            If chkUseProxyForAutomaticUpdates.Checked Then
                If chkUseProxyAuthentication.Checked Then
                    pnlProxyAuthentication.Enabled = True
                Else
                    pnlProxyAuthentication.Enabled = False
                End If
            End If
        End Sub

        Private Sub cboTheme_DropDown(ByVal sender As Object, ByVal e As EventArgs) Handles cboTheme.DropDown, cboTheme.LostFocus
            If ThemeManager.ActiveTheme Is ThemeManager.DefaultTheme Then Return
            ThemeManager.ActiveTheme.Name = cboTheme.Text
        End Sub

        Private Sub cboTheme_SelectionChangeCommitted(ByVal sender As Object, ByVal e As EventArgs) Handles cboTheme.SelectionChangeCommitted ', cboTheme.SelectedIndexChanged, cboTheme.SelectedValueChanged
            If cboTheme.SelectedItem Is Nothing Then cboTheme.SelectedItem = ThemeManager.DefaultTheme

            If cboTheme.SelectedItem Is ThemeManager.DefaultTheme Then
                cboTheme.DropDownStyle = ComboBoxStyle.DropDownList
                btnThemeDelete.Enabled = False
                ThemePropertyGrid.Enabled = False
            Else
                cboTheme.DropDownStyle = ComboBoxStyle.DropDown
                btnThemeDelete.Enabled = True
                ThemePropertyGrid.Enabled = True
            End If

            ThemeManager.ActiveTheme = cboTheme.SelectedItem
            ThemePropertyGrid.SelectedObject = ThemeManager.ActiveTheme
            ThemePropertyGrid.Refresh()
        End Sub

        Private Sub btnThemeNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnThemeNew.Click
            Dim newTheme As ThemeInfo = ThemeManager.ActiveTheme.Clone()
            newTheme.Name = Language.strUnnamedTheme

            _themeList.Add(newTheme)

            cboTheme.SelectedItem = newTheme
            cboTheme_SelectionChangeCommitted(Me, New EventArgs())

            cboTheme.Focus()
        End Sub

        Private Sub btnThemeDelete_Click(sender As Object, e As EventArgs) Handles btnThemeDelete.Click
            Dim theme As ThemeInfo = cboTheme.SelectedItem
            If theme Is Nothing Then Return

            _themeList.Remove(theme)

            cboTheme.SelectedItem = ThemeManager.DefaultTheme
            cboTheme_SelectionChangeCommitted(Me, New EventArgs())
        End Sub
#End Region

        Private Sub lvKeyboardCommands_SelectedIndexChanged(sender As System.Object, e As EventArgs) Handles lvKeyboardCommands.SelectedIndexChanged
            Dim isItemSelected As Boolean = (lvKeyboardCommands.SelectedItems.Count = 1)
            EnableKeyboardShortcutControls(isItemSelected)

            If Not isItemSelected Then Return

            Dim selectedItem As ListViewItem = lvKeyboardCommands.SelectedItems(0)

            lblKeyboardCommand.Text = selectedItem.Text
            lstKeyboardShortcuts.Items.Clear()

            lstKeyboardShortcuts.Items.AddRange(_keyboardShortcutMap.GetShortcutKeys(GetSelectedShortcutCommand()))

            If lstKeyboardShortcuts.Items.Count > 0 Then lstKeyboardShortcuts.SelectedIndex = 0
        End Sub

        Private Function GetSelectedShortcutCommand() As ShortcutCommand
            If Not (lvKeyboardCommands.SelectedItems.Count = 1) Then Return ShortcutCommand.None

            Dim selectedItem As ListViewItem = lvKeyboardCommands.SelectedItems(0)
            If selectedItem Is _previousTabListViewItem Then
                Return ShortcutCommand.PreviousTab
            ElseIf selectedItem Is _nextTabListViewItem Then
                Return ShortcutCommand.NextTab
            End If
        End Function

        Private Sub EnableKeyboardShortcutControls(Optional ByVal enable As Boolean = True)
            lblKeyboardCommand.Visible = enable
            lblKeyboardShortcuts.Enabled = enable
            lstKeyboardShortcuts.Enabled = enable
            btnNewKeyboardShortcut.Enabled = enable
            btnResetKeyboardShortcuts.Enabled = enable

            If Not enable Then
                btnDeleteKeyboardShortcut.Enabled = False
                grpModifyKeyboardShortcut.Enabled = False
                hotModifyKeyboardShortcut.Enabled = False

                lstKeyboardShortcuts.Items.Clear()
                hotModifyKeyboardShortcut.Text = String.Empty
            End If
        End Sub

        Private Sub lstKeyboardShortcuts_SelectedIndexChanged(sender As System.Object, e As EventArgs) Handles lstKeyboardShortcuts.SelectedIndexChanged
            Dim isItemSelected As Boolean = (lstKeyboardShortcuts.SelectedItems.Count = 1)

            btnDeleteKeyboardShortcut.Enabled = isItemSelected
            grpModifyKeyboardShortcut.Enabled = isItemSelected
            hotModifyKeyboardShortcut.Enabled = isItemSelected

            If Not isItemSelected Then
                hotModifyKeyboardShortcut.Text = String.Empty
                Return
            End If

            Dim selectedItem As Object = lstKeyboardShortcuts.SelectedItems(0)
            Dim shortcutKey As ShortcutKey = TryCast(selectedItem, ShortcutKey)
            If shortcutKey Is Nothing Then Return

            Dim keysValue As Keys = shortcutKey
            Dim keyCode As Keys = keysValue And Keys.KeyCode
            Dim modifiers As Keys = keysValue And Keys.Modifiers

            _ignoreKeyboardShortcutTextChanged = True
            hotModifyKeyboardShortcut.KeyCode = keyCode
            hotModifyKeyboardShortcut.HotkeyModifiers = modifiers
            _ignoreKeyboardShortcutTextChanged = False
        End Sub

        Private Sub btnNewKeyboardShortcut_Click(sender As System.Object, e As EventArgs) Handles btnNewKeyboardShortcut.Click
            For Each item As Object In lstKeyboardShortcuts.Items
                Dim shortcutKey As ShortcutKey = TryCast(item, ShortcutKey)
                If shortcutKey Is Nothing Then Continue For
                If shortcutKey = 0 Then
                    lstKeyboardShortcuts.SelectedItem = item
                    Return
                End If
            Next

            lstKeyboardShortcuts.SelectedIndex = lstKeyboardShortcuts.Items.Add(New ShortcutKey(Keys.None))
            hotModifyKeyboardShortcut.Focus()
        End Sub

        Private Sub btnDeleteKeyboardShortcut_Click(sender As System.Object, e As EventArgs) Handles btnDeleteKeyboardShortcut.Click
            Dim selectedIndex As Integer = lstKeyboardShortcuts.SelectedIndex

            Dim command As ShortcutCommand = GetSelectedShortcutCommand()
            Dim key As ShortcutKey = TryCast(lstKeyboardShortcuts.SelectedItem, ShortcutKey)
            If Not command = ShortcutCommand.None And key IsNot Nothing Then
                _keyboardShortcutMap.Remove(GetSelectedShortcutCommand(), key)
            End If

            lstKeyboardShortcuts.Items.Remove(lstKeyboardShortcuts.SelectedItem)

            If selectedIndex >= lstKeyboardShortcuts.Items.Count Then selectedIndex = lstKeyboardShortcuts.Items.Count - 1
            lstKeyboardShortcuts.SelectedIndex = selectedIndex
        End Sub

        Private Sub btnResetAllKeyboardShortcuts_Click(sender As System.Object, e As System.EventArgs) Handles btnResetAllKeyboardShortcuts.Click
            _keyboardShortcutMap = KeyboardShortcuts.DefaultMap.Clone()
            lvKeyboardCommands_SelectedIndexChanged(Me, New EventArgs())
        End Sub

        Private Sub btnResetKeyboardShortcuts_Click(sender As System.Object, e As EventArgs) Handles btnResetKeyboardShortcuts.Click
            Dim command As ShortcutCommand = GetSelectedShortcutCommand()
            If command = ShortcutCommand.None Then Return
            _keyboardShortcutMap.SetShortcutKeys(command, KeyboardShortcuts.DefaultMap.GetShortcutKeys(command))
            lvKeyboardCommands_SelectedIndexChanged(Me, New EventArgs())
        End Sub

        Private Sub hotModifyKeyboardShortcut_TextChanged(sender As System.Object, e As EventArgs) Handles hotModifyKeyboardShortcut.TextChanged
            If _ignoreKeyboardShortcutTextChanged Or _
                lstKeyboardShortcuts.SelectedIndex < 0 Or _
                lstKeyboardShortcuts.SelectedIndex >= lstKeyboardShortcuts.Items.Count Then Return

            Dim keysValue As Keys = (hotModifyKeyboardShortcut.KeyCode And Keys.KeyCode) Or _
                                    (hotModifyKeyboardShortcut.HotkeyModifiers And Keys.Modifiers)

            Dim hadFocus As Boolean = hotModifyKeyboardShortcut.ContainsFocus

            Dim command As ShortcutCommand = GetSelectedShortcutCommand()
            Dim newShortcutKey As New ShortcutKey(keysValue)

            If Not command = ShortcutCommand.None Then
                Dim oldShortcutKey As ShortcutKey = TryCast(lstKeyboardShortcuts.SelectedItem, ShortcutKey)
                If oldShortcutKey IsNot Nothing Then
                    _keyboardShortcutMap.Remove(command, oldShortcutKey)
                End If
                _keyboardShortcutMap.Add(command, newShortcutKey)
            End If

            lstKeyboardShortcuts.Items(lstKeyboardShortcuts.SelectedIndex) = newShortcutKey

            If hadFocus Then
                hotModifyKeyboardShortcut.Focus()
                hotModifyKeyboardShortcut.Select(hotModifyKeyboardShortcut.TextLength, 0)
            End If
        End Sub
    End Class
End Namespace