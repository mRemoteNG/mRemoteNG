Imports mRemoteNG.App.Native
Imports System.Threading
Imports mRemoteNG.App.Runtime
Imports mRemoteNG.Tools

Namespace Connection
    Namespace Protocol
        Public Class vSphere
            Inherits Base
#Region "Public Methods"
            Public Overrides Function SetProps() As Boolean

                Return MyBase.SetProps()
            End Function

            Public Overrides Function Connect() As Boolean
                Try

                    _process = New Process()
                    Dim utils As App.Utils = New App.Utils()
                    Dim win As KeyValuePair(Of IntPtr, String)

                    With _process.StartInfo
                        .UseShellExecute = True
                        .FileName = "C:\Program Files (x86)\VMware\Infrastructure\Virtual Infrastructure Client\Launcher\VpxClient.exe"
                        .Arguments = "-s " + InterfaceControl.Info.Hostname + " -u " + InterfaceControl.Info.Username + " -p " + InterfaceControl.Info.Password
                        .WindowStyle = ProcessWindowStyle.Minimized
                    End With

                    _process.EnableRaisingEvents = True
                    AddHandler _process.Exited, AddressOf ProcessExited

                    _process.Start()
                    _process.WaitForInputIdle(My.Settings.MaxPuttyWaitTime * 1000)

                    Dim startTicks As Integer = Environment.TickCount
                    REM Ecran de connexion
                    While _handle.ToInt32 = 0 And Environment.TickCount < startTicks + ((My.Settings.MaxPuttyWaitTime) * 1000)
                        _process.Refresh()
                        If Not (_process.MainWindowTitle = "Default IME") Then
                            _handle = _process.MainWindowHandle
                        End If
                        If _handle.ToInt32 = 0 Then Thread.Sleep(0)
                    End While

                    SetParent(_handle, InterfaceControl.Handle)
                    Resize(Me, New EventArgs)

                    REM Main screen
                    Dim child As Integer = 0
                    While child = 0 And Environment.TickCount < startTicks + ((My.Settings.MaxPuttyWaitTime + 10) * 1000)
                        _process.Refresh()
                        If Not (_process.MainWindowTitle = "Default IME") Then
                            For Each win In utils.GetOpenWindowsFromPID(_process.Id)
                                If Not win.Value = "VMware vSphere Client" Then
                                    _handle = win.Key
                                    child = 1
                                End If
                            Next
                        End If
                        If child = 0 Then Thread.Sleep(0)
                    End While

                    Console.WriteLine("End")
                    SetParent(_handle, InterfaceControl.Handle)

                    MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Language.strIntAppStuff, True)

                    MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format(My.Language.strIntAppHandle, _handle.ToString), True)
                    MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format(My.Language.strIntAppTitle, _process.MainWindowTitle), True)
                    MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format(My.Language.strIntAppParentHandle, InterfaceControl.Parent.Handle.ToString), True)

                    Resize(Me, New EventArgs)

                    MyBase.Connect()
                    Return True
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage(My.Language.strIntAppConnectionFailed, ex)
                    Return False
                End Try
            End Function

            Public Overrides Sub Focus()
                Try
                    If ConnectionWindow.InTabDrag Then Return
                    SetForegroundWindow(_handle)
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage(My.Language.strIntAppFocusFailed, ex, , True)
                End Try
            End Sub

            Public Overrides Sub Resize(ByVal sender As Object, ByVal e As EventArgs)
                Try
                    If InterfaceControl.Size = Size.Empty Then Return
                    MoveWindow(_handle, -SystemInformation.FrameBorderSize.Width, -(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height), InterfaceControl.Width + (SystemInformation.FrameBorderSize.Width * 2), InterfaceControl.Height + SystemInformation.CaptionHeight + (SystemInformation.FrameBorderSize.Height * 2), True)
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage(My.Language.strIntAppResizeFailed, ex, , True)
                End Try
            End Sub

            Public Overrides Sub Close()
                Try
                    If Not _process.HasExited Then _process.Kill()
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage(My.Language.strIntAppKillFailed, ex, , True)
                End Try

                Try
                    If Not _process.HasExited Then _process.Dispose()
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage(My.Language.strIntAppDisposeFailed, ex, , True)
                End Try

                MyBase.Close()
            End Sub
#End Region

#Region "Private Fields"
            Private _handle As IntPtr
            Private _process As Process
#End Region

#Region "Private Methods"
            Private Sub ProcessExited(ByVal sender As Object, ByVal e As EventArgs)
                Event_Closed(Me)
            End Sub
#End Region

#Region "Enumerations"
            Public Enum Defaults
                Port = 0
            End Enum
#End Region
        End Class
    End Namespace
End Namespace
