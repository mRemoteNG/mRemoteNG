Imports System.Threading
Imports mRemote3G.App
Imports mRemote3G.Messages
Imports mRemote3G.Tools

Namespace Connection

    Namespace Protocol
        Public Class Base

#Region "Properties"

#Region "Control"

            Private _Name As String

            Public Property Name As String
                Get
                    Return Me._Name
                End Get
                Set
                    Me._Name = value
                End Set
            End Property

            Private WithEvents _connectionWindow As UI.Window.Connection

            Public Property ConnectionWindow As UI.Window.Connection
                Get
                    Return _connectionWindow
                End Get
                Set
                    _connectionWindow = value
                End Set
            End Property

            Private _interfaceControl As InterfaceControl

            Public Property InterfaceControl As InterfaceControl
                Get
                    Return _interfaceControl
                End Get
                Set
                    _interfaceControl = value
                    ConnectionWindow = TryCast(_interfaceControl.GetContainerControl(), UI.Window.Connection)
                End Set
            End Property

            Private _Control As Control

            Public Property Control As Control
                Get
                    Return Me._Control
                End Get
                Set
                    Me._Control = value
                End Set
            End Property

#End Region

            Private _Force As Info.Force

            Public Property Force As Info.Force
                Get
                    Return Me._Force
                End Get
                Set
                    Me._Force = value
                End Set
            End Property

            Public WithEvents tmrReconnect As New Timers.Timer(2000)
            Public WithEvents ReconnectGroup As ReconnectGroup

#End Region

#Region "Methods"

            Public Overridable Sub Focus()
                Try
                    Me._Control.Focus()
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't focus Control (Connection.Protocol.Base)" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Public Overridable Sub ResizeBegin(sender As Object, e As EventArgs) Handles _connectionWindow.ResizeBegin
            End Sub

            Public Overridable Sub Resize(sender As Object, e As EventArgs) Handles _connectionWindow.Resize
            End Sub

            Public Overridable Sub ResizeEnd(sender As Object, e As EventArgs) Handles _connectionWindow.ResizeEnd
            End Sub

            Public Overridable Function SetProps() As Boolean
                Try
                    Me._interfaceControl.Parent.Tag = Me._interfaceControl
                    Me._interfaceControl.Show()

                    If Me._Control IsNot Nothing Then
                        Me._Control.Name = Me._Name
                        Me._Control.Parent = Me._interfaceControl
                        Me._Control.Location = Me._interfaceControl.Location
                        Me._Control.Size = Me.InterfaceControl.Size
                        Me._Control.Anchor = Me._interfaceControl.Anchor
                    End If

                    Return True
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't SetProps (Connection.Protocol.Base)" & vbNewLine & ex.ToString(), True)
                    Return False
                End Try
            End Function

            Public Overridable Function Connect() As Boolean
                If InterfaceControl.Info.Protocol <> Protocols.RDP Then
                    RaiseEvent Connected(Me)
                End If
            End Function

            Public Overridable Sub Disconnect()
                Me.Close()
            End Sub

            Public Overridable Sub Close()
                Dim t As New Thread(AddressOf CloseBG)
                t.SetApartmentState(ApartmentState.STA)
                t.IsBackground = True
                t.Start()
            End Sub

            Private Sub CloseBG()
                RaiseEvent Closed(Me)

                Try
                    tmrReconnect.Enabled = False

                    If Me._Control IsNot Nothing Then
                        Try
                            Me.DisposeControl()
                        Catch ex As Exception
                            App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Could not dispose control, probably form is already closed (Connection.Protocol.Base)" & vbNewLine & ex.ToString(), True)
                        End Try
                    End If

                    If Me._interfaceControl IsNot Nothing Then
                        Try
                            If Me._interfaceControl.Parent IsNot Nothing Then
                                If Me._interfaceControl.Parent.Tag IsNot Nothing Then
                                    Me.SetTagToNothing()
                                End If

                                Me.DisposeInterface()
                            End If
                        Catch ex As Exception
                            App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Could not set InterfaceControl.Parent.Tag or Dispose Interface, probably form is already closed (Connection.Protocol.Base)" & vbNewLine & ex.ToString(), True)
                        End Try
                    End If
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't Close InterfaceControl BG (Connection.Protocol.Base)" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Delegate Sub DisposeInterfaceCB()

            Private Sub DisposeInterface()
                If Me._InterfaceControl.InvokeRequired Then
                    Dim s As New DisposeInterfaceCB(AddressOf DisposeInterface)
                    Me._InterfaceControl.Invoke(s)
                Else
                    Me._InterfaceControl.Dispose()
                End If
            End Sub

            Private Delegate Sub SetTagToNothingCB()

            Private Sub SetTagToNothing()
                If Me._InterfaceControl.Parent.InvokeRequired Then
                    Dim s As New SetTagToNothingCB(AddressOf SetTagToNothing)
                    Me._InterfaceControl.Parent.Invoke(s)
                Else
                    Me._InterfaceControl.Parent.Tag = Nothing
                End If
            End Sub

            Private Delegate Sub DisposeControlCB()

            Private Sub DisposeControl()
                If Me._Control.InvokeRequired Then
                    Dim s As New DisposeControlCB(AddressOf DisposeControl)
                    Me._Control.Invoke(s)
                Else
                    Me._Control.Dispose()
                End If
            End Sub

#End Region

#Region "Events"

            Public Event Connecting(sender As Object)
            Public Event Connected(sender As Object)
            Public Event Disconnected(sender As Object, DisconnectedMessage As String)
            Public Event ErrorOccured(sender As Object, ErrorMessage As String)
            Public Event Closing(sender As Object)
            Public Event Closed(sender As Object)

            Public Sub Event_Closing(sender As Object)
                RaiseEvent Closing(sender)
            End Sub

            Public Sub Event_Closed(sender As Object)
                RaiseEvent Closed(sender)
            End Sub

            Public Sub Event_Connecting(sender As Object)
                RaiseEvent Connecting(sender)
            End Sub

            Public Sub Event_Connected(sender As Object)
                RaiseEvent Connected(sender)
            End Sub

            Public Sub Event_Disconnected(sender As Object, DisconnectedMessage As String)
                RaiseEvent Disconnected(sender, DisconnectedMessage)
            End Sub

            Public Sub Event_ErrorOccured(sender As Object, ErrorMsg As String)
                RaiseEvent ErrorOccured(sender, ErrorMsg)
            End Sub

            Public Sub Event_ReconnectGroupCloseClicked() Handles ReconnectGroup.CloseClicked
                Close()
            End Sub

#End Region
        End Class
    End Namespace

End Namespace