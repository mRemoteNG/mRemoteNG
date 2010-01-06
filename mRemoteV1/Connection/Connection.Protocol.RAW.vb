Namespace Connection
    Namespace Protocol
        Public Class RAW
            Inherits Connection.Protocol.PuttyBase

            Public Sub New()
                Me.PuttyProtocol = Putty_Protocol.raw
            End Sub

            Public Enum Defaults
                Port = 23
            End Enum
        End Class
    End Namespace
End Namespace