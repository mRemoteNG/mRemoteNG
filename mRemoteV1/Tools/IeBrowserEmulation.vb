Imports System.IO
Imports System.Security.AccessControl
Imports Microsoft.Win32
Imports mRemote3G.App

Namespace Tools
    Public Class IeBrowserEmulation
        ' found this here:
        ' http://www.neowin.net/forum/topic/1077469-vbnet-webbrowser-control-does-not-load-javascript/#comment-596755046

        Private Shared Sub SetBrowserFeatureControlKey(feature As String, appName As String, value As UInteger)
            If Environment.Is64BitOperatingSystem Then

                Using _
                    key As RegistryKey =
                        Registry.CurrentUser.CreateSubKey(
                            [String].Concat("Software\Wow6432Node\Microsoft\Internet Explorer\Main\FeatureControl\",
                                            feature), RegistryKeyPermissionCheck.ReadWriteSubTree)
                    key.SetValue(appName, value, RegistryValueKind.DWord)
                End Using
            End If


            Using _
                key As RegistryKey =
                    Registry.CurrentUser.CreateSubKey(
                        [String].Concat("Software\Microsoft\Internet Explorer\Main\FeatureControl\", feature),
                        RegistryKeyPermissionCheck.ReadWriteSubTree)
                key.SetValue(appName, value, RegistryValueKind.DWord)
            End Using
        End Sub

        Private Shared Sub DeleteBrowserFeatureControlKey(feature As String, appName As String)
            If Environment.Is64BitOperatingSystem Then

                Using _
                    key As RegistryKey =
                        Registry.CurrentUser.OpenSubKey(
                            [String].Concat("Software\Wow6432Node\Microsoft\Internet Explorer\Main\FeatureControl\",
                                            feature), RegistryKeyPermissionCheck.ReadWriteSubTree)
                    key.DeleteValue(appName)
                End Using
            End If


            Using _
                key As RegistryKey =
                    Registry.CurrentUser.CreateSubKey(
                        [String].Concat("Software\Microsoft\Internet Explorer\Main\FeatureControl\", feature),
                        RegistryKeyPermissionCheck.ReadWriteSubTree)
                key.DeleteValue(appName)
            End Using
        End Sub

        Private Shared Sub SetBrowserFeatureControl()
            ' http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx

            ' FeatureControl settings are per-process
            Dim fileName As String = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName)

            ' make the control is not running inside Visual Studio Designer
            If _
                [String].Compare(fileName, "devenv.exe", True) = 0 OrElse
                [String].Compare(fileName, "XDesProc.exe", True) = 0 Then
                Return
            End If

            SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName, GetBrowserEmulationMode())
            ' Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode.
            SetBrowserFeatureControlKey("FEATURE_AJAX_CONNECTIONEVENTS", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_DOMSTORAGE", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_GPU_RENDERING", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI", fileName, 0)
            SetBrowserFeatureControlKey("FEATURE_DISABLE_LEGACY_COMPRESSION", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_LOCALMACHINE_LOCKDOWN", fileName, 0)
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_OBJECT", fileName, 0)
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_SCRIPT", fileName, 0)
            SetBrowserFeatureControlKey("FEATURE_DISABLE_NAVIGATION_SOUNDS", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_SCRIPTURL_MITIGATION", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_SPELLCHECKING", fileName, 0)
            SetBrowserFeatureControlKey("FEATURE_STATUS_BAR_THROTTLING", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_TABBED_BROWSING", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_VALIDATE_NAVIGATE_URL", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_WEBOC_DOCUMENT_ZOOM", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_WEBOC_POPUPMANAGEMENT", fileName, 0)
            SetBrowserFeatureControlKey("FEATURE_WEBOC_MOVESIZECHILD", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_ADDON_MANAGEMENT", fileName, 0)
            SetBrowserFeatureControlKey("FEATURE_WEBSOCKET", fileName, 1)
            SetBrowserFeatureControlKey("FEATURE_WINDOW_RESTRICTIONS", fileName, 0)
            SetBrowserFeatureControlKey("FEATURE_XMLHTTP", fileName, 1)
        End Sub

        Private Shared Sub DeleteBrowserFeatureControl()
            ' http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx

            ' FeatureControl settings are per-process
            Dim fileName As String = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName)

            ' make the control is not running inside Visual Studio Designer
            If _
                [String].Compare(fileName, "devenv.exe", True) = 0 OrElse
                [String].Compare(fileName, "XDesProc.exe", True) = 0 Then
                Return
            End If

            DeleteBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName)
            ' Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode.
            DeleteBrowserFeatureControlKey("FEATURE_AJAX_CONNECTIONEVENTS", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_DOMSTORAGE", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_GPU_RENDERING", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_DISABLE_LEGACY_COMPRESSION", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_LOCALMACHINE_LOCKDOWN", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_OBJECT", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_SCRIPT", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_DISABLE_NAVIGATION_SOUNDS", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_SCRIPTURL_MITIGATION", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_SPELLCHECKING", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_STATUS_BAR_THROTTLING", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_TABBED_BROWSING", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_VALIDATE_NAVIGATE_URL", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_WEBOC_DOCUMENT_ZOOM", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_WEBOC_POPUPMANAGEMENT", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_WEBOC_MOVESIZECHILD", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_ADDON_MANAGEMENT", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_WEBSOCKET", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_WINDOW_RESTRICTIONS", fileName)
            DeleteBrowserFeatureControlKey("FEATURE_XMLHTTP", fileName)
        End Sub

        Private Shared Function GetBrowserEmulationMode() As UInt32
            ' https://msdn.microsoft.com/en-us/library/ee330730%28v=vs.85%29.aspx

            Dim browserVersion = 9 ' default to IE9.

            Using _
                ieKey As RegistryKey =
                    Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Internet Explorer",
                                                     RegistryKeyPermissionCheck.ReadSubTree, RegistryRights.QueryValues)
                Dim version As Object = ieKey.GetValue("svcVersion")
                If Nothing = version Then
                    version = ieKey.GetValue("Version")
                    If Nothing = version Then
                        Throw New ApplicationException("Microsoft Internet Explorer is required!")
                    End If
                End If
                Integer.TryParse(version.ToString().Split("."c)(0), browserVersion)
            End Using

            Dim mode As UInt32 = 9000 ' default to IE9.

            Select Case browserVersion
                ' https://support.microsoft.com/en-us/lifecycle#gp/Microsoft-Internet-Explorer
                ' IE 7 & 8 are basically not supported any more...
