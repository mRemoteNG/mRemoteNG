Namespace Connection
    Namespace Protocol
        Public Class Telnet
            Inherits Connection.Protocol.PuttyBase

            Public Sub New()
                Me.PuttyProtocol = Putty_Protocol.telnet
            End Sub

            Public Enum Defaults
                Port = 23
            End Enum
        End Class
    End Namespace
End Namespace