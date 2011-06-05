Imports System.Windows.Forms
Imports System.Threading
Imports mRemoteNG.App.Runtime

Namespace Connection
    Namespace Protocol
        Public Class Base
#Region "Properties"
#Region "Control"
            Private _Name As String
            Public Property Name() As String
                Get
                    Return Me._Name
                End Get
                Set(ByVal value As String)
                    Me._Name = value
                End Set
            End Property


            Private _InterfaceControl As Connection.InterfaceControl
            Public Property InterfaceControl() As Connection.InterfaceControl
                Get
                    Return Me._InterfaceControl
                End Get
                Set(ByVal value As Connection.InterfaceControl)
                    Me._InterfaceControl = value
                End Set
            End Property

            Private _Control As Control
            Public Property Control() As Control
                Get
                    Return Me._Control
                End Get
                Set(ByVal value As Control)
                    Me._Control = value
                End Set
            End Property
#End Region

            Private _Force As mRemoteNG.Connection.Info.Force
            Public Property Force() As mRemoteNG.Connection.Info.Force
                Get
                    Return Me._Force
                End Get
                Set(ByVal value As mRemoteNG.Connection.Info.Force)
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
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't focus Control (Connection.Protocol.Base)" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Overridable Function SetProps() As Boolean
                Try
                    Me._InterfaceControl.Parent.Tag = Me._InterfaceControl
                    Me._InterfaceControl.Show()

                    If Me._Control IsNot Nothing Then
                        Me._Control.Name = Me._Name
                        Me._Control.Parent = Me._InterfaceControl
                        Me._Control.Location = Me._InterfaceControl.Location
                        Me._Control.Size = Me.InterfaceControl.Size
                        Me._Control.Anchor = Me._InterfaceControl.Anchor
                    End If

                    Return True
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't SetProps (Connection.Protocol.Base)" & vbNewLine & ex.Message, True)
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

            Public Overridable Sub Resize()

            End Sub

            Public Overridable Sub Close()
                Dim t As New Thread(AddressOf CloseBG)
                t.SetApartmentState(Threading.ApartmentState.STA)
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
                            MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Could not dispose control, probably form is already closed (Connection.Protocol.Base)" & vbNewLine & ex.Message, True)
                        End Try
                    End If

                    If Me._InterfaceControl IsNot Nothing Then
                        Try
                            If Me._InterfaceControl.Parent IsNot Nothing Then
                                If Me._InterfaceControl.Parent.Tag IsNot Nothing Then
                                    Me.SetTagToNothing()
                                End If

                                Me.DisposeInterface()
                            End If
                        Catch ex As Exception
                            MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Could not set InterfaceControl.Parent.Tag or Dispose Interface, probably form is already closed (Connection.Protocol.Base)" & vbNewLine & ex.Message, True)
                        End Try
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't Close InterfaceControl BG (Connection.Protocol.Base)" & vbNewLine & ex.Message, True)
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
            Public Event Connecting(ByVal sender As Object)
            Public Event Connected(ByVal sender As Object)
            Public Event Disconnected(ByVal sender As Object, ByVal DisconnectedMessage As String)
            Public Event ErrorOccured(ByVal sender As Object, ByVal ErrorMessage As String)
            Public Event Closing(ByVal sender As Object)
            Public Event Closed(ByVal sender As Object)

            Public Sub Event_Closing(ByVal sender As Object)
                RaiseEvent Closing(sender)
            End Sub

            Public Sub Event_Closed(ByVal sender As Object)
                RaiseEvent Closed(sender)
            End Sub

            Public Sub Event_Connecting(ByVal sender As Object)
                RaiseEvent Connecting(sender)
            End Sub

            Public Sub Event_Connected(ByVal sender As Object)
                RaiseEvent Connected(sender)
            End Sub

            Public Sub Event_Disconnected(ByVal sender As Object, ByVal DisconnectedMessage As String)
                RaiseEvent Disconnected(sender, DisconnectedMessage)
            End Sub

            Public Sub Event_ErrorOccured(ByVal sender As Object, ByVal ErrorMsg As String)
                RaiseEvent ErrorOccured(sender, ErrorMsg)
            End Sub

            Public Sub Event_ReconnectGroupCloseClicked() Handles ReconnectGroup.CloseClicked
                Close()
            End Sub
#End Region
        End Class
    End Namespace
End Namespace