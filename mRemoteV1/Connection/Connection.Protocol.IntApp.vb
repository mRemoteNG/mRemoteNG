Imports mRemoteNG.App.Native
Imports System.Threading
Imports mRemoteNG.App.Runtime

Namespace Connection
    Namespace Protocol
        Public Class IntApp
            Inherits Connection.Protocol.Base

#Region "Private Properties"
            Private IntAppProcessStartInfo As New ProcessStartInfo()
            Private Arguments As String
            Private ExtApp As Tools.ExternalApp
#End Region

#Region "Public Properties"
            Private _IntAppHandle As IntPtr
            Public Property IntAppHandle() As IntPtr
                Get
                    Return Me._IntAppHandle
                End Get
                Set(ByVal value As IntPtr)
                    Me._IntAppHandle = value
                End Set
            End Property

            Private _IntAppProcess As Process
            Public Property IntAppProcess() As Process
                Get
                    Return Me._IntAppProcess
                End Get
                Set(ByVal value As Process)
                    Me._IntAppProcess = value
                End Set
            End Property

            Private _IntAppPath As String
            Public Property IntAppPath() As String
                Get
                    Return _IntAppPath
                End Get
                Set(ByVal value As String)
                    _IntAppPath = value
                End Set
            End Property

            Public ReadOnly Property Focused() As Boolean
                Get
                    If GetForegroundWindow() = IntAppHandle Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
            End Property
#End Region

#Region "Private Events & Handlers"
            Private Sub ProcessExited(ByVal sender As Object, ByVal e As System.EventArgs)
                MyBase.Event_Closed(Me)
            End Sub
#End Region

#Region "Public Methods"
            Public Sub New()

            End Sub

            Public Overrides Function SetProps() As Boolean
                ExtApp = App.Runtime.GetExtAppByName(InterfaceControl.Info.ExtApp)
                If InterfaceControl.Info IsNot Nothing Then
                    ExtApp.ConnectionInfo = InterfaceControl.Info
                End If

                _IntAppPath = ExtApp.ParseText(ExtApp.FileName)
                Arguments = ExtApp.ParseText(ExtApp.Arguments)

                Return MyBase.SetProps()
            End Function

            Public Overrides Function Connect() As Boolean
                Try
                    If ExtApp.TryIntegrate = False Then
                        ExtApp.Start(Me.InterfaceControl.Info)
                        Me.Close()
                        Return Nothing
                    End If

                    IntAppProcessStartInfo.FileName = _IntAppPath
                    IntAppProcessStartInfo.Arguments = Arguments

                    IntAppProcess = Process.Start(IntAppProcessStartInfo)
                    IntAppProcess.EnableRaisingEvents = True
                    IntAppProcess.WaitForInputIdle()

                    AddHandler IntAppProcess.Exited, AddressOf ProcessExited

                    Dim TryCount As Integer = 0
                    Do Until IntAppProcess.MainWindowHandle <> IntPtr.Zero And Me.InterfaceControl.Handle <> IntPtr.Zero And Me.IntAppProcess.MainWindowTitle <> "Default IME"
                        If TryCount >= My.Settings.MaxPuttyWaitTime * 2 Then
                            Exit Do
                        End If

                        IntAppProcess.Refresh()

                        Thread.Sleep(500)

                        TryCount += 1
                    Loop

                    IntAppHandle = IntAppProcess.MainWindowHandle

                    MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Resources.strIntAppStuff, True)

                    MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format(My.Resources.strIntAppHandle, IntAppHandle.ToString), True)
                    MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format(My.Resources.strIntAppTitle, IntAppProcess.MainWindowTitle), True)
                    MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format(My.Resources.strIntAppParentHandle, Me.InterfaceControl.Parent.Handle.ToString), True)

                    SetParent(Me.IntAppHandle, Me.InterfaceControl.Parent.Handle)
                    SetWindowLong(Me.IntAppHandle, 0, WS_VISIBLE)
                    ShowWindow(Me.IntAppHandle, SW_SHOWMAXIMIZED)

                    Resize()

                    MyBase.Connect()
                    Return True
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strIntAppConnectionFailed & vbNewLine & ex.Message)
                    Return False
                End Try
            End Function


            Public Overrides Sub Focus()
                Try
                    SetForegroundWindow(IntAppHandle)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strIntAppFocusFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Overrides Sub Resize()
                Try
                    MoveWindow(IntAppHandle, -SystemInformation.FrameBorderSize.Width, -(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height), Me.InterfaceControl.Width + (SystemInformation.FrameBorderSize.Width * 2), Me.InterfaceControl.Height + SystemInformation.CaptionHeight + (SystemInformation.FrameBorderSize.Height * 2), True)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strIntAppResizeFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Overrides Sub Close()
                Try
                    If IntAppProcess.HasExited = False Then
                        IntAppProcess.Kill()
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strIntAppKillFailed & vbNewLine & ex.Message, True)
                End Try

                Try
                    If IntAppProcess.HasExited = False Then
                        IntAppProcess.Dispose()
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strIntAppDisposeFailed & vbNewLine & ex.Message, True)
                End Try

                MyBase.Close()
            End Sub
#End Region

#Region "Public Shared Methods"
          
#End Region

#Region "Enums"
            Public Enum Defaults
                Port = 0
            End Enum
#End Region

        End Class
    End Namespace
End Namespace
