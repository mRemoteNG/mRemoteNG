' The following events are available for MyApplication:
' 
' Startup: Raised when the application starts, before the startup form is created.
' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
' UnhandledException: Raised if the application encounters an unhandled exception.
' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
Imports System.Threading
Imports mRemote3G.App

Partial Friend Class MyApplication
    Public mutex As Mutex

    Private Function GetCurrentInstanceWindowHandle() As IntPtr
        Dim hWnd As IntPtr = IntPtr.Zero
        Dim curProc As Process = Process.GetCurrentProcess

        For Each proc As Process In Process.GetProcessesByName(curProc.ProcessName)
            If _
                proc.Id <> curProc.Id And proc.MainModule.FileName = curProc.MainModule.FileName And
                proc.MainWindowHandle <> IntPtr.Zero Then
                hWnd = proc.MainWindowHandle
                Exit For
            End If
        Next

        Return hWnd
    End Function

    Private Sub SwitchToCurrentInstance()
        Dim hWnd As IntPtr = GetCurrentInstanceWindowHandle()

        If hWnd <> IntPtr.Zero Then
            'Restore window if minimized. Do not restore if already in
            'normal or maximised window state, since we don't want to
            'change the current state of the window.
            If NativeMethods.IsIconic(hWnd) <> 0 Then
                NativeMethods.ShowWindow(hWnd, NativeMethods.SW_RESTORE)
            End If

            NativeMethods.SetForegroundWindow(hWnd)
        End If
    End Sub
End Class