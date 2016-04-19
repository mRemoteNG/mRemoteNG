Namespace Connection

    Namespace Protocol
        Public Class RAW
            Inherits PuttyBase

            Public Sub New()
                Me.PuttyProtocol = Putty_Protocol.raw
            End Sub

            Public Enum Defaults
                None = 0
                Port = 23
            End Enum
        End Class
    End Namespace

End Namespace