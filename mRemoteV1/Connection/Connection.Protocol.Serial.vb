Namespace Connection
    Namespace Protocol
        Public Class Serial
            Inherits Connection.Protocol.PuttyBase

            Public Sub New()
                Me.PuttyProtocol = Putty_Protocol.serial
            End Sub

            Public Enum Defaults
                Port = 9600
            End Enum
        End Class
    End Namespace
End Namespace