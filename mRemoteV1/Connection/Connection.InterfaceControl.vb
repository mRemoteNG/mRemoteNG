Imports System.Windows.Forms
Imports System.Drawing
Imports mRemoteNG.App.Runtime

Namespace Connection
    Public Class InterfaceControl
#Region "Properties"
        Private _Protocol As Connection.Protocol.Base
        Public Property Protocol() As Connection.Protocol.Base
            Get
                Return Me._Protocol
            End Get
            Set(ByVal value As Connection.Protocol.Base)
                Me._Protocol = value
            End Set
        End Property

        Private _Info As Connection.Info
        Public Property Info() As Connection.Info
            Get
                Return Me._Info
            End Get
            Set(ByVal value As Connection.Info)
                Me._Info = value
            End Set
        End Property
#End Region

#Region "Methods"
        Public Sub New(ByVal Parent As Control, ByVal Protocol As Connection.Protocol.Base, ByVal Info As Connection.Info)
            Try
                Me._Protocol = Protocol
                Me._Info = Info
                Me.Parent = Parent
                Me.Location = New Point(0, 0)
                Me.Size = Me.Parent.Size
                Me.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
                InitializeComponent()
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't create new InterfaceControl" & vbNewLine & ex.Message)
            End Try
        End Sub
#End Region
    End Class
End Namespace


