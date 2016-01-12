Namespace Connection
    Namespace Protocol
        Public Class Rlogin
            Inherits Connection.Protocol.PuttyBase

            Public Sub New()
                Me.PuttyProtocol = Putty_Protocol.rlogin
            End Sub

            Public Enum Defaults
                Port = 513
            End Enum
        End Class
    End Namespace
End Namespace