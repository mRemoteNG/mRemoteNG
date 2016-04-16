Namespace Connection

    Namespace Protocol
        Public Class Rlogin
            Inherits PuttyBase

            Public Sub New()
                Me.PuttyProtocol = Putty_Protocol.rlogin
            End Sub

            Public Enum Defaults
                None = 0
                Port = 513
            End Enum
        End Class
    End Namespace

End Namespace