Namespace Connection
    Namespace Protocol
        Public Class SSH1
            Inherits Connection.Protocol.PuttyBase

            Public Sub New()
                Me.PuttyProtocol = Putty_Protocol.ssh
                Me.PuttySSHVersion = Putty_SSHVersion.ssh1
            End Sub

            Public Enum Defaults
                Port = 22
            End Enum
        End Class
    End Namespace
End Namespace