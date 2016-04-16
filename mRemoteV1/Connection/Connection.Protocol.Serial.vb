Namespace Connection

    Namespace Protocol
        Public Class Serial
            Inherits PuttyBase

            Public Sub New()
                Me.PuttyProtocol = Putty_Protocol.serial
            End Sub

            Public Enum Defaults
                None = 0
                Port = 9600
            End Enum
        End Class
    End Namespace

End Namespace