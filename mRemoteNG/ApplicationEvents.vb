Namespace My
    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Public mutex As System.Threading.Mutex

        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            If My.Settings.SingleInstance Then
                Dim mutexID As String = "mRemoteNG_SingleInstanceMutex"

                mutex = New System.Threading.Mutex(False, mutexID)

                If Not mutex.WaitOne(0, False) Then
                    Try
                        SwitchToCurrentInstance()
                    Catch ex As Exception
                    End Try

                    End
                End If

                GC.KeepAlive(mutex)
            End If
        End Sub

        Private Function GetCurrentInstanceWindowHandle() As IntPtr
            Dim hWnd As IntPtr = IntPtr.Zero
            Dim curProc As Process = Process.GetCurrentProcess

            For Each proc As Process In Process.GetProcessesByName(curProc.ProcessName)
                If proc.Id <> curProc.Id And proc.MainModule.FileName = curProc.MainModule.FileName And proc.MainWindowHandle <> IntPtr.Zero Then
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
                If App.Native.IsIconic(hWnd) <> 0 Then
                    App.Native.ShowWindow(hWnd, App.Native.SW_RESTORE)
                End If

                App.Native.SetForegroundWindow(hWnd)
            End If
        End Sub

        Private Sub MyApplication_Shutdown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shutdown
            If mutex IsNot Nothing Then
                mutex.Close()
            End If
        End Sub
    End Class
End Namespace