#If OLD_BROWSERS Then
                Case 7
                    mode = 7000
                    ' Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode. Default value for applications hosting the WebBrowser Control.
                    Exit Select
                Case 8
                    mode = 8000
                    ' Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode. Default value for Internet Explorer 8
                    Exit Select
#End If
                Case 9
                    mode = 9000
                    ' Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode. Default value for Internet Explorer 9.
                    Exit Select
                Case 10
                    mode = 10000
                    ' Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode. Default value for Internet Explorer 10.
                    Exit Select
                Case 11
                    mode = 11000
                    ' IE11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 edge mode. Default value for IE11.
                    Exit Select
                Case Else
                    ' use IE10 mode by default
                    Exit Select
            End Select

            Return mode
        End Function

        'Private Const BrowserEmulationKey As String = "Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"
        'Private Shared _previousIeBrowserEmulationValue As Integer = 0
        Public Shared Sub Register()
            Try
                SetBrowserFeatureControl()
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage("IeBrowserEmulation.Register() failed.", ex, , True)
            End Try
        End Sub

        Public Shared Sub Unregister()
#If PORTABLE Then
            Try
                DeleteBrowserFeatureControl()
            Catch ex As Exception
                MessageCollector.AddExceptionMessage("IeBrowserEmulation.Unregister() failed.", ex, , True)
            End Try
#End If
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace

