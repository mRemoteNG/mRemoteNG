Imports mRemote3G.App
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Messages

Namespace Connection
    Public Class InterfaceControl

#Region "Properties"

        Private _Protocol As Base

        Public Property Protocol As Base
            Get
                Return Me._Protocol
            End Get
            Set
                Me._Protocol = value
            End Set
        End Property

        Private _Info As Info

        Public Property Info As Info
            Get
                Return Me._Info
            End Get
            Set
                Me._Info = value
            End Set
        End Property

#End Region

#Region "Methods"

        Public Sub New(Parent As Control, Protocol As Base, Info As Info)
            Try
                Me._Protocol = Protocol
                Me._Info = Info
                Me.Parent = Parent
                Me.Location = New Point(0, 0)
                Me.Size = Me.Parent.Size
                Me.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
                InitializeComponent()
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "Couldn't create new InterfaceControl" & vbNewLine & ex.ToString())
            End Try
        End Sub

#End Region
    End Class
End Namespace


