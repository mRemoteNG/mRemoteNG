Imports System.IO
Imports Microsoft.Win32

Namespace Tools
    Public Class IeBrowserEmulation
        Private Const BrowserEmulationKey As String = "Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"
        Private Shared _previousIeBrowserEmulationValue As Integer = 0
        Public Shared Sub Register()
            Dim registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, True)
            If registryKey Is Nothing Then
                Registry.CurrentUser.CreateSubKey(BrowserEmulationKey)
                registryKey = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, True)
                If registryKey Is Nothing Then Return
            End If
            Dim exeName As String = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName)
            _previousIeBrowserEmulationValue = registryKey.GetValue(exeName, 0)
            registryKey.SetValue(exeName, 11000, RegistryValueKind.DWord)
        End Sub

        Public Shared Sub Unregister()
#If Not PORTABLE Then
            Return
#Else
            Try
                Dim registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, True)
                If registryKey Is Nothing Then Return
                Dim exeName As String = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName)
                If _previousIeBrowserEmulationValue = 0 Then
                    registryKey.DeleteValue(exeName)
                Else
                    registryKey.SetValue(exeName, _previousIeBrowserEmulationValue, RegistryValueKind.DWord)
                End If
            Catch
            End Try
#End If
        End Sub
    End Class
End Namespace

