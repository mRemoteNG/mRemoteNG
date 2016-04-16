Imports mRemote3G.App

Namespace Forms.OptionsPages
    Public Class AppearancePage
        Public Overrides Property PageName As String
            Get
                Return Language.Language.strTabAppearance
            End Get
            Set
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            MyBase.ApplyLanguage()

            lblLanguage.Text = Language.Language.strLanguage
            lblLanguageRestartRequired.Text = String.Format(Language.Language.strLanguageRestartRequired,
                                                            Application.ProductName)
            chkShowDescriptionTooltipsInTree.Text = Language.Language.strShowDescriptionTooltips
            chkShowFullConnectionsFilePathInTitle.Text = Language.Language.strShowFullConsFilePath
            chkShowSystemTrayIcon.Text = Language.Language.strAlwaysShowSysTrayIcon
            chkMinimizeToSystemTray.Text = Language.Language.strMinimizeToSysTray
        End Sub

        Public Overrides Sub LoadSettings()
            MyBase.SaveSettings()

            cboLanguage.Items.Clear()
            cboLanguage.Items.Add(Language.Language.strLanguageDefault)

            For Each nativeName As String In SupportedCultures.CultureNativeNames
                cboLanguage.Items.Add(nativeName)
            Next
            If _
                Not String.IsNullOrEmpty(My.Settings.OverrideUICulture) AndAlso
                SupportedCultures.IsNameSupported(My.Settings.OverrideUICulture) Then
                cboLanguage.SelectedItem = SupportedCultures.CultureNativeName(My.Settings.OverrideUICulture)
            End If
            If cboLanguage.SelectedIndex = - 1 Then
                cboLanguage.SelectedIndex = 0
            End If

            chkShowDescriptionTooltipsInTree.Checked = My.Settings.ShowDescriptionTooltipsInTree
            chkShowFullConnectionsFilePathInTitle.Checked = My.Settings.ShowCompleteConsPathInTitle
            chkShowSystemTrayIcon.Checked = My.Settings.ShowSystemTrayIcon
            chkMinimizeToSystemTray.Checked = My.Settings.MinimizeToTray
        End Sub

        Public Overrides Sub SaveSettings()
            MyBase.SaveSettings()

            If cboLanguage.SelectedIndex > 0 And SupportedCultures.IsNativeNameSupported(cboLanguage.SelectedItem) Then
                My.Settings.OverrideUICulture = SupportedCultures.CultureName(cboLanguage.SelectedItem)
            Else
                My.Settings.OverrideUICulture = String.Empty
            End If

            My.Settings.ShowDescriptionTooltipsInTree = chkShowDescriptionTooltipsInTree.Checked
            My.Settings.ShowCompleteConsPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked
            frmMain.ShowFullPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked

            My.Settings.ShowSystemTrayIcon = chkShowSystemTrayIcon.Checked
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

            My.Settings.MinimizeToTray = chkMinimizeToSystemTray.Checked
        End Sub
    End Class
End Namespace