Imports System.Threading
Imports mRemote3G.App
Imports mRemote3G.Messages
Imports mRemote3G.My
Imports mRemote3G.Tools

Namespace Connection

    Namespace Protocol
        Public Class IntegratedProgram
            Inherits Base

#Region "Public Methods"

            Public Overrides Function SetProps() As Boolean
                If InterfaceControl.Info IsNot Nothing Then
                    _externalTool = Runtime.GetExtAppByName(InterfaceControl.Info.ExtApp)
                    _externalTool.ConnectionInfo = InterfaceControl.Info
                End If

                Return MyBase.SetProps()
            End Function

            Public Overrides Function Connect() As Boolean
                Try
                    If _externalTool.TryIntegrate = False Then
                        _externalTool.Start(InterfaceControl.Info)
                        Close()
                        Return Nothing
                    End If

                    _process = New Process()

                    With _process.StartInfo
                        .UseShellExecute = True
                        .FileName = _externalTool.ParseArguments(_externalTool.FileName)
                        .Arguments = _externalTool.ParseArguments(_externalTool.Arguments)
                    End With

                    _process.EnableRaisingEvents = True
                    AddHandler _process.Exited, AddressOf ProcessExited

                    _process.Start()
                    _process.WaitForInputIdle(Settings.MaxPuttyWaitTime*1000)

                    Dim startTicks As Integer = Environment.TickCount
                    While _handle.ToInt32 = 0 And Environment.TickCount < startTicks + (Settings.MaxPuttyWaitTime*1000)
                        _process.Refresh()
                        If Not _process.MainWindowTitle = "Default IME" Then _handle = _process.MainWindowHandle
                        If _handle.ToInt32 = 0 Then Thread.Sleep(0)
                    End While

                    NativeMethods.SetParent(_handle, InterfaceControl.Handle)

                   App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, Language.Language.strIntAppStuff, True)

                   App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format(Language.Language.strIntAppHandle, _handle.ToString), True)
                   App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format(Language.Language.strIntAppTitle, _process.MainWindowTitle), True)
                   App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format(Language.Language.strIntAppParentHandle, InterfaceControl.Parent.Handle.ToString), True)

                    Resize(Me, New EventArgs)

                    MyBase.Connect()
                    Return True
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage(Language.Language.strIntAppConnectionFailed, ex)
                    Return False
                End Try
            End Function

            Public Overrides Sub Focus()
                Try
                    If ConnectionWindow.InTabDrag Then Return
                    NativeMethods.SetForegroundWindow(_handle)
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage(Language.Language.strIntAppFocusFailed, ex, , True)
                End Try
            End Sub

            Public Overrides Sub Resize(sender As Object, e As EventArgs)
                Try
                    If InterfaceControl.Size = Size.Empty Then Return
                    NativeMethods.MoveWindow(_handle, -SystemInformation.FrameBorderSize.Width,
                                             (SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height),
                                             InterfaceControl.Width + (SystemInformation.FrameBorderSize.Width * 2),
                                             InterfaceControl.Height + SystemInformation.CaptionHeight +
                                             (SystemInformation.FrameBorderSize.Height * 2), True)
                Catch ex As Exception
                   App.Runtime.MessageCollector.AddExceptionMessage(Language.Language.strIntAppResizeFailed, ex, , True)
                End Try
            End Sub

            Public Overrides Sub Close()
                Try
                    If Not _process.HasExited Then _process.Kill()
                Catch ex As Exception
                   App.Runtime.MessageCollector.AddExceptionMessage(Language.Language.strIntAppKillFailed, ex, , True)
                End Try

                Try
                    If Not _process.HasExited Then _process.Dispose()
                Catch ex As Exception
                   App.Runtime.MessageCollector.AddExceptionMessage(Language.Language.strIntAppDisposeFailed, ex, , True)
                End Try

                MyBase.Close()
            End Sub

#End Region

#Region "Private Fields"

            Private _externalTool As ExternalTool
            Private _handle As IntPtr
            Private _process As Process

#End Region

#Region "Private Methods"

            Private Sub ProcessExited(sender As Object, e As EventArgs)
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
