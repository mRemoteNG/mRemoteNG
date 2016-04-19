Namespace Connection

    Namespace Protocol
        Public Class SSH2
            Inherits PuttyBase

            Public Sub New()
                Me.PuttyProtocol = Putty_Protocol.ssh
                Me.PuttySSHVersion = Putty_SSHVersion.ssh2
            End Sub

            Public Enum Defaults
                None = 0
                Port = 22
            End Enum
        End Class
    End Namespace

End Namespace