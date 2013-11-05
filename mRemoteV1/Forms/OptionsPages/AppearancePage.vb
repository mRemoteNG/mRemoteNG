Imports mRemoteNG.App
Imports mRemoteNG.My

Namespace Forms.OptionsPages
    Public Class AppearancePage
        Public Overrides Property PageName() As String
            Get
                Return Language.strTabAppearance
            End Get
            Set(value As String)
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            MyBase.ApplyLanguage()

            lblLanguage.Text = Language.strLanguage
            lblLanguageRestartRequired.Text = String.Format(Language.strLanguageRestartRequired, Application.Info.ProductName)
            chkShowDescriptionTooltipsInTree.Text = Language.strShowDescriptionTooltips
            chkShowFullConnectionsFilePathInTitle.Text = Language.strShowFullConsFilePath
            chkShowSystemTrayIcon.Text = Language.strAlwaysShowSysTrayIcon
            chkMinimizeToSystemTray.Text = Language.strMinimizeToSysTray
        End Sub

        Public Overrides Sub LoadSettings()
            MyBase.SaveSettings()

            cboLanguage.Items.Clear()
            cboLanguage.Items.Add(Language.strLanguageDefault)

            For Each nativeName As String In SupportedCultures.CultureNativeNames
                cboLanguage.Items.Add(nativeName)
            Next
            If Not String.IsNullOrEmpty(Settings.OverrideUICulture) AndAlso SupportedCultures.IsNameSupported(Settings.OverrideUICulture) Then
                cboLanguage.SelectedItem = SupportedCultures.CultureNativeName(Settings.OverrideUICulture)
            End If
            If cboLanguage.SelectedIndex = -1 Then
                cboLanguage.SelectedIndex = 0
            End If

            chkShowDescriptionTooltipsInTree.Checked = Settings.ShowDescriptionTooltipsInTree
            chkShowFullConnectionsFilePathInTitle.Checked = Settings.ShowCompleteConsPathInTitle
            chkShowSystemTrayIcon.Checked = Settings.ShowSystemTrayIcon
            chkMinimizeToSystemTray.Checked = Settings.MinimizeToTray
        End Sub

        Public Overrides Sub SaveSettings()
            MyBase.SaveSettings()

            If cboLanguage.SelectedIndex > 0 And SupportedCultures.IsNativeNameSupported(cboLanguage.SelectedItem) Then
                Settings.OverrideUICulture = SupportedCultures.CultureName(cboLanguage.SelectedItem)
            Else
                Settings.OverrideUICulture = String.Empty
            End If

            Settings.ShowDescriptionTooltipsInTree = chkShowDescriptionTooltipsInTree.Checked
            Settings.ShowCompleteConsPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked

            Settings.ShowSystemTrayIcon = chkShowSystemTrayIcon.Checked
            If Settings.ShowSystemTrayIcon Then
                If Runtime.NotificationAreaIcon Is Nothing Then
                    Runtime.NotificationAreaIcon = New Tools.Controls.NotificationAreaIcon
                End If
            Else
                If Runtime.NotificationAreaIcon IsNot Nothing Then
                    Runtime.NotificationAreaIcon.Dispose()
                    Runtime.NotificationAreaIcon = Nothing
                End If
            End If

            Settings.MinimizeToTray = chkMinimizeToSystemTray.Checked
        End Sub
    End Class
End Namespace